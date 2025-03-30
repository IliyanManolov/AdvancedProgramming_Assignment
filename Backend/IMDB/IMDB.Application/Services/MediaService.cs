using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

public class MediaService : IMediaService
{
    private readonly ILogger<MediaService> _logger;
    private readonly IMediaTransformer _mediaTransformer;
    private readonly IMovieRepository _movieRepository;
    private readonly ITvShowRepository _showRepository;
    private readonly IDirectorRepository _directorRepository;
    private readonly IUserRepository _userRepository;

    public MediaService(IMediaTransformer mediaTransformer,
        IMovieRepository movieRepository,
        ITvShowRepository showRepository,
        IDirectorRepository directorRepository,
        IUserRepository userRepository,
        ILoggerFactory loggerFactory)
    {
        _mediaTransformer = mediaTransformer;
        _movieRepository = movieRepository;
        _showRepository = showRepository;
        _directorRepository = directorRepository;
        _userRepository = userRepository;
        _logger = loggerFactory.CreateLogger<MediaService>();
    }

    public async Task<(long? Id, string? Error)> CreateMovieAsync(CreateMovieDto dto)
    {

        var existingMovie = await _movieRepository.GetByNameAsync(dto.Title);

        if (existingMovie is not null)
            return (null, $"Movie with title '{dto.Title}' already exists");

        var dbUser = await ValidateCreatedByUser(dto.CreatedByUserId);

        if (dbUser is null)
            return (null, "UNAUTHORIZED");

        var (directors, directorErrors) = await ValidateDirectorIds(dto.DirectorIds);

        if (directorErrors is not null && directorErrors.Any())
            return (null, "Directors not found");

        var dbMovie = new Movie()
        {
            CreatedByUserId = dbUser.Id,
            Title = dto.Title,
            CreateTimeStamp = DateTime.UtcNow,
            Description = dto.Description,
            Length = dto.Length,
            PosterImage = dto.PosterImage,
            Rating = 0,
            Reviews = 0,
            ReleaseDate = dto.ReleaseDate,
            Directors = directors!,
            Genres = string.Join(';', dto.Genres)
        };

        await _movieRepository.CreateAsync(dbMovie);

        return (dbMovie.Id, null);
    }

    public async Task<(long? Id, string? Error)> CreateTvShowAsync(CreateTvShowDto dto)
    {
        var existingMovie = await _showRepository.GetByNameAsync(dto.Title);

        if (existingMovie is not null)
            return (null, $"Show with title '{dto.Title}' already exists");

        var dbUser = await ValidateCreatedByUser(dto.CreatedByUserId);

        if (dbUser is null)
            return (null, "UNAUTHORIZED");

        var (directors, directorErrors) = await ValidateDirectorIds(dto.DirectorIds);

        if (directorErrors is not null && directorErrors.Any())
            return (null, "Directors not found");

        if (dto.SeasonsCount is null || dto.SeasonsCount <= 0)
            return (null, "Invalid seasons count");

        if (dto.ShowEndDate is null)
            _logger.LogDebug("Create show received with NULL end date");
        else
            _logger.LogDebug("Create show received with end date");


        var dbShow = new TvShow()
        {
            CreatedByUserId = dbUser.Id,
            Title = dto.Title,
            CreateTimeStamp = DateTime.UtcNow,
            Description = dto.Description,
            PosterImage = dto.PosterImage,
            Rating = 0,
            Reviews = 0,
            ReleaseDate = dto.ReleaseDate,
            Directors = directors!,
            Genres = string.Join(';', dto.Genres),
            EndDate = dto.ShowEndDate,
            Seasons = dto.SeasonsCount
        };

        await _showRepository.CreateAsync(dbShow);

        return (dbShow.Id, null);
    }

    public async Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetAllDiscoveryAsync()
    {
        var shows = await _showRepository.GetAllAsync();
        var movies = await _movieRepository.GetAllAsync();

        var result = new List<MediaShortDto>();

        result.AddRange(_mediaTransformer.ToShortDto(shows));
        result.AddRange(_mediaTransformer.ToShortDto(movies));

        return (result, null);
    }

    public async Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetAllWithGenres(string[] genreNames)
    {
        var result = new List<MediaShortDto>();

        var movies = new List<Movie>();
        var shows = new List<TvShow>();

        foreach (var genre in genreNames)
        {
            movies.AddRange(await _movieRepository.GetAllByGenreNameAsync(genre));
            shows.AddRange(await _showRepository.GetAllByGenreNameAsync(genre));
        }

        result.AddRange(_mediaTransformer.ToShortDto(shows));
        result.AddRange(_mediaTransformer.ToShortDto(movies));

        return (result, null);
    }


    private async Task<User?> ValidateCreatedByUser(long userId)
    {
        var dbUser = await _userRepository.GetByIdAsync(userId);

        if (dbUser is null || (dbUser.Role is not Role.Moderator && dbUser.Role is not Role.Administrator))
        {
            _logger.LogDebug("User does not exist '{exists}' or has insufficient role '{role}'",
                dbUser is not null,
                dbUser?.Role);
            return null;
        }

        return dbUser;
    }
    private async Task<(ISet<Director>? directors, IList<string>? ValidationErrors)> ValidateDirectorIds(ISet<long> IDs)
    {

        var errors = new List<string>();
        var directors = new HashSet<Director>();

        foreach (var id in IDs)
        {
            var director = await _directorRepository.GetByIdAsync(id);
            if (director is null)
            {
                _logger.LogDebug("Director with id '{id}' not found", id);
                errors.Add("Director not found");
            }
            else
                directors.Add(director);
        }

        return (directors, errors);
    }
}