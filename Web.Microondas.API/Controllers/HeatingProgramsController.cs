using Microsoft.AspNetCore.Mvc;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.UseCases.HeatingProgram.Queries;

namespace Web.Microondas.API.Controllers;

using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HeatingProgramsController : ControllerBase
{
    private readonly IHeatingProgramService _service;

    public HeatingProgramsController(IHeatingProgramService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(new GetHeatingProgramByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHeatingProgramRequest request, CancellationToken ct)
        => Ok(await _service.CreateCustomAsync(request, ct));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.DeleteCustomAsync(new DeleteHeatingProgramRequest { Id = id }, ct);
        return NoContent();
    }        
}
