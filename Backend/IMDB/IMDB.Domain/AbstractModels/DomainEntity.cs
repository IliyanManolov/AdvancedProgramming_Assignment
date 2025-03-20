namespace IMDB.Domain.AbstractModels;

public abstract class DomainEntity
{
    public long? Id { get; set; }
    public DateTime? CreateTimeStamp { get; set; }
    public DateTime? UpdateTimeStamp { get; set; }
}
