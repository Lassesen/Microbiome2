
using MicrobiomeLibrary.DataAccessLayer;
using System.Collections.Generic;
using System.Data;

namespace MicrobiomeLibrary.Statistics
{
    public static class LabTests
    {
        public static void ComputeOneLab(int labTestId, int quantiles)
        {
            var dataTable = DataInterfaces.GetTaxonReports(labTestId);
            var statistics = new CoreStatistics(quantiles);
            int activeTaxon = -1;
            var values = new List<double>();
            foreach (DataRow row in dataTable.Rows)
            {
                var id = (int)row["taxon"];
                if (activeTaxon < 0) activeTaxon = id;
                if (activeTaxon != id)
                {
                    if (values.Count > 0 && activeTaxon > 0)
                    {
                        if (values.Count > 16)
                        {
                            statistics.ProcessATaxon(activeTaxon, values.ToArray());
                        }
                        values.Clear();
                        activeTaxon = (int)row["taxon"];
                    }
                }
                values.Add((double)row["value"]);
            }
            // get the last taxon processed
            statistics.ProcessATaxon(activeTaxon, values.ToArray());
             
            DataInterfaces.UpdateStatistics(statistics.StatsDatatable, labTestId);
        }
        public static void ComputeAllLabs(int quantiles)
        {
            var labTests = DataInterfaces.GetLabTests();
            foreach (DataRow row in labTests.Rows)
            {
                ComputeOneLab((int)row["LabTestId"], quantiles);
            }
        }
    }
}
