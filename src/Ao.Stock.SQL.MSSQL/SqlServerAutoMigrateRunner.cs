using Ao.Stock.Comparering;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.MySql
{
    [SuppressMessage("","EF1001")]
    public class SqlServerAutoMigrateRunner : IAutoMigrateRunner
    {
        public SqlServerAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            NewStockType = newStockType;
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string ConnectionString { get; }

        public IStockType NewStockType { get; }

        public string TableName { get; }

        public Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? Project { get; set; }

        public void Migrate()
        {
            using (var auto = new AutoMigrationHelperBuilder()
                .WithBuilderConfig(x => x.UseSqlServer(ConnectionString))
                .WithMigration(x => x.AddSqlServer().WithGlobalConnectionString(ConnectionString))
                .WithScaffold(new SqlConnection(ConnectionString), x => new SqlServerDesignTimeServices().ConfigureDesignTimeServices(x))
                .Build())
            {
                auto.EnsureDatabaseCreated();
                auto.Begin(NewStockType)
                    .ScaffoldCompareAndMigrate(TableName, Project);
            }
        }
    }
}
