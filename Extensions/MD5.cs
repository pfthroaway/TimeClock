using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
    public class MD5
    {
        /// <summary>Converts a byte array to a string.</summary>
        /// <param name="arrayInput">Byte array to be converted</param>
        /// <returns>String from byte array</returns>
        private static string ByteToString(byte[] arrayInput)
        {
            StringBuilder strOutput = new StringBuilder(arrayInput.Length);

            foreach (byte input in arrayInput)
                strOutput.Append(input.ToString("X2"));

            return strOutput.ToString().ToLower();
        }

        /// <summary>Hashes text with MD5 encryption.</summary>
        /// <param name="text">Text to be encrypted</param>
        /// <returns>Hashed text</returns>
        public static string HashMD5(string text)
        {
            MD5CryptoServiceProvider objMD5 = new MD5CryptoServiceProvider();

            byte[] arrayData = Encoding.UTF8.GetBytes(text);
            byte[] arrayHash = objMD5.ComputeHash(arrayData);

            return ByteToString(arrayHash);
        }
    }
}