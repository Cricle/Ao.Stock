using Ao.Stock.IntangibleProviders;

namespace Ao.Stock.SQL
{
    public static class SQLIntangibleContextHelper
    {
        public static IntangibleContext PostgreSQL(string connectionString)
        {
            var ctx = new IntangibleContext();
            PostgreSQL(ctx, connectionString);
            return ctx;
        }
        public static IntangibleContext PostgreSQL(IntangibleContext input)
        {
            var ctx = new IntangibleContext();
            PostgreSQL(ctx, input);
            return ctx;
        }
        public static void PostgreSQL(IntangibleContext context, IntangibleContext input)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = PostgreSQLIntangibleProvider.Default;
            context[SQLStockIntangible.SQLContextKey] = input;
        }
        public static void PostgreSQL(IntangibleContext context, string connectionString)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = PostgreSQLIntangibleProvider.Default;
            context[SQLStockIntangible.ConnectionStringKey] = connectionString;
        }
        public static IntangibleContext Oracle(string connectionString)
        {
            var ctx = new IntangibleContext();
            Oracle(ctx, connectionString);
            return ctx;
        }
        public static IntangibleContext Oracle(IntangibleContext input)
        {
            var ctx = new IntangibleContext();
            Oracle(ctx, input);
            return ctx;
        }
        public static void Oracle(IntangibleContext context, IntangibleContext input)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = OracleSQLIntangibleProvider.Default;
            context[SQLStockIntangible.SQLContextKey] = input;
        }
        public static void Oracle(IntangibleContext context, string connectionString)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = OracleSQLIntangibleProvider.Default;
            context[SQLStockIntangible.ConnectionStringKey] = connectionString;
        }


        public static IntangibleContext MariaDB(string connectionString)
        {
            var ctx = new IntangibleContext();
            MariaDB(ctx, connectionString);
            return ctx;
        }
        public static IntangibleContext MariaDB(IntangibleContext input)
        {
            var ctx = new IntangibleContext();
            MariaDB(ctx, input);
            return ctx;
        }
        public static void MariaDB(IntangibleContext context, IntangibleContext input)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = MariaDBSQLIntangibleProvider.Default;
            context[SQLStockIntangible.SQLContextKey] = input;
        }
        public static void MariaDB(IntangibleContext context, string connectionString)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = MariaDBSQLIntangibleProvider.Default;
            context[SQLStockIntangible.ConnectionStringKey] = connectionString;
        }


        public static IntangibleContext SqlServer(string connectionString)
        {
            var ctx = new IntangibleContext();
            SqlServer(ctx, connectionString);
            return ctx;
        }
        public static IntangibleContext SqlServer(IntangibleContext input)
        {
            var ctx = new IntangibleContext();
            SqlServer(ctx, input);
            return ctx;
        }
        public static void SqlServer(IntangibleContext context, IntangibleContext input)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = SqlServerSQLIntangibleProvider.Default;
            context[SQLStockIntangible.SQLContextKey] = input;
        }
        public static void SqlServer(IntangibleContext context, string connectionString)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = SqlServerSQLIntangibleProvider.Default;
            context[SQLStockIntangible.ConnectionStringKey] = connectionString;
        }


        public static IntangibleContext Sqlite(string connectionString)
        {
            var ctx = new IntangibleContext();
            Sqlite(ctx, connectionString);
            return ctx;
        }
        public static IntangibleContext Sqlite(IntangibleContext input)
        {
            var ctx = new IntangibleContext();
            Sqlite(ctx, input);
            return ctx;
        }
        public static void Sqlite(IntangibleContext context, IntangibleContext input)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = SqliteSQLIntangibleProvider.Default;
            context[SQLStockIntangible.SQLContextKey] = input;
        }
        public static void Sqlite(IntangibleContext context, string connectionString)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = SqliteSQLIntangibleProvider.Default;
            context[SQLStockIntangible.ConnectionStringKey] = connectionString;
        }

        public static IntangibleContext MySql(string connectionString)
        {
            var ctx = new IntangibleContext();
            MySql(ctx, connectionString);
            return ctx;
        }
        public static IntangibleContext MySql(IntangibleContext input)
        {
            var ctx = new IntangibleContext();
            MySql(ctx, input);
            return ctx;
        }
        public static void MySql(IntangibleContext context, IntangibleContext input)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = MySqlSQLIntangibleProvider.Default;
            context[SQLStockIntangible.SQLContextKey] = input;
        }
        public static void MySql(IntangibleContext context, string connectionString)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = MySqlSQLIntangibleProvider.Default;
            context[SQLStockIntangible.ConnectionStringKey] = connectionString;
        }


        public static IntangibleContext Set(string connectionString, IIntangibleProvider provider)
        {
            var ctx = new IntangibleContext();
            Set(ctx, connectionString, provider);
            return ctx;
        }
        public static IntangibleContext Set(IntangibleContext input, IIntangibleProvider provider)
        {
            var ctx = new IntangibleContext();
            Set(ctx, input, provider);
            return ctx;
        }
        public static void Set(IntangibleContext context, IntangibleContext input, IIntangibleProvider provider)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = provider;
            context[SQLStockIntangible.SQLContextKey] = input;
        }
        public static void Set(IntangibleContext context, string connectionString, IIntangibleProvider provider)
        {
            context[SQLStockIntangible.IntangibleProviderKey] = provider;
            context[SQLStockIntangible.ConnectionStringKey] = connectionString;
        }
    }
}
