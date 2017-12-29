﻿$fileName = Join-Path $PSScriptRoot "StringExtensionsTests.g.cs";
Write-Host "Generating $fileName";

# File setup
@"
//
// This code was generated by a tool. Any changes made manually will be lost the next time this code is regenerated.
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unlimitedinf.Tools.IntTests
{
    public partial class StringExtensionsTests
    {
"@ > $fileName;

# Source data for the tests (some pulled from python dateparser)
$testCases = @(
    # MethodName         DateTime.Add*                       StringToParse
    ("Years_Plus",      "Years(1)",                         "1 year"),
    ("Years_Minus",     "Years(-1)",                        "1 year ago"),
    ("Years_Current",   "Years(0)",                         "current year"),
    ("YM_PlusMinus",    "Years(1).AddMonths(-1)",           "1 year, -1 month"),
    ("Yesterday",       "Days(-1)",                         "yesterday"),
    ("Today",           "Days(0)",                          "today"),
    ("Tomorrow",        "Days(1)",                          "tomorrow"),
    ("Hour_Minus_1",    "Hours(-1)",                        "an hour ago"),
    ("Day_Minus",       "Days(-1)",                         "a day ago"),
    ("Week_Minus_1",    "Days(-7)",                         "a week ago"),
    ("Hours_Minus_2",   "Hours(-2)",                        "2 hours ago"),
    ("Hours_Minus_23",  "Hours(-23)",                       "23 hours ago"),
    ("YM_PlusPlus_1_2", "Years(1).AddMonths(2)",            "1 year 2 months"),
    ("YMW_PlusPlusPlus","Years(1).AddMonths(9).AddDays(7)", "1 year, 09 months, 01 weeks"),
    ("YM_PlusPlus_1_11","Years(1).AddMonths(11)",           "1 year 11 months"),
    ("YM_PlusPlus_1_12","Years(1).AddMonths(12)",           "1 year 12 months"),
    ("Hours_Plus_15",   "Hours(15)",                        "15 hr"),
    ("Hours_Plus_15s",  "Hours(15)",                        "15 hrs"),
    ("Mins_Plus_2",     "Minutes(2)",                       "2 min"),
    ("Mins_Plus_2s",    "Minutes(2)",                       "2 mins"),
    ("Secs_Plus_3",     "Seconds(3)",                       "3 sec"),
    ("Years_Minus_1k",  "Years(-1000)",                     "1000 years ago"),
    ("Years_Minus_Cm1", "Years(-DateTime.UtcNow.Year+1)",   "2016 years ago"),
    ("Months_Minus_5k", "Months(-5000)",                    "5000 months ago"),
    ("YMWDHMS_Minus_1s","Years(-1).AddMonths(-1).AddDays(-8).AddHours(-1).AddMinutes(-1).AddSeconds(-1)", "1 year, 1 month, 1 week, 1 day, 1 hour, 1 minute, and 1 second ago")
)

# Turn the above array into actual test cases
foreach ($methodInfo in $testCases) {
@"
        [TestMethod]
        public void StringEx_Parse_{0}()
        {{
            var expected = DateTime.UtcNow.Add{1};
            var actual = "{2}".ParseRelativeDateTime();
            Dassert.AreEqual(expected, actual);
        }}

"@ -f $methodInfo >> $fileName;
}

# File teardown
@"
    }
}
"@ >> $fileName;
