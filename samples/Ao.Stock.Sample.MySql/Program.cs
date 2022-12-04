using Ao.Stock.Comparering;
using Ao.Stock.SQL;
using Ao.Stock.SQL.Announcation;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Ao.Stock.Sample.MySql
{
    class MyMigrationHelper : MigrationHelper
    {
        public MyMigrationHelper(IReadOnlyList<IStockComparisonAction> actions) : base(actions)
        {
        }

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
            var mysql= $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            var conn=new MySqlConnection(mysql);
            //var h = new MysqlScaffoldHelper(conn);
            //var m = h.Scaffold();
            var mt = StockHelper.FromType(typeof(Student), new ReflectionOptions {TypeNameGetter=_=>"Student" });
            var mt1 = StockHelper.FromType(typeof(Student1), new ReflectionOptions { TypeNameGetter = _ => "Student" });
            var res = DefaultStockTypeComparer.Default.Compare(mt, mt1);
            var h = new MyMigrationHelper(res) { GlobalString = mysql };
            h.Migrate();

        }
    }
    class Student
    {
        [Key]
        public int Id { get; set; }
    }
    class Student1
    {
        [Key]
        public int Id { get; set; }

        [SqlIndex]
        public string Name { get; set; }
    }
}