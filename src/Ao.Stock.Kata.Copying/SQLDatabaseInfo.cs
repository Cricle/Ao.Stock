using SqlKata.Compilers;

namespace Ao.Stock.Kata.Copying
{
    public abstract class SQLDatabaseInfo : ISQLDatabaseInfo
    {
        protected SQLDatabaseInfo(string database, IIntangibleContext context, IStockIntangible stockIntangible, Compiler compiler)
        {
            Database = database;
            Context = context;
            StockIntangible = stockIntangible;
            Compiler = compiler;
        }

        public string Database { get; }

        public IIntangibleContext Context { get; }

        public IStockIntangible StockIntangible { get; }

        public Compiler Compiler { get; }

        public abstract string CreateQuerySql(string tableName);
    }
}
