using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Ao.Stock.SQL.MySql
{
    public class MariaDBStockIntangible : MySqlStockIntangible
    {
        private static readonly ServerVersion DefaultServerVersion = ServerVersion.Create(new Version(8, 0, 0), ServerType.MariaDb);

        public static new readonly MariaDBStockIntangible Default = new MariaDBStockIntangible(ConnectStringStockIntangible.Instance);

        public MariaDBStockIntangible(ConnectStringStockIntangible connectStringStockIntangible) 
            : base(connectStringStockIntangible)
        {
        }

        public override string EngineCode => KnowsDbCode.MariaDB;

        protected override ServerVersion GetDefaultServerVersion()
        {
            return DefaultServerVersion;
        }
    }
}
