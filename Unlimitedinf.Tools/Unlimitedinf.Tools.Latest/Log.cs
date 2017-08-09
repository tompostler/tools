using System;

namespace Unlimitedinf.Tools
{
    /// <summary>
    /// A wrapper for the standard <see cref="Console"/> methods that prints the way I want things to be printed.
    /// </summary>
    /// <remarks>
    /// A log message can look like:
    ///     PROGRAMNAME: 2017-04-20 03:14:15.927: INF: here's the message.
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

        private static void WriteLine(string message, ConsoleColor color = DefaultConsoleColor)
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
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Ver(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
        {
            if (Verbosity >= VerbositySetting.Verbose)
                WriteLine(
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "VER: " : string.Empty) +
                    message,
                    ConsoleColor.Blue);
        }
        /// <summary>
        /// Logs a verbose message that starts with 'VER: ' and is blue in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Verbose(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
            => Ver(message, printProgramName, printDateTime, printVerbosityLevel);

        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Inf(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
        {
            if (Verbosity >= VerbositySetting.Informational)
                WriteLine(
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "INF: " : string.Empty) +
                    message);
        }
        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Info(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
            => Inf(message, printProgramName, printDateTime, printVerbosityLevel);
        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Informational(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
            => Inf(message, printProgramName, printDateTime, printVerbosityLevel);

        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Wrn(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
        {
            if (Verbosity >= VerbositySetting.Warning)
                WriteLine(
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "WRN: " : string.Empty) +
                    message, ConsoleColor.Yellow);
        }
        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Warn(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
            => Wrn(message, printProgramName, printDateTime, printVerbosityLevel);
        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Warning(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
            => Wrn(message, printProgramName, printDateTime, printVerbosityLevel);

        /// <summary>
        /// Logs an error message that starts with 'ERR: ' and is red in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Err(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
        {
            if (Verbosity >= VerbositySetting.Error)
                WriteLine(
                    (printProgramName ?? PrintProgramName ? ProgramName + ": " : string.Empty) +
                    (printDateTime ?? PrintDateTime ? DateTime.Now.ToString(DateTimeFormat) + ": " : string.Empty) +
                    (printVerbosityLevel ?? PrintVerbosityLevel ? "ERR: " : string.Empty) +
                    message, ConsoleColor.Red);
        }
        /// <summary>
        /// Logs an error message that starts with 'ERR: ' and is red in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="printProgramName">Override default behavior of <see cref="PrintProgramName"/></param>
        /// <param name="printDateTime">Override default behavior of <see cref="PrintDateTime"/></param>
        /// <param name="printVerbosityLevel">Override default behavior of <see cref="PrintVerbosityLevel"/></param>
        public static void Error(string message, bool? printProgramName = null, bool? printDateTime = null, bool? printVerbosityLevel = null)
            => Err(message, printProgramName, printDateTime, printVerbosityLevel);
    }
}
