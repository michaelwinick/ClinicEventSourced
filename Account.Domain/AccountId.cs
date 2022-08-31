using Eventuous;

namespace Account.Domain;

public record AccountId(string Value) : AggregateId(Value);