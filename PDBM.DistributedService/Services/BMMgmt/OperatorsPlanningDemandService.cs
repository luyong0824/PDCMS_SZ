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
    /// 改造需求确认分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class OperatorsPlanningDemandService : IOperatorsPlanningDemandService
    {
        private readonly IOperatorsPlanningDemandService operatorsPlanningDemandServiceImpl = ServiceLocator.Instance.GetService<IOperatorsPlanningDemandService>();

        public string GetOperatorsPlanningDemandsPage(int pageIndex, int pageSize, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int demand, Guid companyId)
        {
            try
            {
                return operatorsPlanningDemandServiceImpl.GetOperatorsPlanningDemandsPage(pageIndex, pageSize, planningCode, planningName, profession, placeCategoryId, areaId, reseauId, demand, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateOperatorsPlanningDemand(IList<OperatorsPlanningDemandMaintObject> operatorsPlanningDemandMaintObjects)
        {
            try
            {
                operatorsPlanningDemandServiceImpl.AddOrUpdateOperatorsPlanningDemand(operatorsPlanningDemandMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void OperatorsPlanningDemandConfirm(Guid currentUserId, int demand, IList<OperatorsPlanningDemandMaintObject> operatorsPlanningDemandMaintObjects)
        {
            try
            {
                operatorsPlanningDemandServiceImpl.OperatorsPlanningDemandConfirm(currentUserId, demand, operatorsPlanningDemandMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsPlanningsByOperatorsPlanningDemandId(Guid operatorsPlanningDemandId)
        {
            try
            {
                return operatorsPlanningDemandServiceImpl.GetOperatorsPlanningsByOperatorsPlanningDemandId(operatorsPlanningDemandId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            operatorsPlanningDemandServiceImpl.Dispose();
        }
    }
}
