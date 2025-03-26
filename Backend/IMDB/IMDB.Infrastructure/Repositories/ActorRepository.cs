using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class ActorRepository : BaseRepository<Actor>, IActorRepository
{
    public ActorRepository(DatabaseContext dbContext) : base(dbContext)
    {

    }

    public async Task<IEnumerable<Actor>> GetAllCreatedByUserAsync(long? userId)
    {
        return await Query
            .Where(a => a.CreatedByUserId.Value.Equals(userId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Actor>> GetAllInMovieAsync(long? movieId)
    {
        return await Query
            .Where(a => a.ParticipatedMovies.Any(movie => movie.Id.Equals(movieId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Actor>> GetAllInShowAsync(long? showId)
    {
        return await Query
        .Where(a => a.ParticipatedShows.Any(show => show.Id.Equals(showId)))
        .ToListAsync();
    }
}
