namespace Ao.Stock
{
    public interface IStockIntangible
    {
        TReturn Get<TReturn,TContext>(TContext input);

        void Config<TInput,TContext>(ref TInput input, TContext context);
    }
}
