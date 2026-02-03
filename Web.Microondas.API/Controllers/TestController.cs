using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Microondas.Application.Exceptions;

namespace Web.Microondas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("test-exception")]
    [AllowAnonymous]
    public IActionResult TestException()
    {
        throw new Exception("This is a test exception to verify logging!");
    }

    [HttpGet("test-business-exception")]
    [AllowAnonymous]
    public IActionResult TestBusinessException()
    {
        throw new BusinessRuleException("This is a test business rule exception!");
    }

    [HttpGet("test-nested-exception")]
    [AllowAnonymous]
    public IActionResult TestNestedException()
    {
        try
        {
            throw new InvalidOperationException("Inner exception message");
        }
        catch (Exception ex)
        {
            throw new Exception("Outer exception message", ex);
        }
    }

    [HttpGet("health")]
    [AllowAnonymous]
    public IActionResult Health()
    {
        return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
    }
}
