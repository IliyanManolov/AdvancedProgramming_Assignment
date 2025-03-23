using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Media.Movie;

public class CreateMovieDto
{
    [Required]
    public ISet<long> GenreIds{ get; set; }
    public ISet<long> DirectorIds { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public byte[]? PosterImage { get; set; }

    /// <summary>
    /// Property to allow frontend to distinguish between Movie, TvShow, etc.
    /// </summary>
    public MediaType Type { get; set; }

    [Required]
    public long Length { get; set; }

    public int? ShowSeasonsCount { get; set; }
    public long? ShowEpisodesCount { get; set; }
    public DateTime? ShowEndDate { get; set; }
}
