using System;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer
{
    public class DataInterfaces
    {
        /// <summary>
        /// Set this from the calling program on load
        /// </summary>
        public static string ConnectionString = "";
        public static DataTable GetTaxNameRankDataTable()
        {
            var dataTable = new DataTable("TaxNameRankDataTable");
            dataTable.Columns.Add("tax_rank", typeof(string));
            dataTable.Columns.Add("tax_name", typeof(string));
            dataTable.Columns.Add("BaseOneMillion", typeof(double));
            return dataTable;
        }
        public static DataTable GetTaxonDataTable()
        {
            var dataTable = new DataTable("TaxonDataTable");
            dataTable.Columns.Add("taxon", typeof(string));
            dataTable.Columns.Add("taxonBaseOneMillion", typeof(double));
            dataTable.Columns.Add("count", typeof(int));
            dataTable.Columns.Add("count_norm", typeof(int));
            return dataTable;
        }
        public static int Savedata(string ownerEmail, string labName, string labTestName, string sampleName, DateTime sampleDate, DataTable data)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"upload_{labTestName}", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    SqlParameter hParameter = new SqlParameter();
                    hParameter.ParameterName = "@Data";
                    hParameter.SqlDbType = SqlDbType.Structured;
                    hParameter.Value = data;
                    cmd.Parameters.Add(hParameter);
                    cmd.Parameters.AddWithValue("@LabName", labName);
                    cmd.Parameters.AddWithValue("@SampleName", sampleName);
                    cmd.Parameters.AddWithValue("@LabTestName", labTestName);
                    cmd.Parameters.AddWithValue("@ownerEmail", ownerEmail);
                    cmd.Parameters.AddWithValue("@SampleDate", sampleDate);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
        public static DataSet GetNonParametricCategoryDataSet(int labTestId, int categoryId, string quantileRoot, double minCount )
        {
            var reply = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"[GetNonParametricCategoryDataSet]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.AddWithValue("@LabTestId", labTestId);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@QuantileRoot", quantileRoot);
                    cmd.Parameters.AddWithValue("@MinCount", minCount);
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(reply);
                    return reply;
                }
            }
        }
    }
}
