using IMDB.Domain.Models;

namespace IMDB.Domain.AbstractModels;

/// <summary>
/// Abstraction on TV Shows, Movies, etc.
/// </summary>
public abstract class Media : DomainEntity
{
    public DateTime ReleaseDate { get; set; }
    public ISet<Genre> Genres { get; set; } = new HashSet<Genre>();
    public string Title { get; set; }
    public double? Rating { get; set; }
    public ISet<Review>? Reviews { get; set; } = new HashSet<Review>();
    public string? Description { get; set; }
    public byte[]? PosterImage { get; set; }

    public Director Director { get; set; }
    public long? DirectorId { get; set; }

    public User CreatedByUser { get; set; }
    public long? CreatedByUserId { get; set; }

    public ISet<Actor> Actors { get; set; } = new HashSet<Actor>();
}
