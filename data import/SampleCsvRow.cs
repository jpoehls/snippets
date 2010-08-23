using System;
using System.ComponentModel.DataAnnotations;

namespace Samples.DataImport
{
    public class SampleCsvRow : CsvRow
    {
    		[StringLength(256, ErrorMessage = "Name cannot be greater than 256 characters long")]
        [Required(ErrorMessage = "Name is required")]
    		public string Name { get; set; }

        [CsvRowHeader("Real Age")]
        public int RealAge { get; set; }
    }
}