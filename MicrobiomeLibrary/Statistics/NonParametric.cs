using Accord.Statistics.Testing;
using System;
using System.Collections.Generic;
using System.Data;

namespace MicrobiomeLibrary.Statistics
{
    public static class NonParametric
    {
        /// <summary>
        /// DataSet 
        /// * Table 0: Quantiles numbers order by Taxon
        /// * Table 1: Data order by Taxon
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static DataTable CategoricSignficance(DataSet dataSet)
        {
            var columnNames = dataSet.Tables[0].DefaultView.ToTable(true, "StatisticsName");
            var boundCount = columnNames.Rows.Count + 1;
            var taxons = dataSet.Tables[0].DefaultView.ToTable(true, "Taxon");
            var eureka = new DataTable();
            eureka.Columns.Add("taxon", typeof(int));
            eureka.Columns.Add("taxName", typeof(string));
            eureka.Columns.Add("PValue", typeof(double));
            eureka.Columns.Add("HighIndex", typeof(int));
            eureka.Columns.Add("Significant", typeof(double));
            for (var c = 0; c < boundCount; c++)
            {
                eureka.Columns.Add($"Quantile{c}", typeof(double));
            }
            int lastTaxon = -1;
            var bounds = new List<double>();
            double sampleCount = 0;
            foreach (DataRow qrow in dataSet.Tables[0].Rows)
            {
                var taxon = (int)qrow["taxon"];

                if (taxon == lastTaxon)
                {
                    bounds.Add((double)qrow["value"]);
                    sampleCount = (double)qrow["Count"];
                }
                else
                {
                    bounds.Sort();
                    lastTaxon = ProcessTaxon(dataSet, boundCount, eureka, lastTaxon, bounds, taxon, sampleCount);
                    bounds = new List<double>();
                }
            }
            bounds.Sort();
            lastTaxon = ProcessTaxon(dataSet, boundCount, eureka, lastTaxon, bounds, lastTaxon, sampleCount);
            return eureka;
        }

        private static int ProcessTaxon(DataSet dataSet, int boundCount, DataTable eureka, int lastTaxon, List<double> bounds, int taxon, double sampleCount)
        {
            if (bounds.Count > 0)
            {
                var count = new Double[boundCount];
                var scope = dataSet.Tables[1].Select($"[taxon]={lastTaxon}");
                foreach (DataRow srow in scope)
                {
                    var value = (double)srow["value"];
                    var idx = 0;
                    foreach (var b in bounds)
                    {
                        if (value >= b) break;
                        idx++;
                    }
                    count[idx]++;
                }

                var nameRow = dataSet.Tables[2].Select($"[taxon]={lastTaxon}");
                var newRow = eureka.NewRow();
                newRow["taxon"] = taxon;
                newRow["taxonName"] = (string)nameRow[0]["taxonName"];
                var highIndex = 0;
                var highCount = count[0];
                for (var c = 0; c < boundCount; c++)
                {
                    newRow[$"Quantile{c}"] = count[c];
                    if (count[c] > highCount)
                    {
                        highCount = count[c];
                        highIndex = c;
                    }
                }
                newRow["HighIndex"] = highIndex;
                var expectedValue = sampleCount / (double)boundCount;
                double[] expected = new double[boundCount];
                for (var e = 0; e < boundCount; e++) expected[e] = expectedValue;
                var chi = new ChiSquareTest(expected, count, boundCount - 2);
                newRow["PValue"] = chi.PValue;
                newRow["Significant"] = chi.Significant;
                eureka.Rows.Add(newRow);
                lastTaxon = taxon;
            }
            return lastTaxon;
        }
    }
}
