using AutoFixture;
using Eventuous;
using Eventuous.TestHelpers.Fakes;

namespace Clinic_test.Fixtures;

public class NaiveFixture {
    protected IEventStore     EventStore     { get; }
    protected IAggregateStore AggregateStore { get; }
    protected Fixture         Auto           { get; } = new();

    protected NaiveFixture() {
        EventStore     = new InMemoryEventStore();
        AggregateStore = new AggregateStore(EventStore);
    }
}