using Web.Microondas.Application.UseCases.User.Handlers.Interfaces;
using Web.Microondas.Application.UseCases.User.Queries;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Application.UseCases.User.Handlers;

public class GetUserByIdHandler : IGetUserByIdHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Users?> Handle(GetUserByIdQuery query, CancellationToken ct = default)
    {
        return await _userRepository.GetByIdAsync(query.Id, ct);
    }
}
