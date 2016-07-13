namespace Unlimitedinf.Tools
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class GenerateRandom
    {
        /// <summary>
        /// Generate a byte array filled with random bytes.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] Bytes(int length = 4)
        {
            byte[] random = new byte[length];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(random);
            }
            return random;
        }

        /// <summary>
        /// Get a base64 token.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Token(int length = 64)
        {
            long len = length * 3L / 4 + 1;
            byte[] random = GenerateRandom.Bytes((int)len);
            return Convert.ToBase64String(random).Substring(0, length);
        }

        /// <summary>
        /// Get a hexadecimal token.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string HexToken(int length = 64)
        {
            byte[] random = GenerateRandom.Bytes(length / 2 + 1);
            StringBuilder hex = new StringBuilder(length);
            foreach (byte b in random)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString().Substring(0, length);
        }
    }
}
