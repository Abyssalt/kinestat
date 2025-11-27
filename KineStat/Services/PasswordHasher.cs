using System.Security.Cryptography;
using System.Text;

namespace KineStat.Services
{
    /// <summary>
    /// Service for hashing passwords with a high security
    /// Use PBKDF2 with automatic salt
    /// </summary>
    public class PasswordHasher
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 100000; // Recommandation OWASP

        /// <summary>
        /// Hash a password
        /// </summary>
        /// <param name="password">The password we want to hash</param>
        /// <returns>Hashed password, with salt (encoded in Base64)</returns>
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize); // Generate random salt (16 bytes)

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(     // Hash the password with PBKDF2
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verify if the hashed password correspond to the wanted password
        /// </summary>
        /// <param name="password">Clear password to compare</param>
        /// <param name="storedHash">Hashed password to compare</param>
        /// <returns>True if both matches, False otherwise</returns>
        public static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(storedHash);

                // Extract salt (16 first bytes)
                byte[] salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                // Hash the password with the same salt
                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256,
                    HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}