namespace Unlimitedinf.Tools.Numerics.BigInt
{
    using System;
    using System.Numerics;

    /// <summary>
    /// Extensions for the almighty BigInteger. If it says >0, then it does not check for &lt;0.
    /// </summary>
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Checks if integers &gt;0 are palindomatic.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPalindrome(this BigInteger number)
        {
            return number == number.Reverse();
        }

        /// <summary>
        /// Calculate the number of digits in integers &gt;0.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Length(this BigInteger number)
        {
            return number.ToString().Length;
        }

        /// <summary>
        /// Reverses the digits of an integer.
        /// </summary>
        /// <example>78205.Reverse() == 50287</example>
        /// <param name="number"></param>
        /// <returns></returns>
        public static BigInteger Reverse(this BigInteger number)
        {
            char[] num = number.ToString().ToCharArray();
            Array.Reverse(num);
            return BigInteger.Parse(new string(num));
        }
    }
}
