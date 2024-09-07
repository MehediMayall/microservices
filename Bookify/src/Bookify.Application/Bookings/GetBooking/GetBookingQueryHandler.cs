using Bookify.Domain;

namespace Bookify.Application;


internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    public Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}