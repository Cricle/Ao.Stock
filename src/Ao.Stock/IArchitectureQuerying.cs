using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ao.Stock
{
    public interface IArchitectureQuerying : IDisposable
    {
        Task<IList<string>> GetDatabasesAsync();

        Task<IList<string>> GetTablesAsync(string database, IIntangibleContext? context);
    }
}
