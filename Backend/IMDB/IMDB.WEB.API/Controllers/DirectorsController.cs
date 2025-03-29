using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Director;

namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("api/directors")]
public class DirectorsController : ControllerBase
{
    private readonly ILogger<ActorsController> _logger;
    private readonly IDirectorService _directorService;
    private readonly IUserService _userService;

    public DirectorsController(ILoggerFactory loggerFactory, IDirectorService directorService, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<ActorsController>();
        _directorService = directorService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDirectorDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (actorId, error) = await _directorService.CreateAsync(model);

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