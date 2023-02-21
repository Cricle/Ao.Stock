using System;

namespace Ao.Stock.Mirror
{
    public class SQLDatabaseCreateAdapter : ISQLDatabaseCreateAdapter
    {
        public static readonly SQLDatabaseCreateAdapter MySql = new SQLDatabaseCreateAdapter(@"CREATE DATABASE `{0}`;",
            @"CREATE DATABASE IF NOT EXISTS `{0}`;",
            @"DROP DATABASE `{0}`;",
            @"DROP DATABASE IF EXISTS `{0}`;",
            @"DROP TABLE `{0}`;",
            @"DROP TABLE IF EXISTS `{0}`;");
        public static readonly SQLDatabaseCreateAdapter SqlServer = new SQLDatabaseCreateAdapter(@" CREATE DATABASE [{0}];",
            @"IF NOT EXISTS(SELECT [name] FROM [sys].[databases] WHERE [name] = '{0}') CREATE DATABASE [{0}];",
            @"DROP DATABASE [{0}];",
            @"
IF DB_ID('your_database_name') IS NOT NULL
BEGIN
  ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
  DROP DATABASE [{0}];
END",
            @"DROP TABLE [{0}];",
            @"DROP TABLE IF EXISTS [{0}];");
        public static readonly SQLDatabaseCreateAdapter MariaDB = MySql;
        public static readonly SQLDatabaseCreateAdapter Sqlite = new SQLDatabaseCreateAdapter(@"SELECT true",
            @"SELECT true",
            string.Empty,
            string.Empty,
            @"DROP TABLE ""{0}"";",
            @"DROP TABLE IF EXISTS ""{0}"";");
        public static readonly SQLDatabaseCreateAdapter Oracle = new SQLDatabaseCreateAdapter(@"CREATE DATABASE {0};", @"
DECLARE
  db_count NUMBER := 0;
BEGIN
  SELECT COUNT(*) INTO db_count FROM dba_users WHERE username = '{0}';
  IF db_count = 0 THEN
    EXECUTE IMMEDIATE 'CREATE USER {0} IDENTIFIED BY password';
    EXECUTE IMMEDIATE 'GRANT CREATE SESSION, CREATE TABLE, CREATE SEQUENCE TO ""{0}""';
  END IF;
END;",
            @"DROP DATABASE {0};",
            @"
DECLARE
  db_count NUMBER := 0;
BEGIN
  SELECT COUNT(*) INTO db_count FROM dba_users WHERE username = '{0}';
  IF db_count > 0 THEN
    EXECUTE IMMEDIATE 'DROP USER {0} CASCADE';
  END IF;
END;",
            @"DROP TABLE ""{0}"";",
            @"DROP TABLE ""{0}"";");
        public static readonly SQLDatabaseCreateAdapter PostgreSql = new SQLDatabaseCreateAdapter(@"CREATE DATABASE ""{0}"";", @"
DO $$ 
BEGIN
  IF NOT EXISTS (SELECT FROM pg_database WHERE datname = '{0}') THEN
    CREATE DATABASE ""{0}"";
  END IF;
END $$;",
            @"DROP DATABASE ""{0}"";",
            @"DROP DATABASE IF EXISTS ""{0}"";",
            @"DROP TABLE ""{0}"";",
            @"DROP TABLE IF EXISTS ""{0}"";");

        public SQLDatabaseCreateAdapter(string createDatabaseSqlFormatSqlFormat,
            string createDatabaseSqlFormatIfNotExistsFormat, 
            string dropSqlFormat,
            string dropDatabaseSqlFormatIfExistsFormat,
            string dropTableSqlFormatSqlFormat,
            string dropTableSqlFormatIfExistsFormat)
        {
            CreateDatabaseSqlFormat = createDatabaseSqlFormatSqlFormat ?? throw new ArgumentNullException(nameof(createDatabaseSqlFormatSqlFormat));
            CreateDatabaseSqlFormatIfNotExistsFormat = createDatabaseSqlFormatIfNotExistsFormat ?? throw new ArgumentNullException(nameof(createDatabaseSqlFormatIfNotExistsFormat));
            DropDatabaseSqlFormatSqlFormat = dropSqlFormat ?? throw new ArgumentNullException(nameof(dropSqlFormat));
            DropDatabaseSqlFormatIfExistsFormat = dropDatabaseSqlFormatIfExistsFormat ?? throw new ArgumentNullException(nameof(dropDatabaseSqlFormatIfExistsFormat));
            DropTableSqlFormatSqlFormat = dropTableSqlFormatSqlFormat;
            DropTableSqlFormatIfExistsFormat = dropTableSqlFormatIfExistsFormat;
        }

        public string CreateDatabaseSqlFormat { get; }

        public string CreateDatabaseSqlFormatIfNotExistsFormat { get; }

        public string DropDatabaseSqlFormatSqlFormat { get; }

        public string DropDatabaseSqlFormatIfExistsFormat { get; }

        public string DropTableSqlFormatSqlFormat { get; }

        public string DropTableSqlFormatIfExistsFormat { get; }

        public string GenericCreateDatabaseSql(string database)
        {
            return string.Format(CreateDatabaseSqlFormat, database);
        }
        public string GenericCreateDatabaseIfNotExistsSql(string database)
        {
            return string.Format(CreateDatabaseSqlFormatIfNotExistsFormat, database);
        }

        public string GenericDropDatabaseSql(string database)
        {
            return string.Format(DropDatabaseSqlFormatSqlFormat, database);
        }

        public string GenericDropDatabaseIfExistsSql(string database)
        {
            return string.Format(DropDatabaseSqlFormatIfExistsFormat, database);
        }

        public string GenericDropTableSql(string table)
        {
            return string.Format(DropTableSqlFormatSqlFormat, table);
        }

        public string GenericDropTableIfExistsSql(string table)
        {
            return string.Format(DropTableSqlFormatIfExistsFormat, table);
        }
    }
}
