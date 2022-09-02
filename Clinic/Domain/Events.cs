using Eventuous;

namespace Pumper.Domain;

public static class Events
{
    public static class V1
    {
        [EventType("V1.PumperAdded")]
        public record PumperAdded(string PumperId, string AccountId);

        [EventType("V1.PersonalAccountCreatedIntegration")]
        public record PersonalAccountCreatedIntegration(string AccountId);
    }
}