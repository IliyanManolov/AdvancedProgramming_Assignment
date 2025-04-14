using IMDB.Application.DTOs.Media;
using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Watchlist;

public class WatchlistModificationDto
{
    [Required]
    public long Id { get; set; }
    [Required]
    public MediaType Type { get; set; }
}