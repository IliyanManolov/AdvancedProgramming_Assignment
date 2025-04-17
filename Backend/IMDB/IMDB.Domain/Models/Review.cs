using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class Review : DomainEntity
{
    public bool IsDeleted { get; set; }
    public long? MovieId { get; set; }
    public Movie Movie { get; set; }
    public long? ShowId { get; set; }
    public TvShow Show { get; set; }
    public double Rating { get; set; }
    public long? UserId { get; set; }
    public User User { get; set; }
    public string ReviewText { get; set; }
    public ShowEpisode Episode { get; set; }
    public long? EpisodeId { get; set; }
}