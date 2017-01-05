using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;

namespace PDBM.Infrastructure.Data.EntityFramework.Repositories.BaseData
{
    /// <summary>
    /// 单据编码种子仓储
    /// </summary>
    public class OrderCodeSeedRepository : EntityFrameworkRepository<OrderCodeSeed>, IOrderCodeSeedRepository
    {
        public OrderCodeSeedRepository(IRepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// 根据实体名称生成实体单据编码
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="generateDate">生成日期</param>
        /// <param name="profession">专业</param>
        /// <returns>实体单据编码</returns>
        public string GenerateOrderCode(string entityName, DateTime generateDate, int profession)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "EntityName", Type = SqlDbType.VarChar, Value = entityName });
            parameters.Add(new Parameter() { Name = "GenerateDate", Type = SqlDbType.DateTime, Value = generateDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GenerateOrderCode", parameters))
            {
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        /// <summary>
        /// 根据专业生成项目编码
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="generateDate">生成日期</param>
        /// <returns>项目编码编码</returns>
        public string GenerateProjectCode(int profession, DateTime generateDate)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "GenerateDate", Type = SqlDbType.DateTime, Value = generateDate });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GenerateProjectCode", parameters))
            {
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
        }
    }
}
