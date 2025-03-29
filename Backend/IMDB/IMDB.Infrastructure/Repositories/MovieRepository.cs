using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class MovieRepository : BaseRepository<Movie>, IMovieRepository
{
    public MovieRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Movie>> GetAllByActorIdAsync(long? actorId)
    {
        return await Query
            .Where(e => e.Actors.Any(ac => ac.Id.Equals(actorId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllByActorNameAsync(string? actorName)
    {

        return await Query
            .Where(e => e.Actors.Any(ac => $"{ac.FirstName} {ac.LastName}".Equals(actorName)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllByDirectorIdAsync(long? directorId)
    {
        return await Query
            .Where(e => e.Directors.Any(di => di.Id.Equals(directorId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllByGenreIdAsync(long? genreId)
    {
        return await Query
            .Where(e => e.Genres.Any(ge => ge.Id.Equals(genreId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllByGenreNameAsync(string? genreName)
    {
        return await Query
            .Where(e => e.Genres.Any(ge => ge.Name.Equals(genreName)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllCreatedByUserAsync(long? userId)
    {
        return await Query
            .Where(e => e.CreatedByUserId.Equals(userId))
            .ToListAsync();
    }
    public async Task<IEnumerable<Movie>> GetAllBetweenSpecificLengthAsync(long? minLength, long? maxLength)
    {
        return await Query
            .Where(e => minLength >= e.Length && e.Length <= maxLength)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllWithMaxLengthAsync(long? maxLength)
    {
        return await Query
            .Where(e => e.Length <= maxLength)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllWithMinimumLengthAsync(long? minLength)
    {
        return await Query
            .Where(e => minLength >= e.Length)
            .ToListAsync();
    }

    public async Task<Movie?> GetByNameAsync(string? movieName)
    {
        return await Query
            .Where(e => e.Title.Equals(movieName))
            .FirstOrDefaultAsync();
    }
}