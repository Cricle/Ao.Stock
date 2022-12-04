using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock.Comparering
{
    public class DefaultStockTypeComparer : IStockTypeComparer
    {
        public static readonly DefaultStockTypeComparer Default = new DefaultStockTypeComparer(DefaultStockPropertyComparer.Default);

        public DefaultStockTypeComparer(IStockPropertyComparer propertyComparer)
        {
            PropertyComparer = propertyComparer ?? throw new ArgumentNullException(nameof(propertyComparer));
        }

        public IStockPropertyComparer PropertyComparer { get; }

        protected virtual IStockComparisonAction? CompareAttacks(IReadOnlyList<IStockAttack>? lefts, IReadOnlyList<IStockAttack>? rights)
        {
            return StockAttackComparerHelper.CompareAttack(lefts, rights);
        }

        protected virtual bool IsPropertyEquals(IStockProperty left, IStockProperty right)
        {
            return left.Name == right.Name;
        }

        protected virtual void CompareProperies(IStockType left, IStockType right, List<IStockComparisonAction> actions)
        {
            if (left == null && right == null)
            {
                return;
            }
            if (left != null && right != null)
            {
                if (left.Properties == null && right.Properties == null)
                {
                    return;
                }
                else if (left.Properties != null && right.Properties != null)
                {
                    var leftLess = right.Properties.Where(x => !left.Properties.Any(y => IsPropertyEquals(x, y))).ToList();
                    var rightLess = left.Properties.Where(x => !right.Properties.Any(y => IsPropertyEquals(x, y))).ToList();
                    if (leftLess.Count != 0 || rightLess.Count != 0)
                    {
                        actions.Add(new StockTypePropertiesComparisonAction(left, right, leftLess, rightLess));
                    }
                    foreach (var item in left.Properties)
                    {
                        var rigthProp = right.Properties.FirstOrDefault(x => IsPropertyEquals(x, item));
                        if (rigthProp != null)
                        {
                            actions.AddRange(PropertyComparer.Compare(left,item, right, rigthProp));
                        }
                    }
                }
                else
                {
                    actions.Add(new StockTypePropertiesComparisonAction(left, right, left.Properties, right.Properties));
                }
            }
        }
        public IReadOnlyList<IStockComparisonAction> Compare(IStockType left, IStockType right)
        {
            var lst = new List<IStockComparisonAction>(0);
            if (left.Name != right.Name)
            {
                lst.Add(new StockRenameTypeComparisonAction(left, right, left.Name, right.Name));
            }
            if (left.Type != right.Type)
            {
                lst.Add(new StockTypeTypeChangedComparisonAction(left, right, left.Type, right.Type));
            }
            var attackAct = CompareAttacks(left.Attacks, right.Attacks);
            if (attackAct != null)
            {
                lst.Add(attackAct);
            }
            CompareProperies(left, right, lst);
            return lst;
        }
    }
}
