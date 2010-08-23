using System;
using System.Data;
using System.IO;

namespace Samples.Client
{
    public static class HelperExtensions
    {
        public static void ToCsvFile(this DataTable table, string outputFile)
        {
            using (var sw = new StreamWriter(outputFile, false))
            {
                int iColCount = table.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(table.Columns[i]);

                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }

                sw.Write(sw.NewLine);

                foreach (DataRow dr in table.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            value = value.Replace("\"", "\"\"");

                            sw.Write("\"");
                            sw.Write(value);
                            sw.Write("\"");
                        }

                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }

                    sw.Write(sw.NewLine);
                }

                sw.Close();
            }
        }
    }
}