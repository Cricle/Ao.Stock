using Ao.Stock.Kata;
using Microsoft.Extensions.ObjectPool;
using SqlKata.Compilers;
using System.Data.Common;

namespace Ao.Stock.SQLKata
{
    public class StockRuntime
    {
        public StockRuntime(Compiler compiler,
            IIntangibleContext intangibleContext,
            IStockIntangible stock)
        {
            Compiler = compiler;
            IntangibleContextFactory = new ConstIntangibleContextFactory(intangibleContext);
            DbConnectionPool = stock.CreateDbConnectionPool(intangibleContext);
        }
        public StockRuntime(Compiler compiler,
            IIntangibleContextFactory intangibleContextFactory,
            IStockIntangible stock)
        {
            Compiler = compiler;
            IntangibleContextFactory = intangibleContextFactory;
            DbConnectionPool = stock.CreateDbConnectionPool(intangibleContextFactory.Create());
        }
        public StockRuntime(Compiler compiler,
            IIntangibleContextFactory intangibleContextFactory, 
            ObjectPool<DbConnection> dbConnectionPool)
        {
            Compiler = compiler;
            IntangibleContextFactory = intangibleContextFactory;
            DbConnectionPool = dbConnectionPool;
        }

        public Compiler Compiler { get; }

        public IIntangibleContextFactory IntangibleContextFactory { get; }

        public ObjectPool<DbConnection> DbConnectionPool { get; }

        public EntityContext CreateContext(string tableName,bool toRowSql = true)
        {
            var dbc = DbConnectionPool.Get();
            return new EntityContext(this, Compiler.CreateScope(dbc, toRowSql), tableName);
        }
        public EntityContext<T> CreateContext<T>(string tableName, bool toRowSql = true)
        {
            var dbc = DbConnectionPool.Get();
            return new EntityContext<T>(this, Compiler.CreateScope(dbc, toRowSql), tableName);
        }
    }
}
