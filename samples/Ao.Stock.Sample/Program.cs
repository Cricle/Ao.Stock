using Ao.Stock.Comparering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ao.Stock.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = StockHelper.FromType(typeof(A), null);
            var b= StockHelper.FromType(typeof(B), null);

            var res = DefaultStockTypeComparer.Default.Compare(a, b);
        }
    }
    class A
    {
        public int Id { get; set; }
    }
    [Table("a")]
    class B
    {
        [Key]
        public int Id { get; set; }

        public bool Bx { get; set; }
    }
}