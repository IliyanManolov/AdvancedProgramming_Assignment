using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.User;
using IMDB.Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("oauth/")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly Application.Abstractions.Services.IAuthenticationService _authenticationService;
    private readonly IUserService _userService;

    public AuthenticationController(ILoggerFactory loggerFactory, Application.Abstractions.Services.IAuthenticationService authenticationService, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<AuthenticationController>();
        _authenticationService = authenticationService;
        _userService = userService;
    }

    [HttpPost("login/")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthenticateDto model)
    {
        var (details, error) = await _authenticationService.AuthenticateAsync(model.Username, model.Password);

        if (!string.IsNullOrWhiteSpace(error))
            return Unauthorized(error);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, details.Id.ToString()!),
            new Claim(ClaimTypes.Name, details.Username!),
            new Claim(ClaimTypes.Role, details.Role.ToString())
        };

        await HttpContext.SignInAsync(
            new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
            )
        );

        return Ok(details);
    }

    [HttpPost("register/")]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto model)
    {
        var (userId, error) = await _userService.CreateUserAsync(model);

        if (!string.IsNullOrWhiteSpace(error))
            return BadRequest(error);

        return Ok(userId);
    }

    [Authorize(Roles = "Administrator,Moderator")]
    [HttpGet("admin/")]
    public async Task<IActionResult> IsAdmin()
    {
        var claims = HttpContext.User.Claims;

        var idClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);

        if (roleClaim is null || idClaim is null)
            return NotFound();

        var (dbUser, error) = await _userService.GetUserDetailsAsync(long.Parse(idClaim.Value));

        if (!string.IsNullOrWhiteSpace(error))
        {
            _logger.LogWarning("Error when attempting to check admin privileges for user with claims: '{idClaim}', '{roleClaim}'", idClaim.Value, roleClaim.Value);
            return NotFound();
        }

        if (long.Parse(idClaim.Value) != dbUser.Id || roleClaim.Value != dbUser.Role.ToString())
        {
            _logger.LogWarning("Mismatching information from cookie and database when attempting to check admin privileges");
            return NotFound();
        }

        return Ok();
    }
}