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
        return await Query
            .FirstOrDefaultAsync(e => e.UserId.Equals(userId));
    }
}