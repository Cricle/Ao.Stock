namespace Ao.Stock.Mirror
{
    public interface ICircularTable<TKey, TValue> : ICircularTableReader<TKey, TValue>, ICircularTableWriter<TKey, TValue>
    {

    }
}
