namespace IMDB.Application.DTOs.Media;

public class DirectorShortDto
{
    public long? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get => $"{FirstName} {LastName}"; }
}
