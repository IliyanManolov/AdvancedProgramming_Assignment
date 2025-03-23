using IMDB.Application.DTOs.User;

namespace IMDB.Application.Abstractions.Services;

public interface IAuthenticationService
{
    public Task<(bool Status, string Error)> LoginAsync(AuthenticateDto user);
}