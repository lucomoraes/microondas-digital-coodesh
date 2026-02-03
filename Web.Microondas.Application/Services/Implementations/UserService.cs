using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Application.UseCases.User.Commands;
using Web.Microondas.Application.UseCases.User.Queries;
using Web.Microondas.Application.UseCases.User.Handlers.Interfaces;

namespace Web.Microondas.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly ICreateUserHandler _createHandler;
    private readonly IUpdateUserHandler _updateHandler;
    private readonly IDeleteUserHandler _deleteHandler;
    private readonly IGetUserByIdHandler _getByIdHandler;
    private readonly IGetAllUsersHandler _getAllHandler;

    public UserService(
        ICreateUserHandler createHandler,
        IUpdateUserHandler updateHandler,
        IDeleteUserHandler deleteHandler,
        IGetUserByIdHandler getByIdHandler,
        IGetAllUsersHandler getAllHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getByIdHandler = getByIdHandler;
        _getAllHandler = getAllHandler;
    }

    public async Task<UserDTO?> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var result = await _createHandler.Handle(request, ct);
        if (!result) return null;
        // Fetch the created user by username (assuming username is unique)
        var user = await _getAllHandler.Handle(ct);
        var createdUser = user?.FirstOrDefault(u => u.Username == request.Username);
        if (createdUser == null) return null;
        return new UserDTO
        {
            Id = createdUser.Id,
            Firstname = createdUser.Firstname,
            Lastname = createdUser.Lastname,
            Username = createdUser.Username,
            CreatedAt = createdUser.CreatedAt
        };
    }

    public async Task<UserDTO?> UpdateAsync(UpdateUserRequest request, CancellationToken ct = default)
    {
        var updatedUser = await _updateHandler.Handle(request, ct);
        if (updatedUser == null) return null;
        return new UserDTO
        {
            Id = updatedUser.Id,
            Firstname = updatedUser.Firstname,
            Lastname = updatedUser.Lastname,
            Username = updatedUser.Username,
            CreatedAt = updatedUser.CreatedAt
        };
    }

    public async Task DeleteAsync(DeleteUserRequest request, CancellationToken ct = default)
        => await _deleteHandler.Handle(request, ct);

    public async Task<UserDTO?> GetByIdAsync(GetUserByIdQuery query, CancellationToken ct = default)
    {
        var user = await _getByIdHandler.Handle(query, ct);
        if (user == null) return null;
        return new UserDTO
        {
            Id = user.Id,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<IReadOnlyList<UserDTO>> GetAllAsync(GetAllUsersQuery query, CancellationToken ct = default)
    {
        var users = await _getAllHandler.Handle(ct);
        return users.Select(user => new UserDTO
        {
            Id = user.Id,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        }).ToList();
    }
}
