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
            FileInfo indexFile = null;
            FileInfo tabularFile = null;
            var showHelp = args.Length <2;
            //Check folder exists
            if (!showHelp)
            {
                indexFile= new FileInfo(args[0]);
                if (!indexFile.Exists)
                {
                    Console.Write($"The index file {indexFile.FullName} does not exist.");
                    showHelp = true;
                }
                tabularFile = new FileInfo(args[1]);
                if (!tabularFile.Exists)
                {
                    Console.Write($"The tabular file  {tabularFile.FullName} does not exist.");
                    showHelp = true;
                }

            }
            if (showHelp)
            {
                Console.WriteLine(@"
UploadICD - Uploads ICF taxonomy information
         UploadICD {indexFile} {TabularFile}
File should be downloaded from  https://www.cms.gov/medicare/coding/icd10/2019-icd-10-cm.html 
and be this file icd10cm_index_2019.xml  icd10cm_tabular_2019.xml
Ore later versions
");

                return;
            }

            //We need to remove encoding to get the insert working
            var xmlList = File.ReadAllLines(indexFile.FullName);                ;
            var xml = new StringBuilder();
            for (var i = 1; i  < xmlList.Length ;i++)
            {
                xml.Append(xmlList[i]);
            }

            xmlList = File.ReadAllLines(tabularFile.FullName); ;
            var txml = new StringBuilder();
            for (var i = 1; i < xmlList.Length; i++)
            {
                txml.Append(xmlList[i]);
            }

            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("uploadICD", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.AddWithValue("@IndexXml", xml.ToString());
                    cmd.Parameters.AddWithValue("@TabularXml", txml.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
