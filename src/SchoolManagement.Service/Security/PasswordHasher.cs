using System.Security.Cryptography;
using System.Text;

namespace SchoolManagement.Service.Security;

public static class PasswordHasher
{
    private const int DefaultIterations = 100_000;
    private const int SaltSize = 16; // 128-bit
    private const int KeySize = 32;  // 256-bit

    public static string Hash(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, DefaultIterations, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(KeySize);

        var saltB64 = Convert.ToBase64String(salt);
        var keyB64 = Convert.ToBase64String(key);
        return $"PBKDF2${DefaultIterations}${saltB64}${keyB64}";
    }

    public static bool Verify(string password, string hashed)
    {
        try
        {
            var parts = hashed.Split('$');
            if (parts.Length != 4 || parts[0] != "PBKDF2") return false;

            var iterations = int.Parse(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var expectedKey = Convert.FromBase64String(parts[3]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var actualKey = pbkdf2.GetBytes(expectedKey.Length);
            return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
        }
        catch
        {
            return false;
        }
    }
}