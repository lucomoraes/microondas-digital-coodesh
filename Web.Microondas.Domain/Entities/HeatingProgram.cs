using Web.Microondas.Domain.Common;

namespace Web.Microondas.Domain.Entities;

public sealed class HeatingProgram : AggregateRoot
{
    public string Name { get; set; } = null!;
    public string Food { get; set; } = null!;
    public int TimeInSeconds { get; set; }
    public int Power { get; set; }
    public char Character { get; set; }
    public string? Instructions { get; set; }
    public bool IsPreset { get; set; }
    public DateTime CreatedAt { get; set; }

    public HeatingProgram() { }

    public static HeatingProgram CreatePresetHeating(
        string name, string food, int timeInSeconds,
        int power, char character, string instructions)
    {
        return new HeatingProgram
        {
            Id = Guid.NewGuid(),
            Name = name,
            Food = food,
            TimeInSeconds = timeInSeconds,
            Power = power,
            Character = character,
            Instructions = instructions,
            IsPreset = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static HeatingProgram CreateCustomHeating(
        string name, string food, int timeInSeconds,
        int power, char character, string? instructions = null)
    {
        return new HeatingProgram
        {
            Id = Guid.NewGuid(),
            Name = name,
            Food = food,
            TimeInSeconds = timeInSeconds,
            Power = power,
            Character = character,
            Instructions = instructions,
            IsPreset = false,
            CreatedAt = DateTime.UtcNow
        };
    }
}