namespace Unlimitedinf.Tools.Numerics.Int64
{
    using System;
    using Unlimitedinf.Tools.Statics.Numerics;

    /// <summary>
    /// Extensions for the mighty Int64. If it says >0, then it does not check for &lt;0.
    /// </summary>
    public static class Int64Extensions
    {
        /// <summary>
        /// Checks if integers >0 are prime.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPrime(this long number)
        {
            if (number == 2)
                return true;
            if (number == 1 || number % 2 == 0)
                return false;
            if (number < Primes.Max)
                return Primes.Unordered.Contains((int)number);

            long boundary = (long)Math.Ceiling(Math.Sqrt(number));

            if (boundary <= Primes.Max)
                foreach (int prime in Primes.Ordered)
                {
                    // We've exhausted the possible factors of our number
                    if (prime > boundary)
                        return true;
                    // Found a factor
                    if (number % prime == 0)
                        return false;
                }

            for (long i = Primes.Max + 2; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

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
        /// Gets the smallest factor (other than 1) of an integer >1.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static long GetSmallestFactor(this long number)
        {
            if (number % 2 == 0)
                return 2;
            if (number < Primes.Max && Primes.Unordered.Contains((int)number))
                return number;

            long boundary = (long)Math.Floor(Math.Sqrt(number));

            if (boundary <= Primes.Max)
                foreach (int prime in Primes.Ordered)
                    if (prime > boundary || number % prime == 0)
                        return prime;

            for (long i = Primes.Max + 2; i <= boundary; i += 2)
                if (number % i == 0)
                    return i;

            return number;

            // This shouldn't be hit due to int32 limits (would require a prime >1e12)
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Calculate the number of digits in integers &gt;0.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Length(this long number)
        {
            if (number % 10 == number)
                return 1;
            if (number % 100 == number)
                return 2;
            if (number % 1000 == number)
                return 3;
            if (number % 10000 == number)
                return 4;
            if (number % 100000 == number)
                return 5;
            if (number % 1000000 == number)
                return 6;
            if (number % 10000000 == number)
                return 7;
            if (number % 100000000 == number)
                return 8;
            if (number % 1000000000 == number)
                return 9;
            if (number % 10000000000 == number)
                return 10;
            if (number % 100000000000 == number)
                return 11;
            if (number % 1000000000000 == number)
                return 12;
            if (number % 10000000000000 == number)
                return 13;
            if (number % 100000000000000 == number)
                return 14;
            if (number % 1000000000000000 == number)
                return 15;
            if (number % 10000000000000000 == number)
                return 16;
            if (number % 100000000000000000 == number)
                return 17;
            if (number % 1000000000000000000 == number)
                return 18;
            if (number > 1000000000000000000)
                return 19;
            return 0;
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
        /// Two integers contain the same exact digits if they are permutations of each other.
        /// </summary>
        /// <example>4187.ContainsSameExactDigits(8147) == true</example>
        /// <param name="number"></param>
        /// <param name="otherNumber"></param>
        /// <returns></returns>
        public static bool ContainsSameExactDigits(this long number, long otherNumber)
        {
            if (number.Length() != otherNumber.Length())
                return false;

            int[] numDigits = new int[10];
            int[] othDigits = new int[10];

            while (number > 0)
            {
                int digit = (int)(number % 10);
                numDigits[digit]++;
                number /= 10;
            }
            while (otherNumber > 0)
            {
                int digit = (int)(otherNumber % 10);
                othDigits[digit]++;
                otherNumber /= 10;
            }

            for (int i = 0; i < 10; i++)
                if (numDigits[i] != othDigits[i])
                    return false;

            // Must have succeeded
            return true;
        }

        /// <summary>
        /// Checks if a digit is within a number >0.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="containingDigit"></param>
        /// <returns></returns>
        public static bool ContainsDigit(this long number, int containingDigit)
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

        /// <summary>
        /// An equivalent of <see cref="Math.Pow(double, double)"/> but as an extension for Int64. Does not check for
        /// overflow.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static long Pow(this long number, int power)
        {
            long result = 1;
            for (int i = 0; i < power; i++)
                result *= number;
            return result;
        }

        /// <summary>
        /// Count the number of digits in a number. Makes it so that one can easily store and sort to see if numbers
        /// contain the exact same digits.
        /// </summary>
        /// <remarks>
        /// Basically, each nibble represents the count of one digit. So 0xA0 could be for 1,111,111,111. Leading
        /// zeroes are not counted.
        /// </remarks>
        /// <param name="number"></param>
        /// <returns></returns>
        public static long DigitHash(this long number)
        {
            long[] numDigits = new long[10];

            while (number > 0)
            {
                long digit = number % 10;
                numDigits[digit]++;
                number /= 10;
            }

            return numDigits[9] << 36
                | numDigits[8] << 32
                | numDigits[7] << 28
                | numDigits[6] << 24
                | numDigits[5] << 20
                | numDigits[4] << 16
                | numDigits[3] << 12
                | numDigits[2] << 8
                | numDigits[1] << 4
                | numDigits[0];
        }
    }
}
