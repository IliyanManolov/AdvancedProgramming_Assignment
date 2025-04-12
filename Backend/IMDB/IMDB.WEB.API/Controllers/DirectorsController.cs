using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Director;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
    [Authorize(Roles = "Administrator,Moderator")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDirectorDto model)
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

        var (directorId, error) = await _directorService.CreateAsync(model);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(directorId);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var directors = await _directorService.GetAllAsync();

        return Ok(directors);
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