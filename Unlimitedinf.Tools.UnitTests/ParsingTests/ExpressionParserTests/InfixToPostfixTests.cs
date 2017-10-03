using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Unlimitedinf.Tools.Parsing;

namespace Unlimitedinf.Tools.UnitTests.ParsingTests.ExpressionParserTests
{
    [TestClass]
    public class InfixToPostfixTests
    {
        [TestMethod]
        public void WikipediaTest1()
        {
            var input = "3+4";
            var expected = new List<string>{ "3", "4", "+" };
            var actual = ExpressionParser.ConvertInfixToPostfix(input);

            CAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WikipediaTest1_Mod()
        {
            var input = "30+40.2";
            var expected = new List<string>{ "30", "40.2", "+" };
            var actual = ExpressionParser.ConvertInfixToPostfix(input);

            CAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WikipediaTest2()
        {
            var input = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
            var expected = new List<string>{ "3", "4", "2", "*", "1", "5", "-", "2", "3", "^", "^", "/", "+" };
            var actual = ExpressionParser.ConvertInfixToPostfix(input);

            CAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlogTest1()
        {
            // http://scriptasylum.com/tutorials/infix_postfix/algorithms/postfix-evaluation/index.htm
            var input = "2*3-4/5";
            var expected = new List<string>{ "2", "3", "*", "4", "5", "/", "-" };
            var actual = ExpressionParser.ConvertInfixToPostfix(input);

            CAssert.AreEqual(expected, actual);
        }
    }
}
