using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;
using Web.Microondas.Domain.Interfaces.Repository;
using Web.Microondas.Domain.Interfaces.UnitOfWork;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers;

public class CreateHeatingProgramHandler : ICreateHeatingProgramHandler
{
    private readonly IHeatingProgramRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHeatingProgramHandler(IHeatingProgramRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<HeatingProgramDTO?> Handle(CreateHeatingProgramRequest request, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var entity = Domain.Entities.HeatingProgram.CreateCustomHeating(
                request.Name, request.Food, request.TimeInSeconds, request.Power, request.Character, request.Instructions);
            await _repository.AddAsync(entity, ct);
            await _unitOfWork.CommitAsync(ct);
            return new HeatingProgramDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Food = entity.Food,
                TimeInSeconds = entity.TimeInSeconds,
                Power = entity.Power,
                Character = entity.Character,
                Instructions = entity.Instructions,
                IsPreset = entity.IsPreset,
                CreatedAt = entity.CreatedAt
            };
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
