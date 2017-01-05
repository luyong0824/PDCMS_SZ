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
    [ServiceContract]
    public interface IOperatorsPlanningDemandService : IDistributedService
    {
        /// <summary>
        /// 根据条件获取分页运改造站需求确认明细列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="demand">需求确认</param>
        /// <param name="companyId">运营商公司Id</param>
        /// <returns>分页改造站需求确认明细列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsPlanningDemandsPage(int pageIndex, int pageSize, string planningCode, string planningName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int demand, Guid companyId);

        /// <summary>
        /// 新增或者修改改造基站需求确认
        /// </summary>
        /// <param name="operatorsPlanningDemandMaintObjects">要新增或者修改的改造基站需求维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateOperatorsPlanningDemand(IList<OperatorsPlanningDemandMaintObject> operatorsPlanningDemandMaintObjects);

        /// <summary>
        /// 需求确认
        /// </summary>
        /// <param name="currentUserId">确认用户Id</param>
        /// <param name="demand">是否需要</param>
        /// <param name="operatorsPlanningDemandMaintObjects">要确认的运营商改造基站需求确认维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void OperatorsPlanningDemandConfirm(Guid currentUserId, int demand,
            IList<OperatorsPlanningDemandMaintObject> operatorsPlanningDemandMaintObjects);

        /// <summary>
        /// 根据改造站需求确认获取关联的运营商规划列表
        /// </summary>
        /// <param name="operatorsPlanningDemandId">改造站需求确认Id</param>
        /// <returns>运营商规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsPlanningsByOperatorsPlanningDemandId(Guid operatorsPlanningDemandId);
    }
}
