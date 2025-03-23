using IMDB.Application.DTOs.ShowEpisode;

namespace IMDB.Application.Abstractions.Services;

public interface IShowEpisodeService
{
    public Task<(long? Id, string? Error)> CreateAsync(CreateEpisodeDto dto);
}