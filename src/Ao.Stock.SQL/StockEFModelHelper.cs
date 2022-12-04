using Ao.Stock.SQL.Announcation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Ao.Stock.SQL
{
    public static class StockEFModelHelper
    {
        public static IStockType AsStockType(this IEntityType type)
        {
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
                var prop = new StockProperty
                {
                    Name = item.GetFieldName(),
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
            }
            return t;
        }
    }
}
