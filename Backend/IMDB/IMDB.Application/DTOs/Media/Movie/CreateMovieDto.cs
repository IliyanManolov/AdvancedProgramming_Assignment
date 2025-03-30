using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Media.Movie;

public class CreateMovieDto
{
    [Required]
    public ISet<string> Genres{ get; set; }
    public ISet<long> DirectorIds { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public byte[]? PosterImage { get; set; }

    [Required]
    public long Length { get; set; }

    public long CreatedByUserId { get; set; }
}
