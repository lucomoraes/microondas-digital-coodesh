namespace Web.Microondas.Application.DTOs;

public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public UserDTO User { get; set; } = new();
}
