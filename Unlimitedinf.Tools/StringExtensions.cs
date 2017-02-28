using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            public static int FromMatch(string match)
            {
                int whitespaceIndex = match.IndexOfAny(SlashS);
                if (whitespaceIndex >= 0)
                    match = match.Substring(0, whitespaceIndex);
                if (match.StartsWith("0") && match.TrimStart('0').Length > 0)
                    match = match.TrimStart('0');

                int result;
                if (int.TryParse(match, out result))
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
            public static int FromMatch(Match match) => FromMatch(match.Value.Substring(0, match.Value.IndexOfAny(SlashS)));
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

            int yea = 0, mon = 0, wee = 0, day = 0, spe = 0, hou = 0, min = 0, sec = 0;

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

            if (R.InPast.IsMatch(toParse))
            {
                result = DateTime.UtcNow
                    .AddYears(-yea)
                    .AddMonths(-mon)
                    .AddDays(-wee * 7 - day - spe)
                    .AddHours(-hou)
                    .AddMinutes(-min)
                    .AddSeconds(-sec);
            }
            else
                result = DateTime.UtcNow
                    .AddYears(yea)
                    .AddMonths(mon)
                    .AddDays(wee * 7 + day + spe)
                    .AddHours(hou)
                    .AddMinutes(min)
                    .AddSeconds(sec);

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
    }
}
