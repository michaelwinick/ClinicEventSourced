using Eventuous;

namespace Clinic.Domain;

public static class PaymentEvents
{
    [EventType("PaymentRecorded")]
    public record PaymentRecorded(
        string PaymentId, string BookingId, float Amount, string Currency, string Method, string Provider
    );
}