using System.Collections.Immutable;
using Eventuous;
using static Account.Domain.Account.AccountEvents;

namespace Account.Domain.Account;

public record AccountState : AggregateState<AccountState>
{
    public string AccountId { get; set; }
    

    public ImmutableList<PaymentRecord> PaymentRecords { get; init; } = ImmutableList<PaymentRecord>.Empty;

    internal bool HasPaymentBeenRecorded(string paymentId)
        => PaymentRecords.Any(x => x.PaymentId == paymentId);

    public AccountState()
    {
        On<V1.PersonalAccountCreationStarted>(Handle);
    }

    static AccountState Handle(AccountState state, V1.PersonalAccountCreationStarted e)
        => state with
        {
            AccountId = state.AccountId
        };

}

public record PaymentRecord(string PaymentId, Money PaidAmount);

public record DiscountRecord(Money Discount, string Reason);
