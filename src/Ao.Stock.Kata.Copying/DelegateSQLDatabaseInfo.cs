using SqlKata;
using SqlKata.Compilers;

namespace Ao.Stock.Kata.Copying
{
    public class DelegateSQLDatabaseInfo : SQLDatabaseInfo
    {
        public DelegateSQLDatabaseInfo(string database,
            IIntangibleContext context,
            IStockIntangible stockIntangible,
            Compiler compiler)
            : this(database, context, stockIntangible, compiler, DefaultCompileSql)
        {
        }
        public DelegateSQLDatabaseInfo(string database,
            IIntangibleContext context,
            IStockIntangible stockIntangible,
            Compiler compiler,
            Func<DelegateSQLDatabaseInfo, string, string> querySqlGenerator)
            : base(database, context, stockIntangible, compiler)
        {
            QuerySqlGenerator = querySqlGenerator;
        }
        public Func<DelegateSQLDatabaseInfo, string, string> QuerySqlGenerator { get; }

        public override string CreateQuerySql(string tableName)
        {
            return QuerySqlGenerator(this, tableName);
        }

        private static string DefaultCompileSql(DelegateSQLDatabaseInfo info, string tableName)
        {
            return info.Compiler.Compile(new Query().From(tableName)).ToString();
        }
    }
}
