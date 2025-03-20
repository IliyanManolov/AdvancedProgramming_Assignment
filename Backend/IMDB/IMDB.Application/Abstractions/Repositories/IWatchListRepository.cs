using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface IWatchListRepository : IBaseRepository<WatchList>
{
    public Task<WatchList> GetByUserIdAsync(long? userId);
}
