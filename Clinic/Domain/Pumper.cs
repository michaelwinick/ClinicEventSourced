using Eventuous;

namespace Pumper.Domain;

public class Pumper : Aggregate<PumperState>
{
    public void AddPumper(PumperId? pumperId, string accountId)
    {
        EnsureDoesntExist();
        Apply(new Events.V1.PumperAdded(pumperId.Value, accountId));
    }
}

public record PumperId(string Value) : AggregateId(Value);
