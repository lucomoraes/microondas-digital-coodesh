using Web.Microondas.Domain.Common;

namespace Web.Microondas.Domain.Entities;

public class Users : AggregateRoot
{  
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.MinValue;

    public Users() { }
    
    public Users(string firstname, string lastname, string username, string password)
    {
        Firstname = firstname;
        Lastname = lastname;
        Username = username;
        Password = password;
    }
}
