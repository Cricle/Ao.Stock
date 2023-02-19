using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public abstract class UndefinedDataMirrorCopy<TKey, TResult, TInput> : IMirrorCopy<TResult>
    {
        public const int DefaultSize = 400;

        private int batchIndex;

        protected UndefinedDataMirrorCopy(IDataReader dataReader, IQueryTranslateResult? queryTranslateResult)
            : this(dataReader, queryTranslateResult,DefaultSize)
        {
        }
        protected UndefinedDataMirrorCopy(IDataReader dataReader, IQueryTranslateResult? queryTranslateResult, int batchSize)
        {
            if (batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize), "batchSize must > 0");
            }
            QueryTranslateResult = queryTranslateResult;
            DataReader = dataReader;
            BatchSize = batchSize;
        }

        public IQueryTranslateResult? QueryTranslateResult { get; }

        public int BatchSize { get; }

        public IDataReader DataReader { get; }

        public bool StoreWriteResult { get; set; }

        protected virtual IList<TResult> CreateResultStore(IIntangibleContext? context)
        {
            return Array.Empty<TResult>();
        }
        public async Task<IList<TResult>> CopyAsync(IIntangibleContext? context)
        {
            var storeWriteResult = StoreWriteResult;
            var result = CreateResultStore(context);
            await OnCopyingAsync(context);
            FlatArray<object?>? flatArray = null;
            try
            {
                while (DataReader.Read())
                {
                    if (flatArray == null)
                    {
                        flatArray = new FlatArray<object?>(DataReader.FieldCount, BatchSize);
                        await OnFirstReadAsync(context);
                    }
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        var val = DataReader[i];
                        flatArray.Add(ConvertObject(val, i));
                    }
                    batchIndex++;
                    if (flatArray.IsFull)
                    {
                        var res = await WriteAsync(ConvertToInput(flatArray!, null, context), storeWriteResult);
                        if (storeWriteResult)
                        {
                            result.Add(res);
                        }
                        batchIndex = 0;
                        flatArray.Reset();
                    }
                }
                if (batchIndex != 0 && flatArray != null)
                {
                    var res = await WriteAsync(ConvertToInput(flatArray, batchIndex, context), storeWriteResult);
                    if (storeWriteResult)
                    {
                        result.Add(res);
                    }
                }
                await OnCopyedAsync(context, result);
                return result;
            }
            finally
            {
                flatArray?.Dispose();
            }
        }
        protected virtual object? ConvertObject(object? input, int index)
        {
            if (input == DBNull.Value)
            {
                return null;
            }
            return input;
        }
        protected virtual Task OnFirstReadAsync(IIntangibleContext? context)
        {
            return Task.CompletedTask;
        }
        protected virtual Task OnCopyingAsync(IIntangibleContext? context)
        {
            return Task.CompletedTask;
        }
        protected virtual Task OnCopyedAsync(IIntangibleContext? context, IList<TResult> results)
        {
            return Task.CompletedTask;
        }

        protected abstract TInput ConvertToInput(FlatArray<object?> arr, int? size, IIntangibleContext? context);

        protected abstract Task<TResult> WriteAsync(TInput datas, bool storeWriteResult);
    }
}
