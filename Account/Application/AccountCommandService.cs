using Account.Domain;
using Eventuous;
using NodaTime;
using static Account.Application.AccountCommands;

namespace Account.Application;

public class AccountCommandService : ApplicationService<Domain.Account, AccountState, AccountId>
{
    public AccountCommandService(IAggregateStore store) : base(store)
    {
        OnNewAsync<StartCreatingPersonalAccount>(
            cmd => new AccountId(cmd.AccountId),
            (account, cmd, _) => account.StartCreatingPersonalAccount(
                new AccountId(cmd.AccountId)
            )
        );

        OnExistingAsync<AddPersonalAccountInformation>(
            cmd => new AccountId(cmd.AccountId),
            (account, cmd, _) => account.AddPersonalAccountInformation(
                new AccountId(cmd.AccountId),
                cmd.FirstName,
                cmd.LastName,
                cmd.Dob
            )
        );
    }
}