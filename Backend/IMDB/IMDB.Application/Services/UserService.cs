using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.User;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IWatchListRepository _watchlistRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IPasswordService passwordService, IWatchListRepository watchlistRepository, ILoggerFactory loggerFactory)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _watchlistRepository = watchlistRepository;
        _logger = loggerFactory.CreateLogger<UserService>();
    }

    public async Task<(UserShortDto? User, string? Error)> GetUserBasicsAsync(long? userId)
    {
        if (userId is null)
        {
            _logger.LogWarning("Received NULL user id");
            return (null, $"No user with id '{userId}' found");
        }

        var dbUser = await _userRepository.GetByIdAsync(userId);

        if (dbUser == null)
            return (null, $"No user with id '{userId}' found");

        if (dbUser.IsDeleted == true)
        {
            _logger.LogDebug("User is deleted");
            return (null, $"No user with id '{userId}' found");
        }

        var user = new UserShortDto()
        {
            Id = dbUser.Id,
            FullName = $"{dbUser.FirstName} {dbUser.LastName}",
            UserName = dbUser.Username
        };

        return (user, null);
    }

    public async Task<(UserDetailsDto? User, string? Error)> GetUserDetailsAsync(long? userId)
    {
        if (userId is null)
        {
            _logger.LogWarning("Received NULL user id");
            return (null, $"No user with id '{userId}' found");
        }

        var dbUser = await _userRepository.GetByIdAsync(userId);

        if (dbUser == null)
            return (null, $"No user with id '{userId}' found");

        if (dbUser.IsDeleted == true)
        {
            _logger.LogDebug("User is deleted");
            return (null, $"No user with id '{userId}' found");
        }

        var user = new UserDetailsDto()
        {
            Id = dbUser.Id,
            Email = dbUser.Email,
            FirstName = dbUser.FirstName,
            LastName = dbUser.LastName,
            Username = dbUser.Username,
            Role = dbUser.Role
        };

        return (user, null);
    }

    public async Task<(long? Id, string? Error)> CreateUserAsync(CreateUserDto user, Role role)
    {
        if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.ConfirmPassword))
        {
            _logger.LogDebug("Received null password '{isNullPass}' or null confirm-password '{isNullConfirm}'",
                string.IsNullOrEmpty(user.Password),
                string.IsNullOrEmpty(user.ConfirmPassword));
            return (null, "Invalid password");
        }

        if (user.Password != user.ConfirmPassword)
            return (null, "Passwords mismatch");

        var exists = await _userRepository.IsExistingUsernameAsync(user.Username);

        if (exists)
            return (null, "Username already exists");

        if (await _userRepository.IsExistingEmailAsync(user.Email))
            return (null, "Email already used");

        var dbUser = new User()
        {
            CreateTimeStamp = DateTime.UtcNow,
            Username = user.Username,
            Email = user.Email!.ToLower(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Password = _passwordService.GetHash(user.Password!),
            Role = role,

            WatchList = new WatchList()
            {
                CreateTimeStamp = DateTime.UtcNow
            }
        };

        await _userRepository.CreateAsync(dbUser);

        // workaround-ish
        var watchlist = dbUser.WatchList;
        watchlist.UserId = dbUser.Id;
        await _watchlistRepository.UpdateAsync(watchlist);

        return (dbUser.Id, null);
    }
}