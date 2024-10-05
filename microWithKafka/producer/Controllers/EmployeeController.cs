using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroWithKafka;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase {
    private readonly ILogger<EmployeesController> logger;
    private readonly AppDbContext dbContext;

    public EmployeesController(ILogger<EmployeesController> logger, AppDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IEnumerable<Employee>> GetEmployees(){
        logger.LogInformation("Requesting all employees");
        return await dbContext.Employees.ToListAsync();
    }

    [HttpPost("")]
    public async Task<ActionResult<Employee>> SaveEmployee([FromBody] EmployeeDto employee) {
        var newEmployee = new Employee(Guid.NewGuid(), employee.Name, employee.Department);
        dbContext.Employees.Add(newEmployee);
        await dbContext.SaveChangesAsync();

        var message = new Message<string, string>(){
            Key = newEmployee.Id.ToString(),
            Value = JsonSerializer.Serialize(newEmployee),
        };

        // Client
        var producerConfig = new ProducerConfig(){
            BootstrapServers = "localhost:9092",
            Acks = Acks.All,
        };

        // Producer
        var producer = new ProducerBuilder<string, string>(producerConfig)
            .Build();

        await producer.ProduceAsync("employee_created", message);

        producer.Dispose();

        return Ok(employee);
    }

}