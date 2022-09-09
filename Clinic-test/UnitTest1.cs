using Account.Application;
using Account.Domain;
using Clinic_test.Fixtures;
using Eventuous;
using Eventuous.TestHelpers.Fakes;


namespace Clinic_test;


public class UnitTest1 : NaiveFixture
{
    public UnitTest1()
    {
        var streamNameMap = new StreamNameMap();
        streamNameMap.Register<AccountId>(GetStreamName);
        Service = new AccountCommandService(AggregateStore);
        TypeMap.RegisterKnownEventTypes();
    }

    AccountCommandService Service { get; }


    [Fact]
    public async Task Test1()
    {
        Events.V1.PersonalAccountCreationStarted ev = new(Guid.NewGuid().ToString(), "Started", "Pumper");
        var events = new List<Events.V1.PersonalAccountCreationStarted>
        {
            ev
        };

        var streamName = new StreamName(GetStreamName(new AccountId(ev.AccountId)));

        var streamEvents = events.Select(x => new StreamEvent(Guid.NewGuid(), x, new Metadata(), "", 0));

        await EventStore.AppendEvents(
            streamName,
            ExpectedStreamVersion.NoStream, 
            streamEvents.ToArray(),
            default
        );
        
        var expected = new Change[] {
            new(new AccountCommands.AddPersonalAccountInformation(ev.AccountId, "FirstName", "LastName", "1/6/65"), 
                "V1.")
        };

        var (accountState, success, enumerable) = await Service.Handle(
            new AccountCommands.AddPersonalAccountInformation(
                ev.AccountId,
                "FirstName",
                "LastName", 
                "1/6/65"),
            new CancellationToken());

        var readEvents = await EventStore.ReadEvents(
            streamName,
            StreamReadPosition.Start, 2,
            new CancellationToken());
    }
    static StreamName GetStreamName(AccountId accountId) => new($"Account-{accountId}");

}