using System;
using System.Data;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public readonly struct DefaultRowDataReader : IRowDataReader
    {
        private static readonly Task<bool> trueTask = Task.FromResult(true);
        private static readonly Task<bool> falseTask = Task.FromResult(false);

        public DefaultRowDataReader(IDataReader reader, IQueryTranslateResult translateResult)
        {
            Reader = reader ?? throw new ArgumentNullException(nameof(reader));
            TranslateResult = translateResult;
        }

        public IDataReader Reader { get; }

        public object this[string name] => Reader[name];

        public object this[int i] => Reader[i];

        public int FieldCount => Reader.FieldCount;

        public IQueryTranslateResult TranslateResult { get; }

        public bool MoveNext()
        {
            return Reader.Read();
        }

        public Task<bool> MoveNextAsync()
        {
            return MoveNext() ? trueTask : falseTask;
        }

        public string GetName(int index)
        {
            return Reader.GetName(index);
        }

        public Type GetType(int index)
        {
            return Reader.GetFieldType(index);
        }

        public void Dispose()
        {
            Reader.Dispose();
            GC.SuppressFinalize(this);
        }

        public int GetIndex(string name)
        {
            return Reader.GetOrdinal(name);
        }
    }
}
