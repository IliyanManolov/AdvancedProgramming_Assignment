using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface IDirectorRepository : IBaseRepository<Director>
{
    public Task<IEnumerable<Director>> GetAllCreatedByUserAsync(long? userId);
    public Task<IEnumerable<Director>> GetAllInMovieAsync(long? movieId);
    public Task<IEnumerable<Director>> GetAllInShowAsync(long? showId);
    public Task<Director?> GetByNameAsync(string firstName, string lastName);
}
