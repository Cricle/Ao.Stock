using SqlKata.Compilers;

namespace Ao.Stock.Kata
{
    public class KataStockIntangible : IStockIntangible
    {
        public const string EngineCodeKey = "EngineCode";

        public static readonly KataStockIntangible Instance = new KataStockIntangible();

        private KataStockIntangible() { }

        public void Config<T>(ref T input, IIntangibleContext context)
        {
        }

        public T Get<T>(IIntangibleContext context)
        {
            if (typeof(T) == typeof(Compiler))
            {
                return (T)(object)CompilerFetcher.GetCompiler(context.GetOrDefault<string>(EngineCodeKey));
            }
            return default;
        }
    }
}
