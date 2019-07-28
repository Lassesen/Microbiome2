
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TaxonomyStatistics
{
    public static class  DAL
    {        
        /// <summary>
        /// Gat all of the data for one Test Order by Taxon,Value
        /// Technically: you should keep each test separate
        /// </summary>
        /// <param name="labTestId"></param>
        /// <returns></returns>
        public static DataTable GetTaxonReports(int labTestId) {
            var dataSet = new DataSet("RawData");

            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //Note: Do you include Zero or not? There are arguments for both options
                using (SqlCommand cmd = new SqlCommand("getTaxonWithValues", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.AddWithValue("@LabTestId", labTestId);
                    var sqladap = new SqlDataAdapter(cmd);
                    sqladap.Fill(dataSet);
                     
                }
            }
            return dataSet.Tables[0];
        }
        // We have to do a two phrase update because of the quantitles
        internal static void UpdateStatistics(DataTable statsDatatable, int labTestId)
        {
            string[] selectedColumns = new[] { "taxon", "mean" };
            string[] coreColumns = new[] { "taxon", "mean","median","mode","minimum","maximum","StandardDeviation",
            "Variance","Skewness","Kurtosis","Count"};
            //Verify no typos...
            for(var c=0; c < coreColumns.Length; c++)
            {
                coreColumns[c] = statsDatatable.Columns[c].ColumnName;
            }
            var connectionString = File.ReadAllText("DBString.txt");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //Note: Do you include Zero or not? There are arguments for both options
                using (SqlCommand cmd = new SqlCommand("updateCoreLabTestStatics", conn))
                {
                    DataTable dt = new DataView(statsDatatable).ToTable(false, coreColumns);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.AddWithValue("@LabTestId", labTestId);
                    SqlParameter hParameter = new SqlParameter();
                    hParameter.ParameterName = "@Data";
                    hParameter.SqlDbType = SqlDbType.Structured;
                    hParameter.Value = dt;
                    cmd.Parameters.Add(hParameter);
                    cmd.ExecuteNonQuery();
                }
                //We upload each quantilte separately
                foreach(DataColumn col in statsDatatable.Columns)
                {
                    if (col.ColumnName.StartsWith("q"))
                    {
                        selectedColumns[1] = col.ColumnName;
                        DataTable dt = new DataView(statsDatatable).ToTable(false, selectedColumns);
                        using (SqlCommand cmd = new SqlCommand("updateCustomLabTestStatics", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 900;
                            cmd.Parameters.AddWithValue("@LabTestId", labTestId);
                            cmd.Parameters.AddWithValue("@ColumnName", col.ColumnName);
                            SqlParameter hParameter = new SqlParameter();
                            hParameter.ParameterName = "@Data";
                            hParameter.SqlDbType = SqlDbType.Structured;
                            hParameter.Value = dt;
                            cmd.Parameters.Add(hParameter);
                            cmd.ExecuteNonQuery();
                        }

                    }
                }
            }
        }
    }
}
