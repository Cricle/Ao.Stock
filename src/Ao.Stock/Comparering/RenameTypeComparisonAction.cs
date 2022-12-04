namespace Ao.Stock.Comparering
{
    public class RenameTypeComparisonAction : StockTypeComparisonAction
    {
        public RenameTypeComparisonAction(IStockType left, IStockType right, string? leftName, string? rightName)
            : base(left, right)
        {
            LeftName = leftName;
            RightName = rightName;
        }

        public string? LeftName { get; }

        public string? RightName { get; }
    }
}
