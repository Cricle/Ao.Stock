using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Data.Common;

namespace Ao.Stock.SQL.MSSQL
{
    public class MSSQLStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "mssql.OptionAction";

        public static readonly MSSQLStockIntangible Default = new MSSQLStockIntangible(ConnectStringStockIntangible.Instance);

        public MSSQLStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            :base(connectStringStockIntangible)
        {
        }

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var optionAction = context.GetOrDefault<Action<SqlServerDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseSqlServer(box.ConnectionString, optionAction);
        }

        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new SqlConnection(box.ConnectionString);
        }
    }
}
