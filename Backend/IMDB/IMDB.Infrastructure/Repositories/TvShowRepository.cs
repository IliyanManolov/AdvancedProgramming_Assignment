using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class TvShowRepository : BaseRepository<TvShow>, ITvShowRepository
{
    public TvShowRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<TvShow>> GetAllByActorIdAsync(long? actorId)
    {
        return await Query
            .Where(e => e.Actors.Any(ac => ac.Id.Equals(actorId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByActorNameAsync(string? actorName)
    {

        return await Query
            .Where(e => e.Actors.Any(ac => $"{ac.FirstName} {ac.LastName}".Equals(actorName)))
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByDirectorIdAsync(long? directorId)
    {
        return await Query
            .Where(e => e.Directors.Any(di => di.Id.Equals(directorId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByGenreIdAsync(long? genreId)
    {
        return await Query
            .Where(e => e.Genres.Any(ge => ge.Id.Equals(genreId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByGenreNameAsync(string? genreName)
    {
        return await Query
            .Where(e => e.Genres.Any(ge => ge.Name.Equals(genreName)))
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllCreatedByUserAsync(long? userId)
    {
        return await Query
            .Where(e => e.CreatedByUserId.Equals(userId))
            .ToListAsync();
    }

    public async Task<TvShow?> GetByNameAsync(string? movieName)
    {
        return await Query
            .Where(e => e.Title.Equals(movieName))
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TvShow>> GetBySeasonsCountAsync(long? seasonsCount)
    {
        return await Query
            .Where(e => e.Seasons < seasonsCount)
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllEndedAsync()
    {
        return await Query
            .Where(e => e.EndDate != null)
            .ToListAsync();
    }
}
