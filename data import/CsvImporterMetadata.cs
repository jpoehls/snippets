using System;

namespace Samples.DataImport
{
    public class CsvImporterMetadata
    {
        public CsvImporterMetadata(string name, Type importerType)
        {
            Name = name;
            ImporterType = importerType;
        }

        public string Name { get; private set; }
        public Type ImporterType { get; private set; }
        public string ImporterTypeFullName
        {
            get
            {
                return ImporterType == null ? null : ImporterType.FullName;
            }
        }
    }
}