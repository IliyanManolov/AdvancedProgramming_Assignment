namespace IMDB.Application.DTOs.Review;

public class ReviewDetailsDto
{
    public string UserName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastEditDate { get; set; }
    public string ReviewText { get; set; }
    public double Rating { get; set; }
}