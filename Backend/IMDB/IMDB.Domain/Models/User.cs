using IMDB.Domain.AbstractModels;
using IMDB.Domain.Enums;

namespace IMDB.Domain.Models;

public class User : DomainEntity
{
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Role Role { get; set; }
    public bool IsDeleted { get; set; }

    public WatchList WatchList { get; set; }
    public long? WatchListId { get; set; }

    public ISet<TvShow> CreatedShows { get; set; } = new HashSet<TvShow>();
    public ISet<Movie> CreatedMovies { get; set; } = new HashSet<Movie>();
    public ISet<Actor> CreatedActors { get; set; } = new HashSet<Actor>();
    public ISet<Director> CreatedDirectors { get; set; } = new HashSet<Director>();
    public ISet<ShowEpisode> CreatedEpisodes { get; set; } = new HashSet<ShowEpisode>();
    public ISet<Genre> CreatedGenres { get; set; } = new HashSet<Genre>();
    public ISet<Review> Reviews { get; set; } = new HashSet<Review>();
}
