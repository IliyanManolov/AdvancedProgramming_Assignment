using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.ShowEpisode;
using IMDB.Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;
    private readonly IUserService _userService;
    private readonly ILogger<MediaController> _logger;

    public MediaController(IMediaService mediaService, IUserService userService, ILoggerFactory loggerFactory)
    {
        _mediaService = mediaService;
        _userService = userService;
        _logger = loggerFactory.CreateLogger<MediaController>();
    }

    [Authorize(Roles = "Administrator,Moderator")]
    [HttpPost("movies/")]
    public async Task<IActionResult> CreateMovieAsync([FromBody] CreateMovieDto model)
    {

        var (userId, userError) = await GetCreatedById();

        if (!string.IsNullOrEmpty(userError))
        {
            if (userError.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(userError);
        }

        model.CreatedByUserId = userId!.Value;

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (movieId, error) = await _mediaService.CreateMovieAsync(model);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(movieId);
    }

    [HttpGet("movies/")]
    public async Task<IActionResult> GetAllMoviesAsync()
    {

        var (movies, error) = await _mediaService.GetAllMoviesAsync();

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(movies);
    }

    [HttpPatch("movies/{mediaId}")]
    public async Task<IActionResult> UpdateMovieAsync([FromBody] UpdateMovieDto model, [FromRoute] long mediaId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (movieId, error) = await _mediaService.UpdateMovieAsync(model, mediaId);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(movieId);
    }

    [Authorize(Roles = "Administrator,Moderator")]
    [HttpPost("shows/")]
    public async Task<IActionResult> CreateShowAsync([FromBody] CreateTvShowDto model)
    {
        var (userId, userError) = await GetCreatedById();

        if (!string.IsNullOrEmpty(userError))
        {
            if (userError.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(userError);
        }

        model.CreatedByUserId = userId!.Value;

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (showId, error) = await _mediaService.CreateTvShowAsync(model);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(showId);
    }

    [HttpGet("shows/")]
    public async Task<IActionResult> GetAllShowsAsync()
    {

        var (movies, error) = await _mediaService.GetAllShowsAsync();

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(movies);
    }

    [HttpGet("shows/{showId}/episodes")]
    public async Task<IActionResult> GetShowEpisodesAsync(long showId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (episodes, error) = await _mediaService.GetShowEpisodes(showId);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(episodes);
    }

    [HttpPost("episodes/")]
    public async Task<IActionResult> CreateEpisodeAsync([FromBody] CreateEpisodeDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (showId, error) = await _mediaService.CreateEpisodeAsync(model);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(showId);
    }

    [HttpPatch("shows/{mediaId}")]
    public async Task<IActionResult> UpdateShowAsync([FromBody] UpdateTvShowDto model, [FromRoute] long mediaId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (movieId, error) = await _mediaService.UpdateTvShowAsync(model, mediaId);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(movieId);
    }

    [HttpGet]
    public async Task<IActionResult> GetDiscovery([FromBody] DiscoveryRequestDto? dto)
    {
        if (dto is not null && dto.Genres is not null)
        {
            var (genreDiscovery, error) = await _mediaService.GetAllWithGenres(dto.Genres.ToArray());

            if (!string.IsNullOrEmpty(error))
            {
                if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                    return Unauthorized();
                else
                    return BadRequest(error);
            }

            return Ok(genreDiscovery);
        }
        else
        {
            var (mediaDiscovery, error) = await _mediaService.GetAllDiscoveryAsync();

            if (!string.IsNullOrEmpty(error))
            {
                if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                    return Unauthorized();
                else
                    return BadRequest(error);
            }

            return Ok(mediaDiscovery);
        }
    }

    [HttpGet("movies/{id}")]
    public async Task<IActionResult> GetMovieAsync(long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (episodes, error) = await _mediaService.GetMovieByIdAsync(id);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(episodes);
    }

    [HttpGet("shows/{id}")]
    public async Task<IActionResult> GetShowAsync(long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (episodes, error) = await _mediaService.GetShowByIdAsync(id);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(episodes);
    }

    private async Task<(long? id, string? Error)> GetCreatedById()
    {
        var idClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);

        if (roleClaim is null || idClaim is null)
            return (null, "Unauthorized");

        var (dbUser, error) = await _userService.GetUserDetailsAsync(long.Parse(idClaim.Value));

        if (!string.IsNullOrWhiteSpace(error))
        {
            _logger.LogWarning("Error when attempting to check admin privileges for user with claims: '{idClaim}', '{roleClaim}'", idClaim.Value, roleClaim.Value);
            return (null, "Unauthorized");
        }

        if (long.Parse(idClaim.Value) != dbUser.Id || roleClaim.Value != dbUser.Role.ToString())
        {
            _logger.LogWarning("Mismatching information from cookie and database when attempting to check admin privileges");
            return (null, "Unauthorized");
        }

        return (dbUser.Id, null);
    }
}