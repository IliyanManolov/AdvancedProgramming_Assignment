using IMDB.Application.DTOs.Actor;
using IMDB.Application.DTOs.Media;

namespace IMDB.Application.Abstractions.Services;

public interface IActorService
{
    public Task<(long? Id, string? Error)> CreateAsync(CreateActorDto dto);
    public Task<IEnumerable<ActorShortDto>> GetAllAsync();
}
