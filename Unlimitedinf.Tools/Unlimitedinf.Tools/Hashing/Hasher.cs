namespace Unlimitedinf.Tools.Hashing
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// A wrapper for common hashing functions to operate on streams or files.
    /// </summary>
    public sealed class Hasher
    {
        /// <summary>
        /// Helper to make it obvious which hash algorithm we're looking for.
        /// </summary>
        public enum Algorithm
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            MD5,
            Crc32,
            SHA1,
            SHA256,
            SHA512,
            Blockhash   // http://blockhash.io/
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        private Algorithm algorithm;
        private HashAlgorithm hashAlgorithm;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="algorithm"></param>
        public Hasher(Algorithm algorithm)
        {
            this.algorithm = algorithm;
            this.ResetAlgorithm();
        }

        /// <summary>
        /// Ctor that by default discards any setup you may have done to your choice of algorithm.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="preserve"></param>
        public Hasher(HashAlgorithm algorithm, bool preserve = false)
        {
            if (algorithm is MD5)
                this.algorithm = Algorithm.MD5;
            else if (algorithm is Crc32)
                this.algorithm = Algorithm.Crc32;
            else if (algorithm is SHA1)
                this.algorithm = Algorithm.SHA1;
            else if (algorithm is SHA256)
                this.algorithm = Algorithm.SHA256;
            else if (algorithm is SHA512)
                this.algorithm = Algorithm.SHA512;
            else if (algorithm is Blockhash)
                this.algorithm = Algorithm.Blockhash;

            if (preserve)
                this.hashAlgorithm = algorithm;
            else
                this.ResetAlgorithm();
        }

        /// <summary>
        /// Resets the base algorithm to be fresh and unaffected.
        /// </summary>
        private void ResetAlgorithm()
        {
            switch (this.algorithm)
            {
                case Algorithm.Blockhash:
                    this.hashAlgorithm = Blockhash.Create();
                    break;
                case Algorithm.Crc32:
                    this.hashAlgorithm = Crc32.Create();
                    break;
                case Algorithm.MD5:
                    this.hashAlgorithm = MD5.Create();
                    break;
                case Algorithm.SHA1:
                    this.hashAlgorithm = SHA1.Create();
                    break;
                case Algorithm.SHA256:
                    this.hashAlgorithm = SHA256.Create();
                    break;
                case Algorithm.SHA512:
                    this.hashAlgorithm = SHA512.Create();
                    break;
            }
        }

        /// <summary>
        /// Compute the hash from a given stream.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public byte[] ComputeHash(Stream inputStream)
        {
            // Temporary workaround warranting further investigation.
            if (this.algorithm == Algorithm.Blockhash)
                return Blockhash.ComputeHash(inputStream);
            else
                return this.hashAlgorithm.ComputeHash(inputStream);
        }

        /// <summary>
        /// Compute the hash from a given stream and return it as a string.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public string ComputeHashS(Stream inputStream)
        {
            return BitConverter.ToString(this.ComputeHash(inputStream)).Replace("-", "");
        }
    }
}
