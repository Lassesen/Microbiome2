using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace MicrobiomeLibrary.DataAccessLayer
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
        public static DataTable GetLabTests()
        {
            var reply = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM LabTests", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(reply);
                    return reply.Tables[0];
                }
            }
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
        public static DataSet GetNonParametricCategoryDataSet(int labTestId, int categoryId, string quantileRoot, double minCount)
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
        public static DataSet Export(string name = "Some Site")
        {
            var reply = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"[ExportData]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SourceName", name);
                    cmd.CommandTimeout = 900;
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(reply);
                    return reply;
                }
            }
        }

        public static DataTable GetTaxonReports(int labTestId)
        {
            var dataSet = new DataSet("RawData");
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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
        internal static void UpdateStatistics(DataTable statsDatatable, int labTestId)
        {
            string[] selectedColumns = new[] { "taxon", "mean" };
            string[] coreColumns = new[] { "taxon", "mean","median","mode","minimum","maximum","StandardDeviation",
            "Variance","Skewness","Kurtosis","Count"};
            //Verify no typos...
            for (var c = 0; c < coreColumns.Length; c++)
            {
                coreColumns[c] = statsDatatable.Columns[c].ColumnName;
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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
                foreach (DataColumn col in statsDatatable.Columns)
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
        public static void Import(DataSet dataSet)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"[ImportData]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    var adapter = new SqlDataAdapter(cmd);
                    for (var i = 0; i < dataSet.Tables.Count; i++)
                    {
                        dataSet.Tables[i].TableName = $"Table{i}";
                        SqlParameter hParameter = new SqlParameter();
                        hParameter.ParameterName = $"@Table{i}";
                        hParameter.SqlDbType = SqlDbType.Structured;
                        var newCols = new List<string>();
                        foreach (DataColumn col in dataSet.Tables[i].Columns)
                        {
                            var colName = col.ColumnName.ToLowerInvariant();
                            if (colName.Contains("guid"))
                            {
                                newCols.Add(colName.Replace("guid", "id"));
                            }
                        }
                        //for each guid we add in a Id column to be populated.
                        foreach (var newcol in newCols)
                        {
                            dataSet.Tables[i].Columns.Add(newcol, typeof(int));
                        }
                        dataSet.Tables[i].WriteSqlTableTypeDef();
                        hParameter.Value = dataSet.Tables[i];
                        cmd.Parameters.Add(hParameter);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
