using Account.Domain;
using Eventuous;
using NodaTime;
using static Account.Application.AccountCommands;

namespace Account.Application;

public class AccountCommandService : ApplicationService<Domain.Account, AccountState, AccountId>
{
    public AccountCommandService(IAggregateStore store) : base(store)
    {
        OnNew<StartCreatingPersonalAccount>(
            cmd => new AccountId(cmd.AccountId),
            (account, cmd) => account.StartCreatingPersonalAccount(
                new AccountId(cmd.AccountId)
            )
        );

        OnExisting<AddPersonalAccountInformation>(
            cmd => new AccountId(cmd.AccountId),
            (account, cmd) => account.AddPersonalAccountInformation(
                new AccountId(cmd.AccountId),
                cmd.FirstName,
                cmd.LastName,
                cmd.Dob
            )
        );

        OnExisting<CompletePersonalAccount>(
            cmd => new AccountId(cmd.AccountId),
            (account, cmd) => account.CompletePersonalAccount(
                new AccountId(cmd.AccountId),
                cmd.Email,
                cmd.Password,
                cmd.SecurityQuestion,
                cmd.SecurityAnswer,
                cmd.HealthDataNotice,
                cmd.TermsOfUse
            )
        );
    }
}