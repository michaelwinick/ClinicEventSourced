using Eventuous;
using NodaTime;

namespace Account.Domain.Account;

public static class AccountEvents
{
    public static class V1
    {
        [EventType("V1.PersonalAccountCreationStarted")]
        public record PersonalAccountCreationStarted(
            string AccountId
        );

        [EventType("V1.PaymentRecorded")]
        public record PaymentRecorded(
            float PaidAmount,
            float Outstanding,
            string Currency,
            string PaymentId,
            string PaidBy,
            DateTimeOffset PaidAt
        );

        [EventType("V1.FullyPaid")]
        public record BookingFullyPaid(DateTimeOffset FullyPaidAt);

        [EventType("V1.BookingCancelled")]
        public record BookingCancelled(string CancelledBy, DateTimeOffset CancelledAt);
    }
}