using System;

namespace Unlimitedinf.Tools
{
    /// <summary>
    /// A wrapper for the standard <see cref="Console"/> methods that prints the way I want things to be printed.
    /// </summary>
    public static class Log
    {
        private static object consoleLock = new object();
        private const ConsoleColor DefaultConsoleColor = ConsoleColor.Gray;

        /// <summary>
        /// The verbosity of the logger. Default is <see cref="VerbositySetting.Informational"/>.
        /// </summary>
        public static VerbositySetting Verbosity { get; set; } = VerbositySetting.Informational;

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
        public static void Ver(string message)
        {
            if (Verbosity >= VerbositySetting.Verbose)
                WriteLine("VER: " + message, ConsoleColor.Blue);
        }
        /// <summary>
        /// Logs a verbose message that starts with 'VER: ' and is blue in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Verbose(string message) => Ver(message);

        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Inf(string message)
        {
            if (Verbosity >= VerbositySetting.Informational)
                WriteLine("INF: " + message);
        }
        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Info(string message) => Inf(message);
        /// <summary>
        /// Logs an informational message that starts with 'INF: '.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Informational(string message) => Inf(message);

        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Wrn(string message)
        {
            if (Verbosity >= VerbositySetting.Warning)
                WriteLine("WRN: " + message, ConsoleColor.Yellow);
        }
        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Warn(string message) => Wrn(message);
        /// <summary>
        /// Logs a warning message that starts with 'WRN: ' and is yellow in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Warning(string message) => Wrn(message);

        /// <summary>
        /// Logs an error message that starts with 'ERR: ' and is red in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Err(string message)
        {
            if (Verbosity >= VerbositySetting.Error)
                WriteLine("ERR: " + message, ConsoleColor.Red);
        }
        /// <summary>
        /// Logs an error message that starts with 'ERR: ' and is red in color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Error(string message) => Err(message);
    }
}
