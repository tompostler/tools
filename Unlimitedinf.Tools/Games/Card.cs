using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlimitedinf.Tools.Games
{
    /// <summary>
    /// Card number, where value is equal to the number position. Ace high.
    /// </summary>
    public enum CardNumber
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 0xA,
        Jack = 0xB,
        Queen = 0xC,
        King = 0xD,
        Ace = 0xE
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Card suit. Sorted alphabetically lowest to highest.
    /// </summary>
    public enum CardSuit
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Clubs = 0x10,
        Diamonds = 0x20,
        Hearts = 0x40,
        Spades = 0x80
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Full deck of cards, ordered lowest to highest by suit and then by value.
    /// </summary>
    public enum Cards
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        C2 = CardSuit.Clubs | CardNumber.Two,
        C3 = CardSuit.Clubs | CardNumber.Three,
        C4 = CardSuit.Clubs | CardNumber.Four,
        C5 = CardSuit.Clubs | CardNumber.Five,
        C6 = CardSuit.Clubs | CardNumber.Six,
        C7 = CardSuit.Clubs | CardNumber.Seven,
        C8 = CardSuit.Clubs | CardNumber.Eight,
        C9 = CardSuit.Clubs | CardNumber.Nine,
        CT = CardSuit.Clubs | CardNumber.Ten,
        CJ = CardSuit.Clubs | CardNumber.Jack,
        CQ = CardSuit.Clubs | CardNumber.Queen,
        CK = CardSuit.Clubs | CardNumber.King,
        CA = CardSuit.Clubs | CardNumber.Ace,

        D2 = CardSuit.Diamonds | CardNumber.Two,
        D3 = CardSuit.Diamonds | CardNumber.Three,
        D4 = CardSuit.Diamonds | CardNumber.Four,
        D5 = CardSuit.Diamonds | CardNumber.Five,
        D6 = CardSuit.Diamonds | CardNumber.Six,
        D7 = CardSuit.Diamonds | CardNumber.Seven,
        D8 = CardSuit.Diamonds | CardNumber.Eight,
        D9 = CardSuit.Diamonds | CardNumber.Nine,
        DT = CardSuit.Diamonds | CardNumber.Ten,
        DJ = CardSuit.Diamonds | CardNumber.Jack,
        DQ = CardSuit.Diamonds | CardNumber.Queen,
        DK = CardSuit.Diamonds | CardNumber.King,
        DA = CardSuit.Diamonds | CardNumber.Ace,

        H2 = CardSuit.Hearts | CardNumber.Two,
        H3 = CardSuit.Hearts | CardNumber.Three,
        H4 = CardSuit.Hearts | CardNumber.Four,
        H5 = CardSuit.Hearts | CardNumber.Five,
        H6 = CardSuit.Hearts | CardNumber.Six,
        H7 = CardSuit.Hearts | CardNumber.Seven,
        H8 = CardSuit.Hearts | CardNumber.Eight,
        H9 = CardSuit.Hearts | CardNumber.Nine,
        HT = CardSuit.Hearts | CardNumber.Ten,
        HJ = CardSuit.Hearts | CardNumber.Jack,
        HQ = CardSuit.Hearts | CardNumber.Queen,
        HK = CardSuit.Hearts | CardNumber.King,
        HA = CardSuit.Hearts | CardNumber.Ace,

        S2 = CardSuit.Spades | CardNumber.Two,
        S3 = CardSuit.Spades | CardNumber.Three,
        S4 = CardSuit.Spades | CardNumber.Four,
        S5 = CardSuit.Spades | CardNumber.Five,
        S6 = CardSuit.Spades | CardNumber.Six,
        S7 = CardSuit.Spades | CardNumber.Seven,
        S8 = CardSuit.Spades | CardNumber.Eight,
        S9 = CardSuit.Spades | CardNumber.Nine,
        ST = CardSuit.Spades | CardNumber.Ten,
        SJ = CardSuit.Spades | CardNumber.Jack,
        SQ = CardSuit.Spades | CardNumber.Queen,
        SK = CardSuit.Spades | CardNumber.King,
        SA = CardSuit.Spades | CardNumber.Ace,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
