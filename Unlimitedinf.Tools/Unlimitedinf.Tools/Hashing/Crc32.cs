// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2006/08/08/calculating_crc32_in_c_and_net
// Modified by Tom Postler, 2016-11-27
namespace Unlimitedinf.Tools.Hashing
{
#pragma warning disable CS3001 // Argument type is not CLS-compliant
#pragma warning disable CS3002 // Return type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    /// <summary>
    /// Implements a 32-bit CRC hash algorithm compatible with Zip etc.
    /// </summary>
    /// <remarks>
    /// Crc32 should only be used for backward compatibility with older file formats
    /// and algorithms. It is not secure enough for new applications.
    /// If you need to call multiple times for the same data either use the HashAlgorithm
    /// interface or remember that the result of one Compute call needs to be ~ (XOR) before
    /// being passed in as the seed for the next Compute call.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Crc")]
    public sealed class Crc32 : HashAlgorithm
    {
        /// <summary>
        /// Default CRC32 polynomial.
        /// </summary>
        public const UInt32 DefaultPolynomial = 0xedb88320u;
        /// <summary>
        /// Default CRC32 seed value.
        /// </summary>
        public const UInt32 DefaultSeed = 0xffffffffu;

        private static UInt32[] defaultTable;

        private readonly UInt32 seed;
        private readonly UInt32[] table;
        private UInt32 hash;

        /// <summary>
        /// Ctor.
        /// </summary>
        public Crc32()
            : this(DefaultPolynomial, DefaultSeed)
        {
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public Crc32(UInt32 polynomial, UInt32 seed)
        {
            table = InitializeTable(polynomial);
            this.seed = hash = seed;
        }

        /// <summary>
        /// See <see cref="HashAlgorithm.Initialize"/>.
        /// </summary>
        public override void Initialize()
        {
            hash = seed;
        }

        /// <summary>
        /// See <see cref="HashAlgorithm.Create()"/>.
        /// </summary>
        public new static Crc32 Create()
        {
            return new Crc32();
        }

        /// <summary>
        /// See <see cref="HashAlgorithm.HashCore(byte[], int, int)"/>.
        /// </summary>
        protected override void HashCore(byte[] buffer, int start, int length)
        {
            hash = CalculateHash(table, hash, buffer, start, length);
        }

        /// <summary>
        /// See <see cref="HashAlgorithm.HashFinal"/>.
        /// </summary>
        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt32ToBigEndianBytes(~hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        /// <summary>
        /// See <see cref="HashAlgorithm.HashSize"/>.
        /// </summary>
        public override int HashSize => 32;
        
        private static UInt32[] InitializeTable(UInt32 polynomial)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null)
                return defaultTable;

            var createTable = new UInt32[256];
            for (var i = 0; i < 256; i++)
            {
                var entry = (UInt32)i;
                for (var j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial)
                defaultTable = createTable;

            return createTable;
        }

        private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, IList<byte> buffer, int start, int size)
        {
            var crc = seed;
            for (var i = start; i < size - start; i++)
                crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
            return crc;
        }

        private static byte[] UInt32ToBigEndianBytes(UInt32 uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
    }
#pragma warning restore CS3001 // Argument type is not CLS-compliant
#pragma warning restore CS3002 // Return type is not CLS-compliant
#pragma warning restore CS3003 // Type is not CLS-compliant
}
