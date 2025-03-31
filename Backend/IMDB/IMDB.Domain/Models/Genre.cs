using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class Genre : DomainEntity
{
    public string Name { get; set; }

    public User CreatedByUser { get; set; }
    public long? CreatedByUserId { get; set; }
}