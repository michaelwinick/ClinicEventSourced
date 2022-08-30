using Eventuous;

namespace Account.Domain.Account;

public record AccountId(string Value) : AggregateId(Value);