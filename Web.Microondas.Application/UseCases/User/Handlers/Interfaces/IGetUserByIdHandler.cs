using Web.Microondas.Application.UseCases.User.Queries;
using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Application.UseCases.User.Handlers.Interfaces;

public interface IGetUserByIdHandler
{
    Task<Users?> Handle(GetUserByIdQuery query, CancellationToken ct = default);
}
