namespace DocumentIntelligence.Identity.Application.Auth.Register;
public sealed record RegisterUserResponse(
    Guid Id,
    string Email,
    string DisplayName,
    DateTimeOffset CreatedAt);
