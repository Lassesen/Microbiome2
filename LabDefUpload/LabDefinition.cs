using System;
using System.Data;
using System.IO;

namespace LabDefUpload
{
    public class LabDefinition
    {
        DataTable _dataTable = new DataTable();
        public LabDefinition(FileInfo file)
        {
            char[] sep1 = { ',' };
            char[] sep2 = { '.' };

            var labparts = file.Name.Split(sep2, StringSplitOptions.RemoveEmptyEntries);
            LabName = labparts[0];
            LabTestName = labparts[1];
            _dataTable.Columns.Add("taxon", typeof(int));
            _dataTable.Columns.Add("displayOrder", typeof(double));
            var lines = File.ReadAllLines(file.FullName);
            //First line is the header
            for (var i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    var parts1 = lines[i].Split(sep1, StringSplitOptions.RemoveEmptyEntries);
                    if (parts1.Length == 2)
                    {
                        double taxon = 0.0;
                        double displayOrder = 0.0;
                        if (double.TryParse(parts1[0], out taxon) && double.TryParse(parts1[1], out displayOrder))
                        {
                            var row = _dataTable.NewRow();
                            row["taxon"] = taxon;
                            row["displayOrder"] = displayOrder;
                            _dataTable.Rows.Add(row);
                        }
                    }
                }
            }
        }
        public DataTable AsDataTable
        { get { return _dataTable; } }
        public string LabTestName
        { get; set; }
        public string LabName
        { get; set; }
    }
}
