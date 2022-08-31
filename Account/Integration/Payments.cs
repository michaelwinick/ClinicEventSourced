using Eventuous;
using static Account.Application.AccountCommands;
using static Account.Integration.IntegrationEvents;
using EventHandler = Eventuous.Subscriptions.EventHandler;

namespace Account.Integration;

public class PaymentsIntegrationHandler : EventHandler
{
    public static readonly StreamName Stream = new("PaymentsIntegration");

    readonly IApplicationService<Domain.Account> _applicationService;

    public PaymentsIntegrationHandler(IApplicationService<Domain.Account> applicationService)
    {
        _applicationService = applicationService;
        //On<BookingPaymentRecorded>(async ctx => await HandlePayment(ctx.Message, ctx.CancellationToken));
    }

    //Task HandlePayment(BookingPaymentRecorded evt, CancellationToken cancellationToken)
    //    => _applicationService.Handle(
    //        new RecordPayment(
    //            evt.BookingId,
    //            evt.Amount,
    //            evt.Currency,
    //            evt.PaymentId,
    //            ""
    //        ),
    //        cancellationToken
    //    );
}

static class IntegrationEvents
{
    [EventType("BookingPaymentRecorded")]
    public record BookingPaymentRecorded(string PaymentId, string BookingId, float Amount, string Currency);
}