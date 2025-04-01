using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.ShowEpisode;
using IMDB.Domain.AbstractModels;
using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Services;

public interface IMediaTransformer
{
    public IEnumerable<MediaShortDto> ToShortDto(IEnumerable<Media> dbMediaList);
    public IEnumerable<EpisodeDetailsDto> ToDetails(IEnumerable<ShowEpisode> episodes);
}
