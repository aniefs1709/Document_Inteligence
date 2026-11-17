
using DocumentIntelligence.Identity.Application.Abstractions;

using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Identity.Infrastructure.Security;

public sealed class IdentityPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        return _passwordHasher.HashPassword(
            user: new object(),
            password: password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        try
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user: new object(),
                hashedPassword: passwordHash,
                providedPassword: password);

            return result != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            // A malformed stored hash is not a valid password match.
            return false;
        }
        catch (ArgumentException)
        {
            // Invalid hash input must not cause authentication to crash.
            return false;
        }
    }
}
