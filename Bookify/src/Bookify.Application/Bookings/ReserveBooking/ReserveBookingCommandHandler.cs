using Bookify.Domain;

namespace Bookify.Application;

internal sealed record ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{

    private readonly IUserRepository _userRepo;
    private readonly IApartmentRepository _apartmentRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly PricingService _priceServic;

    private readonly IUnitOfWork _unitOfWork;

    public ReserveBookingCommandHandler(
        IUnitOfWork unitOfWork, 
        IUserRepository userRepo, 
        IApartmentRepository apartmentRepo, 
        IBookingRepository bookingRepo, 
        PricingService priceServic)
    {
        _unitOfWork = unitOfWork;
        _userRepo = userRepo;
        _apartmentRepo = apartmentRepo;
        _bookingRepo = bookingRepo;
        _priceServic = priceServic;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound());

    }
}