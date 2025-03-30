using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Media.TvShow;

public class CreateTvShowDto
{
    [Required]
    public ISet<string> Genres { get; set; }
    public ISet<long> DirectorIds { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public int? SeasonsCount { get; set; }
    public DateTime? ShowEndDate { get; set; }

    public byte[]? PosterImage { get; set; }
    public long CreatedByUserId { get; set; }
}