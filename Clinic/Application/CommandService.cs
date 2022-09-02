using System.Text.Json.Serialization;
using Clinic.Domain;
using Eventuous;
using Eventuous.AspNetCore.Web;
using Pumper.Domain;

namespace Pumper.Application;

public class CommandService : ApplicationService<Domain.Pumper, PumperState, PumperId>
{
    public CommandService(IAggregateStore store) : base(store)
    {
        OnNew<Commands.AddPumper>(
            cmd => new PumperId(cmd.AccountId),
            (pumper, cmd) => pumper.AddPumper(
                new PumperId(cmd.AccountId)
            )
        );
    }
}
