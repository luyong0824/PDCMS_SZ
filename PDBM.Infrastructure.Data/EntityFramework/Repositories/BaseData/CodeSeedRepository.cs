using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;

namespace PDBM.Infrastructure.Data.EntityFramework.Repositories.BaseData
{
    /// <summary>
    /// 编码种子仓储
    /// </summary>
    public class CodeSeedRepository : EntityFrameworkRepository<CodeSeed>, ICodeSeedRepository
    {
        public CodeSeedRepository(IRepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// 根据实体名称生成实体编码
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <returns>实体编码</returns>
        public string GenerateCode(string entityName)
        {
            CodeSeed codeSeed = this.Get(Specification<CodeSeed>.Eval(entity => entity.EntityName == entityName));
            codeSeed.Seed++;
            string placeHolder = "";
            if (codeSeed.Digit > 0 && codeSeed.Seed.ToString().Length < codeSeed.Digit)
            {
                int differ = codeSeed.Digit - codeSeed.Seed.ToString().Length;
                for (int i = 0; i < differ; i++)
                {
                    placeHolder += "0";
                }
            }
            this.Update(codeSeed);
            return codeSeed.Prefix + placeHolder + codeSeed.Seed;
        }

        /// <summary>
        /// 根据实体名称，数量，生成实体编码列表，用于批量导入
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="limit">数量</param>
        /// <returns>实体编码列表</returns>
        public IList<string> GenerateCodes(string entityName, int limit)
        {
            IList<string> codeSeeds = new List<string>(limit);
            CodeSeed codeSeed = this.Get(Specification<CodeSeed>.Eval(entity => entity.EntityName == entityName));
            int minSeed = codeSeed.Seed + 1;
            codeSeed.Seed += limit;
            this.Update(codeSeed);
            this.Context.Commit();
            for (int curCodeSeed = minSeed; curCodeSeed <= codeSeed.Seed; curCodeSeed++)
            {
                string placeHolder = "";
                if (codeSeed.Digit > 0 && curCodeSeed.ToString().Length < codeSeed.Digit)
                {
                    int differ = codeSeed.Digit - curCodeSeed.ToString().Length;
                    for (int i = 0; i < differ; i++)
                    {
                        placeHolder += "0";
                    }
                }
                codeSeeds.Add(codeSeed.Prefix + placeHolder + curCodeSeed);
            }
            return codeSeeds;
        }
    }
}
