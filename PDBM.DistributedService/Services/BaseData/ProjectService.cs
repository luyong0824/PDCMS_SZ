using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 项目分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ProjectService : IProjectService
    {
        private readonly IProjectService projectServiceImpl = ServiceLocator.Instance.GetService<IProjectService>();

        public ProjectMaintObject GetProjectById(Guid id)
        {
            try
            {
                return projectServiceImpl.GetProjectById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectsPage(int pageIndex, int pageSize, string projectCode, string projectName, string projectFullName, int projectCategory, Guid accountingEntityId, int projectProgress, int state)
        {
            try
            {
                return projectServiceImpl.GetProjectsPage(pageIndex, pageSize, projectCode, projectName, projectFullName, projectCategory, accountingEntityId, projectProgress, state);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateProject(ProjectMaintObject projectMaintObject)
        {
            try
            {
                projectServiceImpl.AddOrUpdateProject(projectMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveProjects(IList<ProjectMaintObject> projectMaintObjects)
        {
            try
            {
                projectServiceImpl.RemoveProjects(projectMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectsPageBySelect(int pageIndex, int pageSize, string projectCode, string projectName, string projectFullName, Guid accountingEntityId, int isCheckedProjectProgress1, int isCheckedProjectProgress2, int isCheckedState1, int isCheckedState2)
        {
            try
            {
                return projectServiceImpl.GetProjectsPageBySelect(pageIndex, pageSize, projectCode, projectName, projectFullName, accountingEntityId, isCheckedProjectProgress1, isCheckedProjectProgress2, isCheckedState1, isCheckedState2);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            projectServiceImpl.Dispose();
        }
    }
}
