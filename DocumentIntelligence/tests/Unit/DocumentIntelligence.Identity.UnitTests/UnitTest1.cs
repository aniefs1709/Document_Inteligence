
using DocumentIntelligence.Identity.Infrastructure.Security;

namespace DocumentIntelligence.Identity.UnitTests;

public sealed class IdentityPasswordHasherTests
{
    private readonly IdentityPasswordHasher _hasher = new();

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        const string password = "CorrectTestPassword!2026";
        var hash = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var hash = _hasher.HashPassword("CorrectTestPassword!2026");

        // Act
        var result = _hasher.VerifyPassword(
            "WrongTestPassword!2026",
            hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_WithInvalidHash_ReturnsFalse()
    {
        // Act
        var result = _hasher.VerifyPassword(
            "CorrectTestPassword!2026",
            "not-a-valid-password-hash");

        // Assert
        Assert.False(result);
    }
}
