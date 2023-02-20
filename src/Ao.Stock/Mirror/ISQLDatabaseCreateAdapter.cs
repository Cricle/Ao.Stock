namespace Ao.Stock.Mirror
{
    public interface ISQLDatabaseCreateAdapter
    {
        string GenericCreateSql(string database);

        string GenericCreateIfNotExistsSql(string database);

        string GenericDropSql(string database);

        string GenericDropIfExistsSql(string database);
    }
}
