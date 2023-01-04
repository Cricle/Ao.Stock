using Ao.Stock.Mirror;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Ao.Stock.Kata.Mirror
{
    public class SQLCognateMirrorCopy : IMirrorCopy<SQLMirrorCopyResult>
    {
        public SQLCognateMirrorCopy(DbConnection connection, SQLTableInfo source, SQLTableInfo target, Compiler compiler)
        {
            Connection = connection;
            Source = source;
            Target = target;
            Compiler = compiler;
        }

        public DbConnection Connection { get; }

        public SQLTableInfo Source { get; }

        public SQLTableInfo Target { get; }

        public Compiler Compiler { get; }

        public static string GetFullName(SQLTableInfo info, Compiler compiler)
        {
            return compiler.Wrap(info.Database) + "." + compiler.Wrap(info.Table);
        }
        protected virtual Query GetSourceQuery()
        {
            return new Query().FromRaw(GetFullName(Source, Compiler));
        }

        protected virtual Query GetInsertQuery()
        {
            var sourceFullName = GetFullName(Target, Compiler);
            var querySource = GetSourceQuery();
            return new Query()
                .FromRaw(sourceFullName)
                .AsInsert(Array.Empty<string>(), querySource);
        }

        public async Task<IList<SQLMirrorCopyResult>> CopyAsync(IIntangibleContext context)
        {
            var query = GetInsertQuery();
            var sqlResult = Compiler.Compile(query);
            var result = await Connection.ExecuteNoQueryAsync(sqlResult.Sql, sqlResult.NamedBindings);
            return new[]
            {
                new SQLMirrorCopyResult(result,sqlResult.Sql,sqlResult.NamedBindings)
            };
        }
    }
}
