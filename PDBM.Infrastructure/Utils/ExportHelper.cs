using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PDBM.Infrastructure.Utils
{
    public static class ExportHelper
    {
        public static string DataTableToString(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\">");
            sb.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            sb.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            foreach (DataColumn col in dt.Columns)
            {
                sb.AppendLine("<td>" + col.ColumnName + "</td>");
            }
            sb.AppendLine("</tr>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn col in dt.Columns)
                {
                    if (dr[col.ColumnName] == null) continue;
                    sb.AppendLine("<td>" + dr[col.ColumnName].ToString() + "</td>");
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}
