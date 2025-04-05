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
    public override async Task<TvShow?> GetByIdAsync(long? id)
    {
        return await Query
            .Include(E => E.Actors)
            .Include(e => e.Genres)
            .Include(e => e.Episodes)
            .Include(e => e.Director)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public override async Task<IEnumerable<TvShow>> GetAllAsync()
    {
        return await Query
            .Include(E => E.Actors)
            .Include(e => e.Genres)
            .Include(e => e.Director)
            .Include(e => e.Episodes)
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByActorIdAsync(long? actorId)
    {
        return await Query
            .Where(e => e.Actors.Any(ac => ac.Id.Equals(actorId)))
            .Include(e => e.Actors)
            .Include(e => e.Genres)
            .Include(e => e.Director)
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByActorNameAsync(string? actorName)
    {

        return await Query
            .Where(e => e.Actors.Any(ac => $"{ac.FirstName} {ac.LastName}".Equals(actorName)))
            .Include(e => e.Actors)
            .Include(e => e.Genres)
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByDirectorIdAsync(long? directorId)
    {
        return await Query
            .Where(e => e.Director.Id.Equals(directorId))
            .Include(e => e.Actors)
            .Include(e => e.Genres)
            .ToListAsync();
    }


    public async Task<IEnumerable<TvShow>> GetAllByGenreIdAsync(long? genreId)
    {
        return await Query
            .Where(e => e.Genres.Any(ge => ge.Id.Equals(genreId)))
            .Include(e => e.Genres)
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllByGenreNameAsync(string? genreName)
    {
        return await Query
            .Where(e => e.Genres.Any(ge => ge.Name.Equals(genreName)))
            .Include(e => e.Genres)
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllCreatedByUserAsync(long? userId)
    {
        return await Query
            .Where(e => e.CreatedByUserId.Equals(userId))
            .Include(e => e.Actors)
            .ToListAsync();
    }

    public async Task<TvShow?> GetByNameAsync(string? movieName)
    {
        return await Query
            .Where(e => e.Title.Equals(movieName))
            .Include(e => e.Actors)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TvShow>> GetBySeasonsCountAsync(long? seasonsCount)
    {
        return await Query
            .Where(e => e.Seasons < seasonsCount)
            .Include(e => e.Actors)
            .ToListAsync();
    }

    public async Task<IEnumerable<TvShow>> GetAllEndedAsync()
    {
        return await Query
            .Where(e => e.EndDate != null)
            .Include(e => e.Actors)
            .ToListAsync();
    }
}
