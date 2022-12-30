using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.MSSQL
{
    [SuppressMessage("", "EF1001")]
    public class SqlServerStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "mssql.OptionAction";

        public static readonly SqlServerStockIntangible Default = new SqlServerStockIntangible(ConnectStringStockIntangible.Instance);

        public SqlServerStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            : base(connectStringStockIntangible)
        {
        }

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var optionAction = context.GetOrDefault<Action<SqlServerDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseSqlServer(box.ConnectionString, optionAction);
        }
        protected override void ConfigDesignTimeServices(ConnectionStringBox box1, DesignTimeServiceBox box2, IIntangibleContext? context)
        {
            new SqlServerDesignTimeServices().ConfigureDesignTimeServices(box2.Services);
        }

        protected override void ConfigMigrationRunnerBuilder(ConnectionStringBox box, IMigrationRunnerBuilder builder, IIntangibleContext? context)
        {
            builder.AddSqlServer().WithGlobalConnectionString(box.ConnectionString);
        }
        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new SqlConnection(box.ConnectionString);
        }
    }
}
