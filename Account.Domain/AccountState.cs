using System.Collections.Immutable;
using Eventuous;

namespace Account.Domain;

public record AccountState : AggregateState<AccountState>
{
    public string AccountId { get; set; }
    public string FirstName { get; set; }
    public string Dob { get; set; }
    public string LastName { get; set; }

    public string TermsOfUse { get; set; }
    public string HealthDataNotice { get; set; }
    public string SecurityAnswer { get; set; }
    public string SecurityQuestion { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    public string AccountType { get; set; }
    public string CurrentState { get; set; }


    public AccountState()
    {
        On<Events.V1.PersonalAccountCreationStarted>(HandleAccountCreation);
        On<Events.V1.PersonalAccountInformationAdded>(HandleAccountInformationAdded);
        On<Events.V1.PersonalAccountCreated>(HandleAccountCreated);

    }

    static AccountState HandleAccountCreation(AccountState state, Events.V1.PersonalAccountCreationStarted e)
        => state with
        {
            AccountId = e.AccountId,
            CurrentState = e.State,
            AccountType = e.AccountType
        };

    static AccountState HandleAccountInformationAdded(AccountState state,
        Events.V1.PersonalAccountInformationAdded e)
        => state with
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            Dob = e.Dob,
            CurrentState = e.State
        };

    static AccountState HandleAccountCreated(AccountState state,
        Events.V1.PersonalAccountCreated e)
        => state with
        {
            Email = e.Email,
            Password = e.Password,
            SecurityQuestion = e.SecurityQuestion,
            SecurityAnswer = e.SecurityAnswer,
            HealthDataNotice = e.HealthDataNotice,
            TermsOfUse = e.TermsOfUse,
            CurrentState = e.State
        };
}