using Bookify.Domain;
using MediatR;

namespace Bookify.Application;

internal sealed class ReserveBookingDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IUserRepository _userRepo;
    private readonly IEmailService _emailService;

    public ReserveBookingDomainEventHandler(
        IBookingRepository bookingRepo, 
        IUserRepository userRepo, 
        IEmailService emailService)
    {
        _bookingRepo = bookingRepo;
        _userRepo = userRepo;
        _emailService = emailService;
    }


    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepo.GetByIdAsync(notification.id, cancellationToken);
        if (booking is null)
            return;

        var user = await _userRepo.GetByIdAsync(booking.UserId, cancellationToken);
        if ( user is null)
            return;

        await _emailService.SendAsync(user.Email,
            "Booking Reserved!",
            "You have 1 hour to confirm this booking");        
    }
}