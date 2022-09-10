using Account.Application;
using Account.Domain;
using Clinic_test.Fixtures;
using Eventuous;
using Eventuous.TestHelpers.Fakes;
using FluentAssertions;

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
        Events.V1.PersonalAccountCreationStarted ev1 = 
            new(Guid.NewGuid().ToString(), "Started", "Pumper");
        
        Events.V1.PersonalAccountInformationAdded ev2 = 
            new(ev1.AccountId, "FirstName", "LastName", "1/6/65", "InformationAdded");

        var expected = GenerateExpectedEvents(ev1);
        
        var theAccountStream = new StreamName(GetStreamName(new AccountId(ev1.AccountId)));

        var seedEvents = new List<object>
        {
            ev1,
            ev2
        };

        await SeedEventStore(seedEvents, theAccountStream);

        await Service.Handle(
            new AccountCommands.CompletePersonalAccount(
                ev1.AccountId,
                "email",
                "password", 
                "securityQuestion",
                "securityAnswer",
                "healthDataNotice",
                "termsOfUse"),
            new CancellationToken());

        var accountEvents = await ReadEventsFromStream(theAccountStream);

        accountEvents
            .Select(x => x.Payload)
            .Should()
            .BeEquivalentTo(expected.Select(x => x.Event));
    }

    private async Task SeedEventStore(List<object> seedEvents, StreamName streamName)
    {
        var streamEvents =
            seedEvents.Select(e =>
                new StreamEvent(Guid.NewGuid(), e, new Metadata(), "", 0));

        await EventStore.AppendEvents(
            streamName,
            ExpectedStreamVersion.NoStream,
            streamEvents.ToArray(),
            default
        );
    }

    private static Change[] GenerateExpectedEvents(Events.V1.PersonalAccountCreationStarted ev1)
    {
        return new Change[] {
            new(
                new Events.V1.PersonalAccountCreationStarted(ev1.AccountId.ToString(), "Started", "Pumper"),
                "V1.PersonalAccountCreationStarted"
            ),
            new(
                new Events.V1.PersonalAccountInformationAdded(ev1.AccountId, "FirstName", "LastName", "1/6/65", "InformationAdded"),
                "V1.PersonalAccountInformationAdded"
            ),
            new(
                new Events.V1.PersonalAccountCreated(ev1.AccountId, "email", "password", "securityQuestion", "securityAnswer", "healthDataNotice", "termsOfUse", "PersonalAccountCreated"),
                "V1.PersonalAccountCreated"
            )
        };
    }

    private async Task<StreamEvent[]> ReadEventsFromStream(StreamName streamName)
    {
        return await EventStore.ReadEvents(
            streamName,
            StreamReadPosition.Start, 100,
            new CancellationToken());
    }

    static StreamName GetStreamName(AccountId accountId) => new($"Account-{accountId}");

}