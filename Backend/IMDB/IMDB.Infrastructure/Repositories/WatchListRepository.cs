using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class WatchListRepository : BaseRepository<WatchList>, IWatchListRepository
{
    public WatchListRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<WatchList?> GetByUserIdAsync(long? userId)
    {
        // Nested N:N relationships are *very* fun
        return await Query
            .Include(x => x.Movies)
                .ThenInclude(e => e.Director)
            .Include(x => x.Movies)
                .ThenInclude(e => e.Genres)
            .Include(x => x.Shows)
                .ThenInclude(e => e.Director)
            .Include(x => x.Shows)
                .ThenInclude(e => e.Genres)
            .Include(x => x.User)
            .FirstOrDefaultAsync(e => e.UserId.Equals(userId));
    }
}