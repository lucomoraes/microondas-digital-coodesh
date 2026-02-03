using Web.Microondas.Application.UseCases.User.Commands;
using Web.Microondas.Application.UseCases.User.Handlers.Interfaces;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;
using Web.Microondas.Application.Helpers;

namespace Web.Microondas.Application.UseCases.User.Handlers;

public class UpdateUserHandler : IUpdateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Users?> Handle(UpdateUserRequest request, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            // Hash the password if provided
            var hashedPassword = string.IsNullOrEmpty(request.Password) 
                ? request.Password 
                : Sha256Helper.Hash(request.Password);
            
            var userToUpdate = new Users(request.Firstname, request.Lastname, request.Username, hashedPassword);
            await _userRepository.UpdateAsync(userToUpdate, request.Id, ct);
            await _unitOfWork.CommitAsync(ct);
            // Return the updated user (fetch by id)
            return await _userRepository.GetByIdAsync(request.Id, ct);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
