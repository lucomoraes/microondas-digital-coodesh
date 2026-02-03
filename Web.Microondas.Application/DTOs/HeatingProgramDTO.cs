namespace Web.Microondas.Application.DTOs;

public class HeatingProgramDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Food { get; set; } = string.Empty;
    public int TimeInSeconds { get; set; }
    public int Power { get; set; }
    public char Character { get; set; }
    public string? Instructions { get; set; }
    public bool IsPreset { get; set; }
    public DateTime CreatedAt { get; set; }
}
