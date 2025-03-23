using IMDB.Application.DTOs.Director;

namespace IMDB.Application.Abstractions.Services;

interface IDirectorService
{
    public Task<(long? Id, string Error)> CreateAsync(CreateDirectorDto dto);
}
