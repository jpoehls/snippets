using System;
using Samples.Data;

namespace Samples.DataImport
{
    public abstract class LinqToSqlCsvImporter<TRow> : CsvImporter<TRow>
        where TRow : CsvRow, new()
    {
        protected EquipmentCsvImporter(IMyDataContextFactory myDataContextFactory)
        {
            DbFactory = dataContextFactory;
        }

        private readonly IMyDataContextFactory DbFactory;
        protected MyDataContext Db { get; private set; }

        protected override void BeginTransaction()
        {
            Db = DbFactory.CreateContext();
        }

        protected override void CommitTransaction()
        {
            Db.SubmitChanges();
        }

        public override void Dispose()
        {
            if (Db != null)
            {
                Db.Dispose();
            }
        }
    }
}