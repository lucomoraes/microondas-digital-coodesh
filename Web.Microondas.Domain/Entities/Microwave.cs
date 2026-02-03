namespace Web.Microondas.Domain.Entities;

using Web.Microondas.Domain.Common;
using Web.Microondas.Domain.DomainException;
using Web.Microondas.Domain.Enums;

public sealed class Microwave : AggregateRoot
{
    public int? TimeInSeconds { get; private set; }
    public int Power { get; private set; } = 10;
    public char Character { get; private set; } = '.';
    public int RemainingSeconds { get; private set; }
    public int ElapsedSeconds { get; private set; }
    public HeatingState State { get; private set; } = HeatingState.Idle;
    public Guid? CurrentProgramId { get; private set; }
    public string Display { get; private set; } = string.Empty;

    public Microwave() { Id = Guid.NewGuid(); }

    public void QuickStart()
    {
        if (State == HeatingState.Running)
        {
            AddTime(30);
            return;
        }

        Setup(30, 10, '.', null);
        Run();
    }

    public void StartManual(int seconds, int power)
    {
        if (State == HeatingState.Running)
        {
            AddTime(30);
            return;
        }

        Setup(seconds, power, '.', null);
        Run();
    }

    public void StartProgram(HeatingProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);

        if (State == HeatingState.Running && CurrentProgramId.HasValue)
            throw new DomainException("Não é permitido adicionar tempo a programas pré-definidos.");

        if (State == HeatingState.Running)
        {
            AddTime(30);
            return;
        }

        Setup(program.TimeInSeconds, program.Power, program.Character, program.Id);
        Run();
    }

    public void Tick()
    {
        if (State != HeatingState.Running) return;

        ElapsedSeconds++;
        RemainingSeconds--;
        Display += new string(Character, Power);

        if (RemainingSeconds <= 0)
            Complete();
    }

    public void PauseCancelButton()
    {
        State = State switch
        {
            HeatingState.Running => HeatingState.Paused,
            HeatingState.Paused => Reset(),
            HeatingState.Idle => Reset(),
            _ => State
        };
    }

    public void Resume()
    {
        if (State == HeatingState.Paused)
            State = HeatingState.Running;
    }

    public string GetDisplayTime()
    {
        var min = RemainingSeconds / 60;
        var sec = RemainingSeconds % 60;
        return $"{min}:{sec:D2}";
    }

    public string GetFinalDisplay() =>
        State == HeatingState.Completed ? $"{Display} Aquecimento concluído" : Display;

    private void Setup(int seconds, int power, char character, Guid? programId)
    {
        TimeInSeconds = seconds;
        Power = power;
        Character = character;
        RemainingSeconds = seconds;
        CurrentProgramId = programId;
    }

    private void Run()
    {
        ElapsedSeconds = 0;
        Display = string.Empty;
        State = HeatingState.Running;
    }

    private void AddTime(int seconds)
    {
        if (CurrentProgramId.HasValue)
            throw new DomainException("Não é permitido adicionar tempo a programas pré-definidos.");

        RemainingSeconds = Math.Min(RemainingSeconds + seconds, 120);
    }

    private void Complete() => State = HeatingState.Completed;

    private HeatingState Reset()
    {
        TimeInSeconds = null;
        Power = 10;
        Character = '.';
        RemainingSeconds = 0;
        ElapsedSeconds = 0;
        Display = string.Empty;
        CurrentProgramId = null;
        return HeatingState.Idle;
    }
}