using System.Text.Json;
using Confluent.Kafka;

namespace MicroWithKafka;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string ConsumerClientId = "employeeConsumerClient";
    private readonly string GroupId = "employeeConsumerGroup";

    private readonly AppDbContext dbContext;

    public Worker(ILogger<Worker> logger, AppDbContext dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig(){
            BootstrapServers = "localhost:9092",  
            GroupId = GroupId,          
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        using(var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build()){
            consumer.Subscribe("employee_created");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerData = consumer.Consume(TimeSpan.FromSeconds(3));
                if ( consumerData is not null) { 

                    await SaveEmployee(consumerData.Message.Value);

                }
                else
                    _logger.LogInformation("Nothing to consume");
            }
        }
    }

    private async Task SaveEmployee(string employeeInJson) {
        try{
            var employee = JsonSerializer.Deserialize<Employee>(employeeInJson);
            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("New Employee Created");

        }catch(Exception ex) {
            _logger.LogError(ex.Message);
        }
    }
}
