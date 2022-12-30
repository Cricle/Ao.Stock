using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public class DefaultStockPropertyComparer : IStockPropertyComparer
    {
        public static readonly DefaultStockPropertyComparer Default = new DefaultStockPropertyComparer();

        protected virtual IStockComparisonAction? CompareAttacks(IStockType leftType, IStockProperty left, IStockType rightType, IStockProperty right, IReadOnlyList<IStockAttack>? lefts, IReadOnlyList<IStockAttack>? rights)
        {
            return StockAttackComparerHelper.CompareAttack(leftType, left, rightType, right, lefts, rights);
        }

        public IReadOnlyList<IStockComparisonAction> Compare(IStockType leftType, IStockProperty left, IStockType rightType, IStockProperty right)
        {
            var lst = new List<IStockComparisonAction>(0);
            if (left.Name != right.Name)
            {
                lst.Add(new RenamePropertyComparisonAction(leftType, rightType, left, right, left.Name, right.Name));
            }
            if (left.Type != right.Type)
            {
                lst.Add(new StockPropertyTypeChangedComparisonAction(leftType, rightType, left, right, left.Type, right.Type));
            }
            var attackAct = CompareAttacks(leftType, left, rightType, right, left.Attacks, right.Attacks);
            if (attackAct != null)
            {
                lst.Add(attackAct);
            }
            return lst;
        }
    }
}
