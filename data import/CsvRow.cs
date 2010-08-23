using System;

namespace Samples.DataImport
{
    /// <summary>
    /// Base class for classes
    /// that represent rows in a CSV file.
    /// </summary>
    public abstract class CsvRow
    {
        public long RowNumber { get; set; }
    }
}