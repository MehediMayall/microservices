namespace Bookify.Domain;

public sealed record BookingCompletedDomainEvent(Guid id):IDomainEvent;


