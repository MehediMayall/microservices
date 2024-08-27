namespace Bookify.Domain;

public record UserCreatedDomainEvent(Guid UserId): IDomainEvent;