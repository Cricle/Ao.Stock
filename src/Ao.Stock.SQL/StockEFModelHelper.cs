using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ao.Stock.SQL
{
    public static class StockEFModelHelper
    {
        public static IStockType? AsStockType(this IModel model, string tableName, IEFEntityTypeToStockConverter converter, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return AsStockType(model.GetEntityTypes(), tableName, converter, comparison);
        }
        public static IStockType? AsStockType(this IEnumerable<IEntityType> types, string tableName, IEFEntityTypeToStockConverter converter, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return types.FirstOrDefault(x => string.Equals(x.GetTableName(), tableName, comparison))?.AsStockType(converter);
        }
        public static IStockType? AsStockType(this IModel model, string tableName, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return AsStockType(model.GetEntityTypes(), tableName, DefaultEFEntityTypeToStockConverter.Instance, comparison);
        }
        public static IStockType? AsStockType(this IEnumerable<IEntityType> types, string tableName, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return types.FirstOrDefault(x => string.Equals(x.GetTableName(), tableName, comparison))?.AsStockType(DefaultEFEntityTypeToStockConverter.Instance);
        }
        public static IStockType? AsStockType(this IEntityType type)
        {
            return AsStockType(type, DefaultEFEntityTypeToStockConverter.Instance);
        }

        public static IStockType? AsStockType(this IEntityType type, IEFEntityTypeToStockConverter converter)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            return converter.AsStockType(type);
        }
    }
}
