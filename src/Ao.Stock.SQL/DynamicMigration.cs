using Ao.Stock.Comparering;
using Ao.Stock.SQL.Announcation;
using FluentMigrator;
using FluentMigrator.Builders;
using FluentMigrator.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Ao.Stock.SQL
{
    public class DynamicMigration : Migration
    {
        public DynamicMigration(IReadOnlyList<IStockComparisonAction> actions)
        {
            Actions = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        public IReadOnlyList<IStockComparisonAction> Actions { get; }

        public override void Down()
        {
        }
        protected virtual void Attacks(StockAttackChangeComparisonAction action)
        {
            if (action.RightProperty != null)
            {
                var maxLen = (StockAttributeAttack?)action.Ups?.FirstOrDefault(x => x is StockAttributeAttack attack && attack.Attribute is MaxLengthAttribute);
                if (maxLen != null)
                {
                    var prop = action.RightProperty;
                    var syntax=Alter.Column(prop.Name)
                        .OnTable(action.RightType.Name);
                    var next = Property(syntax, prop);
                }
            }
        }
        protected virtual void Columns(StockTypePropertiesComparisonAction action)
        {
            if (action.RightProperies != null)
            {
                foreach (var item in action.RightProperies)
                {
                    Delete.Column(item.Name)
                        .FromTable(action.Left.Name);
                }
            }
            if (action.LeftProperies != null)
            {
                foreach (var item in action.LeftProperies)
                {
                    var syntax = Create.Column(item.Name)
                        .OnTable(action.Right.Name);
                    var next = Property(syntax, item);
                    if (item.Type != null && IsNullable(item.Type))
                    {
                        next.Nullable();
                    }
                    if (IsIndex(item))
                    {
                        next.Indexed();
                    }
                    if (IsKey(item))
                    {
                        next.PrimaryKey();
                    }
                }
            }
        }
        protected virtual void CreateType(StockCreateTypeComparisonAction action)
        {
            var syntax = Create.Table(action.Right.Name);
            if (action.Right.Properties != null)
            {
                foreach (var item in action.Right.Properties)
                {
                    var col = syntax.WithColumn(item.Name);
                    var next = Property(col, item);
                    if (item.Type != null && IsNullable(item.Type))
                    {
                        next.Nullable();
                    }
                    if (IsIndex(item))
                    {
                        next.Indexed();
                    }
                    if (IsKey(item))
                    {
                        next.PrimaryKey();
                    }
                }
            }
        }
        protected virtual void DropType(StockDropTypeComparisonAction action)
        {
            Delete.Table(action.Left.Name);
        }
        protected virtual void ChangePropertyType(StockPropertyTypeChangedComparisonAction action)
        {
            var syntx = Alter.Column(action.LeftPropertyType!.Name)
                .OnTable(action.LeftType.Name);
            var next = Property(syntx, action.Right);
            if (action.RightPropertyType != null && IsNullable(action.RightPropertyType))
            {
                next.Nullable();
            }
        }
        protected virtual void RenameType(StockRenameTypeComparisonAction action)
        {
            Rename.Table(action.Left.Name)
                .To(action.Right.Name);
        }
        protected virtual void RenameProperty(RenamePropertyComparisonAction action)
        {
            Rename.Column(action.Left.Name)
                .OnTable(action.LeftType.Name)
                .To(action.Right.Name);
        }

        protected virtual bool IsIndex(IStockProperty property)
        {
            if (property.Attacks != null)
            {
                return property.Attacks.Any(x => x is StockAttributeAttack att && att.Attribute is SqlIndexAttribute);
            }
            return false;
        }
        protected virtual bool IsKey(IStockProperty property)
        {
            if (property.Attacks != null)
            {
                return property.Attacks.Any(x => x is StockAttributeAttack att && att.Attribute is KeyAttribute);
            }
            return false;
        }

        protected virtual bool IsNullable(Type type)
        {
            return type.IsClass || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        protected virtual TNext AsDateTime<TNext>(IColumnTypeSyntax<TNext> columnTypeSyntax, IStockProperty property)
            where TNext : IFluentSyntax
        {
            return columnTypeSyntax.AsDateTime();
        }
        protected virtual TNext AsString<TNext>(IColumnTypeSyntax<TNext> columnTypeSyntax, IStockProperty property)
            where TNext : IFluentSyntax
        {
            var len = 256;
            if (property.Attacks != null)
            {
                var lenAttribute = property.Attacks.OfType<StockAttributeAttack>()
                    .Where(x => x.Attribute is MaxLengthAttribute)
                    .FirstOrDefault();
                if (lenAttribute != null)
                {
                    if (lenAttribute.Attribute is MaxLengthAttribute maxLengthAttribute)
                    {
                        len = maxLengthAttribute.Length;
                    }
                }
            }
            return columnTypeSyntax.AsString(len);
        }
        protected TNext Property<TNext>(IColumnTypeSyntax<TNext> columnTypeSyntax, IStockProperty property)
            where TNext : IFluentSyntax
        {
            TNext? next = default;
            var type = property.Type;
            if (type.IsGenericType&&type.GetGenericTypeDefinition()==typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }
            if (type == null)
            {
                throw new NotSupportedException("Null type for property");
            }
            if (type == typeof(bool))
            {
                next = columnTypeSyntax.AsBoolean();
            }
            else if (type == typeof(byte) || type == typeof(sbyte))
            {
                next = columnTypeSyntax.AsByte();
            }
            else if (type == typeof(short) || type == typeof(ushort))
            {
                next = columnTypeSyntax.AsInt16();
            }
            else if (type == typeof(int) || type == typeof(int))
            {
                next = columnTypeSyntax.AsInt32();
            }
            else if (type == typeof(long) || type == typeof(long))
            {
                next = columnTypeSyntax.AsInt64();
            }
            else if (type == typeof(byte[]))
            {
                next = columnTypeSyntax.AsBinary();
            }
            else if (type == typeof(DateTime))
            {
                next = AsDateTime(columnTypeSyntax, property);
            }
            else if (type == typeof(DateTimeOffset))
            {
                next = columnTypeSyntax.AsDateTimeOffset();
            }
            else if (type == typeof(decimal))
            {
                next = columnTypeSyntax.AsDecimal();
            }
            else if (type == typeof(double))
            {
                next = columnTypeSyntax.AsDouble();
            }
            else if (type == typeof(float))
            {
                next = columnTypeSyntax.AsFloat();
            }
            else if (type == typeof(string))
            {
                next = AsString(columnTypeSyntax, property);
            }
            return next;
        }
        public override void Up()
        {
            foreach (var item in Actions)
            {
                switch (item)
                {
                    case RenamePropertyComparisonAction action:
                        RenameProperty(action);
                        break;
                    case StockRenameTypeComparisonAction action:
                        RenameType(action);
                        break;
                    case StockPropertyTypeChangedComparisonAction action:
                        ChangePropertyType(action);
                        break;
                    case StockDropTypeComparisonAction action:
                        DropType(action);
                        break;
                    case StockCreateTypeComparisonAction action:
                        CreateType(action);
                        break;
                    case StockTypePropertiesComparisonAction action:
                        Columns(action);
                        break;
                    case StockAttackChangeComparisonAction action:
                        Attacks(action);
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
