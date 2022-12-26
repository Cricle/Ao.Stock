namespace Ao.Stock
{
    public interface IStockIntangible
    {
        T Get<T>(IIntangibleContext context);

        void Config<T>(ref T input, IIntangibleContext context);
    }
}
