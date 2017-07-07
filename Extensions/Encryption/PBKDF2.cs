using System;
using System.Security.Cryptography;

namespace Extensions.Encryption
{
    /// <summary>Handles hashing and validating passwords using PBKDF2.</summary>
    public class PBKDF2
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash
        private const int Pbkdf2Iterations = 1000;
        private const int IterationIndex = 0;
        private const int SaltIndex = 1;
        private const int Pbkdf2Index = 2;

        /// <summary>Hashes the passed password.</summary>
        /// <param name="password">Plaintext password</param>
        /// <returns>Hashed password</returns>
        public static string HashPassword(string password)
        {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            byte[] hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return $"{Pbkdf2Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        /// <summary>Validates the plaintext password against the hashed password.</summary>
        /// <param name="password">Plaintext password</param>
        /// <param name="correctHash">Hashed correct password</param>
        /// <returns>Returns true if successful validation</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            int iterations = int.Parse(split[IterationIndex]);
            byte[] salt = Convert.FromBase64String(split[SaltIndex]);
            byte[] hash = Convert.FromBase64String(split[Pbkdf2Index]);

            byte[] testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>Slowly hashes password to prevent quick attacks.</summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        /// <summary>Returns an array of Bytes based on password.</summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <param name="outputBytes"></param>
        /// <returns></returns>
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = iterations };
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}