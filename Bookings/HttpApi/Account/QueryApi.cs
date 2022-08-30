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
    public async Task<AccountState> GetAccount(string id, CancellationToken cancellationToken)
    {
        var account = await _store.Load<Domain.Account.Account>(StreamName.For<Domain.Account.Account>(id), cancellationToken);
        return account.State;
    }
}