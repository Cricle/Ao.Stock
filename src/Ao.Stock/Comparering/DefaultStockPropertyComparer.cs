using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public class DefaultStockPropertyComparer : IStockPropertyComparer
    {
        public static readonly DefaultStockPropertyComparer Default = new DefaultStockPropertyComparer();

        protected virtual IStockComparisonAction? CompareAttacks(IReadOnlyList<IStockAttack>? lefts, IReadOnlyList<IStockAttack>? rights)
        {
            return StockAttackComparerHelper.CompareAttack(lefts, rights);
        }

        public IReadOnlyList<IStockComparisonAction> Compare(IStockProperty left, IStockProperty right)
        {
            var lst = new List<IStockComparisonAction>(0);
            if (left.Name != right.Name)
            {
                lst.Add(new RenamePropertyComparisonAction(left, right, left.Name, right.Name));
            }
            if (left.Type != right.Type)
            {
                lst.Add(new StockPropertyTypeChangedComparisonAction(left, right, left.Type, right.Type));
            }
            var attackAct = CompareAttacks(left.Attacks, right.Attacks);
            if (attackAct != null)
            {
                lst.Add(attackAct);
            }
            return lst;
        }
    }
}
