using Ao.Stock.Comparering;
using Ao.Stock.SQL;
using Ao.Stock.SQL.Announcation;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Ao.Stock.Sample.MySql
{
    class MyMigrationHelper : MigrationHelper
    {
        public string GlobalString { get; set; }

        protected override void ConfigMigrationRunnerBuilder(IMigrationRunnerBuilder builder)
        {
            builder.AddMySql5().WithGlobalConnectionString(GlobalString);

        }
    }
    class MysqlScaffoldHelper : ScaffoldHelper
    {
        public MysqlScaffoldHelper(DbConnection dbConnection) : base(dbConnection)
        {
        }

        protected override void RegistServices(IServiceCollection services)
        {
            new MySqlDesignTimeServices().ConfigureDesignTimeServices(services);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            var conn = new MySqlConnection(mysql);
            var hx = new MysqlScaffoldHelper(conn);
            hx.DatabaseModelFactoryOptions = new DatabaseModelFactoryOptions(new[] { "student" });
            var m = hx.Scaffold();
            var mx = m.GetEntityTypes().FirstOrDefault(x => x.GetTableName() == "student")?.AsStockType();
            var mt1 = StockHelper.FromType(typeof(Student1), new ReflectionOptions { TypeNameGetter = _ => "student" });
            var res = DefaultStockTypeComparer.Default.Compare(mx, mt1);
            var h = new MyMigrationHelper() { GlobalString = mysql };
            h.Migrate(res);

        }
    }
    class Student1
    {
        [Key]
        public int Id { get; set; }

        [SqlIndex]
        [MaxLength(128)]
        public string Name { get; set; }
    }
}