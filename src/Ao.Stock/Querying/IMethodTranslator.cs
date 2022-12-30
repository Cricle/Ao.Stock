namespace Ao.Stock.Querying
{
    public interface IMethodTranslator
    {
        string Translate(IMethodMetadata metadata, object context);
    }
    public interface IMethodTranslator<T> : IMethodTranslator
    {
        string Translate(IMethodMetadata metadata, T context);
    }

}
