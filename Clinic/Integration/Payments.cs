using Clinic.Domain;
using Eventuous;
using Eventuous.Gateway;
using Eventuous.Subscriptions.Context;
using static Clinic.Integration.IntegrationEvents;

namespace Clinic.Integration;

public static class PaymentsGateway
{
    static readonly StreamName Stream = new("Account.Integration");

    public static ValueTask<GatewayMessage[]> Transform(IMessageConsumeContext original)
    {
        var result = original.Message is PaymentEvents.PaymentRecorded evt
            ? new GatewayMessage(
                Stream,
                new BookingPaymentRecorded(evt.PaymentId, evt.BookingId, evt.Amount, evt.Currency),
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