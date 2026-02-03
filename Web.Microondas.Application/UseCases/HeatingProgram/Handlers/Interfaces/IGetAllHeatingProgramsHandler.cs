using Web.Microondas.Application.DTOs;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;

public interface IGetAllHeatingProgramsHandler
{
    Task<IReadOnlyList<HeatingProgramDTO>> Handle(CancellationToken ct = default);
}
