namespace MicroRabbit.Domain;

public interface IEventHandler<in TEvent> : IEventHandler 
    where TEvent : Event {
        Task Handle(TEvent @event);
    }

public interface IEventHandler {}