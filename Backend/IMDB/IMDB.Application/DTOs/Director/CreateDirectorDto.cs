using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.Director;

public class CreateDirectorDto
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

    public long? CreatedByUserId { get; set; }
}