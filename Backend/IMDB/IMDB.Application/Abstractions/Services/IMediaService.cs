using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;

namespace IMDB.Application.Abstractions.Services;

interface IMediaService
{
    public Task<(IEnumerable<MediaShortDto>? MediaList, string Error)> GetAllDiscoveryAsync();
    public Task<(IEnumerable<MediaShortDto>? MediaList, string Error)> GetAllWithGenres(string[] genres);

    public Task<(long? Id, string Error)> CreateMovieAsync(CreateMovieDto dto);
    public Task<(long? Id, string Error)> CreateTvShowAsync(CreateTvShowDto dto);
}
