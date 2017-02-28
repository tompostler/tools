using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Unlimitedinf.Tools.UnitTests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void StringEx_R_Year_Empty()
        {
            var match = StringExtensions.R.Year.Match("");

            Assert.IsFalse(match.Success);
            Assert.AreEqual(string.Empty, match.Value);
        }

        [TestMethod]
        public void StringEx_R_Year_Happy()
        {
            var match = StringExtensions.R.Year.Match("1 year");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("1 year", match.Value);
        }

        [TestMethod]
        public void StringEx_R_Year_Past()
        {
            var match = StringExtensions.R.Year.Match("last year");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("last year", match.Value);
            Assert.AreEqual(-1, StringExtensions.R.FromMatch(match));
        }

        [TestMethod]
        public void StringEx_R_Year_Present()
        {
            var match = StringExtensions.R.Year.Match("current year");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("current year", match.Value);
            Assert.AreEqual(0, StringExtensions.R.FromMatch(match));
        }

        [TestMethod]
        public void StringEx_R_Year_Future()
        {
            var match = StringExtensions.R.Year.Match("next year");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("next year", match.Value);
            Assert.AreEqual(1, StringExtensions.R.FromMatch(match));
        }

        [TestMethod]
        public void StringEx_R_Year_Mean()
        {
            var match = StringExtensions.R.Year.Match("1\t YeARs");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("1\t YeAR", match.Value);
        }

        [TestMethod]
        public void StringEx_R_Hour_Hour()
        {
            var match = StringExtensions.R.Hour.Match("1 hour");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("1 hour", match.Value);
        }

        [TestMethod]
        public void StringEx_R_Hour_Hour_Neg()
        {
            var match = StringExtensions.R.Hour.Match("-1 hour");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("-1 hour", match.Value);
            Assert.AreEqual(-1, StringExtensions.R.FromMatch(match));
        }

        [TestMethod]
        public void StringEx_R_Hour_Hr()
        {
            var match = StringExtensions.R.Hour.Match("1 HRs");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("1 HR", match.Value);
        }

        [TestMethod]
        public void StringEx_R_FromMatchValue()
        {
            var match = StringExtensions.R.Hour.Match("1 hour");

            Assert.AreEqual(1, StringExtensions.R.FromMatch(match.Value));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StringEx_R_FromMatchValue_Choke()
        {
            StringExtensions.R.FromMatch("This ain't no walk in the park.");
        }

        [TestMethod]
        public void StringEx_R_FromMatch()
        {
            var match = StringExtensions.R.Hour.Match("1 hour");

            Assert.AreEqual(1, StringExtensions.R.FromMatch(match));
        }
    }
}
