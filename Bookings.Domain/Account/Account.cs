using Eventuous;
using NodaTime;
using System.Diagnostics;
using static Account.Domain.Account.AccountEvents;
using static Account.Domain.Services;

namespace Account.Domain.Account;

public class Account : Aggregate<AccountState>
{
    public Task StartCreatingPersonalAccount(AccountId accountId)
    {
        EnsureDoesntExist();
        
        Apply(
            new V1.PersonalAccountCreationStarted(accountId)
        );

        return Task.CompletedTask;
    }
}