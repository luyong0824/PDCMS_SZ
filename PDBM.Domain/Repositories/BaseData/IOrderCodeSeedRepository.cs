using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;

namespace PDBM.Domain.Repositories.BaseData
{
    /// <summary>
    /// 单据编码种子仓储接口
    /// </summary>
    public interface IOrderCodeSeedRepository : IRepository<OrderCodeSeed>
    {
        /// <summary>
        /// 根据实体名称生成实体单据编码
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="generateDate">生成日期</param>
        /// <param name="profession">专业</param>
        /// <returns>实体单据编码</returns>
        string GenerateOrderCode(string entityName, DateTime generateDate, int profession);

        /// <summary>
        /// 根据专业生成项目编码
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="generateDate">生成日期</param>
        /// <returns>项目编码编码</returns>
        string GenerateProjectCode(int profession, DateTime generateDate);
    }
}
