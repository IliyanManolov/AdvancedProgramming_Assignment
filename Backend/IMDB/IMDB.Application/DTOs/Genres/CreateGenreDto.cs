using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Genres;

public class CreateGenreDto
{
    [Required]
    public string Name { get; set; }
    public long? CreatedByUserId { get; set; }
}
