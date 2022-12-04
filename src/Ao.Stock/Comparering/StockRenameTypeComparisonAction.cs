namespace Ao.Stock.Comparering
{
    public class StockRenameTypeComparisonAction : StockTypeComparisonAction
    {
        public StockRenameTypeComparisonAction(IStockType left, IStockType right, string? leftName, string? rightName)
            : base(left, right)
        {
            LeftName = leftName;
            RightName = rightName;
        }

        public string? LeftName { get; }

        public string? RightName { get; }
    }
}
