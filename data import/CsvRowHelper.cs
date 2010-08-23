using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Samples.DataImport
{
    public class CsvRowHelper
    {
        /// <summary>
        /// Gets all properties that should be counted
        /// as part of the CSV row.
        /// </summary>
        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties;
        }

        /// <summary>
        /// Gets all required headers for the CSV row.
        /// </summary>
        public static IEnumerable<string> GetRequiredHeaders<TRow>()
            where TRow : CsvRow
        {
            var headers = typeof(TRow)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0)
                .Select(p => GetHeaderNameForProperty(p));

            return headers;
        }

        /// <summary>
        /// Gets the CSV header name for the given property.
        /// </summary>
        public static string GetHeaderNameForProperty(PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(CsvRowHeaderAttribute), false);
            if (attrs.Length > 0)
            {
                return ((CsvRowHeaderAttribute)attrs[0]).Header;
            }
            return propertyInfo.Name;
        }
    }
}
