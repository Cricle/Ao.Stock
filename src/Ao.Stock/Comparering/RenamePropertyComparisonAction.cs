namespace Ao.Stock.Comparering
{
    public class RenamePropertyComparisonAction : StockPropertyComparisonAction
    {
        public RenamePropertyComparisonAction(IStockType leftStockType,
            IStockType rightStockType, 
            IStockProperty left, 
            IStockProperty right, 
            string? leftName, 
            string? rightName)
            : base(leftStockType, rightStockType,left, right)
        {
            LeftName = leftName;
            RightName = rightName;
        }

        public string? LeftName { get; }

        public string? RightName { get; }
    }
}
