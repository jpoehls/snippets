using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.DataImport
{
    public class CsvValueParser
    {
        private const string BlankConstant = "$BLANK";
        private static readonly Dictionary<string, bool> _boolValueMap = new Dictionary<string, bool>
                                                                             {
                                                                                 {"1", true},
                                                                                 {"yes", true},
                                                                                 {"true", true},
                                                                                 {"0", true},
                                                                                 {"no", true},
                                                                                 {"false", true}
                                                                             };

        public static object Parse(string value, Type targetType)
        {
            value = value.Trim();
            bool isBlank = string.IsNullOrEmpty(value);

            if (targetType == typeof(bool))
            {
                return ParseBool(value);
            }
            if (targetType == typeof(bool?))
            {
                return ParseNullableBool(value);
            }
            if (targetType == typeof(int))
            {
                return isBlank ? default(int) : Int32.Parse(value);
            }
            if (targetType == typeof(int?))
            {
                return isBlank ? default(int?) : Int32.Parse(value);
            }
            if (targetType == typeof(double))
            {
                return isBlank ? default(double) : Double.Parse(value);
            }
            if (targetType == typeof(double?))
            {
                return isBlank ? default(double?) : Double.Parse(value);
            }
            if (targetType == typeof(DateTime))
            {
                return isBlank ? DateTime.MinValue : DateTime.Parse(value);
            }
            if (targetType == typeof(DateTime?))
            {
                return isBlank ? default(DateTime?) : DateTime.Parse(value);
            }
            if (targetType == typeof(string))
            {
                if (string.Equals(value, BlankConstant, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                return value;
            }

            return Convert.ChangeType(value, targetType);
        }

        private static bool? ParseNullableBool(string value)
        {
            foreach (string key in _boolValueMap.Keys)
            {
                if (string.Equals(value, key, StringComparison.OrdinalIgnoreCase))
                {
                    return _boolValueMap[key];
                }
            }

            return null;
        }

        private static bool ParseBool(string value)
        {
            foreach (string key in _boolValueMap.Keys)
            {
                if (string.Equals(value, key, StringComparison.OrdinalIgnoreCase))
                {
                    return _boolValueMap[key];
                }
            }

            //  default to false if not found
            return false;
        }
    }
}