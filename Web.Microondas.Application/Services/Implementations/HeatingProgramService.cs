using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Application.UseCases.HeatingProgram.Handlers.Interfaces;
using Web.Microondas.Application.UseCases.HeatingProgram.Queries;

namespace Web.Microondas.Application.Services.Implementations;

public class HeatingProgramService : IHeatingProgramService
{
    private readonly ICreateHeatingProgramHandler _createHandler;
    private readonly IDeleteHeatingProgramHandler _deleteHandler;
    private readonly IGetAllHeatingProgramsHandler _getAllHandler;
    private readonly IGetHeatingProgramByIdHandler _getByIdHandler;

    public HeatingProgramService(
        ICreateHeatingProgramHandler createHandler,
        IDeleteHeatingProgramHandler deleteHandler,
        IGetAllHeatingProgramsHandler getAllHandler,
        IGetHeatingProgramByIdHandler getByIdHandler)
    {
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
    }

    public async Task<HeatingProgramDTO?> CreateCustomAsync(CreateHeatingProgramRequest request, CancellationToken ct = default)
        => await _createHandler.Handle(request, ct);

    public async Task DeleteCustomAsync(DeleteHeatingProgramRequest request, CancellationToken ct = default)
        => await _deleteHandler.Handle(request, ct);

    public async Task<IReadOnlyList<HeatingProgramDTO>> GetAllAsync(CancellationToken ct = default)
        => await _getAllHandler.Handle(ct);

    public async Task<HeatingProgramDTO?> GetByIdAsync(GetHeatingProgramByIdQuery query, CancellationToken ct = default)
        => await _getByIdHandler.Handle(query, ct);
}