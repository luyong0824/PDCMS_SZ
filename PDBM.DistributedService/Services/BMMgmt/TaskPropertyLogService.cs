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
    /// 子任务历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TaskPropertyLogService : ITaskPropertyLogService
    {
        private readonly ITaskPropertyLogService taskPropertyLogServiceImpl = ServiceLocator.Instance.GetService<ITaskPropertyLogService>();

        public void AddOrUpdateTaskPropertyLog(TaskPropertyMaintObject taskPropertyMaintObject)
        {
            try
            {
                taskPropertyLogServiceImpl.AddOrUpdateTaskPropertyLog(taskPropertyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetTaskPropertyLog(int taskModel, Guid parentId)
        {
            try
            {
                return taskPropertyLogServiceImpl.GetTaskPropertyLog(taskModel, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            taskPropertyLogServiceImpl.Dispose();
        }
    }
}
