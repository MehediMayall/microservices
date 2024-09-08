using Bookify.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure;


internal sealed class BookingRepository: Repository<Booking>, IBookingRepository
{
    private static readonly BookingStatus[] ActiveBookingStatuses =
    {
        BookingStatus.Reserved,
        BookingStatus.Confirmed,
        BookingStatus.Completed
    };
    
    public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<bool> IsOverlappingAsync(
        Apartment apartment, 
        DateRange duration, 
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Booking>()
            .AnyAsync(
                booking =>
                    booking.ApartmentId == apartment.Id && 
                    booking.Duration.Start <= duration.End && 
                    booking.Duration.End >= duration.Start && 
                    ActiveBookingStatuses.Contains(booking.Status),
                    cancellationToken
            );
    }
}