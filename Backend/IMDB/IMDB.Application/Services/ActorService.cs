using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Actor;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

public class ActorService : IActorService
{
    private readonly IUserRepository _userRepository;
    private readonly IActorRepository _actorRepository;
    private readonly ILogger<ActorService> _logger;

    public ActorService(IActorRepository actorRepository, IUserRepository userRepository, ILoggerFactory loggerFactory)
    {
        _userRepository = userRepository;
        _actorRepository = actorRepository;
        _logger = loggerFactory.CreateLogger<ActorService>();
    }

    public async Task<(long? Id, string? Error)> CreateAsync(CreateActorDto dto)
    {
        var dbUser = await _userRepository.GetByIdAsync(dto.CreatedByUserId);

        if (dbUser is null || (dbUser.Role is not Role.Moderator && dbUser.Role is not Role.Administrator) || dbUser.IsDeleted == true)
        {
            return (null, "UNAUTHORIZED");
        }

        var dbActor = await _actorRepository.GetByNameAsync(dto.FirstName, dto.LastName);

        if (dbActor is not null)
            return (null, $"Actor {dto.FirstName} {dto.LastName} already exists");

        if (dto.BirthDate is null || dto.BirthDate > DateTime.UtcNow)
            return (null, "Invalid birth date");

        var newActor = new Actor()
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

        await _actorRepository.CreateAsync(newActor);
        return (newActor.Id, null);
    }

    public async Task<IEnumerable<ActorShortDto>> GetAllAsync()
    {
        var dbActors = await _actorRepository.GetAllAsync();

        return dbActors.Select(x => new ActorShortDto()
        {
            FirstName = x.FirstName,
            Id = x.Id,
            LastName = x.LastName,
            BirthDate = x.BirthDate,
            DateOfDeath = x.DateOfDeath,
        });
    }
}
