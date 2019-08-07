using MicrobiomeLibrary.DataAccessLayer;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;

namespace MicrobiomeLibrary.Uploaders
{
    class Ubiome : IAnalysis
    {
        DataTable _dataTable = new DataTable();
        public Ubiome(FileInfo ubiomeFile)
        {
            var filestream = new System.IO.FileStream(ubiomeFile.FullName,
                                        System.IO.FileMode.Open,
                                        System.IO.FileAccess.Read);
            var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
            var json = JsonConvert.DeserializeObject<UbiomeHeaders>(file.ReadToEnd());

            _dataTable = DataInterfaces.GetTaxonDataTable();
            foreach (var count in json.UbiomeBacteriacounts)
            {
                var row = _dataTable.NewRow();
                row["taxon"] = count.Taxon;
                row["taxonBaseOneMillion"] = (double)count.CountNorm;
                row["count"] = count.Count;
                row["count_norm"] = count.CountNorm;
                _dataTable.Rows.Add(row);
            }
            SampleId = json.SequencingRevision.ToString();
            DateTime sampleDate = DateTime.UtcNow;
            DateTime.TryParse(json.SamplingTime, out sampleDate);
            SampleDateTime = sampleDate;
        }
        public string SampleId { get; set; }
        public DateTime SampleDateTime { get; set; }
        public string LabName { get => "Ubiome"; }
        public string LabTestName { get => "Explorer"; }
        public int SaveToDatabase(string ownerEmail)
        {
            return DataInterfaces.Savedata(ownerEmail, LabName, LabTestName, SampleId, SampleDateTime, _dataTable);
        }
    }
}
