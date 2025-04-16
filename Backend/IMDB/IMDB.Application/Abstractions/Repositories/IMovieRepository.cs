using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface IMovieRepository : IBaseRepository<Movie>
{
    public Task<IEnumerable<Movie>> GetAllCreatedByUserAsync(long? userId);
    public Task<IEnumerable<Movie>> GetAllByDirectorIdAsync(long? directorId);
    public Task<IEnumerable<Movie>> GetAllByActorIdAsync(long? actorId);
    public Task<IEnumerable<Movie>> GetAllByActorNameAsync(string? actorName);
    public Task<IEnumerable<Movie>> GetAllByGenreIdAsync(long? genreId);
    public Task<IEnumerable<Movie>> GetAllByGenreNameAsync(string? genreName);
    public Task<Movie?> GetByNameAsync(string? movieName);
    public Task<IEnumerable<Movie>> GetAllWithMaxLengthAsync(long? maxLength);
    public Task<IEnumerable<Movie>> GetAllWithMinimumLengthAsync(long? minLength);
    public Task<IEnumerable<Movie>> GetAllBetweenSpecificLengthAsync(long? minLength, long? maxLength);
    public Task<IEnumerable<Movie>> GetFiveLatestAsync();
}
