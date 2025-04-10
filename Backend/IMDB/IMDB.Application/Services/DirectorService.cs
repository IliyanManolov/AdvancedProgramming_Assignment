using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Director;
using IMDB.Application.DTOs.Media;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

public class DirectorService : IDirectorService
{
    private readonly IUserRepository _userRepository;
    private readonly IDirectorRepository _directorRepository;
    private readonly ILogger<ActorService> _logger;

    public DirectorService(IDirectorRepository directorRepository, IUserRepository userRepository, ILoggerFactory loggerFactory)
    {
        _userRepository = userRepository;
        _directorRepository = directorRepository;
        _logger = loggerFactory.CreateLogger<ActorService>();
    }

    public async Task<(long? Id, string? Error)> CreateAsync(CreateDirectorDto dto)
    {
        var dbUser = await _userRepository.GetByIdAsync(dto.CreatedByUserId);

        if (dbUser is null || (dbUser.Role is not Role.Moderator && dbUser.Role is not Role.Administrator) || dbUser.IsDeleted == true)
        {
            return (null, "UNAUTHORIZED");
        }

        var dbActor = await _directorRepository.GetByNameAsync(dto.FirstName, dto.LastName);

        if (dbActor is not null)
            return (null, $"Director '{dto.FirstName} {dto.LastName}' already exists");

        var newActor = new Director()
        {
            Biography = dto.Biography,
            BirthDate = dto.BirthDate,
            CreatedByUserId = dto.CreatedByUserId,
            DateOfDeath = dto.DateOfDeath,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Nationality = dto.Nationality,
            ProfileImage = dto.ProfileImage,
            CreateTimeStamp = DateTime.UtcNow
        };

        await _directorRepository.CreateAsync(newActor);
        return (newActor.Id, null);
    }

    public async Task<IEnumerable<DirectorShortDto>> GetAllAsync()
    {
        var dbDirectors = await _directorRepository.GetAllAsync();

        return dbDirectors.Select(d => new DirectorShortDto()
        {
            FirstName = d.FirstName,
            Id = d.Id,
            LastName = d.LastName,
            BirthDate = d.BirthDate,
            DateOfDeath = d.DateOfDeath,
        });
    }
}