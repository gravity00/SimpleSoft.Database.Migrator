using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSoft.Database.Migrator
{

    /// <summary>
    /// Class that represents a migration log.
    /// </summary>
    public class NLogMigrationLogger : IMigrationLogger
    {
        private static readonly object[] EmptyArgs = new object[0];
        private readonly ILogger _logger;

        /// <summary>
        /// Set log.
        /// </summary>
        /// <param name="logger"></param>
        public NLogMigrationLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Log by level.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="exception"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public void Log(MigrationLogLevel level, Exception exception, string messageFormat, params string[] args)
        {
            switch (level)
            {
                case MigrationLogLevel.None:
                    return;
                case MigrationLogLevel.Trace:
                    if (_logger.IsEnabled(LogLevel.Trace))
                        _logger.Trace(exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Debug:
                    if (_logger.IsEnabled(LogLevel.Debug))
                        _logger.Debug(exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Information:
                    if (_logger.IsEnabled(LogLevel.Info))
                        _logger.Info(exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Warning:
                    if (_logger.IsEnabled(LogLevel.Warn))
                        _logger.Warn(exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Error:
                    if (_logger.IsEnabled(LogLevel.Error))
                        _logger.Error(exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Fatal:
                    if (_logger.IsEnabled(LogLevel.Fatal))
                        _logger.Fatal(exception, messageFormat, ToObjectArray(args));
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        /// <summary>
        /// Check if the log level is enabled.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool IsEnabled(MigrationLogLevel level) =>
            _logger.IsEnabled(ToLogLevel(level));

        /// <summary>
        /// Creates the scope for this log transaction.
        /// </summary>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IDisposable Scope(string messageFormat, params string[] args) =>
            NestedDiagnosticsLogicalContext.Push(FormatMessage(messageFormat, args));

        private static string FormatMessage(string message, params object[] args)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (args == null || args.Length == 0)
                return message;

            var currentArg = 0;

            var sb = new StringBuilder(message.Length * 2);
            for (var i = 0; i < message.Length; i++)
            {
                var c = message[i];
                if (c == '{')
                {
                    sb.Append(args[currentArg]);
                    ++currentArg;
                    do
                    {
                        c = message[++i];
                    } while (c != '}' && i < message.Length);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        private static LogLevel ToLogLevel(MigrationLogLevel level)
        {
            switch (level)
            {
                case MigrationLogLevel.None:
                    return LogLevel.Off;
                case MigrationLogLevel.Trace:
                    return LogLevel.Trace;
                case MigrationLogLevel.Debug:
                    return LogLevel.Debug;
                case MigrationLogLevel.Information:
                    return LogLevel.Info;
                case MigrationLogLevel.Warning:
                    return LogLevel.Warn;
                case MigrationLogLevel.Error:
                    return LogLevel.Error;
                case MigrationLogLevel.Fatal:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Off;
            }
        }

        private static object[] ToObjectArray(string[] args)
        {
            if (args == null || args.Length == 0)
                return EmptyArgs;

            var objArgs = new object[args.Length];
            for (var i = 0; i < args.Length; i++)
                objArgs[i] = args[i];

            return objArgs;
        }
    }
}
