using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace PDBM.Infrastructure.DataAccess.EnterpriseLibrary
{
    /// <summary>
    /// Sql Server数据库访问助手
    /// </summary>
    public static class SqlHelper
    {
        private static SqlDatabase db = new SqlDatabase(ConfigurationManager.ConnectionStrings["PDCMS_SZ"].ConnectionString);

        /// <summary>
        /// 执行存储过程返回DataTable
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>执行结果</returns>
        public static DataTable ExecuteDataTable(string procName, IList<Parameter> parameters)
        {
            using (var cmd = db.GetStoredProcCommand(procName))
            {
                foreach (var param in parameters)
                {
                    db.AddInParameter(cmd, param.Name, param.Type, param.Value);
                }
                return db.ExecuteDataSet(cmd).Tables[0];
            }
        }

        /// <summary>
        /// 执行存储过程返回DataSet
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>执行结果</returns>
        public static DataSet ExecuteDataSet(string procName, IList<Parameter> parameters)
        {
            using (var cmd = db.GetStoredProcCommand(procName))
            {
                foreach (var param in parameters)
                {
                    db.AddInParameter(cmd, param.Name, param.Type, param.Value);
                }
                return db.ExecuteDataSet(cmd);
            }
        }

        /// <summary>
        /// 执行存储过程返回第一行第一列
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>执行结果</returns>
        public static object ExecuteScalar(string procName, IList<Parameter> parameters)
        {
            using (var cmd = db.GetStoredProcCommand(procName))
            {
                foreach (var param in parameters)
                {
                    db.AddInParameter(cmd, param.Name, param.Type, param.Value);
                }
                return db.ExecuteScalar(cmd);
            }
        }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public struct Parameter
    {
        public string Name;
        public SqlDbType Type;
        public object Value;
    }
}
