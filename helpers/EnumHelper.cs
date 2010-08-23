using System;

namespace Samples
{
    public static class EnumHelper
    {
        public static T Parse<T>(string value) where T : struct
        {
            return Parse<T>(value, false);
        }

        public static T Parse<T>(string value, bool ignoreCase) where T : struct
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            var result = (T) Enum.Parse(typeof (T), value, ignoreCase);
            return result;
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            return Parse<T>(value);
        }

        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            return Parse<T>(value, ignoreCase);
        }

        public static T ToEnum<T>(this int value) where T : struct
        {
            return Parse<T>(value.ToString());
        }
    }
}