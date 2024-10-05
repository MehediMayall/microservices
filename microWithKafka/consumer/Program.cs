using Microsoft.EntityFrameworkCore;
using MicroWithKafka;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<AppDbContext>(options =>{
    options.UseNpgsql("Host=localhost;Database=microservices;Username=postgres;password=mehedi#007;");
}, ServiceLifetime.Singleton
);

var host = builder.Build();
host.Run();
