using Account.Domain.Bookings;
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
    public async Task<AccountState> GetBooking(string id, CancellationToken cancellationToken)
    {
        var booking = await _store.Load<global::Account.Domain.Bookings.Account>(StreamName.For<global::Account.Domain.Bookings.Account>(id), cancellationToken);
        return booking.State;
    }
}