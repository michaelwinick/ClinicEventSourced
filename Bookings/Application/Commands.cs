namespace Account.Application;

public static class AccountCommands
{
    public record StartCreatingPersonalAccount(
        string AccountId
    );

    public record AddPersonalAccountInformation(
        string AccountId,
        string FirstName,
        string LastName,
        string Dob);
}