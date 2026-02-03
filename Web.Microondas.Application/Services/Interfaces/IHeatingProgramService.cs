using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.UseCases.HeatingProgram.Queries;

namespace Web.Microondas.Application.Services.Interfaces;

public interface IHeatingProgramService
{
    Task<HeatingProgramDTO?> CreateCustomAsync(CreateHeatingProgramRequest request, CancellationToken ct = default);
    Task DeleteCustomAsync(DeleteHeatingProgramRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<HeatingProgramDTO>> GetAllAsync(CancellationToken ct = default);
    Task<HeatingProgramDTO?> GetByIdAsync(GetHeatingProgramByIdQuery query, CancellationToken ct = default);
}