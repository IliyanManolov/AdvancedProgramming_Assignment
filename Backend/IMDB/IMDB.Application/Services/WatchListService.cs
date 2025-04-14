using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Watchlist;
using IMDB.Domain.AbstractModels;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;


public class WatchListService : IWatchListService
{
    private readonly ILogger<WatchListService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IWatchListRepository _watchlistRepository;
    private readonly IMediaTransformer _mediaTransformer;
    private readonly IMovieRepository _movieRepository;
    private readonly ITvShowRepository _showRepository;

    public WatchListService(IUserRepository userRepository,
        IWatchListRepository watchlistRepository,
        IMediaTransformer mediaTransformer,
        ILoggerFactory loggerFactory,
        IMovieRepository movieRepository,
        ITvShowRepository showRepository)
    {
        _logger = loggerFactory.CreateLogger<WatchListService>();
        _userRepository = userRepository;
        _watchlistRepository = watchlistRepository;
        _mediaTransformer = mediaTransformer;
        _movieRepository = movieRepository;
        _showRepository = showRepository;
    }

    public async Task<(bool Success, string? Error)> AddElement(WatchlistModificationDto model, long userId)
    {
        var dbWatchlist = await _watchlistRepository.GetByUserIdAsync(userId);

        if (dbWatchlist is null)
        {
            _logger.LogError("User with id '{userId}' is found but no watchlist is found by repository for it.", userId);
            throw new ArgumentNullException("User must have related watchlist");
        }

        switch (model.Type)
        {
            case MediaType.Movie:

                if (dbWatchlist.Movies.Any(x => x.Id.Equals(model.Id)))
                    return (false, "Movie already exists in watchlist");
                else
                {
                    var dbMovie = await _movieRepository.GetByIdAsync(model.Id);

                    if (dbMovie is null)
                    {
                        return (false, "Movie not found");
                    }
                    else
                    {
                        dbWatchlist.Movies.Add(dbMovie);
                        await _watchlistRepository.UpdateAsync(dbWatchlist);
                    }
                }
                break;

            case MediaType.TvShow:
                if (dbWatchlist.Shows.Any(x => x.Id.Equals(model.Id)))
                    return (false, "Show already exists in watchlist");
                else
                {
                    var dbShow = await _showRepository.GetByIdAsync(model.Id);

                    if (dbShow is null)
                    {
                        return (false, "Movie not found");
                    }
                    else
                    {
                        dbWatchlist.Shows.Add(dbShow);
                        await _watchlistRepository.UpdateAsync(dbWatchlist);
                    }
                }
                break;

            default:
                throw new ArgumentException("Cannot determine media type - {mediaType}", model.Type.ToString());
        }

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteElement(WatchlistModificationDto model, long userId)
    {
        var dbWatchlist = await _watchlistRepository.GetByUserIdAsync(userId);

        if (dbWatchlist is null)
        {
            _logger.LogError("User with id '{userId}' is found but no watchlist is found by repository for it.", userId);
            throw new ArgumentNullException("User must have related watchlist");
        }

        switch (model.Type)
        {
            case MediaType.Movie:

                var movie = dbWatchlist.Movies.FirstOrDefault(x => x.Id.Equals(model.Id));
                if (movie is null)
                {
                    return (false, "Movie not found");
                }
                else
                {
                    dbWatchlist.Movies.Remove(movie);
                    await _watchlistRepository.UpdateAsync(dbWatchlist);   
                }
                break;

            case MediaType.TvShow:

                var show = dbWatchlist.Shows.FirstOrDefault(x => x.Id.Equals(model.Id));
                if (show is null)
                {
                    return (false, "Show not found");
                }
                else
                {
                    dbWatchlist.Shows.Remove(show);
                    await _watchlistRepository.UpdateAsync(dbWatchlist);
                }
                break;

            default:
                throw new ArgumentException("Cannot determine media type - {mediaType}", model.Type.ToString());
        }

        return (true, null);
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
