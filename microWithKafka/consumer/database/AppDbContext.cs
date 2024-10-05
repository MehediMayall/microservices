using Microsoft.EntityFrameworkCore;

namespace MicroWithKafka;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> dbContext) : base(dbContext)
    {

    }

    public DbSet<Employee> Employees { get; set; }

    
   

}