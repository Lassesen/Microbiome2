using System;
using System.Data;
using System.IO;
using Newtonsoft.Json;
namespace UbiomeUpload
{
    class Ubiome : IAnalysis
    {
        DataTable ubiomeDataTable = new DataTable();
        public Ubiome(FileInfo ubiomeFile)
        {
            var filestream = new System.IO.FileStream(ubiomeFile.FullName,
                                        System.IO.FileMode.Open,
                                        System.IO.FileAccess.Read);
            var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
            var json = JsonConvert.DeserializeObject<UbiomeHeaders>(file.ReadToEnd());

            ubiomeDataTable.Columns.Add("taxon", typeof(string));
            ubiomeDataTable.Columns.Add("taxonBaseOneMillion", typeof(double));
            ubiomeDataTable.Columns.Add("count", typeof(int));
            ubiomeDataTable.Columns.Add("count_norm", typeof(int));
            foreach (var count in json.UbiomeBacteriacounts)
            {
                var row = ubiomeDataTable.NewRow();
                row["taxon"] = count.Taxon;
                row["taxonBaseOneMillion"] = (double)count.CountNorm;
                row["count"] = count.Count;
                row["count_norm"] = count.CountNorm;
                ubiomeDataTable.Rows.Add(row);
            }
            SampleId = json.SequencingRevision.ToString();
            DateTime sampleDate = DateTime.UtcNow;
            DateTime.TryParse(json.SamplingTime, out sampleDate);
            SampleDateTime = sampleDate;
        }

        public DataTable AsDataTable
        {
            get { return ubiomeDataTable; }
        }
        public string SampleId { get; set; }
        public DateTime SampleDateTime { get; set; }


        public string LabName { get => "Ubiome"; }
        public string LabTestName { get => "Explorer"; }
    }
}
