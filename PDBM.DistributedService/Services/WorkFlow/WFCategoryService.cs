using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.WorkFlow;

namespace PDBM.DistributedService.Services.WorkFlow
{
    /// <summary>
    /// 工作流类型分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WFCategoryService : IWFCategoryService
    {
        private readonly IWFCategoryService wfCategoryServiceImpl = ServiceLocator.Instance.GetService<IWFCategoryService>();

        public WFCategorySelectObject GetWFCategoryById(Guid id)
        {
            try
            {
                return wfCategoryServiceImpl.GetWFCategoryById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<WFCategorySelectObject> GetUsedWFCategorys()
        {
            try
            {
                return wfCategoryServiceImpl.GetUsedWFCategorys();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            wfCategoryServiceImpl.Dispose();
        }
    }
}
