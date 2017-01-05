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
    /// 工作流活动编辑器分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WFActivityEditorService : IWFActivityEditorService
    {
        private readonly IWFActivityEditorService wfActivityEditorServiceImpl = ServiceLocator.Instance.GetService<IWFActivityEditorService>();

        public IList<WFActivityEditorSelectObject> GetUsedWFActivityEditors(Guid wfCategoryId)
        {
            try
            {
                return wfActivityEditorServiceImpl.GetUsedWFActivityEditors(wfCategoryId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            wfActivityEditorServiceImpl.Dispose();
        }
    }
}
