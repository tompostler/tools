#if NET47
#pragma warning disable CS3001 // Argument type is not CLS-compliant

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.Collections.Generic;

namespace Unlimitedinf.Tools
{
    public static partial class Log
    {
        /// <summary>
        /// Because ILoggers are awesome, provide a method to get one of those.
        /// </summary>
        public static ILogger GetILogger()
        {
            return new UnlimitedinfLogger();
        }

        /// <summary>
        /// ILogger wrapper on Log.
        /// </summary>
        public class UnlimitedinfLogger : ILogger
        {
            private static readonly Dictionary<LogLevel, VerbositySetting> vebosityMap = new Dictionary<LogLevel, VerbositySetting>
            {
                [LogLevel.Trace] = VerbositySetting.Verbose,
                [LogLevel.Debug] = VerbositySetting.Verbose,
                [LogLevel.Information] = VerbositySetting.Informational,
                [LogLevel.Warning] = VerbositySetting.Warning,
                [LogLevel.Error] = VerbositySetting.Error,
                [LogLevel.Critical] = VerbositySetting.Error
            };

            private static readonly Dictionary<LogLevel, Action<string>> logMethodMap = new Dictionary<LogLevel, Action<string>>
            {
                [LogLevel.Trace] = (s) => Verbose(s),
                [LogLevel.Debug] = (s) => Verbose(s),
                [LogLevel.Information] = (s) => Informational(s),
                [LogLevel.Warning] = (s) => Warning(s),
                [LogLevel.Error] = (s) => Error(s),
                [LogLevel.Critical] = (s) => Error(s)
            };

            /// <inheritdoc/>
            public IDisposable BeginScope<TState>(TState state)
            {
                return NullScope.Instance;
            }

            /// <inheritdoc/>
            public bool IsEnabled(LogLevel logLevel)
            {
                return vebosityMap[logLevel] <= Verbosity;
            }

            /// <inheritdoc/>
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (logMethodMap.ContainsKey(logLevel))
                    logMethodMap[logLevel](formatter(state, exception));

                // If an exception is passed in here, it is currently ignored by the LoggerExtensions default formatter
                // Line 430: https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Abstractions/LoggerExtensions.cs
                // Because people may expect it to be logged, go ahead and log it here as an error
                if (exception != null)
                    logMethodMap[LogLevel.Error](exception.ToString());
            }
        }
    }
}

#pragma warning restore CS3001 // Argument type is not CLS-compliant
#endif
