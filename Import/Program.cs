using MicrobiomeLibrary.DataAccessLayer;
using MicrobiomeLibrary.Statistics;
using System.Data;
using System.IO;

namespace ConsoleTestLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            var exportFile = new FileInfo("D:\\Downloads\\Biome2Data.xml");
            var schemaFile = new FileInfo("D:\\Downloads\\Biome2Schema.xml");
            if (!exportFile.Exists)
            {
                DataInterfaces.ConnectionString = "Server=LAPTOP-BQ764BQT;Database=OnlineClone;Trusted_Connection=True; Connection Timeout=1000";
                var data = DataInterfaces.ExportFromV1();

                data.WriteXmlSchema(schemaFile.FullName);
                data.WriteXml(exportFile.FullName);
            }
            var import = new DataSet();
            import.ReadXmlSchema(schemaFile.FullName);
            import.ReadXml(exportFile.FullName);
            DataInterfaces.ConnectionString = "Server=LAPTOP-BQ764BQT;Database=MicrobiomeV2;Trusted_Connection=True; Connection Timeout=1000";
            DataInterfaces.Import(import);
            LabTests.ComputeAllLabs(4);
        }
    }
}
