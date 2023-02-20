using System;
using System.Runtime.CompilerServices;

namespace Ao.Stock
{
    public static class DateTimeToStringExtensions
    {
        public unsafe static void ToFullString(this DateTime dt, ref Span<char> buffer)
        {
            var month = dt.Month;
            var day = dt.Day;
            var hour = dt.Hour;
            var minute = dt.Minute;
            var second = dt.Second;
#if NETSTANDARD2_0
            dt.Year.ToString().AsSpan().CopyTo(buffer);
#else
            dt.Year.ToString().CopyTo(buffer);
#endif
            buffer[4] = '-';
            if (month <= 9)
            {
                buffer[5] = '0';
                buffer[6] = (char)('0' + month);
            }
            else
            {
                var q = month / 10;
                buffer[5] = (char)('0' + q);
                buffer[6] = (char)('0' + month - (q * 10));
            }
            buffer[7] = '-';
            if (day <= 9)
            {
                buffer[8] = '0';
                buffer[9] = (char)('0' + day);
            }
            else
            {
                var q = day / 10;
                buffer[8] = (char)('0' + q);
                buffer[9] = (char)('0' + day - (q * 10));
            }
            buffer[10] = ' ';
            if (hour <= 9)
            {
                buffer[11] = '0';
                buffer[12] = (char)('0' + hour);
            }
            else
            {
                var q = hour / 10;
                buffer[11] = (char)('0' + q);
                buffer[12] = (char)('0' + hour - (q * 10));
            }
            buffer[13] = ':';
            if (minute <= 9)
            {
                buffer[14] = '0';
                buffer[15] = (char)('0' + minute);
            }
            else
            {
                var q = minute / 10;
                buffer[14] = (char)('0' + q);
                buffer[15] = (char)('0' + minute - (q * 10));
            }
            buffer[16] = ':';
            if (second <= 9)
            {
                buffer[17] = '0';
                buffer[18] = (char)('0' + second);
            }
            else
            {
                var q = second / 10;
                buffer[17] = (char)('0' + q);
                buffer[18] = (char)('0' + second - (q * 10));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static string ToFullString(DateTime dt)
        {
            Span<char> buffer = stackalloc char[19];
            ToFullString(dt, ref buffer);
            return buffer.ToString();
        }

    }
}
