using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;
using Web.Microondas.Application.UseCases.HeatingProgram.Queries;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers;

public class GetHeatingProgramByIdHandler : IGetHeatingProgramByIdHandler
{
    private readonly IHeatingProgramRepository _repository;

    public GetHeatingProgramByIdHandler(IHeatingProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<HeatingProgramDTO?> Handle(GetHeatingProgramByIdQuery query, CancellationToken ct = default)
    {
        var x = await _repository.GetByIdAsync(query.Id, ct);
        if (x == null) return null;
        return new HeatingProgramDTO
        {
            Id = x.Id,
            Name = x.Name,
            Food = x.Food,
            TimeInSeconds = x.TimeInSeconds,
            Power = x.Power,
            Character = x.Character,
            Instructions = x.Instructions,
            IsPreset = x.IsPreset,
            CreatedAt = x.CreatedAt
        };
    }
}
