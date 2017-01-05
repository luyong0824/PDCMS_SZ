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
    /// 派工单明细分布式服务接口
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WorkOrderDetailService : IWorkOrderDetailService
    {
        private readonly IWorkOrderDetailService workOrderDetailServiceImpl = ServiceLocator.Instance.GetService<IWorkOrderDetailService>();

        public WorkOrderDetailMaintObject GetWorkOrderDetailById(Guid id)
        {
            try
            {
                return workOrderDetailServiceImpl.GetWorkOrderDetailById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWorkOrderDetail(Guid workOrderId)
        {
            try
            {
                return workOrderDetailServiceImpl.GetWorkOrderDetail(workOrderId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateWorkOrderDetail(WorkOrderDetailMaintObject workOrderDetailMaintObject)
        {
            try
            {
                workOrderDetailServiceImpl.AddOrUpdateWorkOrderDetail(workOrderDetailMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveWorkOrderDetail(IList<WorkOrderDetailMaintObject> workOrderDetailMaintObjects)
        {
            try
            {
                workOrderDetailServiceImpl.RemoveWorkOrderDetail(workOrderDetailMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            workOrderDetailServiceImpl.Dispose();
        }
    }
}
