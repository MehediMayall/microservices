namespace Bookify.Domain;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<bool> IsOverlappingAsync(Apartment apartment, DateRange Duration, CancellationToken cancellationToken = default);
    void Add(Booking booking);
}