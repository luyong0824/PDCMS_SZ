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
    /// 工程任务分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class EngineeringTaskService : IEngineeringTaskService
    {
        private readonly IEngineeringTaskService engineeringTaskServiceImpl = ServiceLocator.Instance.GetService<IEngineeringTaskService>();

        public EngineeringTaskEditObject GetEngineeringTaskEditById(Guid id)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringTaskEditById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddEngineeringTask(EngineeringTaskMaintObject engineeringTaskMaintObject)
        {
            try
            {
                engineeringTaskServiceImpl.AddEngineeringTask(engineeringTaskMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveEngineeringTasks(IList<EngineeringTaskMaintObject> engineeringTaskMaintObjects)
        {
            try
            {
                engineeringTaskServiceImpl.RemoveEngineeringTasks(engineeringTaskMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetEngineeringDesignsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId,
             int taskModel, string designRealName, int designState, int profession, Guid customerUserId)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringDesignsPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId,
                    taskModel, designRealName, designState, profession, customerUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public EngineeringTaskEditObject GetEngineeringDesignById(Guid id)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringDesignById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveEngineeringDesign(EngineeringTaskEditObject engineeringTaskEditObject)
        {
            try
            {
                engineeringTaskServiceImpl.SaveEngineeringDesign(engineeringTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetEngineeringProgresssPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId,
             int taskModel, int engineeringProgress, int profession, Guid currentUserId)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringProgresssPage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId,
                    taskModel, engineeringProgress, profession, currentUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetEngineeringProgresssPageMobile(int profession, string projectCode, string placeName, int taskModel, int engineeringProgress, Guid currentUserId)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringProgresssPageMobile(profession, projectCode, placeName, taskModel, engineeringProgress, currentUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public EngineeringTaskEditObject GetEngineeringProgressById(Guid id)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringProgressById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public EngineeringTaskEditObject GetEngineeringProgressByIdMobile(Guid id, string header)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringProgressByIdMobile(id, header);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveEngineeringProgress(EngineeringTaskEditObject engineeringTaskEditObject)
        {
            try
            {
                engineeringTaskServiceImpl.SaveEngineeringProgress(engineeringTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveEngineeringProgressMobile(EngineeringTaskEditObject engineeringTaskEditObject)
        {
            try
            {
                engineeringTaskServiceImpl.SaveEngineeringProgressMobile(engineeringTaskEditObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetEngineeringProgressReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            int engineeringProgress, int projectType, Guid projectManagerId, Guid constructionCustomerId, Guid supervisionCustomerId, int profession, Guid companyId)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringProgressReportPage(pageIndex, pageSize, projectCode, placeName, areaId, reseauId,
                    taskModel, engineeringProgress, projectType, projectManagerId, constructionCustomerId, supervisionCustomerId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetEngineeringDesignReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            string designRealName, Guid designCustomerId, int profession, Guid companyId)
        {
            try
            {
                return engineeringTaskServiceImpl.GetEngineeringDesignReportPage(pageIndex, pageSize, projectCode, placeName, areaId, reseauId,
                    taskModel, designRealName, designCustomerId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            engineeringTaskServiceImpl.Dispose();
        }
    }
}
