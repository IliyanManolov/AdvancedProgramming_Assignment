using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.ShowEpisode;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace IMDB.Application.Services;



public class ShowEpisodeService : IShowEpisodeService
{
    private readonly IUserRepository _userRepository;
    private readonly ITvShowRepository _showRepository;
    private readonly IEpisodeRepository _episodeRepository;
    private readonly ILogger<ShowEpisodeService> _logger;

    public ShowEpisodeService(IUserRepository userRepository,
        ITvShowRepository showRepository,
        IEpisodeRepository episodeRepository,
        ILoggerFactory loggerFactory)
    {
        _userRepository = userRepository;
        _showRepository = showRepository;
        _episodeRepository = episodeRepository;
        _logger = loggerFactory.CreateLogger<ShowEpisodeService>();
    }

    public async Task<(long? Id, string? Error)> CreateAsync(CreateEpisodeDto dto)
    {
        var (userExists, dbUser) = await ValidateUser(dto);

        if (!userExists)
            return (null, "User does not exist");

        var (showExists, dbShow) = await ValidateShow(dto);

        if (!showExists)
            return (null, "Show does not exist");

        if (dto.Length is null || dto.Length <= 0)
        {
            _logger.LogDebug("Episode length is 0 or null - '{episodeLength}'", dto.Length);
            return (null, "Invalid episode length");
        }

        if (string.IsNullOrEmpty(dto.Title))
        {
            _logger.LogDebug("Episode title is empty or null - '{title}'", dto.Title);
            return (null, "Invalid episode length");
        }

        var dbEpisode = new ShowEpisode()
        {
            DateAired = dto.DateAired,
            CreatedByUserId = dbUser.Id,
            CreateTimeStamp = DateTime.UtcNow,
            Description = dto.Description,
            Length = dto.Length,
            Reviews = 0,
            SeasonNumber = dto.SeasonNumber,
            ShowId = dbShow.Id,
            Title = dto.Title
        };

        await _episodeRepository.CreateAsync(dbEpisode);

        return (dbEpisode.Id, null);
    }





    private async Task<(bool exists, User dbUser)> ValidateUser(CreateEpisodeDto dto)
    {
        if (dto.CreatedByUserId is null)
            return (false, null);

        var dbUser = await _userRepository.GetByIdAsync(dto.CreatedByUserId);

        if (dbUser is null)
            return (false, null);

        if (dbUser.Role is not Role.Moderator && dbUser.Role is not Role.Administrator)
        {
            _logger.LogWarning("Attempted to create episode with user that is not moderator/administrator - '{userId}'", dto.CreatedByUserId);
            return (false, null);
        }

        if (dbUser.IsDeleted == true)
        {
            _logger.LogWarning("Attempted to create episode with user that is deleted '{userId}'", dto.CreatedByUserId);
            return (false, null);
        }

        return (true, dbUser);
    }

    private async Task<(bool exists, TvShow show)> ValidateShow(CreateEpisodeDto dto)
    {
        if (dto.ShowId is null)
            return (false, null);

        var dbShow = await _showRepository.GetByIdAsync(dto.ShowId);

        if (dbShow is null)
            return (false, null);

        if (dto.SeasonNumber is null)
        {
            _logger.LogDebug("Season number received is NULL");
            return (false, null);
        }

        if (dto.SeasonNumber > dbShow.Seasons)
        {
            _logger.LogDebug("Season number for episode ('{episodeSeason}') is higher than show total('{showSeasons}')",
                dto.SeasonNumber,
                dbShow.Seasons);
            return (false, null);
        }

        return (true, dbShow);
    }
}
