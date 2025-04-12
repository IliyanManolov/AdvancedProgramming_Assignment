using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("api/watchlist")]
public class WatchlistController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IWatchListService _watchlistService;
    private readonly ILogger<WatchlistController> _logger;

    public WatchlistController(IUserService userService, IWatchListService watchlistService, ILoggerFactory loggerFactory)
    {
        _userService = userService;
        _watchlistService = watchlistService;
        _logger = loggerFactory.CreateLogger<WatchlistController>();
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetBasic()
    {
        var (userId, userError) = await GetUserId();

        if (!string.IsNullOrEmpty(userError))
        {
            if (userError.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(userError);
        }

        var (watchlist, error) = await _watchlistService.GetListBasicAsync(userId);

        if (!string.IsNullOrEmpty(error))
        {
            _logger.LogWarning(error);
            return BadRequest(error);
        }

        return Ok(watchlist);
    }

    [HttpGet("details")]
    [Authorize]
    public async Task<IActionResult> GetDetails()
    {
        var (userId, userError) = await GetUserId();

        if (!string.IsNullOrEmpty(userError))
        {
            if (userError.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(userError);
        }

        var (watchlist, error) = await _watchlistService.GetListDetailsAsync(userId);

        if (!string.IsNullOrEmpty(error))
        {
            _logger.LogWarning(error);
            return BadRequest(error);
        }

        return Ok(watchlist);
    }


    private async Task<(long? id, string? Error)> GetUserId()
    {
        var idClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);

        if (roleClaim is null || idClaim is null)
            return (null, "Unauthorized");

        var (dbUser, error) = await _userService.GetUserDetailsAsync(long.Parse(idClaim.Value));

        if (!string.IsNullOrWhiteSpace(error))
        {
            _logger.LogWarning("Error when attempting to get userId for user with claims: '{idClaim}', '{roleClaim}'", idClaim.Value, roleClaim.Value);
            return (null, "Unauthorized");
        }

        if (long.Parse(idClaim.Value) != dbUser.Id || roleClaim.Value != dbUser.Role.ToString())
        {
            _logger.LogWarning("Mismatching information from cookie and database when attempting get userId");
            return (null, "Unauthorized");
        }

        return (dbUser.Id, null);
    }
}
