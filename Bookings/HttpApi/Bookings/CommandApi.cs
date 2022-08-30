using Account.Application;
using Bookings.Domain.Bookings;
using Eventuous;
using Eventuous.AspNetCore.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using static Account.Application.AccountCommands;

namespace Account.HttpApi.Bookings;

[Route("/accounts")]
public class CommandApi : CommandHttpApiBase<global::Account.Domain.Bookings.Account>
{
    public CommandApi(IApplicationService<global::Account.Domain.Bookings.Account> service) : base(service) { }

    [HttpPost]
    [Route("startCreatingPersonalAccount")]
    public Task<ActionResult<Result>> StartCreatingPersonalAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);
}
