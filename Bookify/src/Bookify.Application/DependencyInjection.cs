using Bookify.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application;


public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddMediatR(config=>{
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddTransient<PricingService>();
        return services;
    }
}