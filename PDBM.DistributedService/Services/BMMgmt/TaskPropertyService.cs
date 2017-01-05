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
    /// 任务属性分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TaskPropertyService : ITaskPropertyService
    {
        private readonly ITaskPropertyService taskPropertyServiceImpl = ServiceLocator.Instance.GetService<ITaskPropertyService>();

        public TaskPropertyMaintObject GetTaskPropertyById(Guid id)
        {
            try
            {
                return taskPropertyServiceImpl.GetTaskPropertyById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateTaskProperty(TaskPropertyMaintObject taskPropertyMaintObject)
        {
            try
            {
                taskPropertyServiceImpl.AddOrUpdateTaskProperty(taskPropertyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveTaskProperty(TaskPropertyMaintObject taskPropertyMaintObject)
        {
            try
            {
                taskPropertyServiceImpl.SaveTaskProperty(taskPropertyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SettingConstructionCustomer(IList<TaskPropertyMaintObject> taskPropertyMaintObjects)
        {
            try
            {
                taskPropertyServiceImpl.SettingConstructionCustomer(taskPropertyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            taskPropertyServiceImpl.Dispose();
        }
    }
}
