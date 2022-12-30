namespace Ao.Stock.SQL.Announcation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RawDbTypeAttribute : Attribute
    {
        public RawDbTypeAttribute(string? rawType)
        {
            RawType = rawType;
        }

        public string? RawType { get; }
    }
}
