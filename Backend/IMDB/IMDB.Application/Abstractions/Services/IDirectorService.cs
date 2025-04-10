using IMDB.Application.DTOs.Director;
using IMDB.Application.DTOs.Media;

namespace IMDB.Application.Abstractions.Services;

public interface IDirectorService
{
    public Task<(long? Id, string? Error)> CreateAsync(CreateDirectorDto dto);
    public Task<IEnumerable<DirectorShortDto>> GetAllAsync();
}
