using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ao.Stock.Kata
{
    public class KataMetadataVisitor : DefaultMetadataVisitor<DefaultQueryContext>
    {
        public static KataMetadataVisitor Mysql(Query root, MethodTranslator<Compiler> translator=null)
        {
            return new KataMetadataVisitor(new MySqlCompiler(), root, 
                translator ?? new DefaultMethodTranslator<Compiler>(KnowsMethods.Functions,DefaultMethodWrapper<Compiler>.MySql));
        }
        public static KataMetadataVisitor Sqlite(Query root, MethodTranslator<Compiler> translator = null)
        {
            var funcs = KnowsMethods.Functions;
            funcs[KnowsMethods.Year] = "strftime('%Y',{1})";
            funcs[KnowsMethods.Month] = "strftime('%m',{1})";
            funcs[KnowsMethods.Day] = "strftime('%d',{1})";
            funcs[KnowsMethods.Hour] = "strftime('%H',{1})";
            funcs[KnowsMethods.Minute] = "strftime('%M',{1})";
            funcs[KnowsMethods.Second] = "strftime('%S',{1})";
            funcs[KnowsMethods.Date] = "strftime('%Y-%m-%d',{1})";
            funcs[KnowsMethods.Now] = "strftime('%Y-%m-%d %H-%M-%S','now')";
            funcs[KnowsMethods.DateFormat] = "strftime({2},{1})";
            funcs[KnowsMethods.Weak] = "strftime('%W',{1})";
            funcs[KnowsMethods.Quarter] = "COALESCE(NULLIF((SUBSTR({1}, 4, 2) - 1) / 3, 0), 4)";
            funcs[KnowsMethods.StrLen] = "LENGTH({1})";
            funcs[KnowsMethods.StrIndexOf] = "instr({1},{2})";
            return new KataMetadataVisitor(new SqliteCompiler(), root, 
                translator ?? new DefaultMethodTranslator<Compiler>(funcs, DefaultMethodWrapper<Compiler>.Sqlite));
        }
        public static KataMetadataVisitor SqlServer(Query root, MethodTranslator<Compiler> translator = null)
        {
            var funcs = KnowsMethods.Functions;
            funcs[KnowsMethods.StrLen] = "LEN({1})";
            funcs[KnowsMethods.StrIndexOf] = "CHARINDEX({1},{2})";
            funcs[KnowsMethods.StrSub] = "SUBSTRING({1},{2},{3})";
            return new KataMetadataVisitor(new MySqlCompiler(), root, 
                translator ?? new DefaultMethodTranslator<Compiler>(funcs, DefaultMethodWrapper<Compiler>.SqlServer));
        }

        public KataMetadataVisitor(Compiler compiler, Query root, MethodTranslator<Compiler> translator)
        {
            Compiler = compiler;
            Root = root;
            Translator = translator;
        }

        public Compiler Compiler { get; }

        public Query Root { get; }

        public MethodTranslator<Compiler> Translator { get; }

        public override DefaultQueryContext CreateContext(IQueryMetadata metadata)
        {
            return new DefaultQueryContext ();
        }

        protected override void OnVisitSort(SortMetadata value, DefaultQueryContext context)
        {
            Root.OrderByRaw(context.Expression + (value.SortMode == SortMode.Desc ?" DESC":""));
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
            var tk =value.ExpressionType== ExpressionType.Equal?"=": value.GetToken();
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
            context.Expression += $"{ctx.Expression} as {Compiler.Wrap(value.Alias)}";
        }
        public override void VisitValue(ValueMetadata value, DefaultQueryContext context)
        {
            if (context.MustQuto || value.Quto)
            {
                context.Expression += Compiler.Wrap(value.Value?.ToString());
            }
            else
            {
                if (value.Value is DateTime||value .Value is string)
                {
                    context.Expression += "'" + ValueToString(value.Value) + "'";
                }
                else
                {
                    context.Expression += ValueToString(value.Value);
                }
            }
        }
        protected virtual string ValueToString(object value)
        {
            if (value ==null)
            {
                return "null";
            }
            if (value is DateTime||value is DateTime?)
            {
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return value.ToString();
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
