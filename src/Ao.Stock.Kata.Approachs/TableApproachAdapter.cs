using SqlKata;
using SqlKata.Execution;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Kata.Approachs
{
    public class TableApproachAdapter : ApproachAdapter
    {
        public TableApproachAdapter(IStockType stockType, 
            string tableName,
            QueryFactory queryFactory)
            : base(stockType)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            QueryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
        }

        public string TableName { get; }

        public QueryFactory QueryFactory { get; }

        public Query Query()
        {
            return new Query(TableName);
        }

        public Task<int> ClearAsync(IDbTransaction transaction = null, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return Query().DeleteAsync(transaction, timeout, cancellationToken);
        }

        protected override void OnDispose()
        {
            QueryFactory.Dispose();
        }
    }
}
