using Account.Application;
using Account.Domain;
using Clinic_test.Fixtures;
using Eventuous;
using FluentAssertions;

namespace Clinic_test;


public class UnitTest1 : NaiveFixture
{
    AccountCommandService Service { get; }

    public UnitTest1()
    {
        var streamNameMap = new StreamNameMap();
        streamNameMap.Register<AccountId>(GetStreamName);
        Service = new AccountCommandService(AggregateStore);
        TypeMap.RegisterKnownEventTypes();
    }

    [Fact]
    public async Task Test1()
    {
        var theAccountId = new AccountId(Guid.NewGuid().ToString());

        await SeedEventStoreWithEvents(
            new List<object>
            {
                PersonalAccountCreationStarted(theAccountId),
                PersonalAccountInformationAdded(theAccountId)
            }, 
            GetStreamName(theAccountId));

        await Service.Handle(
            new AccountCommands.CompletePersonalAccount(
                PersonalAccountCreationStarted(theAccountId).AccountId,
                "email",
                "password", 
                "securityQuestion",
                "securityAnswer",
                "healthDataNotice",
                "termsOfUse"),
            new CancellationToken());

        var accountEvents = await ReadEventsFromStream(GetStreamName(theAccountId));

        accountEvents
            .Select(x => x.Payload)
            .Should()
            .BeEquivalentTo(
                ExpectedEvents(theAccountId).Select(x => x.Event));
    }

    private async Task SeedEventStoreWithEvents(List<object> seedEvents, StreamName streamName)
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

    private static Change[] ExpectedEvents(string accountId)
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

    private static Events.V1.PersonalAccountInformationAdded PersonalAccountInformationAdded(string accountId)
    {
        return new Events.V1.PersonalAccountInformationAdded(
            accountId, "FirstName", "LastName", "1/6/65", "InformationAdded");
    }

    private static Events.V1.PersonalAccountCreationStarted PersonalAccountCreationStarted(string accountId)
    {
        return new Events.V1.PersonalAccountCreationStarted(
            accountId, "Started", "Pumper");
    }

    private static StreamName GetStreamName(AccountId accountId) => new($"Account-{accountId}");
}