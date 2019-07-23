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
            FileInfo microbiomeFile = null;
            var showHelp = args.Length < 1;
            //Check folder exists
            if (!showHelp)
            {
                microbiomeFile = new FileInfo(args[0]);
                if (!microbiomeFile.Exists)
                {
                    Console.Write($"The file {microbiomeFile.FullName} does not exist.");
                    showHelp = true;
                }

            }
            if (showHelp)
            {
                Console.WriteLine(@"
UbiomeUpload - Uploads a ubiome json file or thryve text file
         UbiomeUpload {filepath}
");

                return;
            }
            IAnalysis analysis = null;
            var headers = File.ReadAllLines(microbiomeFile.FullName);
            if (headers[0].IndexOf("{") == 0) //Json Ubiome
            {
                analysis = new Ubiome(microbiomeFile);
            }
            else
                if (headers[0].IndexOf("\"taxon_id\",\"rank\",\"name\",\"parent\",\"count\"", StringComparison.OrdinalIgnoreCase) == 0) //Json Ubiome
            {
                analysis = new Thryve(microbiomeFile);
            }
            if (analysis == null)
            {
                Console.WriteLine("Failed to identify file type");
                return;
            }
            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("uploadtaxon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    SqlParameter hParameter = new SqlParameter();
                    hParameter.ParameterName = "@Data";
                    hParameter.SqlDbType = SqlDbType.Structured;
                    hParameter.Value = analysis.AsDataTable;
                    cmd.Parameters.Add(hParameter);
                    cmd.Parameters.AddWithValue("@ownerEmail", "Ken@Lassesen.com");
                    cmd.Parameters.AddWithValue("@sampleId", analysis.SampleId);
                    cmd.Parameters.AddWithValue("@SampleDate", analysis.SampleDateTime);
                    cmd.Parameters.AddWithValue("@LabName", analysis.LabName);
                    cmd.Parameters.AddWithValue("@LabTestName", analysis.LabTestName);
                    // For non matches 
                    var dataSet = new DataSet(analysis.LabTestName);
                    var sqladap = new SqlDataAdapter(cmd);
                    sqladap.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        Console.WriteLine($"Could not locate all taxons in sample  {analysis.SampleId}");
                        dataSet.WriteXml($"Missing_{analysis.SampleId}.xml");
                    }
                }
            }
        }
    }
}
