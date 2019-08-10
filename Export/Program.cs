using MicrobiomeLibrary.DataAccessLayer;
using System.IO;

namespace ConsoleTestLibrary
{
    class Program
    {
static void Main(string[] args)
{
    var exportName = "some site";
    var filename = "MyExport";
    if (args.Length > 0) exportName = args[0];
    if (args.Length > 1) filename = args[1];
    DataInterfaces.ConnectionString = "Server=LAPTOP-BQ764BQT;Database=MicrobiomeV2;Trusted_Connection=True; Connection Timeout=1000";
    var schemaFile = new FileInfo($"{filename}Schema.xml");
    var exportFile = new FileInfo($"{filename}.xml");
    var export = DataInterfaces.Export(exportName);
    export.WriteXmlSchema(schemaFile.FullName);
    export.WriteXml(exportFile.FullName);
}
    }
}
