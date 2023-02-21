using Ao.Stock.Querying;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Stock.Warehouse
{
    public abstract class SQLBackup : ReaderBackup<string>
    {
        public SQLBackup(IMethodWrapper methodWrapper, string table, string? database = null)
            : base(methodWrapper)
        {
            Table = table ?? throw new ArgumentNullException(nameof(table));
            Database = database;
            if (database == null)
            {
                tableWraper = MethodWrapper.Quto(table);
            }
            else
            {
                tableWraper = MethodWrapper.Quto(database) + "." + MethodWrapper.Quto(table);
            }
        }
        private readonly string tableWraper;
        private string? namePairs;

        public string TableWraper => tableWraper;

        public string Table { get; }

        public string? Database { get; }


        protected virtual string GetInsertHeader(string names)
        {
            return $"INSERT INTO {tableWraper}({names}) VALUES ";
        }

        protected override Task ConvertBeginAsync(IDataReader dataReader, IReadOnlyList<string> names)
        {
            namePairs = GetInsertHeader(string.Join(",", names.Select(x => MethodWrapper.Quto(x))));
            return base.ConvertBeginAsync(dataReader, names);
        }

        protected override string Create(IEnumerable<object[]> datas)
        {
            var s = new StringBuilder(namePairs);
            var last = datas.Count();
            foreach (var item in datas)
            {
                s.Append('(');
                for (int i = 0; i < item.Length; i++)
                {
                    if (i!=0)
                    {
                        s.Append(',');

                    }
                    s.Append(MethodWrapper.WrapValue(item[i]));
                }
                s.Append(')');
                last--;
                if (last > 0) 
                {
                    s.Append(',');
                }
            }
            s.Append(';');
            return s.ToString();
        }
    }
}
