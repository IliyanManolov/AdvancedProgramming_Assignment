using IMDB.Application.DTOs.Media;

namespace IMDB.Application.DTOs.Watchlist;

public class WatchlistDetailsDto
{
    public long? Id { get; set; }
    public ISet<MediaShortDto> Media { get; set; }
}
