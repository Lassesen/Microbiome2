using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace UbiomeUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo ubiomeFile = null;
            var showHelp = args.Length < 1;
            //Check folder exists
            if (!showHelp)
            {
                ubiomeFile = new FileInfo(args[0]);
                if (!ubiomeFile.Exists)
                {
                    Console.Write($"The file {ubiomeFile.FullName} does not exist.");
                    showHelp = true;
                }

            }
            if (showHelp)
            {
                Console.WriteLine(@"
UbiomeUpload - Uploads a ubiome json file
         UbiomeUpload {filepath}
");

                return;
            }

            var filestream = new System.IO.FileStream(ubiomeFile.FullName,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read);
            var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
            var json = JsonConvert.DeserializeObject<UbiomeHeaders>(file.ReadToEnd());

            var ubiomeDataTable = new DataTable();
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
            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("uploadubiome", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    SqlParameter hParameter = new SqlParameter();
                    hParameter.ParameterName = "@Data";
                    hParameter.SqlDbType = SqlDbType.Structured;
                    hParameter.Value = ubiomeDataTable;
                    cmd.Parameters.Add(hParameter);
                    cmd.Parameters.AddWithValue("ownerEmail", "Ken@Lassesen.com");
                    cmd.Parameters.AddWithValue("sampleId", json.SequencingRevision);
                    DateTime sampleDate = DateTime.UtcNow;
                    DateTime.TryParse(json.SamplingTime, out sampleDate);
                    if (sampleDate > new DateTime(2004, 1, 1))
                    {
                        cmd.Parameters.AddWithValue("SampleDate", sampleDate);
                    }
                    var dataSet = new DataSet("ubiome");
                    var sqladap = new SqlDataAdapter(cmd);
                    sqladap.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        Console.WriteLine($"Could not locate all taxons in ubiome  {json.SequencingRevision}");
                        dataSet.WriteXml($"Missing_{json.SequencingRevision}.xml");
                    }
                }
            }
        }
    }
}
