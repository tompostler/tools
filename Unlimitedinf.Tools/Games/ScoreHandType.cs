using System.Collections.Generic;

namespace Unlimitedinf.Tools.Games
{
    /// <summary>
    /// When used with <see cref="CardHelpers.ScoreHand(IEnumerable{Cards})"/>, will indicate the type of hand.
    /// </summary>
    public enum ScoreHandType
    {
        /// <summary>
        /// When no hands of meaning are available. High cards break ties.
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// One pair. High pair wins, then high cards break ties.
        /// </summary>
        OnePair = 0x100000,
        /// <summary>
        /// Two pairs. High pair, then low pair, then high card to break ties.
        /// </summary>
        TwoPair = 0x200000,
        /// <summary>
        /// 3 cards of the same numerical value. The higher 3 breaks ties.
        /// </summary>
        ThreeOfAKind = 0x300000,
        /// <summary>
        /// All 5 cards are in a numerical sequence. The highest of the 5 breaks ties.
        /// </summary>
        Straight = 0x400000,
        /// <summary>
        /// All 5 cards are of the same suit. High cards break ties.
        /// </summary>
        Flush = 0x500000,
        /// <summary>
        /// 3 cards are of the same numerical value, and the other 2 are as well. The higher 3 breaks ties.
        /// </summary>
        FullHouse = 0x600000,
        /// <summary>
        /// 4 cards of the same numerical value. The higher 4 breaks ties.
        /// </summary>
        FourOfAKind = 0x700000,
        /// <summary>
        /// All 5 cards are in a numerical sequence and of the same suit. The highest care breaks ties.
        /// </summary>
        StraightFlush = 0x800000,
        /// <summary>
        /// All 5 cards are in numerical sequence, of the same suit, and the highest card is an Ace.
        /// </summary>
        RoyalFlush = 0x900000,

        /// <summary>
        /// When anded with the result of <see cref="CardHelpers.ScoreHand(IEnumerable{Cards})"/>, can then be converted into this type.
        /// </summary>
        Mask = 0xF00000
    }
}
