namespace Unlimitedinf.Tools.Numerics.Int64.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class Int64ExtensionsTests
    {
        [TestMethod]
        public void IsPalindromeTest()
        {
            long[] successes = new long[] { 1, 11, 121, 8579521540451259758 };
            long[] failures = new long[] { 10, 47, 9223372036854775804 };

            foreach (long success in successes)
                Assert.IsTrue(success.IsPalindrome(), success.ToString());

            foreach (long failure in failures)
                Assert.IsFalse(failure.IsPalindrome(), failure.ToString());
        }

        [TestMethod]
        public void LengthTest()
        {
            long[] values = new long[]
            {
                1, 12, 123, 1234, 12345, 123456, 1234567, 12345678, 123456789, 1234567890, 12345678901,
                123456789012, 1234567890123, 12345678901234, 123456789012345, 1234567890123456,
                12345678901234567, 123456789012345678, 1234567890123456789
            };

            for (int i = 0; i < values.Length; i++)
                Assert.AreEqual(i + 1, values[i].Length());
        }

        [TestMethod]
        public void ContainsUniqueDigitsTest()
        {
            long[] successes = new long[] { 0, 1, 47, 9876543210, 398275104 };
            long[] failures = new long[] { 11, 100, 9223372036854775804 };

            foreach (long success in successes)
                Assert.IsTrue(success.ContainsUniqueDigits(), success.ToString());

            foreach (long failure in failures)
                Assert.IsFalse(failure.ContainsUniqueDigits(), failure.ToString());
        }

        [TestMethod]
        public void ContainsDigitTest()
        {
            long[] successes = new long[] { 10, 1, 564843315254, 33333333333, 400000000000, 1111111511 };
            long[] failures = new long[] { 11, 2, 987654310, 9227206854775804 };

            for (int i = 0; i < successes.Length; i++)
                Assert.IsTrue(successes[i].ContainsDigit(i), $"{successes[i]}.ContainsDigit({i})");

            for (int i = 0; i < failures.Length; i++)
                Assert.IsFalse(failures[i].ContainsDigit(i), $"{failures[i]}.ContainsDigit({i})");
        }

        [TestMethod]
        public void ReverseTest()
        {
            long[] values = new long[]
            {
                1, 12, 123, 1234, 12345, 123456, 1234567, 12345678, 123456789, 1234567890, 12345678901,
                123456789012, 1234567890123, 12345678901234, 123456789012345, 1234567890123456,
                12345678901234567, 123456789012345678
            };

            for (int i = 0; i < values.Length; i++)
                Assert.AreEqual(long.Parse(string.Join("", values[i].ToString().Reverse())), values[i].Reverse());
        }

        [TestMethod]
        public void ConcatTest()
        {
            long[] values = new long[]
            {
                1, 12, 123, 1234, 12345, 123456, 1234567, 12345678, 123456789, 1234567890
            };

            for (int i = 1; i < values.Length; i++)
                Assert.AreEqual(long.Parse($"{values[i-1]}{values[i]}"), values[i-1].Concat(values[i]));
        }
    }
}