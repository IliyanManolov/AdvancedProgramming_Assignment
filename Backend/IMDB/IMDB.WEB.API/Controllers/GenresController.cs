using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.Genres;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Administrator,Moderator")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGenreDto model)
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

        var (genreId, error) = await _genreService.CreateAsync(model);

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(genreId);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var (genres, error) = await _genreService.GetAllAsync();

        if (!string.IsNullOrEmpty(error))
        {
            if (error.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(error);
        }

        return Ok(genres);
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