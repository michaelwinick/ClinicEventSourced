namespace Account.Application;

public static class AccountCommands
{
    public record StartCreatingPersonalAccount(
        string BookingId,
        string GuestId,
        string RoomId,
        DateTime CheckInDate,
        DateTime CheckOutDate,
        float BookingPrice,
        float PrepaidAmount,
        string Currency,
        DateTimeOffset BookingDate
    );

    public record RecordPayment(string BookingId, float PaidAmount, string Currency, string PaymentId, string PaidBy);

    public class AddPersonalAccountInformation
    {
    }
}