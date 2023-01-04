using Ao.Stock.Mirror;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ao.Stock.Kata.Mirror
{
    public class SQLMirrorCopy : DataMirrorCopy<string>
    {
        public SQLMirrorCopy(IRowDataReader dataReader, SQLMirrorTarget target, Compiler compiler)
            : base(dataReader)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        }

        public SQLMirrorCopy(IRowDataReader dataReader, SQLMirrorTarget target, Compiler compiler, int batchSize)
            : base(dataReader, batchSize)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        }

        public SQLMirrorTarget Target { get; }

        public Compiler Compiler { get; }

        private string[] names;

        public string[] Names => names;

        protected override Task OnFirstReadAsync(IIntangibleContext context)
        {
            names = new string[DataReader.FieldCount];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = DataReader.GetName(i);
            }
            return base.OnFirstReadAsync(context);
        }

        protected override async Task<RowWriteResult<string>> WriteAsync(IEnumerable<IEnumerable<object>> datas, bool storeWriteResult)
        {
            var access = Compiler.Wrap(Target.TableInfo.Database) + "." + Compiler.Wrap(Target.TableInfo.Table);
            var query = new Query().FromRaw(access).AsInsert(names, datas);
            var sqlResult = Compiler.Compile(query).ToString();
            var affect = await Target.Connection.ExecuteNoQueryAsync(sqlResult);
            return new RowWriteResult<string>(
                names,
                new IQueryTranslateResult[] { new QueryTranslateResult(sqlResult) },
                affect);
        }
    }
}
