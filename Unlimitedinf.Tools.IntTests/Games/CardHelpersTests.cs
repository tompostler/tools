using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Unlimitedinf.Tools.Games;

namespace Unlimitedinf.Tools.IntTests.Games
{
    [TestClass]
    public partial class CardHelpersTests
    {
        [TestMethod]
        public void CardsToString()
        {
            // One normal check for sanity
            Assert.AreEqual("Queen of Spades", Cards.SQ.ToNiceString());

            // And then check all of them
            foreach (var cardSuitName in Enum.GetNames(typeof(CardSuit)))
                foreach (var cardNumberName in Enum.GetNames(typeof(CardNumber)))
                    Assert.AreEqual(
                        $"{cardNumberName} of {cardSuitName}",
                        CardHelpers.ToNiceString((Cards)((int)Enum.Parse(typeof(CardSuit), cardSuitName) | (int)Enum.Parse(typeof(CardNumber), cardNumberName))));
        }
    }
}
