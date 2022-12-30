using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Ao.Stock.SQL
{
    public abstract class SQLStockIntangible : IStockIntangible
    {
        public SQLStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
        {
            ConnectStringStockIntangible = connectStringStockIntangible ?? throw new ArgumentNullException(nameof(connectStringStockIntangible));
        }

        public ConnectStringStockIntangible ConnectStringStockIntangible { get; }

        public virtual void Config<T>(ref T input, IIntangibleContext? context)
        {
            if (typeof(DbContextOptionsBuilder).IsAssignableFrom(typeof(T)))
            {
                var builder = (DbContextOptionsBuilder)(object)input!;
                var connBox = ConnectStringStockIntangible.Get<ConnectionStringBox>(context);
                ConfigDbOptionBuilder(connBox, builder, context);
            }
        }

        protected abstract void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context);

        protected abstract DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context);

        public virtual T Get<T>(IIntangibleContext? context)
        {
            if (typeof(T)==typeof(DbContext))
            {
                var builder = new DbContextOptionsBuilder();
                Config(ref builder, context);
                return (T)(object)new DbContext(builder.Options);
            }
            if (typeof(T) == typeof(IDbConnection))
            {
                var connBox = ConnectStringStockIntangible.Get<ConnectionStringBox>(context);
                return (T)(object)CreateDbConnection(connBox,context);
            }
            if (typeof(T) == typeof(DbConnection))
            {
                var connBox = ConnectStringStockIntangible.Get<ConnectionStringBox>(context);
                return (T)(object)CreateDbConnection(connBox, context);
            }
            return default;
        }
    }
}
