namespace Web.Microondas.Application.UseCases.HeatingProgram.Commands;

public class CreateHeatingProgramRequest
{
    public string Name { get; set; } = string.Empty;
    public string Food { get; set; } = string.Empty;
    public int TimeInSeconds { get; set; }
    public int Power { get; set; }
    public char Character { get; set; }
    public string? Instructions { get; set; }
}
