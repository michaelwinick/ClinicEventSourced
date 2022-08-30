using Account.Domain.Account;
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
        var booking = await _store.Load<global::Account.Domain.Account.Account>(StreamName.For<global::Account.Domain.Account.Account>(id), cancellationToken);
        return booking.State;
    }
}