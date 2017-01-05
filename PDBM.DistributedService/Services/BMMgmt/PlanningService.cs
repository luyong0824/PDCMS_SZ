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
    /// 规划分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlanningService : IPlanningService
    {
        private readonly IPlanningService planningServiceImpl = ServiceLocator.Instance.GetService<IPlanningService>();

        public PlanningMaintObject GetPlanningById(Guid id)
        {
            try
            {
                return planningServiceImpl.GetPlanningById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PlanningMaintObject GetPlanningByIdMobile(Guid id, string header)
        {
            try
            {
                return planningServiceImpl.GetPlanningByIdMobile(id, header);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int issued, int addressingState, Guid createUserId)
        {
            try
            {
                return planningServiceImpl.GetPlanningsPage(pageIndex, pageSize, beginDate, endDate, planningCode, planningName,
                    profession, placeCategoryId, areaId, reseauId, importance, issued, addressingState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAddressingUsersPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int isAppoint, int addressingState, Guid createUserId)
        {
            try
            {
                return planningServiceImpl.GetAddressingUsersPage(pageIndex, pageSize, beginDate, endDate, planningCode, planningName,
                    profession, placeCategoryId, areaId, reseauId, importance, isAppoint, addressingState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePlanning(PlanningMaintObject planningMaintObject)
        {
            try
            {
                planningServiceImpl.AddOrUpdatePlanning(planningMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void UpdatePlanningAddressing(PlanningMaintObject planningMaintObject)
        {
            try
            {
                planningServiceImpl.UpdatePlanningAddressing(planningMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlannings(IList<PlanningMaintObject> planningMaintObjects)
        {
            try
            {
                planningServiceImpl.RemovePlannings(planningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AppointAddressingUser(IList<PlanningMaintObject> planningMaintObjects)
        {
            try
            {
                planningServiceImpl.AppointAddressingUser(planningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Issue(IList<PlanningMaintObject> planningMaintObjects)
        {
            try
            {
                planningServiceImpl.Issue(planningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void CancelIssue(IList<PlanningMaintObject> planningMaintObjects)
        {
            try
            {
                planningServiceImpl.CancelIssue(planningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetConstructionPlanningsReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency, int telecomDemand, int mobileDemand, int unicomDemand, int demandState,
            int issued, int addressingState, Guid createUserId, string placeName, Guid addressingUserId, Guid projectManagerId, int constructionProgress)
        {
            try
            {
                return planningServiceImpl.GetConstructionPlanningsReportPage(pageIndex, pageSize, beginDate, endDate, planningCode, planningName,
                    profession, placeCategoryId, areaId, reseauId, urgency, telecomDemand, mobileDemand, unicomDemand, demandState, issued,
                    addressingState, createUserId, placeName, addressingUserId, projectManagerId, constructionProgress);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public NewPlanningMaintObject GetNewPlanningById(Guid id)
        {
            try
            {
                return planningServiceImpl.GetNewPlanningById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateNewPlanning(NewPlanningMaintObject newPlanningMaintObject)
        {
            try
            {
                planningServiceImpl.AddOrUpdateNewPlanning(newPlanningMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveNewPlannings(IList<NewPlanningMaintObject> newPlanningMaintObjects)
        {
            try
            {
                planningServiceImpl.RemoveNewPlannings(newPlanningMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetNewPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid sceneId, int telecomDemand, int mobileDemand, int unicomDemand, int addressingState, Guid createUserId)
        {
            try
            {
                return planningServiceImpl.GetNewPlanningsPage(pageIndex, pageSize, beginDate, endDate, planningCode, planningName,
                    profession, placeCategoryId, areaId, reseauId, sceneId, telecomDemand, mobileDemand, unicomDemand, addressingState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlanningsMobile(int pageIndex, int pageSize, string professionListSql, string planningName, Guid companyId)
        {
            try
            {
                return planningServiceImpl.GetPlanningsMobile(pageIndex, pageSize, professionListSql, planningName, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlanningsPageMobile(int pageIndex, int pageSize, string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId)
        {
            try
            {
                return planningServiceImpl.GetPlanningsPageMobile(pageIndex, pageSize, professionListSql, lng, lat, distance, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlanningPositionMobile(PlanningMaintObject planningMaintObject)
        {
            try
            {
                planningServiceImpl.SavePlanningPositionMobile(planningMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlanningMobile(PlanningMaintObject planningMaintObject)
        {
            try
            {
                planningServiceImpl.SavePlanningMobile(planningMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            planningServiceImpl.Dispose();
        }
    }
}
