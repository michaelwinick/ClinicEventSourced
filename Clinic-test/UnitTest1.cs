using Account.Application;
using Account.Domain;
using Clinic_test.Fixtures;
using Eventuous;
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
        var accountId = Guid.NewGuid().ToString();

        var event1 = GeneratePersonalAccountCreationStarted(accountId);
        var event2 = GeneratePersonalAccountInformationAdded(accountId);
        
        var expectedEvents = GenerateExpectedEvents(accountId);
        var theAccountStream = TheAccountStream(event1.AccountId);

        var seedEvents = new List<object>
        {
            event1,
            event2
        };

        await SeedEventStore(seedEvents, theAccountStream);

        await Service.Handle(
            new AccountCommands.CompletePersonalAccount(
                event1.AccountId,
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
            .BeEquivalentTo(
                expectedEvents.Select(x => x.Event));
    }

    private static Events.V1.PersonalAccountInformationAdded GeneratePersonalAccountInformationAdded(string accountId)
    {
        return new Events.V1.PersonalAccountInformationAdded(
            accountId, "FirstName", "LastName", "1/6/65", "InformationAdded");
    }

    private static Events.V1.PersonalAccountCreationStarted GeneratePersonalAccountCreationStarted(string accountId)
    {
        return new Events.V1.PersonalAccountCreationStarted(
            accountId, "Started", "Pumper");
    }

    private static StreamName TheAccountStream(string accountId) =>
        new(GetStreamName(new AccountId(accountId)));
    

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

    private static Change[] GenerateExpectedEvents(string accountId)
    {
        return new Change[] {
            new(
                new Events.V1.PersonalAccountCreationStarted(accountId.ToString(), "Started", "Pumper"),
                "V1.PersonalAccountCreationStarted"
            ),
            new(
                new Events.V1.PersonalAccountInformationAdded(accountId, "FirstName", "LastName", "1/6/65", "InformationAdded"),
                "V1.PersonalAccountInformationAdded"
            ),
            new(
                new Events.V1.PersonalAccountCreated(accountId, "email", "password", "securityQuestion", "securityAnswer", "healthDataNotice", "termsOfUse", "PersonalAccountCreated"),
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