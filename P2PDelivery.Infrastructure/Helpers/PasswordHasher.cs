using System.Security.Cryptography;

namespace P2PDelivery.Infrastructure.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            var salt = GenerateSalt();
            var hashedPassword = HashPasswordWithSalt(password, salt);

            return $"{salt}:{hashedPassword}";

        }


        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
                return false;

            var salt = parts[0];
            var hash = parts[1];

            var computedHash = HashPasswordWithSalt(password, salt);

            return hash.SequenceEqual(computedHash);
        }


        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPasswordWithSalt(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32)); // 32-byte hash
            }
        }

    }
}
