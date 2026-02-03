using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Application.UseCases.Auth.Command;
using Web.Microondas.Application.UseCases.Auth.Handlers.Interfaces;

namespace Web.Microondas.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ILoginHandler _loginHandler;

    public AuthService(ILoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    public async Task<LoginResponseDTO?> LoginAsync(LoginRequest request, CancellationToken ct = default)
        => await _loginHandler.Handle(request, ct);
}
