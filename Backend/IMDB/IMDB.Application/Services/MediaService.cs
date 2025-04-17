using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;
using IMDB.Application.DTOs.Review;
using IMDB.Application.DTOs.ShowEpisode;
using IMDB.Domain.AbstractModels;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace IMDB.Application.Services;

public class MediaService : IMediaService
{
    private readonly ILogger<MediaService> _logger;
    private readonly IMediaTransformer _mediaTransformer;
    private readonly IMovieRepository _movieRepository;
    private readonly ITvShowRepository _showRepository;
    private readonly IDirectorRepository _directorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IReviewRepository _reviewRepository;

    public MediaService(IMediaTransformer mediaTransformer,
        IMovieRepository movieRepository,
        ITvShowRepository showRepository,
        IDirectorRepository directorRepository,
        IUserRepository userRepository,
        IActorRepository actorRepository,
        IGenreRepository genreRepository,
        IEpisodeRepository episodeRepository,
        IReviewRepository reviewRepository,
        ILoggerFactory loggerFactory)
    {
        _mediaTransformer = mediaTransformer;
        _movieRepository = movieRepository;
        _showRepository = showRepository;
        _directorRepository = directorRepository;
        _userRepository = userRepository;
        _actorRepository = actorRepository;
        _genreRepository = genreRepository;
        _episodeRepository = episodeRepository;
        _reviewRepository = reviewRepository;
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

        var (directors, directorErrors) = await ValidateDirectorIds(new HashSet<long>() { dto.DirectorId });

        if (directorErrors is not null && directorErrors.Any())
            return (null, "Directors not found");

        var (genres, genreErrors) = await ValidateGenreIds(dto.GenreIds);

        if (genreErrors is not null && genreErrors.Any())
            return (null, "Genres not found");

        var (actors, actorsErrors) = await ValidateActorIds(dto.ActorIds);

        if (actorsErrors is not null && actorsErrors.Any())
            return (null, "Genres not found");

        var dbMovie = new Movie()
        {
            CreatedByUserId = dbUser.Id,
            Title = dto.Title,
            CreateTimeStamp = DateTime.UtcNow,
            Description = dto.Description,
            Length = dto.Length,
            PosterImage = dto.PosterImage,
            Rating = 0,
            ReleaseDate = dto.ReleaseDate,
            Director = directors!.First()!,
            DirectorId = directors.First().Id,
            Genres = genres!,
            Actors = actors
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

        var (directors, directorErrors) = await ValidateDirectorIds(new HashSet<long>() { dto.DirectorId });

        if (directorErrors is not null && directorErrors.Any())
            return (null, "Directors not found");

        var (genres, genreErrors) = await ValidateGenreIds(dto.GenreIds);

        if (genreErrors is not null && genreErrors.Any())
            return (null, "Genres not found");

        var (actors, actorsErrors) = await ValidateActorIds(dto.ActorIds);

        if (actorsErrors is not null && actorsErrors.Any())
            return (null, "Genres not found");

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
            ReleaseDate = dto.ReleaseDate,
            Director = directors.First()!,
            DirectorId = directors.First().Id,
            Genres = genres!,
            EndDate = dto.ShowEndDate,
            Seasons = dto.SeasonsCount,
            Actors = actors
        };

        await _showRepository.CreateAsync(dbShow);

        return (dbShow.Id, null);
    }

    public async Task<(long? Id, string? Error)> CreateReview(CreateReviewDto dto)
    {

        if (dto.Rating > 10 || dto.Rating < 0)
        {
            _logger.LogError("Invalid value for review detected - '{reviewRating}'", dto.Rating);
            return (null, "Invalid values for rating");
        }

        return dto.MediaType switch
        {
            MediaType.Movie => await CreateMovieReviewAsync(dto),
            MediaType.TvShow => await CreateShowReviewAsync(dto),
            MediaType.Episode => await CreateEpisodeReviewAsync(dto),
            _ => throw new ArgumentException($"Unable to determine review media type. {dto.MediaType}"),
        };
    }

    public async Task<(IEnumerable<ReviewDetailsDto>? Reviews, string? Error)> GetReviewsForMediaAsync(ReviewsRequestDto model)
    {
        // Use review repository directly since we already know the media type & id and do not need to fetch *everything* from the DB
        return model.MediaType switch
        {
            MediaType.Movie => (_mediaTransformer.ToDetails(await _reviewRepository.GetAllForMovieAsync(model.MediaId)), null),
            MediaType.TvShow => (_mediaTransformer.ToDetails(await _reviewRepository.GetAllForShowAsync(model.MediaId)), null),
            MediaType.Episode => (_mediaTransformer.ToDetails(await _reviewRepository.GetAllForEpisodeAsync(model.MediaId)), null),
            _ => throw new ArgumentException($"Unable to determine review media type. {model.MediaType}"),
        };
    }

    public async Task<(long? Id, string? Error)> CreateEpisodeAsync(CreateEpisodeDto dto)
    {
        var show = await _showRepository.GetByIdAsync(dto.ShowId);

        if (show is null)
            return (null, $"Show with id '{dto.ShowId}' does not exist");

        var dbUser = await ValidateCreatedByUser(dto.CreatedByUserId);

        if (dbUser is null)
            return (null, "UNAUTHORIZED");

        if (show.Episodes.Any(x => x.Title == dto.Title && x.SeasonNumber == dto.SeasonNumber))
            return (null, "Episode with this title and season already exists. Use another title");

        var dbEpisode = new ShowEpisode()
        {
            CreateTimeStamp = DateTime.UtcNow,
            CreatedByUserId = dbUser.Id,
            DateAired = dto.DateAired,
            Description = dto.Description,
            Length = dto.Length,
            Rating = 0,
            ShowId = dto.ShowId,
            Show = show!,
            SeasonNumber = dto.SeasonNumber,
            Title = dto.Title,
        };

        if (dbEpisode.SeasonNumber > show.Seasons)
        {
            show.Seasons = dbEpisode.SeasonNumber;
            show.UpdateTimeStamp = DateTime.UtcNow;
            await _showRepository.UpdateAsync(show);
        }
        await _episodeRepository.CreateAsync(dbEpisode);

        return (dbEpisode.Id, null);
    }

    public async Task<(IEnumerable<EpisodeDetailsDto>? episodesList, string? Error)> GetShowEpisodes(long? showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show is null)
            return (null, $"Show with id '{showId}' does not exist");

        var episodes = _mediaTransformer.ToDetails(show.Episodes);
        return (episodes, null);
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

    public async Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetTopTenAsync()
    {
        var shows = await _showRepository.GetFiveLatestAsync();
        var movies = await _movieRepository.GetFiveLatestAsync();

        var result = new List<MediaShortDto>();

        result.AddRange(_mediaTransformer.ToShortDto(shows));
        result.AddRange(_mediaTransformer.ToShortDto(movies));

        return (result, null);
    }

    public async Task<(MediaShortDto? Media, string? Error)> GetMovieByIdAsync(long? id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie is null)
            return (null, "Movie does not exist");

        return (_mediaTransformer.ToShortDto(new List<Media>() { movie }).First(), null);
    }

    public async Task<(MediaShortDto? Media, string? Error)> GetShowByIdAsync(long? id)
    {
        var movie = await _showRepository.GetByIdAsync(id);

        if (movie is null)
            return (null, "Show does not exist");

        return (_mediaTransformer.ToShortDto(new List<Media>() { movie }).First(), null);
    }

    public async Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetAllMoviesAsync()
    {
        var movies = await _movieRepository.GetAllAsync();

        var result = new List<MediaShortDto>();

        result.AddRange(_mediaTransformer.ToShortDto(movies));

        return (result, null);
    }

    public async Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetAllShowsAsync()
    {
        var shows = await _showRepository.GetAllAsync();

        var result = new List<MediaShortDto>();

        result.AddRange(_mediaTransformer.ToShortDto(shows));

        return (result, null);
    }

    public async Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetAllWithGenres(string[] genreNames)
    {

        var (genres, errors) = await ValidateGenreNames(genreNames);

        if (errors is not null && errors.Any())
            return (null, string.Join(';', errors));

        var result = new List<MediaShortDto>();

        var movies = new List<Movie>();
        var shows = new List<TvShow>();

        foreach (var genre in genres)
        {
            movies.AddRange(await _movieRepository.GetAllByGenreIdAsync(genre.Id));
            shows.AddRange(await _showRepository.GetAllByGenreIdAsync(genre.Id));
        }

        result.AddRange(_mediaTransformer.ToShortDto(shows));
        result.AddRange(_mediaTransformer.ToShortDto(movies));

        return (result, null);
    }

    public async Task<(long? Id, string? Error)> UpdateMovieAsync(UpdateMovieDto dto, long mediaId)
    {
        var dbMedia = await _movieRepository.GetByIdAsync(mediaId);

        if (dbMedia is null)
        {
            return (null, $"Media with id '{mediaId}' does not exist");
        }

        var (actors, errors) = await ValidateActorIds(dto.ActorIds);

        if (errors is not null && errors.Any())
            return (null, "Directors not found");

        dbMedia.Actors = actors!;
        dbMedia.UpdateTimeStamp = DateTime.UtcNow;

        await _movieRepository.UpdateAsync(dbMedia);

        return (dbMedia.Id, null);
    }

    public async Task<(long? Id, string? Error)> UpdateTvShowAsync(UpdateTvShowDto dto, long mediaId)
    {
        var dbMedia = await _showRepository.GetByIdAsync(mediaId);

        if (dbMedia is null)
        {
            return (null, $"Media with id '{mediaId}' does not exist");
        }

        if (dto.ActorIds is not null)
        {
            var (actors, errors) = await ValidateActorIds(dto.ActorIds);

            if (errors is not null && errors.Any())
                return (null, "Directors not found");

            dbMedia.Actors = actors!;
            dbMedia.UpdateTimeStamp = DateTime.UtcNow;
        }

        if (dto.EpisodeIds is not null)
        {
            var (episodes, errors) = await ValidateEpisodeIds(dto.ActorIds);

            if (errors is not null && errors.Any())
                return (null, "Episodes not found");

            dbMedia.Episodes = episodes!;
            dbMedia.UpdateTimeStamp = DateTime.UtcNow;
        }

        if (dto.TotalSeasons is not null)
        {
            dbMedia.Seasons = dto.TotalSeasons;
            dbMedia.UpdateTimeStamp = DateTime.UtcNow;
        }

        await _showRepository.UpdateAsync(dbMedia);

        return (dbMedia.Id, null);
    }

    private async Task<User?> ValidateCreatedByUser(long userId)
    {
        var dbUser = await _userRepository.GetByIdAsync(userId);

        if (dbUser is null || (dbUser.Role is not Role.Moderator && dbUser.Role is not Role.Administrator) || dbUser.IsDeleted == true)
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

    private async Task<(ISet<Actor>? directors, IList<string>? ValidationErrors)> ValidateActorIds(ISet<long> IDs)
    {

        var errors = new List<string>();
        var actors = new HashSet<Actor>();

        foreach (var id in IDs)
        {
            var actor = await _actorRepository.GetByIdAsync(id);
            if (actor is null)
            {
                _logger.LogDebug("Actor with id '{id}' not found", id);
                errors.Add("Actor not found");
            }
            else
                actors.Add(actor);
        }

        return (actors, errors);
    }

    private async Task<(ISet<ShowEpisode>? directors, IList<string>? ValidationErrors)> ValidateEpisodeIds(ISet<long> IDs)
    {

        var errors = new List<string>();
        var episodes = new HashSet<ShowEpisode>();

        foreach (var id in IDs)
        {
            var episode = await _episodeRepository.GetByIdAsync(id);
            if (episode is null)
            {
                _logger.LogDebug("Episode with id '{id}' not found", id);
                errors.Add("Episode not found");
            }
            else
                episodes.Add(episode);
        }

        return (episodes, errors);
    }

    private async Task<(ISet<Genre>? genres, IList<string>? ValidationErrors)> ValidateGenreIds(ISet<long> IDs)
    {
        var errors = new List<string>();
        var genres = new HashSet<Genre>();

        foreach (var id in IDs)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre is null)
            {
                _logger.LogDebug("Genre with id '{id}' not found", id);
                errors.Add("Genre not found");
            }
            else
                genres.Add(genre);
        }

        return (genres, errors);
    }

    private async Task<(ISet<Genre>? genres, IList<string>? ValidationErrors)> ValidateGenreNames(IEnumerable<string> names)
    {
        var errors = new List<string>();
        var genres = new HashSet<Genre>();

        foreach (var name in names)
        {
            var genre = await _genreRepository.GetByGenreNameAsync(name);
            if (genre is null)
            {
                _logger.LogDebug("Genre with name '{name}' not found", name);
                errors.Add("Genre not found");
            }
            else
                genres.Add(genre);
        }

        return (genres, errors);
    }

    private async Task<(long? Id, string? Error)> CreateMovieReviewAsync(CreateReviewDto model)
    {
        var media = await _movieRepository.GetByIdAsync(model.MediaId);

        if (media is null)
            return (null, "Media not found");

        var user = await _userRepository.GetByIdAsync(model.UserId);

        if (media.Reviews.Any(x => x.UserId.Equals(user!.Id)))
            return (null, "User already has a review for this media");

        var dbReview = new Review()
        {
            MovieId = media.Id,
            IsDeleted = false,
            Rating = model.Rating,
            ReviewText = model.Review,
            UserId = user.Id
        };

        media.Rating = CalculateRating(currentReviewCount: media.Reviews.Count, currentRating: media.Rating.Value, newRating: model.Rating);

        await _reviewRepository.CreateAsync(dbReview);

        await _movieRepository.UpdateAsync(media);

        return (dbReview.Id, null);
    }

    private async Task<(long? Id, string? Error)> CreateShowReviewAsync(CreateReviewDto model)
    {
        var media = await _showRepository.GetByIdAsync(model.MediaId);

        if (media is null)
            return (null, "Media not found");

        var user = await _userRepository.GetByIdAsync(model.UserId);

        if (media.Reviews.Any(x => x.UserId.Equals(user!.Id)))
            return (null, "User already has a review for this media");

        var dbReview = new Review()
        {
            ShowId = media.Id,
            IsDeleted = false,
            Rating = model.Rating,
            ReviewText = model.Review,
            UserId = user.Id
        };

        media.Rating = CalculateRating(currentReviewCount: media.Reviews.Count, currentRating: media.Rating.Value, newRating: model.Rating);

        await _reviewRepository.CreateAsync(dbReview);

        await _showRepository.UpdateAsync(media);

        return (dbReview.Id, null);
    }

    private async Task<(long? Id, string? Error)> CreateEpisodeReviewAsync(CreateReviewDto model)
    {
        var media = await _episodeRepository.GetByIdAsync(model.MediaId);

        if (media is null)
            return (null, "Media not found");

        var user = await _userRepository.GetByIdAsync(model.UserId);

        if (media.Reviews.Any(x => x.UserId.Equals(user!.Id)))
            return (null, "User already has a review for this media");

        var dbReview = new Review()
        {
            EpisodeId = media.Id,
            IsDeleted = false,
            Rating = model.Rating,
            ReviewText = model.Review,
            UserId = user.Id
        };


        media.Rating = CalculateRating(currentReviewCount: media.Reviews.Count, currentRating: media.Rating.Value, newRating: model.Rating);

        await _reviewRepository.CreateAsync(dbReview);

        await _episodeRepository.UpdateAsync(media);

        return (dbReview.Id, null);
    }

    private double CalculateRating(int currentReviewCount, double currentRating, double newRating)
    {
        var currentTotal = currentReviewCount * currentRating;

        var newTotal = currentTotal + newRating;

        return Math.Round(newTotal / (currentReviewCount + 1), 2);
    }
}