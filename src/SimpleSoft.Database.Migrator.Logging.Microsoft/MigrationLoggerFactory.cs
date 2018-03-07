using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSoft.Database.Migrator.Logging.Microsoft
{
    /// <summary>
    /// Logger Factory class.
    /// </summary>
    public class MigrationLoggerFactory : IMigrationLoggerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initialize logger factory.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public MigrationLoggerFactory(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Return a new log instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMigrationLogger Get(string name) => new MigrationLogger(_loggerFactory.CreateLogger(name));


    }
}
