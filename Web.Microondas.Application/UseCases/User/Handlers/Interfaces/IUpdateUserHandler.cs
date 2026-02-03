using Web.Microondas.Application.UseCases.User.Commands;

namespace Web.Microondas.Application.UseCases.User.Handlers.Interfaces;

using Web.Microondas.Domain.Entities;

public interface IUpdateUserHandler
{
    Task<Users?> Handle(UpdateUserRequest request, CancellationToken ct = default);
}
