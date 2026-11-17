using System.Text.Json;
using DocumentIntelligence.Identity.Application.Abstractions;
using StackExchange.Redis;

namespace DocumentIntelligence.Identity.Infrastructure.Sessions;

public sealed class ValkeySessionStore(
    IConnectionMultiplexer redis) : ISessionStore
{
    private const string KeyPrefix = "di:identity:session:";   
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task CreateAsync(
        SessionInfo session,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var ttl = session.ExpiresAt - DateTimeOffset.UtcNow;

        if (ttl <= TimeSpan.Zero)
        {
            throw new ArgumentException(
                "Session expiry must be in the future.",
                nameof(session));
        }

        var key = KeyPrefix + session.SessionId;
        var value = JsonSerializer.Serialize(session);

        var created = await _database.StringSetAsync(
            key,
            value,
            ttl,
            When.NotExists);

        if (!created)
        {
            throw new InvalidOperationException(
                "A session with this ID already exists.");
        }
    }

    public async Task<SessionInfo?> GetActiveAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var value = await _database.StringGetAsync(
            KeyPrefix + sessionId);

        if (value.IsNullOrEmpty)
        {
            return null;
        }

        var session = JsonSerializer.Deserialize<SessionInfo>(
            value.ToString());

        if (session is null ||
            session.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return null;
        }

        return session;
    }

    public async Task RevokeAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        await _database.KeyDeleteAsync(KeyPrefix + sessionId);
    }
}