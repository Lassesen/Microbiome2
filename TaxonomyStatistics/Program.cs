 
using System;
using System.Collections.Generic;
using System.Data;

namespace TaxonomyStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            int quantiles = 4;
            int labTestId = 0;
            var showHelp = args.Length < 2;
            //Check folder exists
            if (!showHelp)
            {
                
                if (!int.TryParse(args[0],out quantiles) || quantiles < 2)
                {
                    Console.Write($"Quantitles must be an integer greater than or equal to 2");
                    showHelp = true;
                }
                if (!int.TryParse(args[1], out labTestId) || labTestId <1)
                {
                    Console.Write($"LabTestId must be an integer greater than 0");
                    showHelp = true;
                }
            }
            if (showHelp)
            {
                Console.WriteLine(@"
TaxonomyStatistics - Obtains statistics from current data for one specific labtest
         TaxonomyStatistics {NoOfQuantiles} {labTestId}
");

                return;
            }
            var dataTable = DAL.GetTaxonReports(labTestId);
            var statistics = new Statistics(quantiles);
            int activeTaxon = -1;
            var values = new List<double>();
            foreach(DataRow row in dataTable.Rows)
            {
                var id = (int)row["taxon"];
                if (activeTaxon < 0) activeTaxon = id;
                if (activeTaxon != id)
                {
                    if(values.Count > 0 &&  activeTaxon > 0)
                    {
                        statistics.ProcessATaxon(activeTaxon, values.ToArray());
                        values.Clear();
                        activeTaxon = (int)row["taxon"];
                    }
                }
                values.Add((double) row["value"]);
            }
            // get the last taxon processed
            statistics.ProcessATaxon(activeTaxon, values.ToArray());
            DAL.UpdateStatistics(statistics.StatsDatatable, labTestId);

        }
    }
}
 