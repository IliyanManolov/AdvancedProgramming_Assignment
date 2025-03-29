using IMDB.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using IMDB.Application.DTOs.User;

namespace IMDB.WEB.API.Controllers;

[ApiController]
[Route("oauth/")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;

    public AuthenticationController(ILoggerFactory loggerFactory, IAuthenticationService authenticationService, IUserService userService)
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
}