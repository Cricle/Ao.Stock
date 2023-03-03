using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ao.Stock.Querying;

namespace Ao.Stock.Mirror
{
    public class SQLMirrorCopy : DataMirrorCopy<string>
    {
        public SQLMirrorCopy(IDataReader dataReader, IQueryTranslateResult queryTranslateResult, SQLMirrorTarget target, IMethodWrapper methodWrapper)
            : base(dataReader, queryTranslateResult)
        {
            Target = target;
            MethodWrapper = methodWrapper ?? throw new ArgumentNullException(nameof(methodWrapper));
        }

        public SQLMirrorCopy(IDataReader dataReader, IQueryTranslateResult queryTranslateResult, SQLMirrorTarget target, IMethodWrapper methodWrapper, int batchSize)
            : base(dataReader, queryTranslateResult, batchSize)
        {
            Target = target;
            MethodWrapper = methodWrapper ?? throw new ArgumentNullException(nameof(methodWrapper));
        }


        public SQLMirrorTarget Target { get; }

        public IMethodWrapper MethodWrapper { get; }

        private string[]? names;

        public IReadOnlyList<string>? Names => names;

        protected override Task OnFirstReadAsync()
        {
            names = new string[DataReader.FieldCount];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = DataReader.GetName(i);
            }
            return base.OnFirstReadAsync();
        }

        protected virtual string GetInsertHeader()
        {
            return $"INSERT INTO {Target.Named} VALUES ";
        }

        protected virtual string? CompileInsertScript(IEnumerable<IEnumerable<object?>> datas)
        {
            if (!datas.Any())
            {
                return null;
            }
            var scriptBuilder = new StringBuilder(GetInsertHeader());
            var count = datas.Count();
            foreach (var item in datas)
            {
                scriptBuilder.Append('(');
                scriptBuilder.Append(string.Join(",", item.Select(x => MethodWrapper.WrapValue(x))));
                scriptBuilder.Append(')');
                count--;
                if (count>0)
                {
                    scriptBuilder.Append(',');
                }
            }
            return scriptBuilder.ToString();
        }

        protected override async Task<RowWriteResult<string>> WriteAsync(IEnumerable<IEnumerable<object?>> datas, bool storeWriteResult)
        {
            var script = CompileInsertScript(datas);
            if (script==null)
            {
                return RowWriteResult<string>.Empty;
            }
            var affect = await Target.Connection.ExecuteNonQueryAsync(script);
            return new RowWriteResult<string>(
                names,
                new IQueryTranslateResult[] { new QueryTranslateResult(script) },
                affect);
        }
    }

    public abstract class DataMirrorCopy<TKey> : UndefinedDataMirrorCopy<TKey, RowWriteResult<TKey>, IEnumerable<IEnumerable<object?>>>
    {
        protected DataMirrorCopy(IDataReader dataReader, IQueryTranslateResult? queryTranslateResult) : base(dataReader, queryTranslateResult)
        {
        }

        protected DataMirrorCopy(IDataReader dataReader, IQueryTranslateResult? queryTranslateResult, int batchSize) : base(dataReader, queryTranslateResult, batchSize)
        {
        }

        protected override IEnumerable<IEnumerable<object?>> ConvertToInput(FlatArray<object?> arr, int? size)
        {
            if (size == null)
            {
                return arr;
            }
            return arr.Take(size.Value);
        }
    }
}
