using Ao.Stock.Comparering;
using Ao.Stock.SQL;
using Ao.Stock.SQL.Announcation;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Ao.Stock.Sample.MySql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=355343;database=sakila;characterset=utf8mb4;";
            var mt1 = StockHelper.FromType(typeof(Student1), new ReflectionOptions { TypeNameGetter = _ => "student" });
            using (var auto = new AutoMigrationHelperBuilder()
                .WithBuilderConfig(x => x.UseMySql(mysql, ServerVersion.Create(Version.Parse("8.0.0"), ServerType.MySql)))
                .WithMigration(x => x.AddMySql5().WithGlobalConnectionString(mysql))
                .WithScaffold(new MySqlConnection(mysql), x => new MySqlDesignTimeServices().ConfigureDesignTimeServices(x))
                .Build())
            {
                auto.EnsureDatabaseCreated();
                auto.Begin(mt1)
                    .ScaffoldCompareAndMigrate("student", x => x.Where(y => y is not StockRenameTypeComparisonAction).ToList());
            }
        }
    }
    class Student1
    {
        [Key]
        public int Id { get; set; }

        [SqlIndex]
        [MaxLength(54)]
        public string Name { get; set; }
    }
}