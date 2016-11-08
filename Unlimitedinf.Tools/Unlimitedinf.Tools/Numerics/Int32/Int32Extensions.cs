namespace Unlimitedinf.Tools.Numerics.Int32
{
    using Unlimitedinf.Tools.Statics.Numerics;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for the humble Int32. If it says >0, then it does not check for &lt;0.
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// Checks if integers >0 are prime.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPrime(this int number)
        {
            if (number == 2)
                return true;
            if (number == 1 || number % 2 == 0)
                return false;
            if (number < Primes.Max)
                return Primes.Unordered.Contains(number);

            int boundary = (int)Math.Ceiling(Math.Sqrt(number));

            foreach (int prime in Primes.Ordered)
            {
                // We've exhausted the possible factors of our number
                if (prime > boundary)
                    return true;
                // Found a factor
                if (number % prime == 0)
                    return false;
            }

            // This shouldn't be hit due to int32 limits (would require a prime >1e12)
            throw new ArgumentOutOfRangeException(nameof(number));
        }

        /// <summary>
        /// Checks if integers >0 are palindomatic.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPalindrome(this int number)
        {
            int rev = 0, dig, orig = number;

            while (number > 0)
            {
                dig = number % 10;
                rev = rev * 10 + dig;
                number /= 10;
            }

            return rev == orig;
        }

        /// <summary>
        /// Checks if integers >0 are palindomatic when looking at their base 2 representation.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPalindromeInBase2(this int number)
        {
            if (number % 2 == 0)
                return false;

            int rev = 0, dig, orig = number;

            while (number > 0)
            {
                dig = number & 1;
                rev = rev << 1 | dig;
                number >>= 1;
            }

            return rev == orig;
        }

        /// <summary>
        /// Generates a list containing the factors of a number >0.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Sorted list of factors.</returns>
        public static SortedSet<int> GetFactors(this int number)
        {
            SortedSet<int> factors = new SortedSet<int>();

            int boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 1; i <= boundary; i++)
                if (number % i == 0)
                {
                    factors.Add(i);
                    factors.Add(number / i);
                }

            return factors;
        }

        /// <summary>
        /// Generates a list containing the prime factorization of a number >0.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>List of prime factors, excluding 1 of course.</returns>
        public static List<int> GetPrimeFactorization(this int number)
        {
            List<int> factors = new List<int>();

            int factor = number.GetSmallestFactor();
            while (number != 1)
            {
                factors.Add(factor);
                number /= factor;
                factor = number.GetSmallestFactor();
            }

            return factors;
        }

        /// <summary>
        /// Gets the smallest factor (other than 1) of an integer >1.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int GetSmallestFactor(this int number)
        {
            if (number % 2 == 0)
                return 2;
            if (number < Primes.Max && Primes.Unordered.Contains(number))
                return number;

            int boundary = (int)Math.Floor(Math.Sqrt(number));

            foreach (int prime in Primes.Ordered)
                if (prime > boundary || number % prime == 0)
                    return prime;

            // This shouldn't be hit due to int32 limits (would require a prime >1e12)
            throw new ArgumentOutOfRangeException(nameof(number));
        }

        /// <summary>
        /// Calculate the number of digits in integers.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Length(this int number)
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
        /// An integer is pandigital if it contains the digits 1-# once and only once in the number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPandigital(this int number)
        {
            bool[] digits = new bool[10];
            int length = number.Length();

            while (number > 0)
            {
                int digit = number % 10;
                if (digits[digit])
                    return false;
                digits[digit] = true;
                number /= 10;
            }

            // Not pandigital with 0
            if (digits[0])
                return false;

            // All digits from 1 through length should be true
            for (int i = 1; i <= length; i++)
                if (!digits[i])
                    return false;

            // Must have succeeded
            return true;
        }

        /// <summary>
        /// An integer contains unique digits if no digits in the num repeat.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool ContainsUniqueDigits(this int number)
        {
            bool[] digits = new bool[10];

            while (number > 0)
            {
                int digit = number % 10;
                if (digits[digit])
                    return false;
                digits[digit] = true;
                number /= 10;
            }

            // Must have succeeded
            return true;
        }

        /// <summary>
        /// Two integers contain the same digits if they contain the same digits in any order with repetition allowed.
        /// Putting the smaller number first is potentially more performant.
        /// </summary>
        /// <example>4187.ContainsSameDigits(817748) == true</example>
        /// <param name="number"></param>
        /// <param name="otherNumber"></param>
        /// <returns></returns>
        public static bool ContainsSameDigits(this int number, int otherNumber)
        {
            bool[] numDigits = new bool[10];

            while (number > 0)
            {
                int digit = number % 10;
                numDigits[digit] = true;
                number /= 10;
            }

            while (otherNumber > 0)
            {
                int digit = otherNumber % 10;
                if (!numDigits[digit])
                    return false;
                otherNumber /= 10;
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
        public static bool ContainsSameExactDigits(this int number, int otherNumber)
        {
            if (number.Length() != otherNumber.Length())
                return false;

            int[] numDigits = new int[10];
            int[] othDigits = new int[10];

            while (number > 0)
            {
                int digit = number % 10;
                numDigits[digit]++;
                number /= 10;
            }
            while (otherNumber > 0)
            {
                int digit = otherNumber % 10;
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
        /// Calculate the greatest common divisor of two integers.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="otherNumber"></param>
        /// <returns></returns>
        public static int GCD(this int number, int otherNumber)
        {
            // Well proven algorithm, so not tested.
            int t = otherNumber;
            while (otherNumber != 0)
            {
                t = otherNumber;
                otherNumber = number % otherNumber;
                number = t;
            }
            return number;
        }

        /// <summary>
        /// Rotates the digits of an integer to the right.
        /// </summary>
        /// <example>145.Rotate() == 514</example>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Rotate(this int number)
        {
            int length = number.Length();
            if (length == 1)
                return number;

            int lastDigit = number % 10;
            if (lastDigit == 0)
                return number /= 10;

            number /= 10;

            for (int i = 1; i < length; i++)
                lastDigit *= 10;

            return number + lastDigit;
        }

        /// <summary>
        /// Checks if a digit is within a number.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="containingNumber"></param>
        /// <returns></returns>
        public static bool IsDigitIn(this int number, int containingNumber)
        {
            while (containingNumber > 0)
            {
                int digit = containingNumber % 10;
                if (digit == number)
                    return true;
                containingNumber /= 10;
            }

            // Must not be contained
            return false;
        }

        /// <summary>
        /// A triangular number is defined by: T_n=n(n+1)/2
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsTriangular(this int number)
        {
            double check = (Math.Sqrt(8d * number + 1d) - 1d) / 2d;
            return check == (int)check;
        }

        /// <summary>
        /// A square number is defined by: S_n=n^2
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsSquare(this int number)
        {
            // Not worth testing.
            double check = Math.Sqrt(number);
            return check == (int)check;
        }

        /// <summary>
        /// A pentagonal number is defined by: P_n=n(3n−1)/2
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPentagonal(this int number)
        {
            double check = (Math.Sqrt(24d * number + 1d) + 1d) / 6d;
            return check == (int)check;
        }

        /// <summary>
        /// A hexagonal number is defined by: H_n=n(2n−1)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsHexagonal(this int number)
        {
            double check = (Math.Sqrt(8d * number + 1d) + 1d) / 4d;
            return check == (int)check;
        }

        /// <summary>
        /// A heptagonal number is defined by: H_n=n(5n-3)/2
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsHeptagonal(this int number)
        {
            double check = (Math.Sqrt(40d * number + 9d) + 3d) / 10d;
            return check == (int)check;
        }

        /// <summary>
        /// An octagonal number is defined by: O_n=n(3n-2)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsOctagonal(this int number)
        {
            double check = (Math.Sqrt(3d * number + 1d) + 1d) / 3d;
            return check == (int)check;
        }

        /// <summary>
        /// Reverses the digits of an integer.
        /// </summary>
        /// <example>78205.Reverse() == 50287</example>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Reverse(this int number)
        {
            int length = number.Length();
            if (length == 1)
                return number;

            int newNum = 0;
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
        public static int Concat(this int number, int otherNumber)
        {
            int len = otherNumber.Length();
            for (int i = 0; i < len; i++)
                number *= 10;
            return number + otherNumber;
        }
    }
}
