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
    /// 项目任务分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskService projectTaskServiceImpl = ServiceLocator.Instance.GetService<IProjectTaskService>();

        public ProjectTaskEditObject GetProjectTaskById(Guid id)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectTaskById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ProjectTaskEditObject GetProjectTaskEditById(Guid id)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectTaskEditById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ProjectTaskEditObject GetProjectTaskEditByRemodelingId(Guid id)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectTaskEditByRemodelingId(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddProjectTask(ProjectTaskMaintObject projectTaskMaintObject)
        {
            try
            {
                projectTaskServiceImpl.AddProjectTask(projectTaskMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveProjectTasks(IList<ProjectTaskMaintObject> projectTaskMaintObjects)
        {
            try
            {
                projectTaskServiceImpl.RemoveProjectTasks(projectTaskMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AppointAreaAndDesignUser(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.AppointAreaAndDesignUser(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectDesign(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveProjectDesign(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveDesignDrawing(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveDesignDrawing(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveLogicalNumber(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveLogicalNumber(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectOpening(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveProjectOpening(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectDesignsPage(int pageIndex, int pageSize, Guid areaId, Guid reseauId, string placeName, string projectCode, int profession, Guid areaManagerId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectDesignsPage(pageIndex, pageSize, areaId, reseauId, placeName, projectCode, profession, areaManagerId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetDesignDrawingsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string designRealName, int profession, Guid userId)
        {
            try
            {
                return projectTaskServiceImpl.GetDesignDrawingsPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId, designRealName, profession, userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ProjectTaskEditObject GetProjectDesignEditById(Guid id)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectDesignEditById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectDesignEdit(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveProjectDesignEdit(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectProgresssPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId,
             int projectProgress, int profession, Guid areaManagerId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectProgresssPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId,
                    projectProgress, profession, areaManagerId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectProgresssPageMobile(int profession, Guid areaId, Guid reseauId, string projectCode, string placeName, int projectProgress, Guid areaManagerId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectProgresssPageMobile(profession, areaId, reseauId, projectCode, placeName, projectProgress, areaManagerId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ProjectTaskEditObject GetProjectProgressById(Guid id)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectProgressById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ProjectTaskEditObject GetProjectProgressByIdMobile(Guid id, string header)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectProgressByIdMobile(id, header);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectProgress(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveProjectProgress(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectProgressMobile(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveProjectProgressMobile(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ProjectTaskEditObject GetDesignDrawingById(Guid id)
        {
            try
            {
                return projectTaskServiceImpl.GetDesignDrawingById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveDesignDrawingEdit(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveDesignDrawingEdit(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AppointAreaAndDesignUserR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.AppointAreaAndDesignUserR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectDesignR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveProjectDesignR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveDesignDrawingR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveDesignDrawingR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveLogicalNumberR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveLogicalNumberR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectOpeningR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveProjectOpeningR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlaceState(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SavePlaceState(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectTaskHistory(Guid placeId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectTaskHistory(placeId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectProgresssReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int projectType,
            int projectProgress, Guid projectManagerId, int isOverTime, int profession, Guid companyId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectProgresssReportPage(pageIndex, pageSize, projectCode, placeName, areaId, reseauId, projectType, projectProgress, projectManagerId, isOverTime, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectDesignReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, Guid generalDesignId,
            string designRealName, int profession, Guid companyId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectDesignReportPage(pageIndex, pageSize, projectCode, placeName, areaId, reseauId, generalDesignId, designRealName, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveAreaAndDesignUserAndProjectDesignR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveAreaAndDesignUserAndProjectDesignR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveLogicalNumberAndProjectOpeningR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveLogicalNumberAndProjectOpeningR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveLogicalNumberAndProjectOpeningAndPlaceStateR(ProjectTaskEditObject projectTaskEditObject)
        {
            try
            {
                projectTaskServiceImpl.SaveLogicalNumberAndProjectOpeningAndPlaceStateR(projectTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectTaskProjectManager(DateTime beginDate, DateTime beginDateYear, Guid departmentId, int profession, Guid companyId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectTaskProjectManager(beginDate, beginDateYear, departmentId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectTaskDepartment(DateTime beginDate, DateTime beginDateYear, int profession, Guid companyId)
        {
            try
            {
                return projectTaskServiceImpl.GetProjectTaskDepartment(beginDate, beginDateYear, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            projectTaskServiceImpl.Dispose();
        }
    }
}
