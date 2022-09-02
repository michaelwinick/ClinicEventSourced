using Eventuous;

namespace Pumper.Domain;

public record PumperState : AggregateState<PumperState>
{
    public string AccountId { get; init; }
    public PumperId PumperId { get; set; }


    public PumperState()
    {
        On<Events.V1.PumperAdded>(
            (state, recorded) => state with
            {
                AccountId = recorded.AccountId,
                PumperId = new PumperId(recorded.PumperId)
            }
        );
    }
}