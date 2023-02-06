using System;

namespace Ao.Stock
{
    public class DefaultCastHelper : ICastHelper
    {
        public static readonly DefaultCastHelper Default = new DefaultCastHelper();

        public virtual bool TryConvert(object? input, Type outputType, out object? output, out Exception? exception)
        {
            var isNullable = outputType.IsGenericType &&
                    outputType.GetGenericTypeDefinition() == typeof(Nullable<>);
            output = null;
            exception = null;
            if (input == null)
            {
                return isNullable;
            }
            if (input.GetType() == outputType)
            {
                output = input;
                return true;
            }
            if (isNullable)
            {
                outputType = outputType.GenericTypeArguments[0];
            }
            try
            {
                output = Convert.ChangeType(input, outputType);
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }
    }
}
