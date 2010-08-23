using System;
using System.Runtime.Serialization;

namespace Samples.DataImport
{
    public class CsvImportException : ApplicationException
    {
        public CsvImportException()
        {
        }

        public CsvImportException(string message)
            : base(message)
        {
        }

        public CsvImportException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CsvImportException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}