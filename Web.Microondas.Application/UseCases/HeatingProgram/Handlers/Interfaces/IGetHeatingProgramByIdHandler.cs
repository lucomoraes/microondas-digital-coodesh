using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.HeatingProgram.Queries;

namespace Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;

public interface IGetHeatingProgramByIdHandler
{
    Task<HeatingProgramDTO?> Handle(GetHeatingProgramByIdQuery query, CancellationToken ct = default);
}
