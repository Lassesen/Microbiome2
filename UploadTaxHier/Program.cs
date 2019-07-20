using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace UploadTaxHier
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo nodesFile = null;
            FileInfo namesFile = null;
            DirectoryInfo checkFolder = null;
            var showHelp = args.Length <1;
            //Check folder exists
            if (!showHelp)
            {
                checkFolder = new DirectoryInfo(args[0]);
                if (!checkFolder.Exists)
                {
                    Console.Write($"The folder {checkFolder.FullName} does not exist.");
                    showHelp = true;
                }
                else
                {
                    nodesFile = new FileInfo(Path.Combine(checkFolder.FullName, "nodes.dmp"));
                    namesFile = new FileInfo(Path.Combine(checkFolder.FullName, "names.dmp"));
                    if (!nodesFile.Exists)
                    {
                        Console.Write($"The file {nodesFile.FullName} does not exist.");
                        showHelp = true;
                    }
                    if (!namesFile.Exists)
                    {
                        Console.Write($"The file {namesFile.FullName} does not exist.");
                        showHelp = true;
                    }

                }
            }
            if (showHelp)
            {
                Console.WriteLine(@"
UploadTaxHier - Uploads Ncbi taxonomy information
         UploadTaxHier {PathtoFolder}
Folder is expected to contain nodes.dmp and names.dmp
");

                return;
            }



            char[] sep = { '|', '\t' };
            var hierarchyDataTable = new DataTable();
            hierarchyDataTable.Columns.Add("taxon", typeof(int));
            hierarchyDataTable.Columns.Add("parent", typeof(int));
            hierarchyDataTable.Columns.Add("rank", typeof(string));
            string lineOfText = "";
            var filestream = new System.IO.FileStream(nodesFile.FullName,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read);
            var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);

            while ((lineOfText = file.ReadLine()) != null)
            {
                var parts = lineOfText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                var row = hierarchyDataTable.NewRow();
                row["taxon"] = int.Parse(parts[0]);
                row["parent"] = int.Parse(parts[1]);
                row["rank"] = parts[2];
                hierarchyDataTable.Rows.Add(row);
            }
            // We do a cheat to keep code simpler
           
            var namesDataTable = new DataTable();
            namesDataTable.Columns.Add("taxonName", typeof(string));
            namesDataTable.Columns.Add("taxon", typeof(int));
            filestream = new System.IO.FileStream(namesFile.FullName,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read);
            file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);

            while ((lineOfText = file.ReadLine()) != null)
            {
                var parts = lineOfText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                var row = namesDataTable.NewRow();
                row["taxon"] = int.Parse(parts[0]);
                row["taxonname"] = parts[1];
                namesDataTable.Rows.Add(row);
            }

            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("uploadncbi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    SqlParameter hParameter = new SqlParameter();
                    hParameter.ParameterName = "@hData";
                    hParameter.SqlDbType = SqlDbType.Structured;
                    hParameter.Value = hierarchyDataTable;
                    cmd.Parameters.Add(hParameter);
                    SqlParameter nParameter = new SqlParameter();

                    nParameter.ParameterName = "@nData";
                    nParameter.SqlDbType = SqlDbType.Structured;
                    nParameter.Value = namesDataTable;
                    cmd.Parameters.Add(nParameter);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
