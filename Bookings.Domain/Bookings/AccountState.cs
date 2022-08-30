using System.Collections.Immutable;
using Bookings.Domain;
using Eventuous;
using static Bookings.Domain.Bookings.BookingEvents;

namespace Account.Domain.Bookings;

public record AccountState : AggregateState<AccountState> {
    public string     GuestId     { get; init; }
    public RoomId     RoomId      { get; init; }
    public StayPeriod Period      { get; init; }
    public Money      Price       { get; init; }
    public Money      Outstanding { get; init; }
    public bool       Paid        { get; init; }

    public ImmutableList<PaymentRecord> PaymentRecords { get; init; } = ImmutableList<PaymentRecord>.Empty;

    internal bool HasPaymentBeenRecorded(string paymentId)
        => PaymentRecords.Any(x => x.PaymentId == paymentId);

    public AccountState() {
        On<V1.RoomBooked>(HandleBooked);
        On<V1.PaymentRecorded>(HandlePayment);
        On<V1.BookingFullyPaid>((state, paid) => state with { Paid = true });
    }

    static AccountState HandlePayment(AccountState state, V1.PaymentRecorded e)
        => state with {
            Outstanding = new Money { Amount = e.Outstanding, Currency = e.Currency },
            PaymentRecords = state.PaymentRecords.Add(
                new PaymentRecord(e.PaymentId, new Money { Amount = e.PaidAmount, Currency = e.Currency })
            )
        };

    static AccountState HandleBooked(AccountState state, V1.RoomBooked booked)
        => state with {
            RoomId = new RoomId(booked.RoomId),
            Period = new StayPeriod(booked.CheckInDate, booked.CheckOutDate),
            GuestId = booked.GuestId,
            Price = new Money { Amount       = booked.BookingPrice, Currency      = booked.Currency },
            Outstanding = new Money { Amount = booked.OutstandingAmount, Currency = booked.Currency }
        };
}

public record PaymentRecord(string PaymentId, Money PaidAmount);

public record DiscountRecord(Money Discount, string Reason);
