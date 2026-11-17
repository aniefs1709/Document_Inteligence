using DocumentIntelligence.Identity.Domain.Entities;

namespace DocumentIntelligence.Identity.Application.Abstractions;
public interface IUserRepository
{
    Task<bool> ExistsByNormalizedEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<User?> GetByNormalizedEmailAsync(
    string normalizedEmail,
    CancellationToken cancellationToken = default);
}
