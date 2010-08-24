using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Samples
{
    public static class LinqToSqlExtensions
    {
        public static IEnumerable<TEntity> GetPendingInserts<TEntity>(this Table<TEntity> table)
            where TEntity : class
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            IEnumerable<TEntity> inserts = table.Context.GetChangeSet().Inserts
                .Where(obj => obj is TEntity)
                .Cast<TEntity>();
            return inserts;
        }

        /// <summary>
        /// Unions the table of entities with any pending inserts
        /// and filters the results using the given predicate.
        /// </summary>
        public static IEnumerable<TEntity> IncludePendingInserts<TEntity>(this Table<TEntity> table, Func<TEntity, bool> predicate)
            where TEntity : class
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            IEnumerable<TEntity> results = table.Where(predicate).ToList();
            results = results.Union(table.GetPendingInserts().Where(predicate));

            return results;
        }
    }
}