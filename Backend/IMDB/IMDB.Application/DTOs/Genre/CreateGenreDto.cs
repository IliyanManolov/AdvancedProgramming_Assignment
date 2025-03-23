using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Genre;

public class CreateGenreDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public long? CreatedByUserId { get; set; }
}