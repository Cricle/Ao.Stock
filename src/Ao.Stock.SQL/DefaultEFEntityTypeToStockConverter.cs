using Ao.Stock.SQL.Announcation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Ao.Stock.SQL
{
    public class DefaultEFEntityTypeToStockConverter : IEFEntityTypeToStockConverter
    {
        public static readonly DefaultEFEntityTypeToStockConverter Instance = new DefaultEFEntityTypeToStockConverter();

        protected virtual void ConfigType(IEntityType type, StockType stockType)
        {
            stockType.Name = type.GetTableName();
        }
        protected virtual void ConfigProperty(IEntityType type, IProperty property, StockType stockType, StockProperty stockProperty)
        {
            var isPrimayKey = property.IsPrimaryKey();
            var isIndex = property.IsIndex();
            var store = StoreObjectIdentifier.Create(type, StoreObjectType.Table) ?? default;
            stockProperty.Name = property.GetColumnName(store);
            stockProperty.Type = property.ClrType;
            var columnType = property.GetColumnType();
            var atts = new List<IStockAttack> { new StockAttributeAttack(new RawDbTypeAttribute(columnType)) };
            if (property.ClrType == typeof(string) && property.GetMaxLength() is int len)
            {
                atts.Add(new StockAttributeAttack(new MaxLengthAttribute(len)));
            }
            if (isPrimayKey)
            {
                atts.Add(new StockAttributeAttack(new KeyAttribute()));
            }
            if (isIndex)
            {
                atts.Add(new StockAttributeAttack(new SqlIndexAttribute()));
            }
            stockProperty.Attacks = atts;

        }
        public IStockType AsStockType(IEntityType type)
        {
            var t = new StockType();
            ConfigType(type, t);
            var props = new List<IStockProperty>();
            t.Properties = props;
            foreach (var item in type.GetProperties())
            {
                var prop = new StockProperty();
                ConfigProperty(type, item, t, prop);
                props.Add(prop);
            }
            return t;
        }
    }
}
