using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;

namespace Ao.Stock.SQL
{
    public interface IAutoMigrateRunner
    {
        void Migrate();
    }
}
