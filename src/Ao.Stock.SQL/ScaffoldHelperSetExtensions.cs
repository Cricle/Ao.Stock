using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Ao.Stock.SQL
{
    public static class ScaffoldHelperSetExtensions
    {
        public static ScaffoldHelper OnlyTable(this ScaffoldHelper helper, params string[] tableNames)
        {
            helper.DatabaseModelFactoryOptions = new DatabaseModelFactoryOptions(tableNames);
            return helper;
        }
    }
}
