using Web.Microondas.Application.UseCases.HeatingProgram.Commands;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;

public interface IDeleteHeatingProgramHandler
{
    Task Handle(DeleteHeatingProgramRequest request, CancellationToken ct = default);
}
