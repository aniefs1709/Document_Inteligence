namespace DocumentIntelligence.Identity.Application.Auth.Register;

public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string DisplayName);
