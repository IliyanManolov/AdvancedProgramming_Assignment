using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;
using IMDB.Application.DTOs.Media;
namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;
    private readonly ILogger<MediaController> _logger;

    public MediaController(IMediaService mediaService, ILoggerFactory loggerFactory)
    {
        _mediaService = mediaService;
        _logger = loggerFactory.CreateLogger<MediaController>();
    }

    [HttpPost("movies/")]
    public async Task<IActionResult> CreateMovieAsync([FromBody] CreateMovieDto model)
    {
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

    [HttpPost("shows/")]
    public async Task<IActionResult> CreateShowAsync([FromBody] CreateTvShowDto model)
    {
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
}