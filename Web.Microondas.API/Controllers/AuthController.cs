using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Application.UseCases.Auth.Command;

namespace Web.Microondas.API.Controllers;

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
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request, ct);
        if (result == null)
            return Unauthorized("Invalid username or password.");

        return Ok(result);
    }

    [HttpGet("check")]
    [Authorize]
    public IActionResult Check()
    {
        return Ok(new { status = "ok" });
    }
}
