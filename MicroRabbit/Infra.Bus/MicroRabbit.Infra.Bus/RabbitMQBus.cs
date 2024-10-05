using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using MicroRabbit.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMQBus : IEventBus
{
    private readonly IMediator mediator;
    private readonly Dictionary<string, List<Type>> handlers;
    private readonly List<Type> eventTypes;

    public RabbitMQBus(IMediator mediator) {
        this.mediator = mediator;
        handlers = new();
        eventTypes = new();
    }


    public Task SendCommand<T>(T command) where T : Command
    {
        return mediator.Send(command);
    }
    
    public void Publish<T>(T @event) where T : Event
    {
        // Create Connection Factory
        var factory  = new ConnectionFactory {
            HostName = "localhost",
            UserName = "mehedi",
            Password = "mehedi007",
            VirtualHost = "booking",
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var eventName = @event.GetType().Name;
            channel.QueueDeclare(eventName, false, false, false, null);

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", eventName, null, body);            
        }

    }


    public void Subscribe<T, THandler>()
        where T : Event
        where THandler : IEventHandler<T>
    {
         var eventName = typeof(T).Name;
         var handlerType = typeof(THandler);

         if (!eventTypes.Contains(typeof(T))) {
            eventTypes.Add(typeof(T));
         }

         if (!handlers.ContainsKey(eventName)) {
            handlers.Add(eventName, new List<Type>());
         }

         if (handlers[eventName].Any(s=> s.GetType() == handlerType)) {
            throw new ArgumentException($"Handler type {handlerType.Name} already is registered for {eventName}", nameof(handlerType));
         }

         handlers[eventName].Add(handlerType);
         StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : Event
    {
       var factory = new ConnectionFactory() {
        HostName = "localhost",
        DispatchConsumersAsync = true
       };

       var connection = factory.CreateConnection();
       var channel = connection.CreateModel();

       var eventName = typeof(T).Name;

       channel.QueueDeclare(eventName, false, false, false, null);

       var consumer = new AsyncEventingBasicConsumer(channel);

       consumer.Received += Consumer_Received;

       channel.BasicConsume(eventName, true, consumer);

    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        var eventName = @event.RoutingKey;
        var message = Encoding.UTF8.GetString(@event.Body.ToArray());

        try{

            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }catch(Exception ex){

        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (handlers.ContainsKey(eventName)) {
            var subsriptions = handlers[eventName];
            foreach(var subscription in subsriptions) {
                var handler = Activator.CreateInstance(subscription);
            }
        }
    }
}