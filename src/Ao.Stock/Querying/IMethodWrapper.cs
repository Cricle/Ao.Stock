namespace Ao.Stock.Querying
{
    public interface IMethodWrapper
    {
        string Quto<T>(T input);

        string? WrapValue<T>(T input);
    }
}
