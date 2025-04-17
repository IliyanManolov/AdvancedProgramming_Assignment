using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(DatabaseContext dbContext) : base(dbContext)
    {

    }

    public async Task<IEnumerable<Review>> GetAllForEpisodeAsync(long id)
    {
        return await Query
            .Include(e => e.User)
            .Where(x => x.EpisodeId.Equals(id) && x.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetAllForMovieAsync(long id)
    {

        return await Query
            .Include(e => e.User)
            .Where(x => x.MovieId.Equals(id) && x.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetAllForShowAsync(long id)
    {
        return await Query
            .Include(e => e.User)
            .Where(x => x.ShowId.Equals(id) && x.IsDeleted == false)
            .ToListAsync();
    }
}