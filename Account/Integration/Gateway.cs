using Account.Domain;
using Eventuous;
using Eventuous.Gateway;
using Eventuous.Subscriptions.Context;

namespace Account.Integration;

public static class AccountGateway
{
    static readonly StreamName Stream = new("Account-Integration");

    public static ValueTask<GatewayMessage[]> Transform(IMessageConsumeContext original)
    {
        var result = original.Message is Events.V1.PersonalAccountCreated evt
            ? new GatewayMessage(
                Stream,
                new Events.V1.PersonalAccountCreatedIntegration(evt.AccountId),
                new Metadata()
            )
            : null;

        return ValueTask.FromResult(result != null 
            ? new[] { result } 
            : Array.Empty<GatewayMessage>());
    }
}
