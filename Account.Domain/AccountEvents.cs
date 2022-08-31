using Eventuous;
using NodaTime;

namespace Account.Domain;

public static class AccountEvents
{
    public static class V1
    {
        [EventType("V1.PersonalAccountCreationStarted")]
        public record PersonalAccountCreationStarted(
            string AccountId,
            string State,
            string AccountType
        );

        [EventType("V1.PersonalAccountInformationAdded")]
        public record PersonalAccountInformationAdded(
            string AccountId, 
            string FirstName, 
            string LastName, 
            string Dob, 
            string State);

        [EventType("V1.PersonalAccountCreated")]
        public record PersonalAccountCreated(
            string AccountId,
            string Email, 
            string Password,
            string SecurityQuestion,
            string SecurityAnswer,
            string HealthDataNotice,
            string TermsOfUse,
            string State);
    }
}