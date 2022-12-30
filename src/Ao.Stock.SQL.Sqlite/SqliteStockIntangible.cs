using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.Common;

namespace Ao.Stock.SQL.Sqlite
{
    public class SqliteStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "sqlite.OptionAction";

        public static readonly SqliteStockIntangible Default = new SqliteStockIntangible(ConnectStringStockIntangible.Instance);

        public SqliteStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            : base(connectStringStockIntangible)
        {
        }

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var optionAction = context.GetOrDefault<Action<SqliteDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseSqlite(box.ConnectionString, optionAction);
        }

        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new SqliteConnection(box.ConnectionString);
        }
    }
}
