using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Unlimitedinf.Tools.Parsing;

namespace Unlimitedinf.Tools.IntTests.Parsing
{
    [TestClass]
    public class ExpressionParserTests
    {
        [TestMethod]
        public void WikipediaTest1()
        {
            double expected = 7;
            var input = "3+4";
            var actual = ExpressionParser.EvaluateAsExpression(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WikipediaTest1_Mod()
        {
            double expected = 70.2;
            var input = "30+40.2";
            var actual = ExpressionParser.EvaluateAsExpression(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WikipediaTest2()
        {
            double expected = 3+4*2/Math.Pow(1-5, Math.Pow(2, 3));
            var input = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
            var actual = ExpressionParser.EvaluateAsExpression(input);

            Assert.AreEqual(expected, actual);
        }

        // http://scriptasylum.com/tutorials/infix_postfix/algorithms/postfix-evaluation/index.htm
        [TestMethod]
        public void BlogTest1()
        {
            double expected = 5.2;
            var input = "2*3-4/5";
            var actual = ExpressionParser.EvaluateAsExpression(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
