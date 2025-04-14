using IMDB.Application.DTOs.Watchlist;

namespace IMDB.Application.Abstractions.Services;

public interface IWatchListService
{
    public Task<(WatchlistDetailsDto? Watchlist, string? Error)> GetListDetailsAsync(long? userId);
    public Task<(WatchlistShortDto? Watchlist, string? Error)> GetListBasicAsync(long? userId);
    public Task<(bool Success, string? Error)> DeleteElement(WatchlistModificationDto model, long userId);
    public Task<(bool Success, string? Error)> AddElement(WatchlistModificationDto model, long userId);
}