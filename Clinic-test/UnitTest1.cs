using Account.Domain;
using Eventuous;
using Eventuous.TestHelpers.Fakes;

namespace Clinic_test;


public class UnitTest1
{
    public UnitTest1()
    {
        
    }

    [Fact]
    public async Task Test1()
    {
        var eventStore = new InMemoryEventStore();

        var ev = new Events.V1.PersonalAccountCreationStarted(
            Guid.NewGuid().ToString(), "Started", "Pumper");

        var events = new List<Events.V1.PersonalAccountCreationStarted>
        {
            ev
        };

        var streamEvents = events.Select(x => new StreamEvent(Guid.NewGuid(), x, new Metadata(), "", 0));

        StreamName Stream = new StreamName();
        var appendEventsResult = await eventStore.AppendEvents(
            new StreamName(ev.AccountId),
            ExpectedStreamVersion.NoStream, 
            streamEvents.ToArray(),
            default
        );


        var readEvents = await eventStore.ReadEvents(new StreamName(ev.AccountId), StreamReadPosition.Start, 100, new CancellationToken());

    }
}