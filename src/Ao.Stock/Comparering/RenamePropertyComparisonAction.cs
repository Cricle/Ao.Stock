namespace Ao.Stock.Comparering
{
    public class RenamePropertyComparisonAction : StockPropertyComparisonAction
    {
        public RenamePropertyComparisonAction(IStockProperty left, IStockProperty right, string? leftName, string? rightName)
            : base(left, right)
        {
            LeftName = leftName;
            RightName = rightName;
        }

        public string? LeftName { get; }

        public string? RightName { get; }
    }
}
