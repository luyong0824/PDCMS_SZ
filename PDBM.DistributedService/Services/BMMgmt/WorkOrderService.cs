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
    /// 派工单分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderService workOrderServiceImpl = ServiceLocator.Instance.GetService<IWorkOrderService>();

        public WorkOrderMaintObject GetWorkOrderById(Guid id)
        {
            try
            {
                return workOrderServiceImpl.GetWorkOrderById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public WorkOrderEditorObject GetWorkOrderEditorById(Guid id)
        {
            try
            {
                return workOrderServiceImpl.GetWorkOrderEditorById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public WorkOrderPrintObject GetWorkOrderPrintById(Guid id)
        {
            try
            {
                return workOrderServiceImpl.GetWorkOrderPrintById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWorkOrdersPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid workBigClassId, Guid workSmallClassId, Guid customerId, string maintainContactMan, Guid sendUserId, int isFinish, int orderState, Guid createUserId)
        {
            try
            {
                return workOrderServiceImpl.GetWorkOrdersPage(pageIndex, pageSize, beginDate, endDate, title, reseauId, workBigClassId, workSmallClassId, customerId, maintainContactMan, sendUserId, isFinish, orderState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateWorkOrder(WorkOrderMaintObject workOrderMaintObject)
        {
            try
            {
                workOrderServiceImpl.AddOrUpdateWorkOrder(workOrderMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveWorkOrder(IList<WorkOrderMaintObject> workOrderMaintObjects)
        {
            try
            {
                workOrderServiceImpl.RemoveWorkOrder(workOrderMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveWorkOrderWF(WorkOrderEditorObject workOrderEditorObject)
        {
            try
            {
                workOrderServiceImpl.SaveWorkOrderWF(workOrderEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveWorkOrderByWorkApply(WorkOrderMaintObject workOrderMaintObject, IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            try
            {
                workOrderServiceImpl.SaveWorkOrderByWorkApply(workOrderMaintObject, workApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            workOrderServiceImpl.Dispose();
        }
    }
}
