﻿#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 João Simões
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class SqlServerMigrationManager<TContext> : RelationalMigrationManager<TContext>, ISqlServerMigrationManager<TContext>
        where TContext : class, ISqlServerMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="loggerFactory">An optional class logger factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SqlServerMigrationManager(TContext context, IMigrationLoggerFactory loggerFactory = null)
            : base(context, loggerFactory)
        {

        }

        #region Overrides of MigrationManager<TContext>

        /// <inheritdoc />
        protected override async Task<bool> MigrationsHistoryExistAsync(CancellationToken ct)
        {
            var tableId = await Context.QuerySingleAsync<long?>(
                    "SELECT OBJECT_ID('__DbMigratorHistory', 'U') as TableId", ct: ct)
                .ConfigureAwait(false);
            return tableId.HasValue;
        }

        /// <inheritdoc />
        protected override async Task CreateMigrationsHistoryAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
CREATE TABLE __DbMigratorHistory
(
    ContextName NVARCHAR(256) NOT NULL,
    MigrationId NVARCHAR(256) NOT NULL,
    ClassName NVARCHAR(1024) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    AppliedOn DATETIME2 NOT NULL,
    PRIMARY KEY (ContextName, MigrationId)
)", ct: ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task InsertMigrationEntryAsync(string contextName, string migrationId,
            string className, string description, DateTimeOffset appliedOn, CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
INSERT INTO __DbMigratorHistory(ContextName, MigrationId, ClassName, Description, AppliedOn) 
VALUES (@ContextName, @MigrationId, @ClassName, @Description, @AppliedOn)", new
                {
                    ContextName = contextName,
                    MigrationId = migrationId,
                    ClassName = className,
                    Description = description,
                    AppliedOn = appliedOn
                }, ct: ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(string contextName, CancellationToken ct)
        {
            var result = await Context.QueryAsync<string>(@"
SELECT 
    MigrationId
FROM __DbMigratorHistory
WHERE
    ContextName = @ContextName
ORDER BY 
    ContextName ASC, MigrationId DESC", new
                {
                    ContextName = contextName
                }, ct: ct)
                .ConfigureAwait(false);

            return result as IReadOnlyCollection<string> ?? result.ToList();
        }

        /// <inheritdoc />
        protected override async Task<string> GetMostRecentMigrationEntryIdAsync(string contextName, CancellationToken ct)
        {
            var migrationId = await Context.QuerySingleOrDefaultAsync<string>(@"
SELECT 
    TOP(1) MigrationId 
FROM __DbMigratorHistory 
WHERE
    ContextName = @ContextName
ORDER BY 
    ContextName ASC, MigrationId DESC", new
                {
                    ContextName = contextName
                }, ct: ct)
                .ConfigureAwait(false);

            return migrationId;
        }

        /// <inheritdoc />
        protected override async Task DeleteMigrationEntryByIdAsync(string contextName, string migrationId, CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
DELETE FROM __DbMigratorHistory 
WHERE 
    ContextName = @ContextName
    AND MigrationId = @MigrationId", new
                {
                    ContextName = contextName,
                    MigrationId = migrationId
                }, ct: ct)
                .ConfigureAwait(false);
        }

        #endregion
    }
}
