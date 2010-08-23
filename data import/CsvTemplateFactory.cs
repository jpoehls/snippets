using System;
using System.Text;

namespace Samples.DataImport
{
    public class CsvTemplateFactory
    {
        /// <summary>
        /// Name of the RowNumber property in the CsvRow class.
        /// </summary>
        private const string RowNumberPropertyName = "RowNumber";

        public static string CreateTemplate(Type importerType)
        {
            //  use convention to find the class that represents
            //  a csv row for the importer
            //  convention example: if importer type is "VesselCsvImporter"
            //  then the row class type would be: "VesselCsvRow"

            Type rowType;
            if (importerType.FullName.EndsWith("Importer"))
            {
                var rowTypeName = importerType.FullName.Substring(0, importerType.FullName.Length - 8) + "Row";
                rowType = Type.GetType(rowTypeName, true);
            }
            else
            {
                throw new InvalidOperationException("The importer type " + importerType.FullName + " does not match the required naming conventions. The type name should end with 'Importer'.");
            }
            
            var genericImporterArgs = importerType.GetGenericArguments();
            foreach (var arg in genericImporterArgs)
            {
                if (typeof(CsvRow).IsAssignableFrom(arg))
                {
                    rowType = arg;
                    break;
                }
            }

            if (rowType == null)
            {
                throw new InvalidOperationException("Could not find a generic argument for " +
                                                    importerType.FullName + " that inherits from " +
                                                    typeof (CsvRow).FullName);
            }

            var props = CsvRowHelper.GetProperties(rowType);

            var builder = new StringBuilder();
            foreach (var prop in props)
            {
                if (builder.Length > 0)
                    builder.Append(",");

                var headerName = CsvRowHelper.GetHeaderNameForProperty(prop);

                //  exclude the RowNumber header, this is a special property
                //  that shouldn't be shown in the template
                if (headerName != RowNumberPropertyName)
                {
                    builder.Append(headerName);
                }
            }

            return builder.ToString();
        }


    }
}
