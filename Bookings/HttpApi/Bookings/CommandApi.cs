using Account.Application;
using Eventuous;
using Eventuous.AspNetCore.Web;
using Microsoft.AspNetCore.Mvc;
//using static Account.Application.AccountCommands;

namespace Account.HttpApi.Bookings;

[Route("/accounts")]
public class CommandApi : CommandHttpApiBase<global::Account.Domain.Account.Account>
{
    public CommandApi(IApplicationService<global::Account.Domain.Account.Account> service) : base(service) { }

    [HttpPost]
    [Route("startCreatingPersonalAccount")]
    public Task<ActionResult<Result>> StartCreatingPersonalAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);
}
