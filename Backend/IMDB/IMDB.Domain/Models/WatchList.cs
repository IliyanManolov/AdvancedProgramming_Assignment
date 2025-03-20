using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class WatchList : DomainEntity
{
    public ISet<TvShow> Shows { get; set; } = new HashSet<TvShow>();
    public ISet<Movie> Movies { get; set; } = new HashSet<Movie>();
    public User User { get; set; }
    public long? UserId { get; set; }
}
