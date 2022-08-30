using Account.Domain.Account;

namespace Account.Application;

public static class AccountCommands
{
    public record StartCreatingPersonalAccount(
        string AccountId
    );

    //public record RecordPayment(string BookingId, float PaidAmount, string Currency, string PaymentId, string PaidBy);

    public class AddPersonalAccountInformation
    {
    }
}