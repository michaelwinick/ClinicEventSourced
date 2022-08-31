using Eventuous;
using static Eventuous.Diagnostics.TelemetryTags;

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

    public void CompletePersonalAccount(
        AccountId accountId, 
        string email, 
        string password, 
        string securityQuestion, 
        string securityAnswer, 
        string healthDataNotice, 
        string termsOfUse)
    {
        EnsureExists();

        Apply(
            new AccountEvents.V1.PersonalAccountCreated(
                accountId, email, password, securityQuestion, securityAnswer, healthDataNotice, termsOfUse, 
                "PersonalAccountCreated")
        );
    }
}