using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;

namespace Ao.Stock.Kata
{
    public class KataMetadataVisitor : DefaultMetadataVisitor<DefaultQueryContext>
    {
        public static KataMetadataVisitor Mysql(Query root, MethodTranslator<Compiler> translator=null)
        {
            return new KataMetadataVisitor(new MySqlCompiler(), root, translator ?? new DefaultMethodTranslator<Compiler>(KnowsMethods.Functions,DefaultMethodWrapper<Compiler>.MySql));
        }
        public static KataMetadataVisitor SqlServer(Query root, MethodTranslator<Compiler> translator = null)
        {
            var funcs = KnowsMethods.Functions;
            funcs[KnowsMethods.StrLen] = "LEN({1})";
            funcs[KnowsMethods.StrIndexOf] = "CHARINDEX({1},{2})";
            funcs[KnowsMethods.StrSub] = "SUBSTRING({1},{2},{3})";
            return new KataMetadataVisitor(new MySqlCompiler(), root, translator ?? new DefaultMethodTranslator<Compiler>(funcs, DefaultMethodWrapper<Compiler>.SqlServer));
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
    }
}
