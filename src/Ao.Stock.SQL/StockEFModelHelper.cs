using Ao.Stock.SQL.Announcation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Ao.Stock.SQL
{
    public static class StockEFModelHelper
    {
        public static IStockType? AsStockType(this IEntityType type)
        {
            if (type == null)
            {
                return null;
            }
            var tableName = type.GetTableName();
            var t = new StockType
            {
                Name = tableName,
            };
            var props = new List<IStockProperty>();
            t.Properties = props;
            foreach (var item in type.GetProperties())
            {
                var isPrimayKey = item.IsPrimaryKey();
                var isIndex = item.IsIndex();
                var store = StoreObjectIdentifier.Create(type, StoreObjectType.Table) ?? default;
                var prop = new StockProperty
                {
                    Name = item.GetColumnName(store),
                    Type = item.ClrType,
                };
                var atts = new List<IStockAttack>(0);
                if (isPrimayKey)
                {
                    atts.Add(new StockAttributeAttack(new KeyAttribute()));
                }
                if (isIndex)
                {
                    atts.Add(new StockAttributeAttack(new SqlIndexAttribute()));
                }
                prop.Attacks = atts;
                props.Add(prop);
            }
            return t;
        }
    }
}
