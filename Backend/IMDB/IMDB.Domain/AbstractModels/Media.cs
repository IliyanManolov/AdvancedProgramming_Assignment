using IMDB.Domain.Models;

namespace IMDB.Domain.AbstractModels;

/// <summary>
/// Abstraction on TV Shows, Movies, etc.
/// </summary>
public abstract class Media : DomainEntity
{
    public DateTime ReleaseDate { get; set; }
    public string Genres { get; set; } = string.Empty;
    public string Title { get; set; }
    public double? Rating { get; set; }
    public long? Reviews { get; set; }
    public string? Description { get; set; }
    public byte[]? PosterImage { get; set; }

    public ISet<Director> Directors { get; set; } = new HashSet<Director>();

    public User CreatedByUser { get; set; }
    public long? CreatedByUserId { get; set; }

    public ISet<Actor> Actors { get; set; } = new HashSet<Actor>();
}
