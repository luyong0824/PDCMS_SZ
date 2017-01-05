using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.WorkFlow
{
    /// <summary>
    /// 工作流实例接口
    /// </summary>
    [ServiceContract]
    public interface IWFInstanceService : IDistributedService
    {
        /// <summary>
        /// 根据工作流活动实例Id获取工作流活动实例
        /// </summary>
        /// <param name="id">工作流活动实例Id</param>
        /// <returns>工作流活动实例选择对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WFActivityInstanceSelectObject GetWFActivityInstanceById(Guid id);

        /// <summary>
        /// 发送工作流实例
        /// </summary>
        /// <param name="wfProcessInstanceSendObject">要发送的工作流过程实例发送对象</param>
        /// <param name="wfActivityInstanceSendObjects">要发送的工作流活动实例发送对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SendWFInstance(WFProcessInstanceSendObject wfProcessInstanceSendObject, IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects);

        /// <summary>
        /// 处理工作流实例
        /// </summary>
        /// <param name="wfActivityInstanceDoObject">要处理的工作流活动实例处理对象</param>
        /// <param name="wfActivityInstanceSendObjects">要发送的工作流活动实例发送对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void DoWFInstance(WFActivityInstanceDoObject wfActivityInstanceDoObject, IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects);

        /// <summary>
        /// 获取用户待办工作流实例列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户待办工作流实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFInstancesToDo(Guid userId);

        /// <summary>
        /// 获取用户待办任务流实例列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户待办工作流实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetTaskToDo(Guid userId);

        /// <summary>
        /// 获取用户待办任务流实例列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户待办工作流实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetTaskToDoMobile(Guid userId);

        /// <summary>
        /// 获取报表列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>报表列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetReports(Guid userId);

        /// <summary>
        /// 根据工作流过程实例Id获取工作流活动实例列表
        /// </summary>
        /// <param name="wfProcessInstanceId">工作流过程实例Id</param>
        /// <returns>工作流活动实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFActivityInstances(Guid wfProcessInstanceId);

        /// <summary>
        /// 根据条件获取分页工作流过程实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="wfProcessInstanceState">工作流过程实例状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页工作流过程实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFProcessInstancesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode,
            string wfProcessInstanceName, Guid wfProcessId, int wfProcessInstanceState, Guid createUserId);

        /// <summary>
        /// 根据条件获取分页待处理工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>分页待处理工作流实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFInstancesDoingPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode,
            string wfProcessInstanceName, Guid wfProcessId, Guid userId);

        /// <summary>
        /// 根据条件获取分页已处理工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>分页已处理工作流实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFInstancesDoedPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode,
            string wfProcessInstanceName, Guid wfProcessId, Guid userId);

        /// <summary>
        /// 根据条件获取分页已发送(待处理)工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页已发送(待处理)工作流实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFInstancesSendedToDoingPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode,
            string wfProcessInstanceName, Guid wfProcessId, Guid createUserId);

        /// <summary>
        /// 根据条件获取分页已发送(已处理)工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="wfProcessInstanceState">工作流过程实例状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页已发送(已处理)工作流实例列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFInstancesSendedToDoedPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode,
            string wfProcessInstanceName, Guid wfProcessId, int wfProcessInstanceState, Guid createUserId);

        /// <summary>
        /// 获取公文标题
        /// </summary>
        /// <param name="wfCategoryId">公文类型Id</param>
        /// <param name="entityId">发送实体Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFProcessInstanceName(Guid wfCategoryId, Guid entityId);
    }
}
