using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 公司应用层服务
    /// </summary>
    public class CompanyService : DataService, ICompanyService
    {
        private readonly IRepository<Company> companyRepository;

        public CompanyService(IRepositoryContext context,
            IRepository<Company> companyRepository)
            : base(context)
        {
            this.companyRepository = companyRepository;
        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns>公司维护对象列表</returns>
        public IList<CompanyMaintObject> GetCompanys()
        {
            IList<CompanyMaintObject> companyMaintObjects = new List<CompanyMaintObject>();
            IEnumerable<Company> companys = companyRepository.FindAll(null, "CompanyCode");
            if (companys != null)
            {
                foreach (var company in companys)
                {
                    companyMaintObjects.Add(MapperHelper.Map<Company, CompanyMaintObject>(company));
                }
            }
            return companyMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的公司列表
        /// </summary>
        /// <returns>公司选择对象列表</returns>
        public IList<CompanySelectObject> GetUsedCompanys()
        {
            IList<CompanySelectObject> companySelectObjects = new List<CompanySelectObject>();
            IEnumerable<Company> companys = companyRepository.FindAll(Specification<Company>.Eval(entity => entity.State == State.使用), "CompanyCode");
            if (companys != null)
            {
                foreach (var company in companys)
                {
                    companySelectObjects.Add(MapperHelper.Map<Company, CompanySelectObject>(company));
                }
            }
            return companySelectObjects;
        }

        /// <summary>
        /// 根据公司性质获取状态为使用的公司列表
        /// </summary>
        /// <param name="companyNature">公司性质</param>
        /// <returns>公司选择对象列表</returns>
        public IList<CompanySelectObject> GetUsedCompanysByNature(int companyNature)
        {
            IList<CompanySelectObject> companySelectObjects = new List<CompanySelectObject>();
            IEnumerable<Company> companys = companyRepository.FindAll(Specification<Company>.Eval(entity => entity.CompanyNature == (CompanyNature)companyNature && entity.State == State.使用), "CompanyCode");
            if (companys != null)
            {
                foreach (var company in companys)
                {
                    companySelectObjects.Add(MapperHelper.Map<Company, CompanySelectObject>(company));
                }
            }
            return companySelectObjects;
        }
    }
}
