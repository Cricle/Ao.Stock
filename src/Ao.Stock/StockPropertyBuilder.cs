using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public class StockPropertyBuilder : StockBuilder
    {
        public StockPropertyBuilder(StockTypeBuilder stockTypeBuilder, StockProperty property)
        {
            StockTypeBuilder = stockTypeBuilder;
            Property = property;
        }

        public StockTypeBuilder StockTypeBuilder { get; }

        public StockProperty Property { get; }

        public StockPropertyBuilder WithType(Type? type)
        {
            Property.Type = type;
            return this;
        }
        protected override void SetAttack(List<IStockAttack> attacks)
        {
            Property.Attacks = attacks;
        }
        public StockPropertyBuilder HasAttack(IStockAttack attack)
        {
            HasAttack(Property, attack);
            return this;
        }
        public StockPropertyBuilder HasAttribute(StockAttributeAttack attack)
        {
            return HasAttack(attack);
        }
        public StockPropertyBuilder HasAttribute(Attribute attribute, string? name = null)
        {
            return HasAttack(new StockAttributeAttack { Attribute = attribute, Name = name });
        }
    }
}
