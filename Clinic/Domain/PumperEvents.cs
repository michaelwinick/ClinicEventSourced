using Eventuous;

namespace Pumper.Domain;

public static class PumperEvents
{
    [EventType("PaymentRecorded")]
    public record PaymentRecorded(
        string PaymentId, string BookingId, float Amount, string Currency, string Method, string Provider
    );
}