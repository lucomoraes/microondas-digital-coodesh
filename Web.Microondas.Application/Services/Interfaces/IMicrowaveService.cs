using Web.Microondas.Application.DTOs;

namespace Web.Microondas.Application.Services.Interfaces;

public interface IMicrowaveService
{
    MicrowaveDTO GetState();
    void QuickStart();
    void StartManual(int seconds, int power);
    void StartProgram(Guid programId);
    void Tick();
    void PauseOrCancel();
    void Resume();
    void Reset();
}