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
    /// 运营商确认服务接口
    /// </summary>
    [ServiceContract]
    public interface IOperatorsConfirmService : IDistributedService
    {
        /// <summary>
        /// 根据条件获取分页运营商确认明细列表
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
        /// <returns>分页运营商确认明细列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsConfirmDetailsPage(int pageIndex, int pageSize, string planningCode, string planningName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int demand, Guid companyId);

        /// <summary>
        /// 新增运营商需求确认
        /// </summary>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="operatorsConfirmDetailMaintObjects">要新增的运营商需求确认明细维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOperatorsConfirm(Guid createUserId, IList<OperatorsConfirmDetailMaintObject> operatorsConfirmDetailMaintObjects);

        /// <summary>
        /// 需求确认
        /// </summary>
        /// <param name="currentUserId">当前操作人用户Id</param>
        /// <param name="currentCompanyId">当前操作人所在公司Id</param>
        /// <param name="currentCompanyNature">当前操作人所在公司性质</param>
        /// <param name="demand">是否需要</param>
        /// <param name="operatorsConfirmDetailMaintObjects">要确认的运营商确认明细维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void Confirm(Guid currentUserId, Guid currentCompanyId, int currentCompanyNature, int demand,
            IList<OperatorsConfirmDetailMaintObject> operatorsConfirmDetailMaintObjects);
    }
}
