using System.Security.Cryptography;
using System.Text;

namespace Web.Microondas.Application.Helpers;

public static class Sha256Helper
{
    public static string Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
}
