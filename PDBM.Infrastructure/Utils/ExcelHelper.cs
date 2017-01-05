using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// Excel工具
    /// </summary>
    public static class ExcelHelper
    {
        public static DataTable ExcelToDataTable(string filePath, string sheetName)
        {
            string connectionString = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                DataTable dtSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                bool hasTableName = false;
                foreach (DataRow dr in dtSchema.Rows)
                {
                    if (dr["TABLE_NAME"].ToString() == sheetName + "$")
                    {
                        hasTableName = true;
                        break;
                    }
                }
                if (hasTableName)
                {
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from [" + sheetName + "$]", connection))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            dataAdapter.Fill(dt);
                            return dt;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Excel文件中不存在名称为" + sheetName + "的工作表");
                }
            }
        }
    }
}
