/* Perceptual image hash calculation tool based on algorithm descibed in
 * Block Mean Value Based Image Perceptual Hashing by Bian Yang, Fan Gu and Xiamu Niu
 *
 * Copyright 2014 Commons Machinery http://commonsmachinery.se/
 * Distributed under an MIT license, please see LICENSE in the top dir.
 */
// Source drawn from https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c on 2016-11-27
// Modified under the MIT license by Tom Postler to work with C#

namespace Unlimitedinf.Tools.Hashing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Perceptual image hash calculation tool based on algorithm descibed in 
    /// Block Mean Value Based Image Perceptual Hashing by Bian Yang, Fan Gu and Xiamu Niu.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Blockhash")]
    public sealed class Blockhash : HashAlgorithm
    {
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }
    }
}
