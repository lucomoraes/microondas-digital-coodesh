using Microsoft.Extensions.DependencyInjection;
using Web.Microondas.Application.DTOs;
using Web.Microondas.Application.Exceptions;
using Web.Microondas.Application.Services.Interfaces;
using Web.Microondas.Domain.Entities;
using Web.Microondas.Domain.Enums;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Application.Services.Implementations;

public class MicrowaveService : IMicrowaveService
{
    private Microwave _microwave = new();
    private readonly IServiceProvider _serviceProvider;

    public MicrowaveService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public MicrowaveDTO GetState() => new()
    {
        TimeInSeconds = _microwave.TimeInSeconds,
        Power = _microwave.Power,
        Character = _microwave.Character,
        RemainingSeconds = _microwave.RemainingSeconds,
        ElapsedSeconds = _microwave.ElapsedSeconds,
        State = _microwave.State.ToString(),
        CurrentProgramId = _microwave.CurrentProgramId,
        Display = _microwave.Display
    };

    public void QuickStart() => _microwave.QuickStart();
    
    public void StartManual(int seconds, int power) => _microwave.StartManual(seconds, power);
    
    public void StartProgram(Guid programId)
    {
        using var scope = _serviceProvider.CreateScope();
        var programRepository = scope.ServiceProvider.GetRequiredService<IHeatingProgramRepository>();
        
        var program = programRepository.GetByIdAsync(programId, CancellationToken.None).GetAwaiter().GetResult();
        if (program == null)
            throw new BusinessRuleException($"Programa de aquecimento com ID {programId} não encontrado.");
        
        _microwave.StartProgram(program);
    }
    
    public void Tick() => _microwave.Tick();
    
    public void PauseOrCancel() => _microwave.PauseCancelButton();
    
    public void Resume() => _microwave.Resume();
    
    public void Reset() => _microwave = new Microwave();
}