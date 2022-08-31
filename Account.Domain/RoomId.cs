using Eventuous;

namespace Account.Domain;

public record RoomId(string Value) : AggregateId(Value);