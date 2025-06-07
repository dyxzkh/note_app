using Microsoft.AspNetCore.Mvc;
using note_webapi.DTOs;
using note_webapi.Interfaces;

namespace note_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.AuthenticateAsync(request);
        return result is null ? Unauthorized("Invalid credentials") : Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);
        return result is null ? Unauthorized("Invalid refresh token") : Ok(result);
    }
}