using Eventuous;

namespace Pumper.Domain;

public class Pumper : Aggregate<PumperState>
{
    public void AddPumper(PumperId pumperId)
    {
        EnsureDoesntExist();
        Apply(new Events.V1.PumperAdded(pumperId));
    }
}

public record PumperId(string Value) : AggregateId(Value);