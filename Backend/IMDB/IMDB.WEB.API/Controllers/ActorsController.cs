using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Actor;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using IMDB.Application.Services;

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
    [Authorize(Roles = "Administrator,Moderator")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateActorDto model)
    {

        var (userId, userError) = await GetCreatedById();

        if (!string.IsNullOrEmpty(userError))
        {
            if (userError.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(userError);
        }

        model.CreatedByUserId = userId;

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

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var actors = await _actorService.GetAllAsync();

        return Ok(actors);
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
