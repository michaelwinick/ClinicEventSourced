using Account.Application;
using Eventuous;
using Eventuous.AspNetCore.Web;
using Microsoft.AspNetCore.Mvc;

//using static Account.Application.AccountCommands;

namespace Account.HttpApi.Account;

[Route("/accounts")]
public class CommandApi : CommandHttpApiBase<Domain.Account>
{
    public CommandApi(IApplicationService<Domain.Account> service) : base(service) { }

    [HttpPost]
    [Route("startCreatingPersonalAccount")]
    public Task<ActionResult<Result>> StartCreatingPersonalAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("addPersonalAccountInformation")]
    public Task<ActionResult<Result>> AddPersonalAccountInformation([FromBody] AccountCommands.AddPersonalAccountInformation cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("completePersonalAccount")]
    public Task<ActionResult<Result>> CompletePersonalAccount([FromBody] AccountCommands.CompletePersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);


    [HttpPost]
    [Route("startCreatingParentAccount")]
    public Task<ActionResult<Result>> StartCreatingParentAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("addParentAccountInformation")]
    public Task<ActionResult<Result>> AddParentAccountInformation([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("createDependentAccount")]
    public Task<ActionResult<Result>> CreateDependentAccount(
        [FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);


    [HttpPost]
    [Route("completeCreatingParentAccount")]
    public Task<ActionResult<Result>> CompleteCreatingParentAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("startCreatingClinicAccount")]
    public Task<ActionResult<Result>> StartCreatingClinicAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("completeCreatingClinicAccount")]
    public Task<ActionResult<Result>> CompleteCreatingClinicAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("startCreatingAdministratorAccount")]
    public Task<ActionResult<Result>> StartCreatingAdministratorAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("completeCreatingAdministratorAccount")]
    public Task<ActionResult<Result>> CompleteCreatingAdministratorAccount(
        [FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("startCreatingEmployeeAccount")]
    public Task<ActionResult<Result>> StartCreatingEmployeeAccount([FromBody] AccountCommands.StartCreatingPersonalAccount cmd, CancellationToken cancellationToken)
        => Handle(cmd, cancellationToken);

}
