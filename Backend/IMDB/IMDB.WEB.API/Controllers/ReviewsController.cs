using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IMediaService _mediaService;
    private readonly IUserService _userService;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(IMediaService mediaService, IUserService userService, ILoggerFactory loggerFactory)
    {
        _mediaService = mediaService;
        _userService = userService;
        _logger = loggerFactory.CreateLogger<ReviewsController>();

    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto model)
    {
        var (userId, userError) = await GetUserId();

        if (!string.IsNullOrEmpty(userError))
        {
            if (userError.Contains("unauthorized", StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();
            else
                return BadRequest(userError);
        }

        model.UserId = userId!.Value;

        var (id, error) = await _mediaService.CreateReview(model);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }
        else
            return Ok(id);
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
            _logger.LogWarning("Error when attempting to check get user with claims: '{idClaim}', '{roleClaim}'", idClaim.Value, roleClaim.Value);
            return (null, "Unauthorized");
        }

        return (dbUser.Id, null);
    }
}
