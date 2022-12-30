using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System.Data.Common;

namespace Ao.Stock.SQL.PostgreSql
{
    public class PostgreSqlStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "postgresql.OptionAction";

        public static readonly PostgreSqlStockIntangible Default = new PostgreSqlStockIntangible(ConnectStringStockIntangible.Instance);

        public PostgreSqlStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            : base(connectStringStockIntangible)
        {
        }

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var optionAction = context.GetOrDefault<Action<NpgsqlDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseNpgsql(box.ConnectionString, optionAction);
        }

        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new NpgsqlConnection(box.ConnectionString);
        }
    }
}
