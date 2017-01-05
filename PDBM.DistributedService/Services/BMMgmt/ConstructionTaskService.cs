using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;
using System.ServiceModel;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 任务分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ConstructionTaskService : IConstructionTaskService
    {
        private readonly IConstructionTaskService constructionTaskServiceImpl = ServiceLocator.Instance.GetService<IConstructionTaskService>();

        public ConstructionTaskMaintObject GetConstructionTaskById(Guid id)
        {
            try
            {
                return constructionTaskServiceImpl.GetConstructionTaskById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateConstructionTask(ConstructionTaskMaintObject constructionTaskMaintObject)
        {
            try
            {
                constructionTaskServiceImpl.AddOrUpdateConstructionTask(constructionTaskMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveConstructionTask(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects)
        {
            try
            {
                constructionTaskServiceImpl.RemoveConstructionTask(constructionTaskMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetConstructionPlanningsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid projectId, int constructionProgress, Guid projectManagerId, int constructionMethod)
        {
            try
            {
                return constructionTaskServiceImpl.GetConstructionPlanningsPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId, projectId, constructionProgress, projectManagerId, constructionMethod);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ConstructionTaskEditorObject GetConstructionPlanningById(Guid id, Guid placeId)
        {
            try
            {
                return constructionTaskServiceImpl.GetConstructionPlanningById(id, placeId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveConstructionPlanning(ConstructionTaskEditorObject constructionTaskEditorObject)
        {
            try
            {
                constructionTaskServiceImpl.SaveConstructionPlanning(constructionTaskEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetRegisterPlanningsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, int constructionProgress, int isFinish, Guid companyId, int constructionMethod)
        {
            try
            {
                return constructionTaskServiceImpl.GetRegisterPlanningsPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId, constructionProgress, isFinish, companyId, constructionMethod);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetRegisterRemodeingsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, int constructionProgress, int isFinish, Guid companyId, int constructionMethod)
        {
            try
            {
                return constructionTaskServiceImpl.GetRegisterRemodeingsPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId, constructionProgress, isFinish, companyId, constructionMethod);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PlacePropertyEditorObject GetRegisterPlanningById(Guid id, Guid constructionTaskId, Guid companyId)
        {
            try
            {
                return constructionTaskServiceImpl.GetRegisterPlanningById(id, constructionTaskId, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveRegisterPlanning(PlacePropertyEditorObject placePropertyEditorObject)
        {
            try
            {
                constructionTaskServiceImpl.SaveRegisterPlanning(placePropertyEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetConstructionTasksPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid customerId, int constructionProgress, Guid userId)
        {
            try
            {
                return constructionTaskServiceImpl.GetConstructionTasksPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId, customerId, constructionProgress, userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveConstructionTaskProgress(ConstructionTaskEditorObject constructionTaskEditorObject)
        {
            try
            {
                constructionTaskServiceImpl.SaveConstructionTaskProgress(constructionTaskEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetTaskPropertysPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid customerId, int constructionProgress, int taskModel, Guid supervisorCustomerId, Guid userId)
        {
            try
            {
                return constructionTaskServiceImpl.GetTaskPropertysPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId, customerId, constructionProgress, taskModel, supervisorCustomerId, userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ConstructionTaskPrintObject GetConstructionTaskCardById(Guid id)
        {
            try
            {
                return constructionTaskServiceImpl.GetConstructionTaskCardById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ResourceUpdateObject GetResourceUpdatePrint(Guid id)
        {
            try
            {
                return constructionTaskServiceImpl.GetResourceUpdatePrint(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectInformationPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string propertyRightSql, string groupPlaceCode, string placeName, Guid areaId, Guid reseauId, int constructionMethod, int constructionProgress)
        {
            try
            {
                return constructionTaskServiceImpl.GetProjectInformationPage(pageIndex, pageSize, beginDate, endDate, propertyRightSql, groupPlaceCode, placeName, areaId, reseauId, constructionMethod, constructionProgress);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SettingProjectManager(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects)
        {
            try
            {
                constructionTaskServiceImpl.SettingProjectManager(constructionTaskMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SettingSupervisorCustomer(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects)
        {
            try
            {
                constructionTaskServiceImpl.SettingSupervisorCustomer(constructionTaskMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            constructionTaskServiceImpl.Dispose();
        }
    }
}
