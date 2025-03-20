using IMDB.Application.DTOs.User;
using IMDB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Application.Abstractions.Services;

interface IUserService
{
    public Task<(long? Id, string Error)> CreateUserAsync(CreateUserDto user, Role role = Role.User);
    public Task<(UserDetailsDto? user, string Error)> GetUserAsync(long? userId);
}
