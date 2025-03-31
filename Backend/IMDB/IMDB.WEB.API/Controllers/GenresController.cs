using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Genres;
namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("api/genres")]
public class GenresController : ControllerBase
{
    private readonly ILogger<GenresController> _logger;
    private readonly IGenreService _genreService;
    private readonly IUserService _userService;

    public GenresController(ILoggerFactory loggerFactory, IGenreService genresService, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<GenresController>();
        _genreService = genresService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGenreDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (actorId, error) = await _genreService.CreateAsync(model);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(actorId);
    }
}