using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class TvShow : Media
{
    public int? Seasons { get; set; }

    public ISet<ShowEpisode> Episodes { get; set; } = new HashSet<ShowEpisode>();

    /// <summary>
    /// Null in case the show is ongoing
    /// </summary>
    public DateTime? EndDate { get; set; }
}
