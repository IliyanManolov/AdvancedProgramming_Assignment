using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class Director : Person
{
    // Relationships are Many to Many since multiple directors can work on the same piece of media
    public ISet<TvShow> DirectedShows { get; set; } = new HashSet<TvShow>();
    public ISet<Movie> DirectedMovies { get; set; } = new HashSet<Movie>();

    public User CreatedByUser { get; set; }
    public long? CreatedByUserId { get; set; }
}
