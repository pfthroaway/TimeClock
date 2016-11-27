using System;
using System.Security.Cryptography;

namespace TimeClock
{
    public class PasswordHash
    {
        public const int SaltByteSize = 24;
        public const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash
        public const int Pbkdf2Iterations = 1000;
        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        /// <summary>
        /// Hashes the passed password.
        /// </summary>
        /// <param name="password">Plaintext password</param>
        /// <returns>Hashed password</returns>
        public static string HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Validates the plaintext password against the hashed password.
        /// </summary>
        /// <param name="password">Plaintext password</param>
        /// <param name="correctHash">Hashed correct password</param>
        /// <returns>Hashed password</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = Int32.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

            var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Slowly hashes password to prevent quick attacks.
        /// </summary>
        /// <param name="a">Array of bytes</param>
        /// <param name="b">Array of bytes</param>
        /// <returns></returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// Returns an array of Bytes based on password.
        /// </summary>
        /// <param name="password">Unhashed password</param>
        /// <param name="salt">Extra characters</param>
        /// <param name="iterations">Number of iterations</param>
        /// <param name="outputBytes"></param>
        /// <returns></returns>
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}