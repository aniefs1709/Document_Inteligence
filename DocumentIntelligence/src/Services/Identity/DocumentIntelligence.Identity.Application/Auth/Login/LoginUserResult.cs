namespace DocumentIntelligence.Identity.Application.Auth.Login;

public sealed record LoginUserResult(
    bool Succeeded,
    LoginUserResponse? User,
    string? ErrorCode,
    string? ErrorMessage)
{
    public static LoginUserResult Success(LoginUserResponse user) =>
        new(true, user, null, null);

    public static LoginUserResult Failure(
        string errorCode,
        string errorMessage) =>
        new(false, null, errorCode, errorMessage);
}