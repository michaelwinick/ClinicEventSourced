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
            cmd => new AccountId(Guid.NewGuid().ToString()),
            (account, cmd, _) => account.StartCreatingPersonalAccount(
                new AccountId(cmd.AccountId)
            )
        );
    }
}