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
    }
}