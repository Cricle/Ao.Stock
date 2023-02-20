namespace Ao.Stock.Mirror
{
    public interface ISQLDatabaseCreateAdapter
    {
        string GenericCreateDatabaseSql(string database);

        string GenericCreateDatabaseIfNotExistsSql(string database);

        string GenericDropDatabaseSql(string database);

        string GenericDropDatabaseIfExistsSql(string database);

        string GenericDropTableSql(string database);

        string GenericDropTableIfExistsSql(string database);
    }
}
