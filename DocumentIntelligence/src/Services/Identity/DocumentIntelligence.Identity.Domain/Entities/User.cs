namespace DocumentIntelligence.Identity.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Email { get; private set; } = string.Empty;

    public string NormalizedEmail { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    // Required by Entity Framework Core.
    private User()
    {
    }

    public User(string email, string passwordHash, string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

        Email = email.Trim();
        NormalizedEmail = Email.ToUpperInvariant();
        PasswordHash = passwordHash;
        DisplayName = displayName.Trim();
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateDisplayName(string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        DisplayName = displayName.Trim();
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
