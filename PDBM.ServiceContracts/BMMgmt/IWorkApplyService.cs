using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 隐患上报服务接口
    /// </summary>
    [ServiceContract]
    public interface IWorkApplyService : IDistributedService
    {
        /// <summary>
        /// 根据隐患上报Id获取任务
        /// </summary>
        /// <param name="id">隐患上报Id</param>
        /// <returns>隐患上报维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkApplyMaintObject GetWorkApplyById(Guid id);

        /// <summary>
        /// 根据隐患上报Id获取隐患上报打印信息
        /// </summary>
        /// <param name="id">隐患上报Id</param>
        /// <returns>隐患上报打印对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkApplyPrintObject GetWorkApplyPrintById(Guid id);

        /// <summary>
        /// 根据条件获取分页隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWorkApplysPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, int orderState, int isSoved, Guid createUserId);

        /// <summary>
        /// 新增或者修改隐患上报
        /// </summary>
        /// <param name="workApplyMaintObject">要新增或者修改的隐患上报维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateWorkApply(WorkApplyMaintObject workApplyMaintObject);

        /// <summary>
        /// 发送隐患上报单
        /// </summary>
        /// <param name="workApplyMaintObjects">要发送的隐患上报维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SendWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects);

        /// <summary>
        /// 删除隐患上报
        /// </summary>
        /// <param name="workApplyMaintObjects">要删除的隐患上报维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects);

        /// <summary>
        /// 根据条件获取分页待处理隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="Title">标题</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="sendUserId">派单人Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWorkApplyWaitPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, int isSoved, Guid sendUserId);

        /// <summary>
        /// 关联隐患上报与零星派工单
        /// </summary>
        /// <param name="workOrderMaintObject">零星派工单维护对象</param>
        /// <param name="workApplyMaintObjects">隐患上报维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveWorkApplyAssociate(WorkOrderMaintObject workOrderMaintObject, IList<WorkApplyMaintObject> workApplyMaintObjects);

        /// <summary>
        /// 根据条件获取分页隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="customerId">申请单位Id</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWorkApplysReport(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid customerId, int orderState, int isSoved, Guid createUserId);

        /// <summary>
        /// 退回隐患上报
        /// </summary>
        /// <param name="workApplyMaintObject">要退回的隐患上报维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void ReturnWorkApply(WorkApplyMaintObject workApplyMaintObject);

        /// <summary>
        /// 根据条件获取分页待立项隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="isProject">是否立项</param>
        /// <param name="sendUserId">派单人Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWorkApplyProjectPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, string projectCode, int isProject, Guid sendUserId);

        /// <summary>
        /// 保存项目编码
        /// </summary>
        /// <param name="workApplyMaintObject">要保存项目编码的隐患上报维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveWorkApplyProjectCode(WorkApplyMaintObject workApplyMaintObject);

        /// <summary>
        /// 保存立项完成隐患上报
        /// </summary>
        /// <param name="workApplyMaintObjects">要保存立项完成的隐患上报维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveIsProjectWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects);
    }
}
