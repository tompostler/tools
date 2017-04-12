using System;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using Unlimitedinf.Tools.Hashing;

namespace Unlimitedinf.Tools
{
    /// <summary>
    /// Extensions to the ever-popular string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// A class holding all the various regexes needed to match the things.
        /// While this can be extended and changed as necessary, assume English and slightly well-formatted strings.
        /// </summary>
        internal static class R
        {
            private const RegexOptions opts = RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline;

            public const string Amt = @"(?:^last|^next|^current|^an?|(?:-\s?)?\d+)";
            public static readonly Regex Year = new Regex($@"{Amt}\s+year", opts);
            public static readonly Regex Month = new Regex($@"{Amt}\s+month", opts);
            public static readonly Regex Week = new Regex($@"{Amt}\s+week", opts);
            public static readonly Regex Day = new Regex($@"{Amt}\s+day", opts);
            public static readonly Regex SpecialDay = new Regex("(?:yesterday|today|tomorrow)", opts);
            public static readonly Regex Hour = new Regex($@"{Amt}\s+(?:hour|hr)", opts);
            public static readonly Regex Minute = new Regex($@"{Amt}\s+(?:minute|min)", opts);
            public static readonly Regex Second = new Regex($@"{Amt}\s+sec", opts);
            public static readonly Regex InPast = new Regex("(?:ago)", opts);
            // default to future computation

            private static char[] SlashS = new char[] { ' ', '\t', '\r', '\n', '\f', '\v' };
            public static long FromMatch(string match)
            {
                int whitespaceIndex = match.IndexOfAny(SlashS);
                if (whitespaceIndex >= 0)
                    match = match.Substring(0, whitespaceIndex);
                if (match.StartsWith("0") && match.TrimStart('0').Length > 0)
                    match = match.TrimStart('0');

                long result;
                if (long.TryParse(match, out result))
                    return result;
                match = match.ToUpperInvariant();
                if (match.StartsWith("LAST"))
                    return -1;
                if (match.StartsWith("NEXT") || match.StartsWith("A"))
                    return 1;
                if (match.StartsWith("CURRENT"))
                    return 0;
                throw new ArgumentException("Didn't know what to do with " + nameof(match));
            }
            public static long FromMatch(Match match) => FromMatch(match.Value.Substring(0, match.Value.IndexOfAny(SlashS)));
        }

        /// <summary>
        /// Given a string, treat it as an engligh-formatted relative datetime and attempt to parse an actual datetime
        /// out. Parsed datetime is UTC.
        /// This method is to be treated as unstable until this warning is removed.
        /// </summary>
        public static bool TryParseRelativeDateTime(this string toParse, out DateTime result)
        {
            // Try for nicely formatted
            if (DateTime.TryParse(toParse, out result))
                return true;

            long yea = 0, mon = 0, wee = 0, day = 0, spe = 0, hou = 0, min = 0, sec = 0;

            bool success = false;
            var match = R.Year.Match(toParse);
            if (match.Success)
            {
                yea = R.FromMatch(match);
                success = true;
            }
            match = R.Month.Match(toParse);
            if (match.Success)
            {
                mon = R.FromMatch(match);
                success = true;
            }
            match = R.Week.Match(toParse);
            if (match.Success)
            {
                wee = R.FromMatch(match);
                success = true;
            }
            match = R.Day.Match(toParse);
            if (match.Success)
            {
                day = R.FromMatch(match);
                success = true;
            }
            match = R.SpecialDay.Match(toParse);
            if (match.Success)
            {
                switch (match.Value.ToUpperInvariant())
                {
                    case "YESTERDAY":
                        spe = -1;
                        break;
                    case "TODAY":
                        spe = 0;
                        break;
                    case "TOMORROW":
                        spe = 1;
                        break;
                }
                success = true;
            }
            match = R.Hour.Match(toParse);
            if (match.Success)
            {
                hou = R.FromMatch(match);
                success = true;
            }
            match = R.Minute.Match(toParse);
            if (match.Success)
            {
                min = R.FromMatch(match);
                success = true;
            }
            match = R.Second.Match(toParse);
            if (match.Success)
            {
                sec = R.FromMatch(match);
                success = true;
            }

            result = default(DateTime);
            if (!success)
                return false;

            try
            {
                if (yea > int.MaxValue || mon > int.MaxValue)
                    return false;

                if (R.InPast.IsMatch(toParse))
                    result = DateTime.UtcNow
                        .AddYears(-(int)yea)
                        .AddMonths(-(int)mon)
                        .AddDays(-wee * 7 - day - spe)
                        .AddHours(-hou)
                        .AddMinutes(-min)
                        .AddSeconds(-sec);
                else
                    result = DateTime.UtcNow
                        .AddYears((int)yea)
                        .AddMonths((int)mon)
                        .AddDays(wee * 7 + day + spe)
                        .AddHours(hou)
                        .AddMinutes(min)
                        .AddSeconds(sec);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Like <see cref="TryParseRelativeDateTime(string, out DateTime)"/>, but sometimes you want it do-or-die.
        /// This method is to be treated as unstable until this warning is removed.
        /// </summary>
        public static DateTime ParseRelativeDateTime(this string toParse)
        {
            DateTime result;
            if (!TryParseRelativeDateTime(toParse, out result))
                throw new FormatException($"Could not parse relative DateTime from '{toParse}'");
            return result;
        }

        private static readonly MemoryCache getHashCodeCache = new MemoryCache(nameof(getHashCodeCache));
        private static readonly CacheItemPolicy defaultCachePolicy = new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, 10, 0) };
        /// <summary>
        /// An extension to <see cref="String.GetHashCode"/> that gets hash codes other than the default int.
        /// </summary>
        public static string GetHashCode(this string input, Hasher.Algorithm algorithm)
        {
            string alg = algorithm.ToString();
            if (getHashCodeCache.Contains(alg))
                return (getHashCodeCache.Get(alg) as Hasher).ComputeHashS(input);

            Hasher hasher = new Hasher(algorithm);
            getHashCodeCache.Add(alg, hasher, defaultCachePolicy);
            return hasher.ComputeHashS(input);
        }
        /// <summary>
        /// Get a hash code for a string using MD5.
        /// </summary>
        public static string GetHashCodeMd5(this string input)
        {
            return input.GetHashCode(Hasher.Algorithm.MD5);
        }
        /// <summary>
        /// Get a hash code for a string using Crc32.
        /// </summary>
        public static string GetHashCodeCrc32(this string input)
        {
            return input.GetHashCode(Hasher.Algorithm.Crc32);
        }
        /// <summary>
        /// Get a hash code for a string using SHA1.
        /// </summary>
        public static string GetHashCodeSha1(this string input)
        {
            return input.GetHashCode(Hasher.Algorithm.SHA1);
        }
        /// <summary>
        /// Get a hash code for a string using SHA256.
        /// </summary>
        public static string GetHashCodeSha256(this string input)
        {
            return input.GetHashCode(Hasher.Algorithm.SHA256);
        }
        /// <summary>
        /// Get a hash code for a string using SHA512.
        /// </summary>
        public static string GetHashCodeSha512(this string input)
        {
            return input.GetHashCode(Hasher.Algorithm.SHA512);
        }

        /// <summary>
        /// Chop a string from 0 to length.
        /// </summary>
        public static string Chop(this string input, int length)
        {
            if (input == null)
                throw new ArgumentNullException(input);
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 0)
                return string.Empty;

            return input.Substring(0, length);
        }

        /// <summary>
        /// Convert a string to Base64.
        /// </summary>
        public static string ToBase64String(this string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Convert from a Base64 string.
        /// </summary>
        public static string FromBase64String(this string input)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }
    }
}
