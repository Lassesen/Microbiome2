using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace UploadICD
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo xmlFile = null;
             var showHelp = args.Length <1;
            //Check folder exists
            if (!showHelp)
            {
                xmlFile= new FileInfo(args[0]);
                if (!xmlFile.Exists)
                {
                    Console.Write($"The folder {xmlFile.FullName} does not exist.");
                    showHelp = true;
                }
            }
            if (showHelp)
            {
                Console.WriteLine(@"
UploadICD - Uploads ICF taxonomy information
         UploadICD {PathtoFile}
File should be downloaded from  https://www.cms.gov/medicare/coding/icd10/2019-icd-10-cm.html 
and be this file icd10cm_index_2019.xml or equivalent
");

                return;
            }

            //We need to remove encoding to get the insert working
            var xmlList = File.ReadAllLines(xmlFile.FullName);                ;
            var xml = new StringBuilder();
            for (var i = 1; i  < xmlList.Length ;i++)
            {
                xml.Append(xmlList[i]);
            }

            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("uploadICD", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.AddWithValue("@xml", xml.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
