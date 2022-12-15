using System.Collections.Generic;

namespace Ao.Stock
{
    public abstract class StockBuilder
    {
        protected abstract void SetAttack(List<IStockAttack> attacks);

        protected void HasAttack<T>(T input,IStockAttack attack)
            where T:IStockAttachable
        {
            if (input.Attacks == null)
            {
                SetAttack(new List<IStockAttack> { attack });
            }
            else if (input.Attacks is List<IStockAttack> lst)
            {
                lst.Add(attack);
            }
            else
            {
                lst = new List<IStockAttack>(input.Attacks) { attack };
                SetAttack(lst);
            }
        }
    }
}
