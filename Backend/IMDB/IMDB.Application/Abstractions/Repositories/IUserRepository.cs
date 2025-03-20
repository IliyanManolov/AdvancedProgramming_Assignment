using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> GetByUsernameAsync(string username);
    public Task<bool> IsExistingUsernameAsync(string username);
    public Task<bool> IsExistingEmailAsync(string email);
}
