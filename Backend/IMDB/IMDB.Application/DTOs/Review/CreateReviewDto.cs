using IMDB.Application.DTOs.Media;
using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Review;

public class CreateReviewDto
{
    public long UserId { get; set; }
    [Required]
    public MediaType MediaType { get; set; }
    [Required]
    public long MediaId { get; set; }
    [Required]
    public double Rating { get; set; }
    public string Review { get; set; }
}
