using Account.Domain;
using Eventuous;
using Eventuous.Gateway;
using Eventuous.Subscriptions.Context;
using static Account.Integration.IntegrationEvents;

namespace Account.Integration;

public static class PaymentsGateway
{
    static readonly StreamName Stream = new("Account-Integration");

    public static ValueTask<GatewayMessage[]> Transform(IMessageConsumeContext original)
    {
        var result = original.Message is AccountEvents.V1.PersonalAccountCreated evt
            ? new GatewayMessage(
                Stream,
                new PersonalAccountCreatedIntegration(evt.AccountId),
                new Metadata()
            )
            : null;

        return ValueTask.FromResult(result != null 
            ? new[] { result } 
            : Array.Empty<GatewayMessage>());
    }
}

public static class IntegrationEvents
{
    [EventType("PersonalAccountCreatedIntegration")]
    public record PersonalAccountCreatedIntegration(string AccountId);
}