using IMDB.Application.DTOs.Media;
using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Review;

public class ReviewsRequestDto
{
    [Required]
    public MediaType MediaType { get; set; }
    [Required]
    public long MediaId { get; set; }

}