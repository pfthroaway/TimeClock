using Sodium;

namespace Extensions
{
    /// <summary>Provides an extension into the libsodium Argon2 hashing scheme.</summary>
    public static class Argon2
    {/// <summary>Hashes a password using the Argon2 hashing scheme.</summary>
     /// <param name="password">Password to be hashed</param>
     /// <returns>Hashed password</returns>
        public static string HashPassword(string password)
        {
            return PasswordHash.ArgonHashString(password);
        }

        /// <summary>Verifies a password with its hashed counterpart.</summary>
        /// <param name="hashedPassword">Hashed password</param>
        /// <param name="password">Plaintext password</param>
        /// <returns>Returns result of validation</returns>
        public static bool ValidatePassword(string hashedPassword, string password)
        {
            return PasswordHash.ArgonHashStringVerify(hashedPassword, password);
        }
    }
}