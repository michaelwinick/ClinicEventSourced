using System.Text.Json.Serialization;
using Clinic.Domain;
using Eventuous;
using Eventuous.AspNetCore.Web;
using Pumper.Domain;

namespace Pumper.Application;

public class CommandService : ApplicationService<Domain.Pumper, PumperState, PaymentId>
{
    public CommandService(IAggregateStore store) : base(store)
    {
        //OnNew<PumperCommands.RecordPayment>(
        //    cmd => new PaymentId(cmd.PaymentId),
        //    (payment, cmd) => payment.ProcessPayment(
        //        new PaymentId(cmd.PaymentId),
        //        cmd.BookingId,
        //        new Money(cmd.Amount, cmd.Currency),
        //        cmd.Method,
        //        cmd.Provider
        //    )
        //);
    }
}

// [AggregateCommands(typeof(Pumper))]
public static class PumperCommands
{
    [HttpCommand]
    public record RecordPayment(
        string PaymentId,
        string BookingId,
        float Amount,
        string Currency,
        string Method,
        string Provider,
        [property: JsonIgnore] string PaidBy
    );
}
