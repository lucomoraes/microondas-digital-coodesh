using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;

public interface ICreateHeatingProgramHandler
{
    Task<HeatingProgramDTO?> Handle(CreateHeatingProgramRequest request, CancellationToken ct = default);
}
