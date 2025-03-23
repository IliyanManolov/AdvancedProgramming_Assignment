using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Genre;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly ILogger<GenreService> _logger;

    public GenreService(IGenreRepository genreRepository, ILoggerFactory loggerFactory)
    {
        _genreRepository = genreRepository;
        _logger = loggerFactory.CreateLogger<GenreService>();
    }

    public Task<(long? Id, string? Error)> CreateAsync(CreateGenreDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<(IEnumerable<GenreDetailsDto> GenresList, string? Error)> GetAllAsync()
    {
        var dbGenres = await _genreRepository.GetAllAsync();

        var genres = dbGenres.Select(x => GetDetailsDto(x)).ToList();
        return (genres, null);
    }

    public async Task<(GenreDetailsDto? Genre, string? Error)> GetByIdAsync(long? Id)
    {
        if (Id is null)
        {
            _logger.LogWarning("Received NULL ID when getting by id");
            return (null, $"Genre with id '{Id}' not found");
        }

        var dbGenre = await _genreRepository.GetByIdAsync(Id);

        if (dbGenre is null)
            return (null, $"Genre with id '{Id}' not found");

        return (GetDetailsDto(dbGenre), null);
    }

    public async Task<(GenreDetailsDto? Genre, string? Error)> GetByNameAsync(string? Name)
    {
        if (string.IsNullOrEmpty(Name))
        {
            _logger.LogWarning("Received NULL Name when getting by Name");
            return (null, $"Genre with name '{Name}' not found");
        }

        var dbGenre = await _genreRepository.GetByGenreNameAsync(Name);

        if (dbGenre is null)
            return (null, $"Genre with name '{Name}' not found");

        return (GetDetailsDto(dbGenre), null);
    }

    private static GenreDetailsDto GetDetailsDto(Genre dbModel)
        => new GenreDetailsDto()
        {
            Id = dbModel.Id,
            Name = dbModel.Name
        };
}
