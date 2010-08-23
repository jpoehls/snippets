using System;
using Samples.Data;
using System.Linq;
using Samples.Model;

namespace Samples.DataImport
{
    public class SampleCsvImporter : LinqToSqlCsvImporter<SampleCsvRow>
    {
        public SampleCsvImporter(MyDataContextFactory myDataContextFactory)
            : base(myDataContextFactory)
        {
        }

        protected override void ProcessRow(SampleCsvRow row)
        {
        		//	todo: use base.Db to access the DataContext and insert/update records for this csv row
        		
        		//	note: throw new CsvImportException() if needed
        		
        		//	DON'T call Db.SubmitChanges() here, that will be done later
        		//	after all rows have been imported
        }
    }
}