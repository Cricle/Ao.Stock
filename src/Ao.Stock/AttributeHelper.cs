using System;

namespace Ao.Stock
{
    public static class AttributeHelper
    {
        public static bool Equals(Attribute? left, Attribute? right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            if (left == null || right == null)
            {
                return false;
            }
            return left.Equals(right);
        }
    }
}
