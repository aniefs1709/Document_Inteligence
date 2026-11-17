namespace DocumentIntelligence.Identity.Application.Auth.Login;

public sealed record LoginUserResponse(
    Guid Id,
    string Email,
    string DisplayName);