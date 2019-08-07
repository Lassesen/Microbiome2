using MicrobiomeLibrary.DataAccessLayer;
using System;
using System.Data;
using System.IO;

namespace MicrobiomeLibrary.Uploaders
{
    class Thryve : IAnalysis
    {
        DataTable _dataTable = new DataTable();
        public Thryve(FileInfo ubiomeFile)
        {
            char[] sep = { ',' };
            var lines = File.ReadAllLines(ubiomeFile.FullName);

            _dataTable = DataInterfaces.GetTaxonDataTable();
            for (var i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    var parts = lines[i].Split(sep, StringSplitOptions.None);
                    int count = 0;
                    int.TryParse(parts[parts.Length - 1], out count);
                    var row = _dataTable.NewRow();
                    row["taxon"] = parts[0];
                    row["taxonBaseOneMillion"] = (double)count;
                    row["count"] = count;
                    _dataTable.Rows.Add(row);
                }
            }
            SampleId = Guid.NewGuid().ToString(); ;
            DateTime sampleDate = DateTime.UtcNow;
            SampleDateTime = sampleDate;
        }
        public string SampleId { get; set; }
        public DateTime SampleDateTime { get; set; }
        public string LabName { get => "Thyrve"; }
        public string LabTestName { get => "Thyrve"; }
        public int SaveToDatabase(string ownerEmail)
        {
            return DataInterfaces.Savedata(ownerEmail, LabName, LabTestName, SampleId, SampleDateTime, _dataTable);
        }
    }
}
