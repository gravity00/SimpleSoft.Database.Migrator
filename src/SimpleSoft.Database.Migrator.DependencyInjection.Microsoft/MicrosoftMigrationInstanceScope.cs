using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Scope used to resolve instances using a <see cref="IServiceScope"/>.
    /// </summary>
    public class MicrosoftMigrationInstanceScope : IMigrationInstanceScope
    {
        private readonly IServiceScope _scope;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="scope">The service scope</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMigrationInstanceScope(IServiceScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <inheritdoc />
        ~MicrosoftMigrationInstanceScope() => Dispose(false);

        /// <inheritdoc />
        public object GetInstance(Type type) => _scope.ServiceProvider.GetService(type);

        /// <inheritdoc />
        public T GetInstance<T>() => _scope.ServiceProvider.GetService<T>();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        /// <param name="disposing">Is the instance beeing disposed?</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
                _scope.Dispose();
        }
    }
}
