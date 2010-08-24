using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Samples.Validation;

namespace Samples.DataImport
{
    public abstract class CsvImporter<TRow> : ICsvImporter
        where TRow : CsvRow, new()
    {
        #region ICsvImporter Members

        public void Import(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(fileName);

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var streamReader = new StreamReader(fileStream))
            using (var reader = new CsvReader(streamReader, true))
            {
                ValidateHeaderRow(reader.GetFieldHeaders());
                BeginTransaction();

                long rowNumber = 1;
                while (reader.ReadNextRecord())
                {
                    try
                    {
                        TRow row = ParseRowFromReader(reader);
                        row.RowNumber = rowNumber;

                        //  validate the row a second time
                        //  (in addition to the validation done in the parse method above)
                        //  this is needed to ensure any properties not parsed
                        //  from the CSV file are valid as well before we process the row
                        ValidateRow(row);

                        //  do the actual import for this row
                        ProcessRow(row);
                    }
                    catch (Exception ex)
                    {
                        if (ex is ValidationException ||
                            ex is CsvImportException ||
                            ex is FormatException)
                        {
                            var record = new string[reader.FieldCount];
                            reader.CopyCurrentRecordTo(record);
                            var message = string.Format("Error importing row {0}. [Current record: \"{1}\"]",
                                                        rowNumber, string.Join("\", \"", record));
                            throw new CsvImportException(message, ex);
                        }

                        throw;
                    }

                    rowNumber++;
                }

                CommitTransaction();
            }
        }

        public abstract void Dispose();

        #endregion

        protected abstract void ProcessRow(TRow row);
        protected abstract void BeginTransaction();
        protected abstract void CommitTransaction();

        private static TRow ParseRowFromReader(CsvReader reader)
        {
            var row = new TRow();

            var properties = CsvRowHelper.GetProperties(typeof(TRow));
            foreach (var prop in properties)
            {
                var header = CsvRowHelper.GetHeaderNameForProperty(prop);
                var index = reader.GetFieldIndex(header);
                if (index >= 0)
                {
                    try
                    {
                        object value = CsvValueParser.Parse(reader[index], prop.PropertyType);
                        prop.SetValue(row, value, null);

                        //  validate this property so that if there is something wrong
                        //  we can show a detailed error with the actual CSV field number and value
                        DataAnnotationValidator.Validate(row, prop);
                    }
                    catch (Exception ex)
                    {
                        int fieldNumber = index + 1;
                        string message = string.Format("Error parsing field {0}, {1}, into a {2}. [Field value: \"{3}\"]",
                                                       fieldNumber, header, prop.PropertyType, reader[index]);
                        throw new CsvImportException(message, ex);
                    }
                }
            }

            return row;
        }

        private static void ValidateRow(TRow row)
        {
            var properties = CsvRowHelper.GetProperties(typeof(TRow));
            DataAnnotationValidator.Validate(row, properties);
        }

        private static void ValidateHeaderRow(IEnumerable<string> actualHeaders)
        {
            IEnumerable<string> requiredHeaders = CsvRowHelper.GetRequiredHeaders<TRow>();

            IEnumerable<string> missingHeaders = requiredHeaders.Except(actualHeaders, StringComparer.OrdinalIgnoreCase);
            if (missingHeaders.Count() > 0)
            {
                throw new CsvImportException("Not all required header fields were present. " +
                                             "[Missing headers: " +
                                             string.Join(", ", missingHeaders.ToArray())
                                             + "]");
            }
        }
    }
}