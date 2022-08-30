using Bookings.Domain.Bookings;
using Eventuous;
using Microsoft.AspNetCore.Mvc;

namespace Account.HttpApi.Bookings;

[Route("/bookings")]
public class QueryApi : ControllerBase
{
    readonly IAggregateStore _store;

    public QueryApi(IAggregateStore store) => _store = store;

    [HttpGet]
    [Route("{id}")]
    public async Task<BookingState> GetBooking(string id, CancellationToken cancellationToken)
    {
        var booking = await _store.Load<global::Bookings.Domain.Bookings.Account>(StreamName.For<global::Bookings.Domain.Bookings.Account>(id), cancellationToken);
        return booking.State;
    }
}