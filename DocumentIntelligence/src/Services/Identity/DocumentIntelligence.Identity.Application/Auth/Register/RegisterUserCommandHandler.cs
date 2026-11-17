using System.Net.Mail;
using DocumentIntelligence.Identity.Application.Abstractions;
using DocumentIntelligence.Identity.Domain.Entities;

using MediatR;

namespace DocumentIntelligence.Identity.Application.Auth.Register;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<RegisterUserResult> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.DisplayName))
        {
            return RegisterUserResult.Failure(
                "validation_error",
                "Email, password, and display name are required.");
        }

        var email = request.Email.Trim();

        if (email.Length > 320 ||
            !MailAddress.TryCreate(email, out var parsedEmail) ||
            !string.Equals(
                parsedEmail.Address,
                email,
                StringComparison.OrdinalIgnoreCase))
        {
            return RegisterUserResult.Failure(
                "invalid_email",
                "Enter a valid email address.");
        }

        if (request.Password.Length is < 12 or > 128)
        {
            return RegisterUserResult.Failure(
                "invalid_password",
                "Password must be between 12 and 128 characters.");
        }

        var displayName = request.DisplayName.Trim();

        if (displayName.Length is < 2 or > 100)
        {
            return RegisterUserResult.Failure(
                "invalid_display_name",
                "Display name must be between 2 and 100 characters.");
        }

        var normalizedEmail = email.ToUpperInvariant();

        if (await userRepository.ExistsByNormalizedEmailAsync(
                normalizedEmail,
                cancellationToken))
        {
            return RegisterUserResult.Failure(
                "email_already_registered",
                "An account with this email address already exists.");
        }

        var passwordHash = passwordHasher.HashPassword(request.Password);

        var user = new User(
            email,
            passwordHash,
            displayName);

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return RegisterUserResult.Success(
            new RegisterUserResponse(
                user.Id,
                user.Email,
                user.DisplayName,
                user.CreatedAt));
    }
}
