namespace Bookify.Domain;


public sealed class Booking: Entity
{
    private Booking(Guid id,
        Guid aparmentid,
        Guid userid,
        DateRange duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc):base(id)
    {
        ApartmentId = aparmentid;
        UserId = userid;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ConfirmedOnUtc { get; private set; }
    public DateTime RejectedOnUtc { get; private set; }
    public DateTime CompletedOnUtc { get; private set; }
    public DateTime CancelledOnUtc { get; private set; }

    public static Booking Reserve(
        Apartment apartment,
        Guid userid,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService
    )
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        var booking = new Booking(
            Guid.NewGuid(),
            apartment.Id,
            userid,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow
        );

        apartment.LastBookedOnUtc = utcNow;

        booking.RaiseDomainEvents(new BookingReservedDomainEvent(booking.Id));

        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        if( Status != BookingStatus.Reserved )
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Confirmed;
        RejectedOnUtc = utcNow;

        RaiseDomainEvents(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if( Status != BookingStatus.Reserved )
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;

        RaiseDomainEvents(new BookingRejectedDomainEvent(Id));

        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        if( Status != BookingStatus.Reserved )
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Completed;
        RejectedOnUtc = utcNow;

        RaiseDomainEvents(new BookingCompletedDomainEvent(Id));

        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if( Status != BookingStatus.Reserved )
            return Result.Failure(BookingErrors.NotReserved);

        var currentDate = DateOnly.FromDateTime(utcNow);

        if( currentDate > Duration.Start )
            return Result.Failure(BookingErrors.AlreadyStarted);

        Status = BookingStatus.Cancelled;
        RejectedOnUtc = utcNow;

        RaiseDomainEvents(new BookingCancelledDomainEvent(Id));

        return Result.Success();
    }

    // public static Booking Reserve(
    //     Guid apartmentid,
    //     Guid userid,
    //     DateRange duration,
    //     DateTime utcNow,
    //     PricingDetails pricingDetails
    // )
    // {
    //     var booking = new Booking(
    //         Guid.NewGuid(),
    //         apartmentid,
    //         userid,
    //         duration,
    //         pricingDetails.PriceForPeriod,
    //         pricingDetails.CleaningFee,
    //         pricingDetails.AmenitiesUpCharge,
    //         pricingDetails.TotalPrice,
    //         BookingStatus.Reserved,
    //         utcNow
    //     );

    //     booking.RaiseDomainEvents(new BookingReservedDomainEvents(booking.Id));

    //     return booking;
    // }

}


