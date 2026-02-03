using Web.Microondas.Application.UseCases.User.Handlers.Interfaces;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Application.UseCases.User.Handlers;

public class GetAllUsersHandler : IGetAllUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<Users>> Handle(CancellationToken ct = default)
    {
        return await _userRepository.GetAllAsync(ct);
    }
}
