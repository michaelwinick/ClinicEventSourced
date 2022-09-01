using Clinic.Domain;
using Eventuous;
using static Pumper.Domain.PumperEvents;

namespace Pumper.Domain;

public class Pumper : Aggregate<PumperState>
{
    public void ProcessPayment(
        PaymentId paymentId, string bookingId, Money amount, string method, string provider
    )
        => Apply(new PaymentRecorded(paymentId, bookingId, amount.Amount, amount.Currency, method, provider));
}

public record PumperState : AggregateState<PumperState>
{
    public string BookingId { get; init; } = null!;
    public float Amount { get; init; }

    public PumperState()
    {
        On<PaymentRecorded>(
            (state, recorded) => state with
            {
                BookingId = recorded.BookingId,
                Amount = recorded.Amount
            }
        );
    }
}

public record PaymentId(string Value) : AggregateId(Value);