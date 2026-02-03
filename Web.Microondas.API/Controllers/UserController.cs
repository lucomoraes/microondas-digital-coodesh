using Microsoft.AspNetCore.Mvc;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Application.UseCases.User.Commands;
using Web.Microondas.Application.UseCases.User.Queries;
using Web.Microondas.Application.DTOs;

namespace Web.Microondas.API.Controllers;

using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) => _userService = userService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var result = await _userService.CreateAsync(request, ct);
        if (result == null)
            return BadRequest("Não foi possível criar o usuário.");
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await _userService.UpdateAsync(request, ct);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _userService.DeleteAsync(new DeleteUserRequest { Id = id }, ct);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _userService.GetByIdAsync(new GetUserByIdQuery(id), ct);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _userService.GetAllAsync(new GetAllUsersQuery(), ct));
}
