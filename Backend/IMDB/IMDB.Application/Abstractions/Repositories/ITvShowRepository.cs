using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface ITvShowRepository : IBaseRepository<TvShow>
{
    public Task<IEnumerable<TvShow>> GetAllCreatedByUserAsync(long? userId);
    public Task<IEnumerable<TvShow>> GetAllByDirectorIdAsync(long? directorId);
    public Task<IEnumerable<TvShow>> GetAllByActorIdAsync(long? actorId);
    public Task<IEnumerable<TvShow>> GetAllByActorNameAsync(string? actorName);
    public Task<IEnumerable<TvShow>> GetAllByGenreNameAsync(string? genreName);
    public Task<TvShow> GetByNameAsync(string? showName);
    public Task<IEnumerable<TvShow>> GetBySeasonsCountAsync(long? seasonsCount);
    public Task<IEnumerable<TvShow>> GetAllEndedAsync();
}
