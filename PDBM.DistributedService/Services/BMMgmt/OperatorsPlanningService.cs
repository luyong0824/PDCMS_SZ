using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 运营商规划分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class OperatorsPlanningService : IOperatorsPlanningService
    {
        private readonly IOperatorsPlanningService operatorsPlanningServiceImpl = ServiceLocator.Instance.GetService<IOperatorsPlanningService>();

        public OperatorsPlanningMaintObject GetOperatorsPlanningById(Guid id)
        {
            try
            {
                return operatorsPlanningServiceImpl.GetOperatorsPlanningById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, int urgency, int solved, int toShared)
        {
            try
            {
                return operatorsPlanningServiceImpl.GetOperatorsPlanningsPage(pageIndex, pageSize, beginDate, endDate, planningCode, planningName, companyId, profession, placeCategoryId, areaId, urgency, solved, toShared);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsPlanningsPageBySelect(int pageIndex, int pageSize, string planningCode, string planningName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, int urgency)
        {
            try
            {
                return operatorsPlanningServiceImpl.GetOperatorsPlanningsPageBySelect(pageIndex, pageSize, planningCode, planningName, companyId, profession, placeCategoryId, areaId, urgency);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsPlanningsByDistance(Guid id, Guid planningId, int profession, decimal distance)
        {
            try
            {
                return operatorsPlanningServiceImpl.GetOperatorsPlanningsByDistance(id, planningId, profession, distance);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsPlanningsByPlanning(Guid planningId)
        {
            try
            {
                return operatorsPlanningServiceImpl.GetOperatorsPlanningsByPlanning(planningId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateOperatorsPlanning(OperatorsPlanningMaintObject operatorsPlanningMaintObject)
        {
            try
            {
                operatorsPlanningServiceImpl.AddOrUpdateOperatorsPlanning(operatorsPlanningMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Associate(Guid planningId, Guid planningCreateUserId, Guid currentUserId, IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects)
        {
            try
            {
                operatorsPlanningServiceImpl.Associate(planningId, planningCreateUserId, currentUserId, operatorsPlanningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveOperatorsPlannings(IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects)
        {
            try
            {
                operatorsPlanningServiceImpl.RemoveOperatorsPlannings(operatorsPlanningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void DemandSolved(IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects)
        {
            try
            {
                operatorsPlanningServiceImpl.DemandSolved(operatorsPlanningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            operatorsPlanningServiceImpl.Dispose();
        }
    }
}
