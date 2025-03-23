using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Actor;

public class CreateActorDto
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    public string? Biography { get; set; }

    [Required]
    public DateTime? BirthDate { get; set; }
    public DateTime? DateOfDeath { get; set; }

    [Required]
    public string Nationality { get; set; }
    public byte[]? ProfileImage { get; set; }
}
