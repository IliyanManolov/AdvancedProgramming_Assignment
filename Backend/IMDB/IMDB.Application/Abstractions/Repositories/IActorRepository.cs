using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface IActorRepository : IBaseRepository<Actor>
{
    public Task<IEnumerable<Actor>> GetAllCreatedByUserAsync(long? userId);
    public Task<IEnumerable<Actor>> GetAllInMovieAsync(long? movieId);
    public Task<IEnumerable<Actor>> GetAllInShowAsync(long? showId);
    public Task<Actor?> GetByNameAsync(string firstName, string lastName);
}
