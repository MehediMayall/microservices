namespace Bookify.Domain;

public abstract class Entity
{
    private readonly List<IDomainEvent> domainEvents = new();
    protected Entity(Guid id){ Id = id;}
    public Guid Id { get; init; }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() =>
        domainEvents.ToList();

    public void ClearDomainEvents() =>
        domainEvents.Clear();

    protected void RaiseDomainEvents(IDomainEvent domainEvent) 
    {
        domainEvents.Add(domainEvent);        
    }
}