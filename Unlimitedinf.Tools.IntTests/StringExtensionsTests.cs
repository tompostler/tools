using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unlimitedinf.Tools.IntTests
{
    [TestClass]
    public partial class StringExtensionsTests
    {
    }

    public static class Dassert
    {
        /// <summary>
        /// Check if two DateTimes are equal (i.e. within one second of each other).
        /// </summary>
        public static void AreEqual(DateTime expected, DateTime actual)
        {
            var diff = (actual - expected).Duration();
            if (diff.TotalSeconds > 1)
                throw new AssertFailedException($"Dassert.AreEqual failed. Expected:<{expected}>. Actual:<{actual}>.");
        }
    }
}
