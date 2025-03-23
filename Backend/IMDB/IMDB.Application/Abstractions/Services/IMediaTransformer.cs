using IMDB.Application.DTOs.Media;
using IMDB.Domain.AbstractModels;

namespace IMDB.Application.Abstractions.Services;

public interface IMediaTransformer
{
    public IEnumerable<MediaShortDto> ToShortDto(IEnumerable<Media> dbMediaList);
}
