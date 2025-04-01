using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;
using IMDB.Application.DTOs.ShowEpisode;

namespace IMDB.Application.Abstractions.Services;

public interface IMediaService
{
    public Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetAllDiscoveryAsync();
    public Task<(IEnumerable<MediaShortDto>? MediaList, string? Error)> GetAllWithGenres(string[] genres);

    public Task<(long? Id, string? Error)> CreateMovieAsync(CreateMovieDto dto);
    public Task<(long? Id, string? Error)> CreateTvShowAsync(CreateTvShowDto dto);
    public Task<(long? Id, string? Error)> CreateEpisodeAsync(CreateEpisodeDto dto);


    public Task<(long? Id, string? Error)> UpdateMovieAsync(UpdateMovieDto dto, long id);
    public Task<(long? Id, string? Error)> UpdateTvShowAsync(UpdateTvShowDto dto, long id);

    public Task<(IEnumerable<EpisodeDetailsDto>? episodesList, string? Error)> GetShowEpisodes(long? showId);
}
