namespace Ao.Stock
{
    public interface IStockEnviromnent
    {
        public string? CurrentCode { get; }

        public IStockIntangible? Current { get; }
    }
}
