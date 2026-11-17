
using DocumentIntelligence.Identity.Application.Abstractions;
using DocumentIntelligence.Identity.Domain.Entities;
using DocumentIntelligence.Identity.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace DocumentIntelligence.Identity.Infrastructure.Repositories;

public sealed class UserRepository(IdentityDbContext dbContext) : IUserRepository
{
    public Task<bool> ExistsByNormalizedEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AnyAsync(
            user => user.NormalizedEmail == normalizedEmail,
            cancellationToken);
    }

    public Task<User?> GetByNormalizedEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Users.SingleOrDefaultAsync(
            user => user.NormalizedEmail == normalizedEmail,
            cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
