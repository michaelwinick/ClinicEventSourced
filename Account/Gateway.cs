using Account.Domain;
using Eventuous;
using Eventuous.Gateway;
using Eventuous.Subscriptions.Context;

namespace Clinic.Integration;

public static class PaymentsGateway
{
    static readonly StreamName Stream = new("PaymentsIntegration");

    public static ValueTask<GatewayMessage[]> Transform(IMessageConsumeContext original)
    {
        var result = original.Message is AccountEvents.V1.PersonalAccountCreated evt
            ? new GatewayMessage(
                Stream,
                //new BookingPaymentRecorded(evt.PaymentId, evt.BookingId, evt.Amount, evt.Currency),
                new object(),
                new Metadata()
            )
            : null;
        return ValueTask.FromResult(result != null ? new[] { result } : Array.Empty<GatewayMessage>());
    }
}

public static class IntegrationEvents
{
    [EventType("BookingPaymentRecorded")]
    public record BookingPaymentRecorded(string PaymentId, string BookingId, float Amount, string Currency);
}