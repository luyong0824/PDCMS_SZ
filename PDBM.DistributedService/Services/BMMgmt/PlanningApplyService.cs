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
    /// 建设申请分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlanningApplyService : IPlanningApplyService
    {
        private readonly IPlanningApplyService planningApplyServiceImpl = ServiceLocator.Instance.GetService<IPlanningApplyService>();

        public PlanningApplyMaintObject GetPlanningApplyById(Guid id)
        {
            try
            {
                return planningApplyServiceImpl.GetPlanningApplyById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PlanningApplyHeaderPrintObject GetPlanningApplyPrintById(Guid id)
        {
            try
            {
                return planningApplyServiceImpl.GetPlanningApplyPrintById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlanningApplysPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningName, Guid areaId, Guid reseauId,
            int issued, Guid createUserId, int profession)
        {
            try
            {
                return planningApplyServiceImpl.GetPlanningApplysPage(pageIndex, pageSize, beginDate, endDate, planningName,
                     areaId, reseauId, issued, createUserId, profession);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePlanningApply(PlanningApplyMaintObject planningApplyMaintObject)
        {
            try
            {
                planningApplyServiceImpl.AddOrUpdatePlanningApply(planningApplyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlanningApplys(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.RemovePlanningApplys(planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveBusinessAudit(PlanningApplyMaintObject planningApplyMaintObject)
        {
            try
            {
                planningApplyServiceImpl.SaveBusinessAudit(planningApplyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveTechnicalAudit(PlanningApplyMaintObject planningApplyMaintObject)
        {
            try
            {
                planningApplyServiceImpl.SaveTechnicalAudit(planningApplyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlanningApplyHeader(PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject, IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.SavePlanningApplyHeader(planningApplyHeaderMaintObject, planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlanningApplysByHeaderId(Guid id)
        {
            try
            {
                return planningApplyServiceImpl.GetPlanningApplysByHeaderId(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlanningApplyDetail(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.RemovePlanningApplyDetail(planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlanningApplyHeaders(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.RemovePlanningApplyHeaders(planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlanningAdvice(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.SavePlanningAdvice(planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AppointPlanningUser(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.AppointPlanningUser(planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void IssuePlanningApply(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.IssuePlanningApply(planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void CancelIssuePlanningApply(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            try
            {
                planningApplyServiceImpl.CancelIssuePlanningApply(planningApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlanningApplysWaitPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningName, Guid areaId, Guid reseauId,
            int doState, Guid createUserId, Guid planningUserId, int profession)
        {
            try
            {
                return planningApplyServiceImpl.GetPlanningApplysWaitPage(pageIndex, pageSize, beginDate, endDate, planningName,
                     areaId, reseauId, doState, createUserId, planningUserId, profession);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            planningApplyServiceImpl.Dispose();
        }
    }
}
