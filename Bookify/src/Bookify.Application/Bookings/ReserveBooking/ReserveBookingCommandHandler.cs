using Bookify.Domain;

namespace Bookify.Application;

internal sealed record ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{

    private readonly IUserRepository _userRepo;
    private readonly IApartmentRepository _apartmentRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly PricingService _priceService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReserveBookingCommandHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepo,
        IApartmentRepository apartmentRepo,
        IBookingRepository bookingRepo,
        PricingService priceService,
        IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _userRepo = userRepo;
        _apartmentRepo = apartmentRepo;
        _bookingRepo = bookingRepo;
        _priceService = priceService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound());

        var apartment = await _apartmentRepo.GetByIdAsync(request.ApartmentId, cancellationToken);
        if( apartment is null )
            return Result.Failure<Guid>(ApartmentErrors.NotFound());

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if( await _bookingRepo.IsOverlappingAsync(apartment, duration, cancellationToken))
            return Result.Failure<Guid>(BookingErrors.Overlap);

        var booking = Booking.Reserve(
            apartment,
            request.UserId,
            duration,
            _dateTimeProvider.UtcNow,
            _priceService
        );

        _bookingRepo.Add(booking);
        await _unitOfWork.SaveChangesAsync( cancellationToken);

        return booking.Id;
    }
}