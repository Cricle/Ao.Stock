using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public class SQLCognateMirrorCopy : IMirrorCopy<SQLMirrorCopyResult>
    {
        public SQLCognateMirrorCopy(DbConnection connection, string sourceSql, string targetNamed)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            SourceSql = sourceSql ?? throw new ArgumentNullException(nameof(sourceSql));
            TargetNamed = targetNamed ?? throw new ArgumentNullException(nameof(targetNamed));
        }

        public DbConnection Connection { get; }

        public string SourceSql { get; }

        public string TargetNamed { get; }

        public int CommandTimeout { get; set; } = 60 * 5;

        protected virtual string GetInsertQuery()
        {
            return $"INSERT INTO {TargetNamed} {SourceSql}";
        }

        public async Task<IList<SQLMirrorCopyResult>> CopyAsync()
        {
            var query = GetInsertQuery();
            var result = await Connection.ExecuteNonQueryAsync(query,timeout:CommandTimeout);
            return new[]
            {
                new SQLMirrorCopyResult(result,query)
            };
        }
    }
}
