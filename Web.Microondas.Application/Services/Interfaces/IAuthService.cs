using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.Auth.Command;

namespace Web.Microondas.Application.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
