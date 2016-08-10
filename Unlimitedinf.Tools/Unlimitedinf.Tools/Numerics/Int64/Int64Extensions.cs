namespace Unlimitedinf.Tools.Numerics.Int64
{
    /// <summary>
    /// Extensions for the mighty Int64. If it says >0, then it does not check for &lt;0.
    /// </summary>
    public static class Int64Extensions
    {
        /// <summary>
        /// Checks if integers &gt;0 are palindomatic.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPalindrome(this long number)
        {
            long rev = 0, dig, orig = number;

            while (number > 0)
            {
                dig = number % 10;
                rev = rev * 10 + dig;
                number /= 10;
            }

            return rev == orig;
        }

        /// <summary>
        /// Calculate the number of digits in integers &gt;0.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Length(this long number)
        {
            int count = 0;
            while (number > 0)
            {
                count++;
                number /= 10;
            }
            return count;
        }

        /// <summary>
        /// An integer contains unique digits if no digits in the num repeat.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool ContainsUniqueDigits(this long number)
        {
            bool[] digits = new bool[10];

            while (number > 0)
            {
                int digit = (int)(number % 10);
                if (digits[digit])
                    return false;
                digits[digit] = true;
                number /= 10;
            }

            // Must have succeeded
            return true;
        }

        /// <summary>
        /// Checks if a digit is within a number.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="containingDigit"></param>
        /// <returns></returns>
        public static bool ContainsDigit(this long number, long containingDigit)
        {
            while (number > 0)
            {
                int digit = (int)(number % 10);
                if (digit == containingDigit)
                    return true;
                number /= 10;
            }

            // Must not be contained
            return false;
        }

        /// <summary>
        /// Reverses the digits of an integer.
        /// </summary>
        /// <example>78205.Reverse() == 50287</example>
        /// <param name="number"></param>
        /// <returns></returns>
        public static long Reverse(this long number)
        {
            int length = number.Length();
            if (length == 1)
                return number;

            long newNum = 0;
            for (int i = 0; i < length; i++)
            {
                newNum *= 10;
                newNum += number % 10;
                number /= 10;
            }

            return newNum;
        }

        /// <summary>
        /// Concatenate two integers together. Does not check for overflow.
        /// </summary>
        /// <example>45.Concat(87) == 4587</example>
        /// <param name="number"></param>
        /// <param name="otherNumber"></param>
        /// <returns></returns>
        public static long Concat(this long number, long otherNumber)
        {
            int len = otherNumber.Length();
            for (int i = 0; i < len; i++)
                number *= 10;
            return number + otherNumber;
        }
    }
}
