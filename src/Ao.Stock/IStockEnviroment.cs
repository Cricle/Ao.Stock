namespace Ao.Stock
{
    public interface IStockEnviroment
    {
        T Get<T>(IIntangibleContext context);

        void Config<T>(ref T input, IIntangibleContext context);
    }

}
