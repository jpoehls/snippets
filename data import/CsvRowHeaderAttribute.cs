using System;

namespace Samples.DataImport
{
    public class CsvRowHeaderAttribute : Attribute
    {
        public string Header { get; private set; }

        public CsvRowHeaderAttribute(string header)
        {
            Header = header;
        }
    }
}