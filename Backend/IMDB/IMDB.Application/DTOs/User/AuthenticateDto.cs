using System.ComponentModel.DataAnnotations;

namespace IMDB.Application.DTOs.User;

public class AuthenticateDto
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
}
