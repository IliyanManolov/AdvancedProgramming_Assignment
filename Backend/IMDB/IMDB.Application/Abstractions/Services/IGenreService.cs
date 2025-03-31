using IMDB.Application.DTOs.Genres;

namespace IMDB.Application.Abstractions.Services;

public interface IGenreService
{
    public Task<(long? Id, string? Error)> CreateAsync(CreateGenreDto dto);
    public Task<(GenreDetailsDto? Genre, string? Error)> GetByNameAsync(string? Name);
    public Task<(GenreDetailsDto? Genre, string? Error)> GetByIdAsync(long? Id);
    public Task<(IEnumerable<GenreDetailsDto> GenresList, string? Error)> GetAllAsync();
}