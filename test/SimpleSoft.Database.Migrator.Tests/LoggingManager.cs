using System;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Logging.Microsoft;

namespace SimpleSoft.Database.Migrator.Tests
{
    public static class LoggingManager
    {
        public static readonly IMigrationLoggerFactory LoggerFactory;

        static LoggingManager()
        {
            LoggerFactory = new MigrationLoggerFactory(
                new LoggerFactory()
                    .AddConsole(LogLevel.Trace, true)
                    .AddDebug(LogLevel.Trace));
        }
    }
}
