using IMDB.Application.DTOs.Director;

namespace IMDB.Application.Abstractions.Services;

public interface IDirectorService
{
    public Task<(long? Id, string Error)> CreateAsync(CreateDirectorDto dto);
}
