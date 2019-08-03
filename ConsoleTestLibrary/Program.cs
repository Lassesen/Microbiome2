using DataAccessLayer;
using System;

namespace ConsoleTestLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            DataInterfaces.ConnectionString = "Server=LAPTOP-BQ764BQT;Database=MicrobiomeV2;Trusted_Connection=True; Connection Timeout=1000";
            var data = DataInterfaces.GetNonParametricCategoryDataSet(1, 1, "Q4_", 20);
            var matrix = MicrobiomeLibrary.Statistics.NonParametric.CategoricSignficance(data);
            if(matrix.Rows.Count > 0) //Only write when we have data
            matrix.WriteXml("Matrix.xml");
        }
    }
}
