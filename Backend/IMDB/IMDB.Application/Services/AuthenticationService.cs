using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.User;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<AuthenticationService> _logger;
    private const string _errorMessage = "Invalid username or password";

    public AuthenticationService(IUserRepository userRepository, IPasswordService passwordService, ILoggerFactory loggerFactory)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _logger = loggerFactory.CreateLogger<AuthenticationService>();
    }

    public async Task<(UserDetailsDto? user, string? Error)> AuthenticateAsync(string username, string password)
    {

        if (string.IsNullOrWhiteSpace(username))
        {
            _logger.LogDebug("Invalid username");
            return Invalid();
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            _logger.LogDebug("Invalid password");
            return Invalid();
        }

        var dbUser = await _userRepository.GetByUsernameAsync(username);

        if (dbUser == null)
        {
            _logger.LogDebug("User with username '{username}' does not exist", username);
            return Invalid();
        }

        if (dbUser.IsDeleted == true)
        {
            _logger.LogDebug("User with username '{username}' is deleted", username);
            return Invalid();
        }

        var passwordHesh = _passwordService.GetHash(password);

        if (passwordHesh != dbUser.Password)
        {
            _logger.LogDebug("Invalid user password");
            return Invalid();
        }

        var userDto = new UserDetailsDto()
        {
            Id = dbUser.Id,
            Email = dbUser.Email,
            FirstName = dbUser.FirstName,
            LastName = dbUser.LastName,
            Username = dbUser.Username,
            Role = dbUser.Role
        };

        return (userDto, null);
    }

    private static (UserDetailsDto? user, string? Error) Invalid()
        => (null, _errorMessage);
}