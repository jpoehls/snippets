using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Samples.DataImport
{
    public class CsvImporterRepository
    {
        public static IEnumerable<CsvImporterMetadata> GetCsvImporters()
        {
            var importers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => typeof (ICsvImporter).IsAssignableFrom(x) && !x.IsAbstract)
                .OrderBy(x => x.Name)
                .Select(x => new CsvImporterMetadata(x.Name, x));

            return importers;
        }
    }
}