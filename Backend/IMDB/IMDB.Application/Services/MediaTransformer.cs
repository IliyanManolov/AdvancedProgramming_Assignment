using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Domain.AbstractModels;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;

public class MediaTransformer : IMediaTransformer
{
    private readonly ILogger<MediaTransformer> _logger;

    public MediaTransformer(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MediaTransformer>();
    }

    public IEnumerable<MediaShortDto> ToShortDto(IEnumerable<Media> dbMediaList)
    {
        var result = new List<MediaShortDto>();

        foreach (var dbMedia in dbMediaList)
        {
            if (dbMedia is TvShow tvShow)
            {
                result.Add(TransformShow(tvShow));
            }
            else if (dbMedia is Movie movie)
                result.Add(TransformMovie(movie));
            else
            {
                _logger.LogError("Could not determine media type - {objectType}", dbMedia.GetType().Name);
                throw new ArgumentException("Could not determine media type");
            }
                
        }

        return result;
    }


    private static MediaShortDto TransformShow(TvShow show)
    {
        return new MediaShortDto()
        {
            ShowSeasonsCount = show.Seasons,
            ShowEpisodesCount = show.Episodes.Count,
            ShowEndDate = show.EndDate,

            ReleaseDate = show.ReleaseDate,
            Genres = show.Genres.Select(x => x.Name).ToHashSet(),
            Title = show.Title,
            Rating = show.Rating,
            Id = show.Id,
            Description = show.Description,
            Type = MediaType.TvShow,
            Director = $"{show.Director.FirstName} {show.Director.LastName}",
            Reviews = show.Reviews ?? 0,
            Length = show.Episodes.Any() ? show.Episodes.Sum(x => x.Length)!.Value : 0
        };
    }

    private static MediaShortDto TransformMovie(Movie movie)
    {
        return new MediaShortDto()
        {
            Length = movie.Length,

            ReleaseDate = movie.ReleaseDate,
            Genres = movie.Genres.Select(x => x.Name).ToHashSet(),
            Title = movie.Title,
            Rating = movie.Rating,
            Id = movie.Id,
            Description = movie.Description,
            Type = MediaType.Movie,
            Director = $"{movie.Director.FirstName} {movie.Director.LastName}",
            Reviews = movie.Reviews ?? 0,
        };
    }
}