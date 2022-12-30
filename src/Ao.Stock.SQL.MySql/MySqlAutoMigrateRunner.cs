using Ao.Stock.Comparering;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.MySql
{

    [SuppressMessage("","EF1001")]
    public class MySqlAutoMigrateRunner : IAutoMigrateRunner
    {
        public MySqlAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            NewStockType = newStockType;
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string ConnectionString { get; }

        public IStockType NewStockType { get; }

        public string TableName { get; }

        public Version? Version { get; set; }

        public ServerType ServerType { get; set; } = ServerType.MySql;

        public Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? Project { get; set; }

        public void Migrate()
        {
            var version = Version ?? Version.Parse("8.0.0");
            using (var auto = new AutoMigrationHelperBuilder()
                .WithBuilderConfig(x => x.UseMySql(ConnectionString, ServerVersion.Create(version, ServerType)))
                .WithMigration(x => x.AddMySql5().WithGlobalConnectionString(ConnectionString))
                .WithScaffold(new MySqlConnection(ConnectionString), x => new MySqlDesignTimeServices().ConfigureDesignTimeServices(x))
                .Build())
            {
                auto.EnsureDatabaseCreated();
                auto.Begin(NewStockType)
                    .ScaffoldCompareAndMigrate(TableName, Project);
            }
        }
    }
}
