namespace Bookify.Application;

public sealed record GetBookingQuery(Guid BookingId): IQuery<BookingResponse>;
