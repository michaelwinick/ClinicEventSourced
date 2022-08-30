using Account.Domain;
using Account.Domain.Account;
using Eventuous;
using NodaTime;
using static Account.Application.AccountCommands;

namespace Account.Application;

public class AccountCommandService : ApplicationService<Domain.Account.Account, AccountState, AccountId>
{
    public AccountCommandService(IAggregateStore store, Services.IsRoomAvailable isRoomAvailable) : base(store)
    {
        OnNewAsync<StartCreatingPersonalAccount>(
            cmd => new AccountId(cmd.BookingId),
            (booking, cmd, _) => booking.BookRoom(
                new AccountId(cmd.BookingId),
                cmd.GuestId,
                new RoomId(cmd.RoomId),
                new StayPeriod(LocalDate.FromDateTime(cmd.CheckInDate), LocalDate.FromDateTime(cmd.CheckOutDate)),
                new Money(cmd.BookingPrice, cmd.Currency),
                new Money(cmd.PrepaidAmount, cmd.Currency),
                DateTimeOffset.Now,
                isRoomAvailable
            )
        );

        OnExisting<RecordPayment>(
            cmd => new AccountId(cmd.BookingId),
            (booking, cmd) => booking.RecordPayment(
                new Money(cmd.PaidAmount, cmd.Currency),
                cmd.PaymentId,
                cmd.PaidBy,
                DateTimeOffset.Now
            )
        );
    }
}