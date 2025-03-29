using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Actor;

namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("api/actors")]
public class ActorsController : ControllerBase
{
    private readonly ILogger<ActorsController> _logger;
    private readonly IActorService _actorService;
    private readonly IUserService _userService;

    public ActorsController(ILoggerFactory loggerFactory, IActorService actorService, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<ActorsController>();
        _actorService = actorService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateActorDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (actorId, error) = await _actorService.CreateAsync(model);

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
