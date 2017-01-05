using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.WorkFlow;

namespace PDBM.DistributedService.Services.WorkFlow
{
    /// <summary>
    /// 工作流实例分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WFInstanceService : IWFInstanceService
    {
        private readonly IWFInstanceService wfInstanceServiceImpl = ServiceLocator.Instance.GetService<IWFInstanceService>();

        public WFActivityInstanceSelectObject GetWFActivityInstanceById(Guid id)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFActivityInstanceById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SendWFInstance(WFProcessInstanceSendObject wfProcessInstanceSendObject, IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects)
        {
            try
            {
                wfInstanceServiceImpl.SendWFInstance(wfProcessInstanceSendObject, wfActivityInstanceSendObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void DoWFInstance(WFActivityInstanceDoObject wfActivityInstanceDoObject, IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects)
        {
            try
            {
                wfInstanceServiceImpl.DoWFInstance(wfActivityInstanceDoObject, wfActivityInstanceSendObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFInstancesToDo(Guid userId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFInstancesToDo(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetTaskToDo(Guid userId)
        {
            try
            {
                return wfInstanceServiceImpl.GetTaskToDo(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetTaskToDoMobile(Guid userId)
        {
            try
            {
                return wfInstanceServiceImpl.GetTaskToDoMobile(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetReports(Guid userId)
        {
            try
            {
                return wfInstanceServiceImpl.GetReports(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFActivityInstances(Guid wfProcessInstanceId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFActivityInstances(wfProcessInstanceId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFProcessInstancesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, int wfProcessInstanceState, Guid createUserId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFProcessInstancesPage(pageIndex, pageSize, beginDate, endDate, wfProcessInstanceCode, wfProcessInstanceName, wfProcessId, wfProcessInstanceState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFInstancesDoingPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, Guid userId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFInstancesDoingPage(pageIndex, pageSize, beginDate, endDate, wfProcessInstanceCode, wfProcessInstanceName, wfProcessId, userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFInstancesDoedPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, Guid userId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFInstancesDoedPage(pageIndex, pageSize, beginDate, endDate, wfProcessInstanceCode, wfProcessInstanceName, wfProcessId, userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFInstancesSendedToDoingPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, Guid createUserId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFInstancesSendedToDoingPage(pageIndex, pageSize, beginDate, endDate, wfProcessInstanceCode, wfProcessInstanceName, wfProcessId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFInstancesSendedToDoedPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, int wfProcessInstanceState, Guid createUserId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFInstancesSendedToDoedPage(pageIndex, pageSize, beginDate, endDate, wfProcessInstanceCode, wfProcessInstanceName, wfProcessId, wfProcessInstanceState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFProcessInstanceName(Guid wfCategoryId, Guid entityId)
        {
            try
            {
                return wfInstanceServiceImpl.GetWFProcessInstanceName(wfCategoryId, entityId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            wfInstanceServiceImpl.Dispose();
        }
    }
}
