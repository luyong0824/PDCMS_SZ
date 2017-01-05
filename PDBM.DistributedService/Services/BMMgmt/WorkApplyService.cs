using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 隐患上报分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WorkApplyService : IWorkApplyService
    {
        private readonly IWorkApplyService workApplyServiceImpl = ServiceLocator.Instance.GetService<IWorkApplyService>();

        public WorkApplyMaintObject GetWorkApplyById(Guid id)
        {
            try
            {
                return workApplyServiceImpl.GetWorkApplyById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public WorkApplyPrintObject GetWorkApplyPrintById(Guid id)
        {
            try
            {
                return workApplyServiceImpl.GetWorkApplyPrintById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWorkApplysPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, int orderState, int isSoved, Guid createUserId)
        {
            try
            {
                return workApplyServiceImpl.GetWorkApplysPage(pageIndex, pageSize, beginDate, endDate, title, reseauId, orderState, isSoved, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateWorkApply(WorkApplyMaintObject workApplyMaintObject)
        {
            try
            {
                workApplyServiceImpl.AddOrUpdateWorkApply(workApplyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SendWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            try
            {
                workApplyServiceImpl.SendWorkApplys(workApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            try
            {
                workApplyServiceImpl.RemoveWorkApplys(workApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWorkApplyWaitPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, int isSoved, Guid sendUserId)
        {
            try
            {
                return workApplyServiceImpl.GetWorkApplyWaitPage(pageIndex, pageSize, beginDate, endDate, title, isSoved, sendUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveWorkApplyAssociate(WorkOrderMaintObject workOrderMaintObject, IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            try
            {
                workApplyServiceImpl.SaveWorkApplyAssociate(workOrderMaintObject, workApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWorkApplysReport(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid customerId, int orderState, int isSoved, Guid createUserId)
        {
            try
            {
                return workApplyServiceImpl.GetWorkApplysReport(pageIndex, pageSize, beginDate, endDate, title, reseauId, customerId, orderState, isSoved, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void ReturnWorkApply(WorkApplyMaintObject workApplyMaintObject)
        {
            try
            {
                workApplyServiceImpl.ReturnWorkApply(workApplyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWorkApplyProjectPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, string projectCode, int isProject, Guid sendUserId)
        {
            try
            {
                return workApplyServiceImpl.GetWorkApplyProjectPage(pageIndex, pageSize, beginDate, endDate, title, projectCode, isProject, sendUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveWorkApplyProjectCode(WorkApplyMaintObject workApplyMaintObject)
        {
            try
            {
                workApplyServiceImpl.SaveWorkApplyProjectCode(workApplyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveIsProjectWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            try
            {
                workApplyServiceImpl.SaveIsProjectWorkApplys(workApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            workApplyServiceImpl.Dispose();
        }
    }
}
