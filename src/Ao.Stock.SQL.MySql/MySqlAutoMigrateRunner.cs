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
    public class MySqlAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public MySqlAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName) 
            : base(connectionString, newStockType, tableName, MySqlStockIntangible.Default)
        {
        }
    }
}
