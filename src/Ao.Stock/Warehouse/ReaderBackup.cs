using Ao.Stock.Querying;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ao.Stock.Warehouse
{
    public abstract class ReaderBackup<TOutput>
    {
        public ReaderBackup(IMethodWrapper methodWrapper)
        {
            MethodWrapper = methodWrapper ?? throw new ArgumentNullException(nameof(methodWrapper));
        }

        public int BulkSize { get; set; } = 100;

        public IMethodWrapper MethodWrapper { get; }

        protected virtual Task ConvertBeginAsync(IDataReader dataReader,IReadOnlyList<string> names)
        {
            return Task.CompletedTask;
        }

        public async Task<int> ConvertAsync(IDataReader input)
        {
            var count = 0;
            var arr = new object[BulkSize][];
            var index = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new object[input.FieldCount];
            }
            var names = new string[input.FieldCount];
            for (int i = 0; i < input.FieldCount; i++)
            {
                names[i] = input.GetName(i);
            }
            await ConvertBeginAsync(input,names);
            while (input.Read())
            {
                input.GetValues(arr[index++]!);
                if (index > arr.Length - 1)
                {
                    var sql = Create(arr);
                    await WriteAsync(sql);
                    index = 0;
                    count += arr.Length;
                }
            }
            if (index != 0)
            {
                var sql = Create(arr.Take(index));
                count += index;
                await WriteAsync(sql);
            }
            return count;
        }
        protected abstract TOutput Create(IEnumerable<object[]> datas);

        protected abstract Task WriteAsync(TOutput output);

        public void Dispose()
        {
            OnDispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose()
        {

        }
    }
}
