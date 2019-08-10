using MicrobiomeLibrary.DataAccessLayer;
using System.IO;

namespace ConsoleTestLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            DataInterfaces.ConnectionString = "Server=LAPTOP-BQ764BQT;Database=MicrobiomeV2;Trusted_Connection=True; Connection Timeout=1000";
            var export = DataInterfaces.GetFlatTaxonomy("species").ToCsvString();
            File.WriteAllLines("DataScience_Taxon.csv", DataInterfaces.GetFlatTaxonomy("species").ToCsvString());
            File.WriteAllLines("DataScience_Continuous.csv", DataInterfaces.GetFlatContinuous( ).ToCsvString());
            File.WriteAllLines("DataScience_Category.csv", DataInterfaces.GetFlatCategory().ToCsvString());
            File.WriteAllLines("DataScience_LabReport.csv", DataInterfaces.GetLabReport().ToCsvString());
        }

    }
}
