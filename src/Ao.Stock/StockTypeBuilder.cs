using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public class StockTypeBuilder: StockBuilder
    {
        public StockTypeBuilder(StockType stockType)
        {
            StockType = stockType ?? throw new ArgumentNullException(nameof(stockType));
        }

        public StockTypeBuilder(string name)
            :this(new StockType { Name=name})
        {
        }

        public StockType StockType { get; }

        public StockType Finish()
        {
            return StockType;
        }

        public StockTypeBuilder WithProperty(Func<StockProperty> propertyFunc)
        {
            return WithProperty(propertyFunc());
        }
        public StockTypeBuilder WithProperty(string name, Type type,Action<StockPropertyBuilder>? propertyAction=null)
        {
            var prop = new StockProperty { Name = name, Type = type };
            if (propertyAction != null)
            {
                propertyAction?.Invoke(new StockPropertyBuilder(this, prop));
            }
            return WithProperty(prop);
        }
        public StockTypeBuilder WithProperty(StockProperty property)
        {
            if (StockType.Properties == null)
            {
                StockType.Properties = new List<IStockProperty> { property };
            }
            else if (StockType.Properties is List<IStockProperty> lst)
            {
                lst.Add(property);
            }
            else
            {
                lst = new List<IStockProperty>(StockType.Properties) { property };
                StockType.Properties = lst;
            }
            return this;
        }

        public StockTypeBuilder WithType(Type? type)
        {
            StockType.Type = type;
            return this;
        }
        public StockTypeBuilder HasAttack(IStockAttack attack)
        {
            HasAttack(StockType, attack);
            return this;
        }
        public StockTypeBuilder HasAttribute(StockAttributeAttack attack)
        {
            return HasAttack(attack);
        }
        public StockTypeBuilder HasAttribute(Attribute attribute,string? name=null)
        {
            return HasAttack(new StockAttributeAttack { Attribute = attribute, Name = name });
        }

        protected override void SetAttack(List<IStockAttack> attacks)
        {
            StockType.Attacks = attacks;
        }
    }
}
