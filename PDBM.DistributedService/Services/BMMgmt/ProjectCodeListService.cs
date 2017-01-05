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
    /// 立项信息分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ProjectCodeListService : IProjectCodeListService
    {
        private readonly IProjectCodeListService projectCodeListServiceImpl = ServiceLocator.Instance.GetService<IProjectCodeListService>();

        public ProjectCodeListMaintObject GetProjectCodeListById(Guid id)
        {
            try
            {
                return projectCodeListServiceImpl.GetProjectCodeListById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateProjectCodeList(ProjectCodeListMaintObject projectCodeListMaintObject)
        {
            try
            {
                projectCodeListServiceImpl.AddOrUpdateProjectCodeList(projectCodeListMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectCodeListPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, Guid createUserId)
        {
            try
            {
                return projectCodeListServiceImpl.GetProjectCodeListPage(pageIndex, pageSize, beginDate, endDate, projectCode, projectType, placeName, reseauId, projectManagerId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            projectCodeListServiceImpl.Dispose();
        }
    }
}
