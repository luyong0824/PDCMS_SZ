using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// 编码工具
    /// </summary>
    public static class CodeHelper
    {
        /// <summary>
        /// 生成编码列表
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="limit">个数</param>
        /// <returns>编码列表</returns>
        public static DataTable GenerateCodes(string entityName, int limit)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "EntityName", Type = SqlDbType.VarChar, Value = entityName });
            parameters.Add(new Parameter() { Name = "Limit", Type = SqlDbType.Int, Value = limit });
            return SqlHelper.ExecuteDataTable("prc_GenerateCodes", parameters);
        }
    }
}
