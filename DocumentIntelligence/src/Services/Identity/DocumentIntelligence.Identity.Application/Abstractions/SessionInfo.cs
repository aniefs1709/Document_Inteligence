namespace DocumentIntelligence.Identity.Application.Abstractions;

public sealed record SessionInfo(
    string SessionId,
    Guid UserId,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt);