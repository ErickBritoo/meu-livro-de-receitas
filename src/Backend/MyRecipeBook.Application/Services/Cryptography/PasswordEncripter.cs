using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Cryptography;

public class PasswordEncripter
{
    public string Encrypt(string password)
    {
        const string additionalKey = "ABC";

        password = $"{password}{additionalKey}";
        
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = SHA512.HashData(bytes);

        return SringBytes(hashBytes);
    }

    private static string SringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        
        foreach (var b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}