using System;

namespace LibraryApi.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static long? ToTimestamp(this DateTime? value)
        {
            if (value == null) return null;

            return new DateTimeOffset(value.Value).ToUnixTimeSeconds();
        }

        public static long ToTimestamp(this DateTime value)
        {
            return new DateTimeOffset(value).ToUnixTimeSeconds();
        }
    }
}