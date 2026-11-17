namespace DocumentIntelligence.Identity.Application.Abstractions;

public interface ISessionStore
{
    Task CreateAsync(
        SessionInfo session,
        CancellationToken cancellationToken = default);

    Task<SessionInfo?> GetActiveAsync(
        string sessionId,
        CancellationToken cancellationToken = default);

    Task RevokeAsync(
        string sessionId,
        CancellationToken cancellationToken = default);
}