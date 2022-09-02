using Eventuous;
using Pumper.Application;
using Pumper.Domain;
using EventHandler = Eventuous.Subscriptions.EventHandler;

namespace Pumper.Integration;

public class IntegrationHandler : EventHandler
{
    public static readonly StreamName Stream = new("Account-Integration");

    readonly IApplicationService<Domain.Pumper> _applicationService;

    public IntegrationHandler(IApplicationService<Domain.Pumper> applicationService)
    {
        _applicationService = applicationService;
        On<Events.V1.PersonalAccountCreatedIntegration>(async ctx => 
            await HandlePayment(ctx.Message, ctx.CancellationToken));
    }

    Task HandlePayment(Events.V1.PersonalAccountCreatedIntegration evt, CancellationToken cancellationToken)
    {
        return _applicationService.Handle(
            new Commands.AddPumper(evt.AccountId),
            cancellationToken
        );
    }
}