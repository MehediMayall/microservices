using Bookify.Application;
using Bookify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure;


public sealed class ApplicationDbContext: DbContext, IUnitOfWork
{
    private readonly IPublisher publisher;

    public ApplicationDbContext(DbContextOptions options, IPublisher publisher ): base(options)
    {
        this.publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        
        base.OnModelCreating(modelBuilder);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            // Publishing domain events after database save changes.
            await PublishDomainEventsAsync();
            return result;
        }
        catch(DbUpdateConcurrencyException ex) {
            throw new ConcurrencyException("Concurrency exception occured", ex);
        }
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entity => entity.Entity)
            .SelectMany(entity => {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach(var domainEvent in domainEvents) {
            await publisher.Publish(domainEvent);
        }
    }
}