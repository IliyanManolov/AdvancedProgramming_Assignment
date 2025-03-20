using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class Actor : Person
{
    public ISet<TvShow> ParticipatedShows { get; set; } = new HashSet<TvShow>();
    public ISet<Movie> ParticipatedMovies { get; set; } = new HashSet<Movie>();

    public User CreatedByUser { get; set; }
    public long? CreatedByUserId { get; set; }
}
