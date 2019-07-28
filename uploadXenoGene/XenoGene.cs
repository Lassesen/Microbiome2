using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace TaxonNameUpload
{
    public class XenoGene
    {
        DataTable _dataTable = new DataTable("XenoGene");
        public XenoGene(FileInfo file)
        {
            _dataTable.Columns.Add("tax_rank", typeof(string));
            _dataTable.Columns.Add("tax_name", typeof(string));
            _dataTable.Columns.Add("BaseOneMillion", typeof(double));

            char[] lineSep = { '\r','\n'   };
            char[] sep = { ' '  };
            var txt = ReadPdfFile(file.FullName);
            var lines = txt.Split(lineSep, StringSplitOptions.RemoveEmptyEntries);
            string appliesToNextLine = "";
            int lineNo = 0;
            double amount = 0.0;
            foreach (var line in lines)
            {
                var skip = false;
                string rank = "";
                string name = "";
                if (line.IndexOf("ORDEN",StringComparison.OrdinalIgnoreCase)==0) skip = true;
                if (line.IndexOf("INFO", StringComparison.OrdinalIgnoreCase) == 0) skip = true;
                if (line.IndexOf("REPORT", StringComparison.OrdinalIgnoreCase) == 0) skip = true;
                if (line.IndexOf("Xeno", StringComparison.OrdinalIgnoreCase) == 0) skip = true;

                if (line.IndexOf("Inscrita", StringComparison.OrdinalIgnoreCase) == 0) skip = true;
                if (line.IndexOf("Pág", StringComparison.OrdinalIgnoreCase) == 0) skip = true;
                if(!skip)
                {
                    var firstSpace = line.IndexOf(" ");
                    var firstComma = line.IndexOf(",");
                    var lineNoTest = firstSpace < 0 ? "na": line.Substring(0, firstSpace);
                    if(int.TryParse(lineNoTest,out lineNo))
                    {
                        //If missing no number
                        if(firstComma < 0)
                        {
                            name = line;
                        }
                        else
                        {
                            var parts = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            var lastItem = parts[parts.Length - 1];
                            if (lastItem.Contains(",")) //european number
                            {
                                lastItem = lastItem.Replace(",", ".");
                                if (double.TryParse(lastItem, out amount))
                                {
                                    amount = amount * 10000.0; //permission
                                    switch (parts[2])
                                    {
                                        case "G":rank = "genus";break;
                                        case "G1": rank = "genus"; break;
                                        case "G2": rank = "genus"; break;
                                        case "C": rank = "class"; break;
                                        case "C1": rank = "class"; break;
                                        case "C2": rank = "class"; break;
                                        case "F": rank = "family"; break;
                                        case "S": rank = "species"; break;
                                        case "S1": rank = "strain"; break;
                                        case "S2": rank = "strain"; break;
                                        case "S3": rank = "strain"; break;
                                        case "O": rank = "Order"; break;
                                        case "F1": rank = "no rank"; break;
                                        case "F2": rank = "no rank"; break;
                                        case "F3": rank = "no rank"; break;
                                        case "O1": rank = "no rank"; break;
                                        case "O2": rank = "no rank"; break;
                                        case "O3": rank = "no rank"; break;
                                        case "P1": rank = "no rank"; break;
                                        case "D1": rank = "skip"; break;
                                        case "D2": rank = "skip"; break;
                                        case "D3": rank = "skip"; break;
                                        case "U": rank = "skip"; break;
                                        case "R": rank = "skip"; break;
                                        case "R1": rank = "skip"; break;
                                        case "P": rank = "phylum"; break;
                                        default:
                                            rank = "skip";
                                            Console.WriteLine($"Unknown {parts[2]}");
                                            break;
                                    }
                                    if (parts.Length > 4)
                                    {
                                        var sb = new StringBuilder(parts[3]);
                                        for (var p = 4; p < parts.Length - 1; p++)
                                        {
                                            sb.Append($" {parts[p]}");
                                        }
                                        name = sb.ToString();
                                    }
                                    
                                    if(!rank.Equals("skip",StringComparison.OrdinalIgnoreCase)  && amount > 0.0 && ! string.IsNullOrWhiteSpace(name))
                                        {
                                        Console.WriteLine(name);
                                        var row = _dataTable.NewRow();
                                        row["tax_rank"] = rank;
                                        row["tax_name"] = name;
                                        row["BaseOneMillion"] = amount;
                                        _dataTable.Rows.Add(row);
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        appliesToNextLine = line;
                    }


                }
                



            }
            SampleId = file.Name;
         
        }
        public DataTable AsDataTable
        { get { return _dataTable; } }
        public string LabTest
        { get { return "American Gut"; } }
        public string Lab
        {            get { return "American Gut"; }        }
        public string SampleId
        { get; set; }
        private string ReadPdfFile(object Filename )
        {
            PdfReader reader2 = new PdfReader((string)Filename);
            string strText = string.Empty;

            for (int page = 1; page <= reader2.NumberOfPages; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                PdfReader reader = new PdfReader((string)Filename);
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);

                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                strText = strText + s;
                reader.Close();
            }
            return strText;
        }
    }
}
