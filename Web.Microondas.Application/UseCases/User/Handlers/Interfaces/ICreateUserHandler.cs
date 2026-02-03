using Web.Microondas.Application.UseCases.User.Commands;

namespace Web.Microondas.Application.UseCases.User.Handlers.Interfaces;

public interface ICreateUserHandler
{
    Task<bool> Handle(CreateUserRequest request, CancellationToken ct = default);
}
