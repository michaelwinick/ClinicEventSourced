using Eventuous;
using NodaTime;
using System.Diagnostics;
using static Account.Domain.Services;

namespace Account.Domain;

public class Account : Aggregate<AccountState>
{
    public void StartCreatingPersonalAccount(AccountId accountId)
    {
        EnsureDoesntExist();

        Apply(
            new AccountEvents.V1.PersonalAccountCreationStarted(
                accountId, "Started", "Pumper")
        );
    }

    public void AddPersonalAccountInformation(
        AccountId accountId,
        string firstName,
        string lastName,
        string dob)
    {
        EnsureExists();

        Apply(
            new AccountEvents.V1.PersonalAccountInformationAdded(
                accountId, firstName, lastName, dob, "InformationAdded")
        );
    }
}