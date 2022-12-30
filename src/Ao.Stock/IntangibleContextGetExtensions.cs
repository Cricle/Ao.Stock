namespace Ao.Stock
{
    public static class IntangibleContextGetExtensions
    {
#nullable disable
        public static T GetOrDefault<T>(this IIntangibleContext context,object key)
        {
            return context.TryGetValue(key, out var value) && value is T t ? t : default(T);
        }
#nullable enable

    }
}
