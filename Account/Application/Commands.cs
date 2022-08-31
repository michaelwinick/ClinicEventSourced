namespace Account.Application;

public static class AccountCommands
{
    public record StartCreatingPersonalAccount(
        string AccountId);

    public record AddPersonalAccountInformation(
        string AccountId,
        string FirstName,
        string LastName,
        string Dob);

    public record CompletePersonalAccount(
        string AccountId,
        string Email,
        string Password,
        string SecurityQuestion,
        string SecurityAnswer,
        string HealthDataNotice,
        string TermsOfUse);
}