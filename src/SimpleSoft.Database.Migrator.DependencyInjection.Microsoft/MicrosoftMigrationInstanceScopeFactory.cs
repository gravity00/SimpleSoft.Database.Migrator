using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Factory used to build <see cref="IMigrationInstanceScope"/> instances
    /// using a <see cref="IServiceScopeFactory"/>.
    /// </summary>
    public class MicrosoftMigrationInstanceScopeFactory : IMigrationInstanceScopeFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="scopeFactory">The service scope factory to be used</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMigrationInstanceScopeFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        /// <inheritdoc />
        public IMigrationInstanceScope Build() => new MicrosoftMigrationInstanceScope(_scopeFactory.CreateScope());
    }
}