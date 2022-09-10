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
        Events.V1.PersonalAccountCreationStarted ev1 = new(Guid.NewGuid().ToString(), "Started", "Pumper");
        Events.V1.PersonalAccountInformationAdded ev2 = new(ev1.AccountId, "FirstName", "LastName", "1/6/65", "InformationAdded");
        
        var streamName = new StreamName(GetStreamName(new AccountId(ev1.AccountId)));

        var newEvents = new List<object>();

        newEvents.Add(ev1);
        newEvents.Add(ev2);
        
        var streamEvents = 
            newEvents.Select(e => 
                new StreamEvent(Guid.NewGuid(), e, new Metadata(), "", 0));

        await EventStore.AppendEvents(
            streamName,
            ExpectedStreamVersion.NoStream, 
            streamEvents.ToArray(),
            default
        );

        var expected = new Change[] {
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

        var (accountState, success, enumerable) = await Service.Handle(
            new AccountCommands.CompletePersonalAccount(
                ev1.AccountId,
                "email",
                "password", 
                "securityQuestion",
                "securityAnswer",
                "healthDataNotice",
                "termsOfUse"),
            new CancellationToken());

        var readEvents = await EventStore.ReadEvents(
            streamName,
            StreamReadPosition.Start, 100,
            new CancellationToken());

        readEvents.Select(x => x.Payload)
            .Should().BeEquivalentTo(expected.Select(x => x.Event));
    }

    static StreamName GetStreamName(AccountId accountId) => new($"Account-{accountId}");

}