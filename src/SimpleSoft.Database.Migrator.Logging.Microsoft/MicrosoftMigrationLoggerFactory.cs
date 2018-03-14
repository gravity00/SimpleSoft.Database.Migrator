using System;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Logger Factory class.
    /// </summary>
    public class MicrosoftMigrationLoggerFactory : IMigrationLoggerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initialize logger factory.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public MicrosoftMigrationLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Return a new log instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMigrationLogger Get(string name) => new MicrosoftMigrationLogger(_loggerFactory.CreateLogger(name));


    }
}
