using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unlimitedinf.Tools.IntTests
{
    [TestClass]
    public partial class StringExtensionsTests
    {
        [TestMethod]
        public void StringEx_GetHashCode_Sha512()
        {
            string expected = "73475CB40A568E8DA8A045CED110137E159F890AC4DA883B6B17DC651B3A8049";
            string actual = "42".GetHashCode(Tools.Hashing.Hasher.Algorithm.SHA256);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StringEx_Parse_TooManyYears_Plus()
        {
            DateTime dt;
            var result = "50000000000 year".TryParseRelativeDateTime(out dt);
            Assert.IsFalse(result);
        }
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
