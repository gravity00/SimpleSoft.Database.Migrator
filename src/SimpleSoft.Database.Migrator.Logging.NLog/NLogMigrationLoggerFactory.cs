using NLog;
using System;

namespace SimpleSoft.Database.Migrator
{

    /// <summary>
    /// Logger Factory class.
    /// </summary>
    public class NLogMigrationLoggerFactory : IMigrationLoggerFactory
    {
        private readonly LogFactory _loggerFactory;

        /// <summary>
        /// Initialize logger factory.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public NLogMigrationLoggerFactory(LogFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Return a new log instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMigrationLogger Get(string name) => new NLogMigrationLogger(_loggerFactory.GetLogger(name));


    }
}
