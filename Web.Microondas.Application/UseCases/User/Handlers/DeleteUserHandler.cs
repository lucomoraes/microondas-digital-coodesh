using Web.Microondas.Application.UseCases.User.Commands;
using Web.Microondas.Application.UseCases.User.Handlers.Interfaces;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;

namespace Web.Microondas.Application.UseCases.User.Handlers;

public class DeleteUserHandler : IDeleteUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUserRequest request, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _userRepository.DeleteAsync(request.Id, ct);
            await _unitOfWork.CommitAsync(ct);
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
