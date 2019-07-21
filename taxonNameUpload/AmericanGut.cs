using System;
using System.Data;
using System.IO;

namespace TaxonNameUpload
{
    public class AmericanGut
    {
        DataTable _dataTable = new DataTable();
        public AmericanGut(FileInfo file)
        {
            SampleId = file.Name;
            char[] sep1 = { '\t' };
            char[] sep2 = { ';' };
            _dataTable.Columns.Add("tax_rank", typeof(string));
            _dataTable.Columns.Add("tax_name", typeof(string));
            _dataTable.Columns.Add("BaseOneMillion", typeof(double));
            var lines = File.ReadAllLines(file.FullName);
            //First line is the header
            for (var i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    var parts1 = lines[i].Split(sep1, StringSplitOptions.RemoveEmptyEntries);
                    if (parts1.Length == 2)
                    {
                        var rank = "no rank";
                        var name = "";
                        double count = 0.0;
                        double.TryParse(parts1[1], out count);
                        count = count * 1000000;
                        var parts2 = parts1[0].Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                        // We start with the last one and walk back.
                        for (var j= parts2.Length-1; j > 0; j-- )
                        {
                            var part = parts2[j];
                            if (part.Length > 5) //elimate empty ones, i.e. ;g__ with nothing
                            {

                                switch (part.Substring(0, 1))
                                {
                                    case "k": rank = "kingdom"; break;
                                    case "p": rank = "phylum"; break;
                                    case "c": rank = "class"; break;
                                    case "o": rank = "order"; break;
                                    case "f": rank = "family"; break;
                                    case "g": rank = "genus"; break;
                                    case "s": rank = "species"; break;
                                }
                                name = part.Substring(3);
                                break;
                            }
                           
                        }
                        if (name.Length > 3)
                        {
                            var row = _dataTable.NewRow();
                            row["tax_rank"] = rank;
                            row["tax_name"] = name;
                            row["BaseOneMillion"] = count;
                            _dataTable.Rows.Add(row);
                        }
                    }
                }
            }
        }
        public DataTable AsDataTable
        { get { return _dataTable; } }
        public string LabTest
        { get { return "American Gut"; } }
        public string Lab
        {            get { return "American Gut"; }        }
        public string SampleId
        { get; set; } 
    }
}
