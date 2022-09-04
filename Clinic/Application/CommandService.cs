using Eventuous;
using Pumper.Domain;

namespace Pumper.Application;

public class CommandService : ApplicationService<Domain.Pumper, PumperState, PumperId>
{
    public CommandService(IAggregateStore store) : base(store)
    {
        PumperId? pumperId = null;

        OnNew<Commands.AddPumper>(
            cmd =>
            {
                pumperId = new PumperId(Guid.NewGuid().ToString());
                return new PumperId(pumperId);
            },
            (pumper, cmd) => pumper.AddPumper(
                pumperId, cmd.AccountId));
    }
}
