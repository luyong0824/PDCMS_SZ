using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 运营商规划服务接口
    /// </summary>
    [ServiceContract]
    public interface IOperatorsPlanningService : IDistributedService
    {
        /// <summary>
        /// 根据运营商规划Id获取运营商规划
        /// </summary>
        /// <param name="id">运营商规划Id</param>
        /// <returns>运营商规划维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        OperatorsPlanningMaintObject GetOperatorsPlanningById(Guid id);

        /// <summary>
        /// 根据条件获取分页运营商规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否采纳</param>
        /// <param name="toShared">是否转为基站改造</param>
        /// <returns>分页运营商规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate,
            string planningCode, string planningName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, int urgency, int solved, int toShared);

        /// <summary>
        /// 根据条件获取分页运营商规划列表，用于选择运营商规划
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <returns>分页运营商规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsPlanningsPageBySelect(int pageIndex, int pageSize, string planningCode, string planningName,
            Guid companyId, int profession, Guid placeCategoryId, Guid areaId, int urgency);

        /// <summary>
        /// 根据条件获取指定距离内的运营商规划列表
        /// </summary>
        /// <param name="id">运营商规划Id</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="profession">专业</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>运营商规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsPlanningsByDistance(Guid id, Guid planningId, int profession, decimal distance);

        /// <summary>
        /// 根据规划获取关联的运营商规划列表
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <returns>运营商规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsPlanningsByPlanning(Guid planningId);

        /// <summary>
        /// 新增或者修改运营商规划
        /// </summary>
        /// <param name="operatorsPlanningMaintObject">要新增或者修改的运营商规划维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateOperatorsPlanning(OperatorsPlanningMaintObject operatorsPlanningMaintObject);

        /// <summary>
        /// 运营商规划关联规划
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="planningCreateUserId">规划创建人用户Id</param>
        /// <param name="currentUserId">当前操作人用户Id</param>
        /// <param name="operatorsPlanningMaintObjects">要关联的运营商规划维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void Associate(Guid planningId, Guid planningCreateUserId, Guid currentUserId, IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects);

        /// <summary>
        /// 删除运营商规划
        /// </summary>
        /// <param name="operatorsPlanningMaintObjects">要删除的运营商规划维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveOperatorsPlannings(IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects);


        /// <summary>
        /// 修改运营商规划是否解决
        /// </summary>
        /// <param name="operatorsPlanningMaintObjects">要修改的运营商规划维护实体</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void DemandSolved(IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects);
    }
}
