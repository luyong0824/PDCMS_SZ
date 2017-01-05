using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 公司分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyService companyServiceImpl = ServiceLocator.Instance.GetService<ICompanyService>();

        public IList<CompanyMaintObject> GetCompanys()
        {
            try
            {
                return companyServiceImpl.GetCompanys();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<CompanySelectObject> GetUsedCompanys()
        {
            try
            {
                return companyServiceImpl.GetUsedCompanys();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<CompanySelectObject> GetUsedCompanysByNature(int companyNature)
        {
            try
            {
                return companyServiceImpl.GetUsedCompanysByNature(companyNature);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            companyServiceImpl.Dispose();
        }
    }
}
