using MicrobiomeLibrary.DataAccessLayer;
using MicrobiomeLibrary.Statistics;
using System;
using System.Data;
using System.IO;

namespace ConsoleTestLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            DataInterfaces.ConnectionString = "Server=LAPTOP-BQ764BQT;Database=OnlineClone;Trusted_Connection=True; Connection Timeout=1000";
            var exportFile = new FileInfo("data\\Biome2Data.xml");
            var schemaFile = new FileInfo("data\\Biome2Schema.xml");
            if (!(exportFile.Exists && schemaFile.Exists))
            {
                throw new Exception($"Data file does not exits {exportFile.FullName} or schema {schemaFile.FullName}");
            }
            var import = new DataSet();
            import.ReadXmlSchema(schemaFile.FullName);
            import.ReadXml(exportFile.FullName);
            DataInterfaces.Import(import);
            LabTests.ComputeFull();
        }
    }
}
