using Account.Domain.Bookings;
using Bookings.Domain;
using Bookings.Domain.Bookings;
using Eventuous;
using NodaTime;
using static Account.Application.AccountCommands;

namespace Account.Application;

public class AccountCommandService : ApplicationService<Domain.Bookings.Account, AccountState, BookingId>
{
    public AccountCommandService(IAggregateStore store, Services.IsRoomAvailable isRoomAvailable) : base(store)
    {
        OnNewAsync<StartCreatingPersonalAccount>(
            cmd => new BookingId(cmd.BookingId),
            (booking, cmd, _) => booking.BookRoom(
                new BookingId(cmd.BookingId),
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
            cmd => new BookingId(cmd.BookingId),
            (booking, cmd) => booking.RecordPayment(
                new Money(cmd.PaidAmount, cmd.Currency),
                cmd.PaymentId,
                cmd.PaidBy,
                DateTimeOffset.Now
            )
        );
    }
}