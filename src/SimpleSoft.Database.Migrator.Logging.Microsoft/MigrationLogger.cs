using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSoft.Database.Migrator.Logging.Microsoft
{
    /// <summary>
    /// Class that represents a migration log.
    /// </summary>
    public class MigrationLogger : IMigrationLogger
    {
        private static readonly object[] EmptyArgs = new object[0];
        private readonly ILogger _logger;

        /// <summary>
        /// Set log.
        /// </summary>
        /// <param name="logger"></param>
        public MigrationLogger(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
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
                        _logger.LogTrace(0, exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Debug:
                    if (_logger.IsEnabled(LogLevel.Debug))
                        _logger.LogDebug(0, exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Information:
                    if (_logger.IsEnabled(LogLevel.Information))
                        _logger.LogInformation(0, exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Warning:
                    if (_logger.IsEnabled(LogLevel.Warning))
                        _logger.LogWarning(0, exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Error:
                    if (_logger.IsEnabled(LogLevel.Error))
                        _logger.LogError(0, exception, messageFormat, ToObjectArray(args));
                    return;
                case MigrationLogLevel.Fatal:
                    if (_logger.IsEnabled(LogLevel.Critical))
                        _logger.LogCritical(0, exception, messageFormat, ToObjectArray(args));
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
            _logger.BeginScope(messageFormat, ToObjectArray(args));

        private static LogLevel ToLogLevel(MigrationLogLevel level)
        {
            switch (level)
            {
                case MigrationLogLevel.None:
                    return LogLevel.None;
                case MigrationLogLevel.Trace:
                    return LogLevel.Trace;
                case MigrationLogLevel.Debug:
                    return LogLevel.Debug;
                case MigrationLogLevel.Information:
                    return LogLevel.Information;
                case MigrationLogLevel.Warning:
                    return LogLevel.Warning;
                case MigrationLogLevel.Error:
                    return LogLevel.Error;
                case MigrationLogLevel.Fatal:
                    return LogLevel.Critical;
                default:
                    return LogLevel.None;
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
