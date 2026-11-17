namespace DocumentIntelligence.Identity.Application.Auth.Register;
public sealed record RegisterUserResult(
    bool Succeeded,
    RegisterUserResponse? User,
    string? ErrorCode,
    string? ErrorMessage)
{
    public static RegisterUserResult Success(RegisterUserResponse user) =>
        new(true, user, null, null);

    public static RegisterUserResult Failure(
        string errorCode,
        string errorMessage) =>
        new(false, null, errorCode, errorMessage);
}
