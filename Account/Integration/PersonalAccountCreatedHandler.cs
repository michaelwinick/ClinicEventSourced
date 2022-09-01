using Account.Domain;
using Eventuous;
using EventHandler = Eventuous.Subscriptions.EventHandler;

namespace Account.Integration;

public class PersonalAccountCreatedHandler : EventHandler
{
    public static readonly StreamName Stream = new("Account-Integration");

    readonly IApplicationService<Domain.Account> _applicationService;

    public PersonalAccountCreatedHandler(IApplicationService<Domain.Account> applicationService)
    {
        _applicationService = applicationService;
        On<AccountEvents.V1.PersonalAccountCreatedIntegration>(async ctx => await HandlePayment(ctx.Message, ctx.CancellationToken));
    }

    Task HandlePayment(AccountEvents.V1.PersonalAccountCreatedIntegration evt, CancellationToken cancellationToken)
    {
        return _applicationService.Handle(
            new object(),
            cancellationToken
        );
    }
}


