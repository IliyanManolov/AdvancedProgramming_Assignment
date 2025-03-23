using IMDB.Application.DTOs.User;
using IMDB.Domain.Enums;

namespace IMDB.Application.Abstractions.Services;

interface IUserService
{
    public Task<(long? Id, string Error)> CreateUserAsync(CreateUserDto user, Role role = Role.User);
    public Task<(UserDetailsDto? User, string Error)> GetUserDetailsAsync(long? userId);
    public Task<(UserShortDto? User, string Error)> GetUserBasicsAsync(long? userId);
}
