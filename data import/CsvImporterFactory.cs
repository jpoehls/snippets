using System;
using Samples.Data;
using Samples.Model;

namespace Samples.DataImport
{
    public class CsvImporterFactory
    {
        private readonly BackendDataContextFactory _backendDataContextFactory;

        public CsvImporterFactory(BackendDataContextFactory backendDataContextFactory)
        {
            _backendDataContextFactory = backendDataContextFactory;
        }

        public ICsvImporter GetImporter(EquipmentType equipmentType)
        {
            ICsvImporter importer;

            switch (equipmentType)
            {
                case EquipmentType.Vessel:
                    importer = new VesselCsvImporter(_backendDataContextFactory);
                    break;
                default:
                    throw new ApplicationException(equipmentType + " is not supported for CSV import.");
            }

            return importer;
        }

        public static Type GetImporterType(string fullTypeName)
        {
            return Type.GetType(fullTypeName, true);
        }
    }
}