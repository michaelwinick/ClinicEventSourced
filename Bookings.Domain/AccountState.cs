using System.Collections.Immutable;
using Eventuous;

namespace Account.Domain;

public record AccountState : AggregateState<AccountState>
{
    public string AccountId { get; set; }
    public string FirstName { get; set; }
    public string Dob { get; set; }
    public string LastName { get; set; }



    public AccountState()
    {
        On<AccountEvents.V1.PersonalAccountCreationStarted>(HandleAccountCreation);
        On<AccountEvents.V1.PersonalAccountInformationAdded>(HandleAccountInformationAdded);

    }

    static AccountState HandleAccountCreation(AccountState state, AccountEvents.V1.PersonalAccountCreationStarted e)
        => state with
        {
            AccountId = e.AccountId
        };

    static AccountState HandleAccountInformationAdded(AccountState state,
        AccountEvents.V1.PersonalAccountInformationAdded e)
        => state with
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            Dob = e.Dob
        };
}

