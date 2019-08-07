using MathNet.Numerics.Statistics;
using System;
using System.Data;
using System.Linq;

namespace MicrobiomeLibrary.Statistics
{
    public class CoreStatistics
    {
        DataTable _dataTable = new DataTable("XenoGene");
        double _quantiles;
        public CoreStatistics(int quantiles)
        {
            _quantiles = (double)quantiles;
            _dataTable.Columns.Add("taxon", typeof(int));
            _dataTable.Columns.Add("Mean", typeof(double));
            _dataTable.Columns.Add("Median", typeof(double));
            _dataTable.Columns.Add("Mode", typeof(double));
            _dataTable.Columns.Add("Minimum", typeof(double));
            _dataTable.Columns.Add("Maximum", typeof(double));
            _dataTable.Columns.Add("StandardDeviation", typeof(double));
            _dataTable.Columns.Add("Variance", typeof(double));
            _dataTable.Columns.Add("Skewness", typeof(double));
            _dataTable.Columns.Add("Kurtosis", typeof(double));
            _dataTable.Columns.Add("Count", typeof(double));

            for (var q = 1; q < quantiles; q++)
            {
                _dataTable.Columns.Add($"q{quantiles}_{q}", typeof(double));
            }

        }
        /// <summary>
        /// Converting from double to sql float can have overflow
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double VerifyRange(double value)
        {
            if (double.IsNaN(value)) value = double.MaxValue;
            return value;
        }
        public void ProcessATaxon(int taxon, double[] values)
        {
            var row = _dataTable.NewRow();
            if (values.Length < 16) return;
            var stats = new DescriptiveStatistics(values);
            row["taxon"] = taxon;
            row["Mean"] = VerifyRange(stats.Mean);
            row["Minimum"] = VerifyRange(stats.Minimum);
            row["Maximum"] = VerifyRange(stats.Maximum);
            row["Skewness"] = VerifyRange(stats.Skewness);
            row["Kurtosis"] = VerifyRange(stats.Kurtosis);
            row["StandardDeviation"] = VerifyRange(stats.StandardDeviation);
            row["Variance"] = VerifyRange(stats.Variance);
            row["Count"] = VerifyRange(stats.Count);
            var index = 0.5 * (double)stats.Count;
            var indexLow = (int)Math.Floor(index);
            var indexHigh = (int)Math.Ceiling(index);
            if (indexHigh > values.Length - 1) indexHigh = indexLow;
            var qValue = (values[indexLow] + values[indexHigh]) / 2;
            row["Median"] = qValue;
            row["Mode"] = values.GroupBy(v => v)
             .OrderByDescending(g => g.Count())
             .First()
             .Key;
            Array.Sort(values);

            for (double q = 1; q < _quantiles; q++)
            {
                index = q / _quantiles * (double)stats.Count;
                indexLow = (int)Math.Floor(index);
                indexHigh = (int)Math.Ceiling(index);
                if (indexHigh > values.Length - 1) indexHigh = indexLow;
                qValue = (values[indexLow] + values[indexHigh]) / 2;
                var colName = $"q{_quantiles}_{(int)q}";
                row[colName] = qValue;
            }
            _dataTable.Rows.Add(row);
        }
        public DataTable StatsDatatable { get { return _dataTable; } }
    }
}
