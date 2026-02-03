using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers;

public class GetAllHeatingProgramsHandler : IGetAllHeatingProgramsHandler
{
    private readonly IHeatingProgramRepository _repository;

    public GetAllHeatingProgramsHandler(IHeatingProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<HeatingProgramDTO>> Handle(CancellationToken ct = default)
    {
        var list = await _repository.GetAllAsync(ct);
        return list.Select(x => new HeatingProgramDTO
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
        }).ToList();
    }
}
