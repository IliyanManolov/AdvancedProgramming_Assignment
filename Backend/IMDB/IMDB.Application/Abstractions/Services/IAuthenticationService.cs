using IMDB.Application.DTOs.User;

namespace IMDB.Application.Abstractions.Services;

public interface IAuthenticationService
{
    public Task<(UserDetailsDto? user, string? Error)> AuthenticateAsync(string username, string password);
}