using Web.Microondas.Application.UseCases.User.Commands;

namespace Web.Microondas.Application.UseCases.User.Handlers.Interfaces;

public interface IDeleteUserHandler
{
    Task<bool> Handle(DeleteUserRequest request, CancellationToken ct = default);
}
