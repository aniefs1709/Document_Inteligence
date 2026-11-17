
using DocumentIntelligence.Identity.Application.Abstractions;

using MediatR;

namespace DocumentIntelligence.Identity.Application.Auth.Login;

public sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    public async Task<LoginUserResult> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return LoginUserResult.Failure(
                "invalid_credentials",
                "Email or password is incorrect.");
        }

        var normalizedEmail = request.Email.Trim().ToUpperInvariant();

        var user = await userRepository.GetByNormalizedEmailAsync(
            normalizedEmail,
            cancellationToken);

        if (user is null ||
            !passwordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash) ||
            !user.IsActive)
        {
            return LoginUserResult.Failure(
                "invalid_credentials",
                "Email or password is incorrect.");
        }

        var response = new LoginUserResponse(
            user.Id,
            user.Email,
            user.DisplayName);

        return LoginUserResult.Success(response);
    }
}
