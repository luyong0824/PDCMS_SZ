using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.WorkFlow
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WFActivityInstanceEditorService : IWFActivityInstanceEditorService
    {
        private readonly IWFActivityInstanceEditorService wfActivityInstanceEditorImpl = ServiceLocator.Instance.GetService<IWFActivityInstanceEditorService>();

        public WFActivityInstanceEditorMaintObject GetWFActivityInstanceEditorById(Guid id)
        {
            try
            {
                return wfActivityInstanceEditorImpl.GetWFActivityInstanceEditorById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddWFActivityInstanceEditor(WFActivityInstanceEditorMaintObject wfActivityInstanceEditorMaintObject)
        {
            try
            {
                wfActivityInstanceEditorImpl.AddWFActivityInstanceEditor(wfActivityInstanceEditorMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            wfActivityInstanceEditorImpl.Dispose();
        }
    }
}
