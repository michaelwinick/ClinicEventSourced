using Eventuous.Projections.MongoDB.Tools;
using NodaTime;

namespace Account.Application.Queries;

public record AccountDocument : ProjectedDocument
{
    public AccountDocument(string id) : base(id)
    {
        AccountId = id;
    }

    public string AccountId { get; init; }
    public string AccountType { get; init; }
    public string State { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Dob { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string SecurityQuestion { get; init; }
    public string SecurityAnswer { get; init; }
    public string HealthDataNotice { get; init; }
    public string TermsOfUser { get; init; }
}