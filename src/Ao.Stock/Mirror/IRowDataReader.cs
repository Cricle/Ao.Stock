using System;

namespace Ao.Stock.Mirror
{
    public interface IRowDataReader : IDisposable
    {
        object? this[string name] { get; }

        object? this[int i] { get; }

        int FieldCount { get; }

        IQueryTranslateResult? TranslateResult { get; }

        string? GetName(int index);

        int GetIndex(string name);

        Type? GetType(int index);

        bool MoveNext();
    }
}
