namespace Web.Microondas.Application.DTOs;

public class MicrowaveDTO
{
    public int? TimeInSeconds { get; set; }
    public int Power { get; set; }
    public char Character { get; set; }
    public int RemainingSeconds { get; set; }
    public int ElapsedSeconds { get; set; }
    public string State { get; set; } = string.Empty;
    public Guid? CurrentProgramId { get; set; }
    public string Display { get; set; } = string.Empty;
}