using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class ShowEpisode : DomainEntity
{
    public DateTime DateAired { get; set; }
    public long? Length { get; set; }
    public string? Title { get; set; }
    public TvShow Show { get; set; }
    public long? ShowId { get; set; }
    public string? Description { get; set; }
    public int? SeasonNumber { get; set; }
    public double? Rating { get; set; }
    public long? Reviews { get; set; }


    public User CreatedByUser { get; set; }
    public long? CreatedByUserId { get; set; }
}
