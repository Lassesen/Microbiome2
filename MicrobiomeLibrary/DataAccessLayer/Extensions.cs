using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace MicrobiomeLibrary.DataAccessLayer
{
    public static class Extensions
    {
        public static void WriteSqlTableTypeDef(this DataTable t)
        {
            var sb = new StringBuilder($"Create Type [tbi_{t.TableName}] As Table (\r\n");
            foreach (DataColumn col in t.Columns)
            {
                var sqlType = "varchar(100)";
                var netType = col.DataType.ToString();
                switch (netType)
                {
                    case "System.Double": sqlType = "float"; break;
                    case "System.Int32": sqlType = "int"; break;
                    case "System.DateTime": sqlType = "datetime"; break;
                    case "System.String": sqlType = "varchar(100)"; break;
                    case "System.Guid": sqlType = "[UniqueIdentifier]"; break;
                    default:
                        sqlType = "float"; break;
                }
                sb.AppendLine($"[{col.ColumnName}] {sqlType},");
            }
            sb.Length = sb.Length - 3;
            sb.Append(")");
            File.WriteAllText($"tbi_{t.TableName}.sql", sb.ToString());
        }
        public static List<string> ToCsvString(this DataTable t, char delimiter = ',')
        {
            var reply = new List<string>();
            var sb = new StringBuilder();
            foreach (DataColumn col in t.Columns)
            {
                //remove delimiters from names
                sb.Append($"{ col.ColumnName.Replace(delimiter, ' ')}{delimiter}");
            }
            sb.Length--;
            reply.Add(sb.ToString());
            foreach (DataRow row in t.Rows)
            {
                sb.Clear();
                foreach (DataColumn col in t.Columns)
                {
                    if (!row.IsNull(col.ColumnName))
                    {
                        sb.Append(row[col.ColumnName].ToString());
                    }
                    sb.Append(delimiter);
                }
                sb.Length--;
                reply.Add(sb.ToString());
            }
            return reply;
        }
    }
}
