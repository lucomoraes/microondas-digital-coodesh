using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers;

public class DeleteHeatingProgramHandler : IDeleteHeatingProgramHandler
{
    private readonly IHeatingProgramRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHeatingProgramHandler(IHeatingProgramRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteHeatingProgramRequest request, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _repository.DeleteAsync(request.Id, ct);
            await _unitOfWork.CommitAsync(ct);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
