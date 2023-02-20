using System;

namespace Ao.Stock.Mirror
{
    public class SQLDatabaseCreateAdapter : ISQLDatabaseCreateAdapter
    {
        public static readonly SQLDatabaseCreateAdapter MySql = new SQLDatabaseCreateAdapter(@"CREATE DATABASE `{0}`;",
            @"CREATE DATABASE IF NOT EXISTS `{0}`;",
            @"DROP DATABASE `{0}`;",
            @"DROP DATABASE IF EXISTS `{0}`;");
        public static readonly SQLDatabaseCreateAdapter SqlServer = new SQLDatabaseCreateAdapter(@" CREATE DATABASE [{0}];",
            @"IF NOT EXISTS(SELECT [name] FROM [sys].[databases] WHERE [name] = '{0}') CREATE DATABASE [{0}];",
            @"DROP DATABASE [{0}];",
            @"
IF DB_ID('your_database_name') IS NOT NULL
BEGIN
  ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
  DROP DATABASE [{0}];
END");
        public static readonly SQLDatabaseCreateAdapter MariaDB = MySql;
        public static readonly SQLDatabaseCreateAdapter Sqlite = new SQLDatabaseCreateAdapter(@"SELECT true",
            @"SELECT true",
            string.Empty,
            string.Empty);
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
END;");
        public static readonly SQLDatabaseCreateAdapter PostgreSql = new SQLDatabaseCreateAdapter(@"CREATE DATABASE ""{0}"";", @"
DO $$ 
BEGIN
  IF NOT EXISTS (SELECT FROM pg_database WHERE datname = '{0}') THEN
    CREATE DATABASE ""{0}"";
  END IF;
END $$;",
            @"DROP DATABASE ""{0}"";",
            @"DROP DATABASE IF EXISTS ""{0}"";");

        public SQLDatabaseCreateAdapter(string createSqlFormat, string createIfNotExistsFormat, string dropSqlFormat, string dropIfExistsFormat)
        {
            CreateSqlFormat = createSqlFormat ?? throw new ArgumentNullException(nameof(createSqlFormat));
            CreateIfNotExistsFormat = createIfNotExistsFormat ?? throw new ArgumentNullException(nameof(createIfNotExistsFormat));
            DropSqlFormat = dropSqlFormat ?? throw new ArgumentNullException(nameof(dropSqlFormat));
            DropIfExistsFormat = dropIfExistsFormat ?? throw new ArgumentNullException(nameof(dropIfExistsFormat));
        }

        public string CreateSqlFormat { get; }

        public string CreateIfNotExistsFormat { get; }

        public string DropSqlFormat { get; }

        public string DropIfExistsFormat { get; }

        public string GenericCreateSql(string database)
        {
            return string.Format(CreateSqlFormat, database);
        }
        public string GenericCreateIfNotExistsSql(string database)
        {
            return string.Format(CreateIfNotExistsFormat, database);
        }

        public string GenericDropSql(string database)
        {
            return string.Format(DropSqlFormat, database);
        }

        public string GenericDropIfExistsSql(string database)
        {
            return string.Format(DropIfExistsFormat, database);
        }
    }
}
