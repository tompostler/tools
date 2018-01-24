using System;

namespace Unlimitedinf.Tools
{
    /// <summary>
    /// A wrapper for the standard <see cref="Console"/> methods that prints the way I want things to be printed.
    /// </summary>
    /// <remarks>
    /// A log message can look like:
    ///     2017-04-20 03:14:15.927: INF: PROGRAMNAME: here's the message.
    /// Each portion of this message is customizable and can be turned on or off per invocation.
    /// </remarks>
    public static class Log
    {
        private static object consoleLock = new object();
        private const ConsoleColor DefaultConsoleColor = ConsoleColor.Gray;

        /// <summary>
        /// The verbosity of the logger. Default is <see cref="VerbositySetting.Informational"/>.
        /// </summary>
        public static VerbositySetting Verbosity { get; set; } = VerbositySetting.Informational;

        /// <summary>
        /// Add a program name to the message. Defaults to null.
        /// </summary>
        public static string ProgramName { get; set; } = null;

        /// <summary>
        /// Print the program name (<see cref="ProgramName"/>). Defaults to false.
        /// </summary>
        public static bool PrintProgramName { get; set; } = false;

        /// <summary>
        /// The format string to use for <see cref="DateTime.ToString(string)"/>. Defaults to 'yyyy-MM-dd HH:mm:ss.fff'.
        /// </summary>
        public static string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// Print the datetime (<see cref="DateTimeFormat"/>). Defaults to false.
        /// </summary>
        public static bool PrintDateTime { get; set; } = false;

        /// <summary>
        /// Includes the text representing the verbosity level ('VER:', 'INF:', 'WRN:', 'ERR:'). Defaults to true.
        /// </summary>
        public static bool PrintVerbosityLevel { get; set; } = true;

        /// <summary>
        /// The verbosity setting.
        /// </summary>
        public enum VerbositySetting
        {
            /// <summary>
            /// Highest verbosity. Disabled by default.
            /// </summary>
            Verbose,
            /// <summary>
            /// Regular verbosity. Default level.
            /// </summary>
            Informational,
            /// <summary>
            /// Warning verbosity. Generally for unusual circumstances that can be recovered from.
            /// </summary>
            Warning,
            /// <summary>
            /// Error verbosity. Generally for exceptional circumstances that stop normal program flow.
            /// </summary>
            Error
        }

        /// <summary>
        /// My preferred settings for small console apps. Call this at the beginning of the main method.
        /// </summary>
        /// <remarks>
        /// Logs the following line verbosely right away:
        ///     yyyy-MM-dd HH:mm:ss.fff: Program start
        /// Then sets the <see cref="DateTimeFormat"/> to <c>HH:mm:ss.fff</c> with <see cref="PrintDateTime"/> true.
        /// Then sets the <see cref="Verbosity"/> to <see cref="VerbositySetting.Verbose"/> with <see cref="PrintVerbosityLevel"/> false.
        /// </remarks>
        public static void ConfigureDefaultConsoleApp()
        {
            Log.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": Program start", ConsoleColor.Blue);
            PrintDateTime = true;
            DateTimeFormat = "HH:mm:ss.fff";
            PrintVerbosityLevel = false;
            Verbosity = VerbositySetting.Verbose;
        }

        internal static void WriteLine(string message, ConsoleColor color = DefaultConsoleColor)
        {
            lock(consoleLock)
            {
                if (color != DefaultConsoleColor)
                    Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = DefaultConsoleColor;
            }
        }

        /// <summary>
        /// Logs a verbose message that starts with 'VER: ' and is blue in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Ver(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
        {
            if (Verbosity <= VerbositySetting.Verbose)
                Log.WriteLine(
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "VER: " : string.Empty) +
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    message,
                    ConsoleColor.Blue);
        }
        /// <summary>
        /// Logs a verbose message that starts with 'VER: ' and is blue in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Verbose(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
            => Ver(message, printDateTime, printVerbosityLevel, printProgramName);

        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Inf(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
        {
            if (Verbosity <= VerbositySetting.Informational)
                Log.WriteLine(
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "INF: " : string.Empty) +
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    message);
        }
        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Info(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
            => Inf(message, printDateTime, printVerbosityLevel, printProgramName);
        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Informational(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
            => Inf(message, printDateTime, printVerbosityLevel, printProgramName);

        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Wrn(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
        {
            if (Verbosity <= VerbositySetting.Warning)
                Log.WriteLine(
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "WRN: " : string.Empty) +
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    message, ConsoleColor.Yellow);
        }
        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Warn(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
            => Wrn(message, printDateTime, printVerbosityLevel, printProgramName);
        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Warning(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
            => Wrn(message, printDateTime, printVerbosityLevel, printProgramName);

        /// <summary>
        /// Logs an error message that starts with 'ERR: ' and is red in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Err(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
        {
            if (Verbosity <= VerbositySetting.Error)
                Log.WriteLine(
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "ERR: " : string.Empty) +
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    message, ConsoleColor.Red);
        }
        /// <summary>
        /// Logs an error message that starts with 'ERR: ' and is red in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void Error(string message, bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
            => Err(message, printDateTime, printVerbosityLevel, printProgramName);

        /// <summary>
        /// Logs an empty line.
        /// </summary>
        public static void Line() => WriteLine(string.Empty);

        /// <summary>
        /// Print one character at a time like a typewriter. Will grab the lock for the whole time, and prints in default console color.
        /// Verbosity is 'TYP:'.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="delay">The delay between each character printing. Default is 10ms.</param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        public static void LikeATypewriter(string message, TimeSpan delay = default(TimeSpan), bool? printDateTime = null, bool? printVerbosityLevel = null, bool? printProgramName = null)
        {
            if (delay == default(TimeSpan))
                delay = TimeSpan.FromMilliseconds(10);

            message = (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "TYP: " : string.Empty) +
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    message;

            lock (consoleLock)
            {
                foreach (char ch in message)
                {
                    Console.Write(ch);
                    System.Threading.Thread.Sleep(delay);
                }
                Console.WriteLine();
            }
        }
    }
}
