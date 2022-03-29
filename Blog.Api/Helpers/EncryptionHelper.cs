using System.Security.Cryptography;
using System.Text;

namespace Blog_Api.Helpers;

public static class EncryptionHelper
{
    public static string EncryptPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = $"{salt}{password}";
        var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
        return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
    }
}