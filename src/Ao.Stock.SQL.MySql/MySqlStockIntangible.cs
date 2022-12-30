using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Data.Common;

namespace Ao.Stock.SQL.MySql
{
    public class MySqlStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "mysql.OptionAction";
        public const string ServiceVersionKey = "mysql.ServiceVersion";

        private static readonly ServerVersion DefaultServerVersion = ServerVersion.Create(new Version(8, 0, 0), ServerType.MySql);

        public static readonly MySqlStockIntangible Default = new MySqlStockIntangible(ConnectStringStockIntangible.Instance);

        public MySqlStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            : base(connectStringStockIntangible)
        {
        }

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var serviceVersion = context.GetOrDefault<ServerVersion>(ServiceVersionKey) ?? DefaultServerVersion;
            var optionAction = context.GetOrDefault<Action<MySqlDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseMySql(box.ConnectionString, serviceVersion, optionAction);
        }

        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new MySqlConnection(box.ConnectionString);
        }
    }
}
