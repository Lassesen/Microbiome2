 
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TaxonNameUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo TaxonNameUpload = null;
            var showHelp = args.Length < 1;
            //Check folder exists
            if (!showHelp)
            {
                TaxonNameUpload = new FileInfo(args[0]);
                if (!TaxonNameUpload.Exists)
                {
                    Console.Write($"The file {TaxonNameUpload.FullName} does not exist.");
                    showHelp = true;
                }

            }
            if (showHelp)
            {
                Console.WriteLine(@"
UploadXenoGene - Uploads a PDF file from XenoGene Spain
         UploadXenoGene {filepath}
");

                return;
            }

             var parsedFile = new XenoGene(TaxonNameUpload);          
            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("uploadTaxNameRankData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    SqlParameter hParameter = new SqlParameter();
                    hParameter.ParameterName = "@Data";
                    hParameter.SqlDbType = SqlDbType.Structured;
                    hParameter.Value = parsedFile.AsDataTable;
                    cmd.Parameters.Add(hParameter);
                    cmd.Parameters.AddWithValue("@LabName",  parsedFile.Lab);
                    cmd.Parameters.AddWithValue("@SampleId", parsedFile.SampleId);
                    cmd.Parameters.AddWithValue("@LabTestName", parsedFile.LabTest);
                    cmd.Parameters.AddWithValue("ownerEmail", "Ken@Lassesen.com");
                    DateTime sampleDate = DateTime.UtcNow; // replace as appropriate                   
                    cmd.Parameters.AddWithValue("SampleDate", sampleDate);                 
                    var dataSet = new DataSet(parsedFile.LabTest);
                    var sqladap = new SqlDataAdapter(cmd);
                    sqladap.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        dataSet.Tables[0].TableName = "XenoGene";
                        Console.WriteLine($"Could not locate all taxon names in   {parsedFile.LabTest}");
                        dataSet.WriteXml($"Missing_{parsedFile.SampleId}.xml");
                    }
                }
            }
        }
    }
}
 