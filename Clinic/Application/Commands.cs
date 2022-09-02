using Eventuous.AspNetCore.Web;

namespace Pumper.Application;

//[AggregateCommands(typeof(Domain.Pumper))]
public static class Commands
{
    [HttpCommand]
    public record AddPumper(string AccountId);
}