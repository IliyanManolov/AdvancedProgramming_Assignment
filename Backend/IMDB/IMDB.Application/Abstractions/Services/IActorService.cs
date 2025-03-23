using IMDB.Application.DTOs.Actor;

namespace IMDB.Application.Abstractions.Services;

interface IActorService
{
    public Task<(long? Id, string Error)> CreateAsync(CreateActorDto dto);
}
