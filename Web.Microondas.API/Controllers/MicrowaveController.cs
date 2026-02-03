using Microsoft.AspNetCore.Mvc;
using Web.Microondas.Application.Services.Interfaces;

namespace Web.Microondas.API.Controllers;

using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MicrowaveController : ControllerBase
{
    private readonly IMicrowaveService _service;

    public MicrowaveController(IMicrowaveService service)
    {
        _service = service;
    }

    [HttpGet("state")]
    public IActionResult GetState() => Ok(_service.GetState());

    [HttpPost("quickstart")]
    public IActionResult QuickStart()
    {
        _service.QuickStart();
        return Ok(_service.GetState());
    }

    [HttpPost("manualstart")]
    public IActionResult StartManual([FromQuery] int seconds, [FromQuery] int power)
    {
        _service.StartManual(seconds, power);
        return Ok(_service.GetState());
    }

    [HttpPost("programstart/{programId}")]
    public IActionResult StartProgram(Guid programId)
    {
        _service.StartProgram(programId);
        return Ok(_service.GetState());
    }

    [HttpPost("tick")]
    public IActionResult Tick()
    {
        _service.Tick();
        return Ok(_service.GetState());
    }

    [HttpPost("pause-cancel")]
    public IActionResult PauseOrCancel()
    {
        _service.PauseOrCancel();
        return Ok(_service.GetState());
    }

    [HttpPost("resume")]
    public IActionResult Resume()
    {
        _service.Resume();
        return Ok(_service.GetState());
    }

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _service.Reset();
        return Ok(_service.GetState());
    }
}
