using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Application.UseCases.User.Handlers.Interfaces;

public interface IGetAllUsersHandler
{
    Task<IReadOnlyList<Users>> Handle(CancellationToken ct = default);
}
