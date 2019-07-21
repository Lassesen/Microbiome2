 
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace LabDefUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo LabDefUpload = null;
            var showHelp = args.Length < 1;
            //Check folder exists
            if (!showHelp)
            {
                LabDefUpload = new DirectoryInfo(args[0]);
                if (!LabDefUpload.Exists)
                {
                    Console.Write($"The folder {LabDefUpload.FullName} does not exist.");
                    showHelp = true;
                }

            }
            if (showHelp)
            {
                Console.WriteLine(@"
LabDefUpload - Uploads folder of .txt files containing 3rd party lab definitions
         LabDefUpload {folderpath}
");

                return;
            }
            var connectionString = File.ReadAllText("DBString.txt");

            foreach (FileInfo file in LabDefUpload.GetFiles("*.txt"))
            {

                var parsedFile = new LabDefinition(file);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("uploadLabDef", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 900;
                        SqlParameter hParameter = new SqlParameter();
                        hParameter.ParameterName = "@Data";
                        hParameter.SqlDbType = SqlDbType.Structured;
                        hParameter.Value = parsedFile.AsDataTable;
                        cmd.Parameters.Add(hParameter);
                        cmd.Parameters.AddWithValue("@LabName", parsedFile.LabName);
                        cmd.Parameters.AddWithValue("@LabTestName", parsedFile.LabTestName);
                        var dataSet = new DataSet($"{parsedFile.LabName}-{parsedFile.LabTestName}");
                        var sqladap = new SqlDataAdapter(cmd);
                        sqladap.Fill(dataSet);
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            Console.WriteLine($"Could not locate all taxon names in  {parsedFile.LabName}-{parsedFile.LabTestName}");
                            dataSet.WriteXml($"Missing_{parsedFile.LabName}-{parsedFile.LabTestName}.xml");
                        }
                    }
                }
            }
        }
    }
}