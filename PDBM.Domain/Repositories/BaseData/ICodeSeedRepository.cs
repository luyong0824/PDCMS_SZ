using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;

namespace PDBM.Domain.Repositories.BaseData
{
    /// <summary>
    /// 编码种子仓储接口
    /// </summary>
    public interface ICodeSeedRepository : IRepository<CodeSeed>
    {
        /// <summary>
        /// 根据实体名称生成实体编码
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <returns>实体编码</returns>
        string GenerateCode(string entityName);

        /// <summary>
        /// 根据实体名称，数量，生成实体编码列表，用于批量导入
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="limit">数量</param>
        /// <returns>实体编码列表</returns>
        IList<string> GenerateCodes(string entityName, int limit);
    }
}
