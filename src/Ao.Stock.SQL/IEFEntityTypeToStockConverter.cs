using Microsoft.EntityFrameworkCore.Metadata;

namespace Ao.Stock.SQL
{
    public interface IEFEntityTypeToStockConverter
    {
        public IStockType AsStockType(IEntityType type);
    }
}
