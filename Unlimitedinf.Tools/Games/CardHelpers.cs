using System;
using System.Collections.Generic;
using System.Linq;

namespace Unlimitedinf.Tools.Games
{
    /// <summary>
    /// Helper methods for the card enums
    /// </summary>
    public static class CardHelpers
    {
        /// <summary>
        /// Convert a <see cref="Cards"/> value to a nice string. E.g. 'Ace of Spades'
        /// </summary>
        public static string ToNiceString(this Cards card)
        {
            return $"{Enum.GetName(typeof(CardNumber), card.ToNumber())} of {Enum.GetName(typeof(CardSuit), card.ToSuit())}";
        }

        /// <summary>
        /// Convert a <see cref="Cards"/> value to a <see cref="CardNumber"/>.
        /// </summary>
        public static CardNumber ToNumber(this Cards card)
        {
            return (CardNumber)((int)card & 0xF);
        }

        /// <summary>
        /// Convert a <see cref="Cards"/> value to a <see cref="CardSuit"/>.
        /// </summary>
        public static CardSuit ToSuit(this Cards card)
        {
            return (CardSuit)((int)card & 0xF0);
        }

        /// <summary>
        /// Convert the result of <see cref="ScoreHand(IEnumerable{Cards})"/> to a <see cref="ScoreHandType"/>.
        /// </summary>
        public static ScoreHandType ScoreHand(int score)
        {
            return (ScoreHandType)(score & (int)ScoreHandType.Mask);
        }

        /// <summary>
        /// Score a five-card hand in a way that will sort easily from highest to lowest.
        /// </summary>
        public static int ScoreHand(IEnumerable<Cards> hand)
        {
            if (hand == null)
                throw new ArgumentNullException(nameof(hand));
            if (hand.Count() != 5)
                throw new ArgumentException("Must be exactly 5 cards in hand.", nameof(hand));

            return ScoreHandInternal(hand);
        }

        /// <summary>
        /// Score a five-card hand in a way that will sort easily from highest to lowest.
        /// Distance bewteen score values is meaningless.
        /// </summary>
        /// <remarks>
        /// The resulting score is an integer. The integer is assembled from a nibble representing each column below:
        /// [0-9][2-E]{5}
        /// 
        /// ORDER           DETERMINATOR 1  DETERMINATOR 2  D3  D4  D5
        /// ==============  ==============  ==============  ==  ==  ==
        /// Royal flush
        /// Straight flush  High card
        /// 4 of a kind     4 card value    (can't tie)
        /// Full house      3 card value    (can't tie)
        /// Flush           High card       High card       HC  HC  HC
        /// Straight        High card       (can tie)
        /// 3 of a kind     3 card value    (can't tie)
        /// 2 pairs         High pair       Low pair        HC
        /// 1 pair          Pair value      High card       HC  HC
        /// Nothing         High card       High card       HC  HC  HC
        /// </remarks>
        private static int ScoreHandInternal(IEnumerable<Cards> hand)
        {
            // Check if flush
            var suits = hand.Select(_ => _.ToSuit());
            var firstSuit = suits.First();
            var isFlush = suits.All(_ => _ == firstSuit);

            // Check if straight
            var numbers = hand.Select(_ => _.ToNumber()).ToList();
            numbers.Sort();
            var isStraight = (numbers[0] == numbers[1] - 1
                && numbers[1] == numbers[2] - 1
                && numbers[2] == numbers[3] - 1
                && numbers[3] == numbers[4] - 1);

            // Royal flush and straight flush
            if (isFlush && isStraight)
                if (numbers[4] == CardNumber.Ace)
                    return 0x900000;
                else
                    return 0x800000 | (int)numbers[4];

            // 4 of a kind
            // Since numbers are sorted, only need to check if 0 == 3 or 1 == 4
            if (numbers[0] == numbers[3])
                return 0x700000 | (int)numbers[0];
            if (numbers[1] == numbers[4])
                return 0x700000 | (int)numbers[1];

            // Full house
            // Since numbers are sorted, we can just check if 2 == 0 or 2 == 4 and then the remaining two
            if (numbers[0] == numbers[2] && numbers[3] == numbers[4])
                return 0x600000 | (int)numbers[2];
            if (numbers[0] == numbers[1] && numbers[2] == numbers[4])
                return 0x600000 | (int)numbers[2];

            // Flush
            if (isFlush)
                return 0x500000 | (int)numbers[4] << 16 | (int)numbers[3] << 12 | (int)numbers[2] << 8 | (int)numbers[1] << 4 | (int)numbers[0];

            // Straight
            if (isStraight)
                return 0x400000 | (int)numbers[4];

            // 3 of a kind
            // Since numbers are sorted, we can just check if 0 == 2, 1 == 3, or 2 == 4
            if (numbers[0] == numbers[2] || numbers[1] == numbers[3] || numbers[2] == numbers[4])
                return 0x300000 | (int)numbers[2];

            // 2 pairs
            // Since numbers are sorted, it'll either be 01/23, 01/34, 12/34
            if (numbers[0] == numbers[1] && numbers[2] == numbers[3])
                return 0x200000 | (int)numbers[2] << 8 | (int)numbers[0] << 4 | (int)numbers[4];
            if (numbers[0] == numbers[1] && numbers[3] == numbers[4])
                return 0x200000 | (int)numbers[3] << 8 | (int)numbers[0] << 4 | (int)numbers[2];
            if (numbers[1] == numbers[2] && numbers[3] == numbers[4])
                return 0x200000 | (int)numbers[3] << 8 | (int)numbers[1] << 4 | (int)numbers[0];

            // 1 pair
            if (numbers[0] == numbers[1])
                return 0x100000 | (int)numbers[0] << 12 | (int)numbers[4] << 8 | (int)numbers[3] << 4 | (int)numbers[2];
            if (numbers[1] == numbers[2])
                return 0x100000 | (int)numbers[1] << 12 | (int)numbers[4] << 8 | (int)numbers[3] << 4 | (int)numbers[0];
            if (numbers[2] == numbers[3])
                return 0x100000 | (int)numbers[2] << 12 | (int)numbers[4] << 8 | (int)numbers[1] << 4 | (int)numbers[0];
            if (numbers[3] == numbers[4])
                return 0x100000 | (int)numbers[3] << 12 | (int)numbers[2] << 8 | (int)numbers[1] << 4 | (int)numbers[0];

            // Nothing
            return (int)numbers[4] << 16 | (int)numbers[3] << 12 | (int)numbers[2] << 8 | (int)numbers[1] << 4 | (int)numbers[0];
        }
    }
}
