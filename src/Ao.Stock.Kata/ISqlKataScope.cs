using SqlKata.Compilers;
using System.Data.Common;

namespace Ao.Stock.Kata
{
    public interface ISqlKataScope
    {
        Compiler Compiler { get; }

        DbConnection Connection { get; }
    }
}
