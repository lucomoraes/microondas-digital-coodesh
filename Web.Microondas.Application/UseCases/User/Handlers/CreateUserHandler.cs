using Web.Microondas.Application.UseCases.User.Commands;
using Web.Microondas.Application.UseCases.User.Handlers.Interfaces;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;
using Web.Microondas.Application.Helpers;

namespace Web.Microondas.Application.UseCases.User.Handlers;

public class CreateUserHandler : ICreateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateUserRequest request, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var hashedPassword = Sha256Helper.Hash(request.Password);
            var user = new Domain.Entities.Users(request.Firstname, request.Lastname, request.Username, hashedPassword);
            var brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            user.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);
            await _userRepository.AddAsync(user, ct);
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
