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

using System.Data;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    public interface IRelationalMigrationContext : IMigrationContext
    {
        /// <summary>
        /// The database connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// The current transaction or null
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// Isolation level
        /// </summary>
        IsolationLevel DefaultIsolationLevel { get; }

        #region QuerySingleAsync

        /// <summary>
        /// Queries a single result from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> QuerySingleAsync<T>(
            string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null);

        #endregion

        #region ExecuteAsync

        /// <summary>
        /// Executes the command into the database
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(
            string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        #endregion
    }

    /// <summary>
    /// The relational migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public interface IRelationalMigrationContext<out TOptions> : IMigrationContext<TOptions>, IRelationalMigrationContext 
        where TOptions : MigrationOptions
    {

    }
}