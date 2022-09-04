using Account.Domain;
using Eventuous;
using Microsoft.AspNetCore.Mvc;

namespace Account.HttpApi.Account;

[Route("/accounts")]
public class QueryApi : ControllerBase
{
    readonly IAggregateStore _store;

    public QueryApi(IAggregateStore store) => _store = store;

    [HttpGet]
    [Route("{id}")]
    public async Task<AccountState> GetAccount(string id, CancellationToken cancellationToken)
    {
        var account = await _store.Load<Domain.Account>(StreamName.For<Domain.Account>(id), cancellationToken);
        return account.State;
    }
}