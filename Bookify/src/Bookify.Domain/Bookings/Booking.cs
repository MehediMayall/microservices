namespace Bookify.Domain;


public sealed class Booking: Entity
{
    public Booking(Guid id):base(id)
    {

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

}
