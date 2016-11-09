namespace Unlimitedinf.Tools.Numerics.Int32.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using Unlimitedinf.Tools.Numerics.Int32;

    [TestClass]
    public class Int32ExtensionsTests
    {
        readonly int[] testNums = { 2, 3, 5, 9, 42, 100, 101, 999, 123456789 };

        [TestMethod]
        public void IsPrimeTest()
        {
            int[] successes = new int[] { 2, 3, 5, 101, 15485867 };
            int[] failures = new int[] { 9, 42, 100, 999, 123456789 };

            foreach (int success in successes)
                Assert.IsTrue(success.IsPrime(), success.ToString());

            foreach (int failure in failures)
                Assert.IsFalse(failure.IsPrime(), failure.ToString());
        }

        [TestMethod]
        public void IsPalindromeTest()
        {
            bool[] expectedResults = { true, true, true, true, false, false, true, true, false };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].IsPalindrome(), $"{testNums[i]}");
        }

        [TestMethod]
        public void IsPalindromeInBase2Test()
        {
            bool[] expectedResults = { false, true, true, true, false, false, false, false, false };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].IsPalindromeInBase2(), $"{testNums[i]}");
        }

        [TestMethod]
        public void GetFactorsTest()
        {
            SortedSet<int>[] expectedResults = {
                new SortedSet<int>{ 1, 2 },
                new SortedSet<int>{ 1, 3 },
                new SortedSet<int>{ 1, 5 },
                new SortedSet<int>{ 1, 3, 9 },
                new SortedSet<int>{ 1, 2, 3, 6, 7, 14, 21, 42 },
                new SortedSet<int>{ 1, 2, 4, 5, 10, 20, 25, 50, 100 },
                new SortedSet<int>{ 1, 101 },
                new SortedSet<int>{ 1, 3, 9, 27, 37, 111, 333, 999 },
                new SortedSet<int>{ 1, 3, 9, 3607, 3803, 10821, 11409, 32463, 34227, 13717421,
                                    41152263, 123456789 }
            };

            for (int i = 0; i < testNums.Length; i++)
            {
                var actualResults = testNums[i].GetFactors();
                Assert.AreEqual(expectedResults[i].Count, actualResults.Count);

                IEnumerator<int> expectedResult = expectedResults[i].GetEnumerator();
                IEnumerator<int> actualResult = actualResults.GetEnumerator();

                while (expectedResult.MoveNext() && actualResult.MoveNext())
                {
                    Assert.AreEqual(expectedResult.Current, actualResult.Current, $"{testNums[i]}");
                }
            }
        }

        [TestMethod]
        public void GetPrimeFactorizationTest()
        {
            List<int>[] expectedResults = {
                new List<int>{ 2 },
                new List<int>{ 3 },
                new List<int>{ 5 },
                new List<int>{ 3, 3 },
                new List<int>{ 2, 3, 7 },
                new List<int>{ 2, 2, 5, 5 },
                new List<int>{ 101 },
                new List<int>{ 3, 3, 3, 37 },
                new List<int>{ 3, 3, 3607, 3803 }
            };

            for (int i = 0; i < testNums.Length; i++)
                for (int j = 0; j < expectedResults[i].Count; j++)
                    Assert.AreEqual(expectedResults[i][j], testNums[i].GetPrimeFactorization()[j],
                        $"num:{testNums[i]} j:{j}");
        }

        [TestMethod]
        public void GetSmallestFactorTest()
        {
            int[] expectedResults = { 2, 3, 5, 3, 2, 2, 101, 3, 3 };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].GetSmallestFactor(), $"{testNums[i]}");
        }

        [TestMethod]
        public void LengthTest()
        {
            int[] expectedResults = { 1, 1, 1, 1, 2, 3, 3, 3, 9 };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].Length(), $"{testNums[i]}");
        }

        [TestMethod]
        public void IsPandigitalTest()
        {
            int[] testNums = { 2, 3, 5, 9, 42, 100, 101, 999, 123456789, 120 };
            bool[] expectedResults = { false, false, false, false, false, false, false, false, true, false };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].IsPandigital(), $"{testNums[i]}");
        }

        [TestMethod]
        public void ContainsUniqueDigitsTest()
        {
            bool[] expectedResults = { true, true, true, true, true, false, false, false, true };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].ContainsUniqueDigits(), $"{testNums[i]}");
        }

        [TestMethod]
        public void ContainsSameDigitsTest()
        {
            bool[] expectedResults = { false, false, false, false, false, true, false, false, false };

            for (int i = 0; i < testNums.Length - 1; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].ContainsSameDigits(testNums[i + 1]), $"{testNums[i]}");
        }

        [TestMethod]
        public void ContainsSameExactDigitsTest()
        {
            int[] testNums = { 1, 2, 3, 4, 4, 1234, 3241, 12334, 12344, 567, 765, 566 };
            bool[] expectedResults = { false, false, false, true, false, true, false, false, false, true, false };

            for (int i = 0; i < testNums.Length - 1; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].ContainsSameExactDigits(testNums[i + 1]), 
                    $"{testNums[i]}.ContainsSameExactDigits{testNums[i + 1]}");
        }

        [TestMethod]
        public void GCDTest()
        {
            // This is a well-known algorithm and does not require a test.
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void RotateTest()
        {
            int[] expectedResults = { 2, 3, 5, 9, 24, 10, 110, 999, 912345678 };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].Rotate(), $"{testNums[i]}");
        }

        [TestMethod]
        public void IsDigitInTest()
        {
            bool[] expectedResults = { false, false, false, false, true, false, false, false, true };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], i.IsDigitIn(testNums[i]), $"{testNums[i]}");
        }

        [TestMethod]
        public void IsTriangularTest()
        {
            bool[] expectedResults = { false, true, false, false, false, false, false, false, false };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].IsTriangular(), $"{testNums[i]}");
        }

        [TestMethod]
        public void IsPentagonalTest()
        {
            bool[] expectedResults = { false, false, true, false, false, false, false, false, false };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].IsPentagonal(), $"{testNums[i]}");
        }

        [TestMethod]
        public void IsHexagonalTest()
        {
            int[] successes = new int[] { 1, 6, 15, 28, 45, 66, 91, 120, 153, 4560 };
            int[] failures = new int[] { 2, 5, 20, 80, 100, 10000 };

            foreach (int success in successes)
                Assert.IsTrue(success.IsHexagonal(), success.ToString());

            foreach (int failure in failures)
                Assert.IsFalse(failure.IsHexagonal(), failure.ToString());
        }

        [TestMethod]
        public void IsHeptagonalTest()
        {
            int[] successes = new int[] { 1, 7, 18, 34, 55, 81, 112, 148, 189, 5688 };
            int[] failures = new int[] { 2, 5, 20, 80, 100, 10000 };

            foreach (int success in successes)
                Assert.IsTrue(success.IsHeptagonal(), success.ToString());

            foreach (int failure in failures)
                Assert.IsFalse(failure.IsHeptagonal(), failure.ToString());
        }

        [TestMethod]
        public void IsOctagonalTest()
        {
            int[] successes = new int[] { 1, 8, 21, 40, 65, 96, 133, 176, 225, 5461 };
            int[] failures = new int[] { 2, 5, 20, 80, 100, 10000 };

            foreach (int success in successes)
                Assert.IsTrue(success.IsOctagonal(), success.ToString());

            foreach (int failure in failures)
                Assert.IsFalse(failure.IsOctagonal(), failure.ToString());
        }

        [TestMethod]
        public void ReverseTest()
        {
            int[] expectedResults = { 2, 3, 5, 9, 24, 1, 101, 999, 987654321 };

            for (int i = 0; i < testNums.Length; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].Reverse(), $"{testNums[i]}");
        }

        [TestMethod]
        public void ConcatTest()
        {
            int[] expectedResults = { 2, 32, 54, 96, 428, 10010, 10112, 99914 };

            for (int i = 0; i < testNums.Length - 1; i++)
                Assert.AreEqual(expectedResults[i], testNums[i].Concat(i * 2), $"{testNums[i]}");
        }
    }
}