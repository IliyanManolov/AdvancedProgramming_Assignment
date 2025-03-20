using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class Director : Person
{
    public ISet<TvShow> DirectedShows { get; set; } = new HashSet<TvShow>();
    public ISet<Movie> DirectedMovies { get; set; } = new HashSet<Movie>();

    public User CreatedByUser { get; set; }
    public long? CreatedByUserId { get; set; }
}
