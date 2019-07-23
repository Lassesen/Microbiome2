using System;
using System.Data;
using System.IO;
using Newtonsoft.Json;
namespace UbiomeUpload
{
    class Thryve : IAnalysis
    {
        DataTable taxonDataTable = new DataTable();
        public Thryve(FileInfo ubiomeFile)
        {
            char[] sep = { ',' };
            var lines = File.ReadAllLines(ubiomeFile.FullName);

            taxonDataTable.Columns.Add("taxon", typeof(string));
            taxonDataTable.Columns.Add("taxonBaseOneMillion", typeof(double));
            taxonDataTable.Columns.Add("count", typeof(int));
            taxonDataTable.Columns.Add("count_norm", typeof(int));
            for (var i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    var parts = lines[i].Split(sep, StringSplitOptions.None);
                    int count = 0;
                    int.TryParse(parts[parts.Length - 1], out count);
                    var row = taxonDataTable.NewRow();
                    row["taxon"] = parts[0];
                    row["taxonBaseOneMillion"] = (double)count;
                    row["count"] = count;
                    taxonDataTable.Rows.Add(row);
                }
            }
            SampleId = Guid.NewGuid().ToString(); ;
            DateTime sampleDate = DateTime.UtcNow;
            SampleDateTime = sampleDate;
        }

        public DataTable AsDataTable
        {
            get { return taxonDataTable; }
        }
        public string SampleId { get; set; }
        public DateTime SampleDateTime { get; set; }
        public string LabName { get => "Thyrve"; }
        public string LabTestName { get => "Thyrve"; }

    }
}
