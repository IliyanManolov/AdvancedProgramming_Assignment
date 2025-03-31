using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public override async Task<User> DeleteAsync(User entity)
    {
        entity.IsDeleted = true;
        await base.UpdateAsync(entity);
        return entity;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Query
            .FirstOrDefaultAsync(e => e.Username.Equals(username));
    }

    public Task<bool> IsExistingEmailAsync(string email)
    {
        return Query
            .AnyAsync(e => e.Email.Equals(email));
    }

    public Task<bool> IsExistingUsernameAsync(string username)
    {
        return Query
            .AnyAsync(e => e.Username.Equals(username));
    }
}