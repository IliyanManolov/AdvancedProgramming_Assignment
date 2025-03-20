namespace IMDB.Domain.AbstractModels;

public abstract class Person : DomainEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Null in case the person is still alive
    /// </summary>
    public DateTime? DateOfDeath { get; set; }

    public string Nationality { get; set; }
    public byte[]? ProfileImage { get; set; }
}
