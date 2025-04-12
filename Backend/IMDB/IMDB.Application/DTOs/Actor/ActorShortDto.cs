namespace IMDB.Application.DTOs.Actor;

public class ActorShortDto
{
    public long? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get => $"{FirstName} {LastName}"; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DateOfDeath { get; set; }
}
