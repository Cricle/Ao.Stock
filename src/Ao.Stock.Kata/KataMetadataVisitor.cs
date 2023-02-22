using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ao.Stock.Kata
{
    public class KataMetadataVisitor : DefaultMetadataVisitor<DefaultQueryContext>
    {
        public static KataMetadataVisitor Mysql(Query root, MethodTranslator<Compiler> translator = null)
        {
            return new KataMetadataVisitor(new MySqlCompiler(), root,
                translator ?? SqlMethodTranslatorHelpers<Compiler>.Mysql(), DefaultMethodWrapper.MySql);
        }
        public static KataMetadataVisitor MariaDB(Query root, MethodTranslator<Compiler> translator = null)
        {
            return new KataMetadataVisitor(new MySqlCompiler(), root,
                translator ?? SqlMethodTranslatorHelpers<Compiler>.Mysql(), DefaultMethodWrapper.MySql);
        }
        public static KataMetadataVisitor Sqlite(Query root, MethodTranslator<Compiler> translator = null)
        {
            return new KataMetadataVisitor(new SqliteCompiler(), root,
                translator ?? SqlMethodTranslatorHelpers<Compiler>.Sqlite(), DefaultMethodWrapper.Sqlite);
        }
        public static KataMetadataVisitor SqlServer(Query root, MethodTranslator<Compiler> translator = null)
        {
            return new KataMetadataVisitor(new SqlServerCompiler(), root,
                translator ?? SqlMethodTranslatorHelpers<Compiler>.SqlServer(), DefaultMethodWrapper.SqlServer);
        }
        public static KataMetadataVisitor PostgrSql(Query root, MethodTranslator<Compiler> translator = null)
        {
            return new KataMetadataVisitor(new PostgresCompiler(), root,
                translator ?? SqlMethodTranslatorHelpers<Compiler>.PostgrSql(),DefaultMethodWrapper.PostgreSql);
        }

        public KataMetadataVisitor(Compiler compiler, Query root, MethodTranslator<Compiler> translator, IMethodWrapper methodWrapper)
        {
            Compiler = compiler;
            Root = root;
            Translator = translator;
            MethodWrapper = methodWrapper;
        }

        public Compiler Compiler { get; }

        public Query Root { get; }

        public MethodTranslator<Compiler> Translator { get; }

        public IMethodWrapper MethodWrapper { get; }

        public override DefaultQueryContext CreateContext(IQueryMetadata metadata)
        {
            return new DefaultQueryContext();
        }

        protected override void OnVisitSort(SortMetadata value, DefaultQueryContext context)
        {
            Root.OrderByRaw(context.Expression + (value.SortMode == SortMode.Desc ? " DESC" : ""));
        }

        public override void VisitSort(SortMetadata value, DefaultQueryContext context)
        {
            base.VisitSort(value, context);
        }

        protected override void OnVisitFilter(FilterMetadata value, IQueryMetadata metadata, DefaultQueryContext context)
        {
            Root.WhereRaw(context.Expression);
        }

        protected override void OnVisitGroup(GroupMetadata value, DefaultQueryContext context, List<string> groups)
        {
            Root.GroupByRaw(string.Join(", ", groups));
        }

        protected override void OnVisitMethod(MethodMetadata method, DefaultQueryContext context, string[] args)
        {
            context.Expression = Translator.Translate(method, Compiler);
        }
        protected override void OnVisitBinary(BinaryMetadata value, DefaultQueryContext context, DefaultQueryContext leftContext, DefaultQueryContext rightContext)
        {
            var tk = value.ExpressionType == ExpressionType.Equal ? "=" : value.GetToken();
            context.Expression += leftContext.Expression + tk + rightContext.Expression;
        }
        protected override void OnVisitSelect(SelectMetadata value, DefaultQueryContext context, List<string> selects)
        {
            Root.SelectRaw(string.Join(", ", selects));
        }
        public override void VisitAlias(AliasMetadata value, DefaultQueryContext context)
        {
            var ctx = CreateContext(value.Target);
            Visit(value.Target, ctx);
            context.Expression += $"{ctx.Expression} as {MethodWrapper.Quto(value.Alias)}";
        }
        public override void VisitValue(ValueMetadata value, DefaultQueryContext context)
        {
            if (context.MustQuto || value.Quto)
            {
                context.Expression += MethodWrapper.Quto(value.Value?.ToString());
            }
            else
            {
                context.Expression += ValueToString(value.Value);
            }
        }
        protected virtual string ValueToString(object value)
        {
            return MethodWrapper.WrapValue(value);
        }
        public override void VisitSkip(SkipMetadata value, DefaultQueryContext context)
        {
            Root.Skip(value.Value);
        }
        public override void VisitLimit(LimitMetadata value, DefaultQueryContext context)
        {
            Root.Limit(value.Value);
        }
    }
}
