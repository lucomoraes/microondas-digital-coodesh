using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.Auth.Command;

namespace Web.Microondas.Application.UseCases.Auth.Handlers.Interfaces;

public interface ILoginHandler
{
    Task<LoginResponseDTO?> Handle(LoginRequest request, CancellationToken ct = default);
}
