using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.User.Commands;
using Web.Microondas.Application.UseCases.User.Queries;

namespace Web.Microondas.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO?> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<UserDTO?> UpdateAsync(UpdateUserRequest request, CancellationToken ct = default);
    Task DeleteAsync(DeleteUserRequest request, CancellationToken ct = default);
    Task<UserDTO?> GetByIdAsync(GetUserByIdQuery query, CancellationToken ct = default);
    Task<IReadOnlyList<UserDTO>> GetAllAsync(GetAllUsersQuery query, CancellationToken ct = default);
}
