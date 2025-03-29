using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class DirectorRepository : BaseRepository<Director>, IDirectorRepository
{
    public DirectorRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Director>> GetAllCreatedByUserAsync(long? userId)
    {
        return await Query
            .Where(a => a.CreatedByUserId.Value.Equals(userId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Director>> GetAllInMovieAsync(long? movieId)
    {
        return await Query
            .Where(a => a.DirectedMovies.Any(movie => movie.Id.Equals(movieId)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Director>> GetAllInShowAsync(long? showId)
    {
        return await Query
            .Where(a => a.DirectedShows.Any(show => show.Id.Equals(showId)))
            .ToListAsync();
    }

    public async Task<Director?> GetByNameAsync(string firstName, string lastName)
    {
        return await Query
            .FirstOrDefaultAsync(x => x.FirstName.Equals(firstName) && x.LastName.Equals(lastName));
    }
}
