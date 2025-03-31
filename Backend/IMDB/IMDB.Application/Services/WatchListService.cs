using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Watchlist;
using IMDB.Domain.AbstractModels;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

// TODO: Double check if using watchlist repository is redundant here
public class WatchListService : IWatchListService
{
    private readonly ILogger<WatchListService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IWatchListRepository _watchlistRepository;
    private readonly IMediaTransformer _mediaTransformer;

    public WatchListService(IUserRepository userRepository, IWatchListRepository watchlistRepository, IMediaTransformer mediaTransformer, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<WatchListService>();
        _userRepository = userRepository;
        _watchlistRepository = watchlistRepository;
        _mediaTransformer = mediaTransformer;
    }

    public async Task<(WatchlistShortDto? Watchlist, string? Error)> GetListBasicAsync(long? userId)
    {
        if (userId is null)
            return (null, $"No user with id '{userId}' found");

        var dbUser = await _userRepository.GetByIdAsync(userId);

        if (dbUser is null)
            return (null, $"No user with id '{userId}' found");

        if (dbUser.IsDeleted == true)
        {
            _logger.LogDebug("User is deleted");
            return (null, $"No user with id '{userId}' found");
        }

        var dbWatchlist = await _watchlistRepository.GetByUserIdAsync(userId);

        if (dbWatchlist is null)
        {
            _logger.LogError("User with id '{userId}' is found but no watchlist is found by repository for it.", userId);
            throw new ArgumentNullException("User must have related watchlist");
        }

        var watchlist = new WatchlistShortDto()
        {
            MediaCount = dbWatchlist.Shows.Count + dbWatchlist.Movies.Count
        };

        return (watchlist, null);
    }

    public async Task<(WatchlistDetailsDto? Watchlist, string? Error)> GetListDetailsAsync(long? userId)
    {
        if (userId is null)
            return (null, $"No user with id '{userId}' found");

        var dbUser = await _userRepository.GetByIdAsync(userId);

        if (dbUser is null)
            return (null, $"No user with id '{userId}' found");

        var dbWatchlist = await _watchlistRepository.GetByUserIdAsync(userId);

        if (dbWatchlist is null)
        {
            _logger.LogError("User with id '{userId}' is found but no watchlist is found by repository for it.", userId);
            throw new ArgumentNullException("User must have related watchlist");
        }

        var dbMediaList = new List<Media>();
        dbMediaList.AddRange(dbWatchlist.Movies);
        dbMediaList.AddRange(dbWatchlist.Shows);

        var transformedMedia = _mediaTransformer.ToShortDto(dbMediaList);
        var watchList = new WatchlistDetailsDto()
        {
            Id = dbWatchlist.Id,
            Media = transformedMedia.ToHashSet()
        };

        return (watchList, null);
    }
}
