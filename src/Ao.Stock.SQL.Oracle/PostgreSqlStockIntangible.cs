using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Design.Internal;
using Oracle.EntityFrameworkCore.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.Oracle
{
    [SuppressMessage("", "EF1001")]
    public class OracleStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "oracle.OptionAction";

        public static readonly OracleStockIntangible Default = new OracleStockIntangible(ConnectStringStockIntangible.Instance);

        public OracleStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            : base(connectStringStockIntangible)
        {
        }

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var optionAction = context.GetOrDefault<Action<OracleDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseOracle(box.ConnectionString, optionAction);
        }
        protected override void ConfigDesignTimeServices(ConnectionStringBox box1, DesignTimeServiceBox box2, IIntangibleContext? context)
        {
            new OracleDesignTimeServices().ConfigureDesignTimeServices(box2.Services);
        }

        protected override void ConfigMigrationRunnerBuilder(ConnectionStringBox box, IMigrationRunnerBuilder builder, IIntangibleContext? context)
        {
            builder.AddPostgres().WithGlobalConnectionString(box.ConnectionString);
        }

        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new OracleConnection(box.ConnectionString);
        }
    }
}
