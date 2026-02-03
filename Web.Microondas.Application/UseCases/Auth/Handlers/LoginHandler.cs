using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.Helpers;
using Web.Microondas.Application.UseCases.Auth.Command;
using Web.Microondas.Application.UseCases.Auth.Handlers.Interfaces;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Application.UseCases.Auth.Handlers;

public class LoginHandler : ILoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtHelper _jwtHelper;

    public LoginHandler(IUserRepository userRepository, IJwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponseDTO?> Handle(LoginRequest request, CancellationToken ct = default)
    {
        var passwordHash = Sha256Helper.Hash(request.Password);
        var user = await _userRepository.GetByUsernameAndPasswordAsync(request.Username, passwordHash, ct);
        if (user == null) return null;

        var token = _jwtHelper.GenerateToken(user);
        return new LoginResponseDTO
        {
            Token = token,
            User = new UserDTO
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            }
        };
    }
}
