using System.Security.Cryptography;

namespace BulletinBoard.Login
{
    public static class LoginRoles
    {
        public const string User = "user";
        public const string Admin = "admin";

        // С правами (проверка внутри метода)
        public const string UserOrAdmin = User + "," + Admin;
    }

    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            var hashToCompare = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(
                hash, hashToCompare);
        }
    }
}