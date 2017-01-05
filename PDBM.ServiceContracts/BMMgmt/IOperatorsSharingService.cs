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
    /// 运营商共享服务接口
    /// </summary>
    [ServiceContract]
    public interface IOperatorsSharingService : IDistributedService
    {
        /// <summary>
        /// 根据运营商共享Id获取运营商共享
        /// </summary>
        /// <param name="id">运营商共享Id</param>
        /// <returns>运营商共享维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        OperatorsSharingMaintObject GetOperatorsSharingById(Guid id);

        /// <summary>
        /// 根据条件获取分页运营商共享列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否采纳</param>
        /// <returns>分页运营商共享列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsSharingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeCode, string placeName,
            Guid companyId, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency, int solved);

        /// <summary>
        /// 根据条件获取分页运营商共享列表，用于选择运营商共享申请
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <returns>分页运营商共享列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsSharingsPageBySelect(int pageIndex, int pageSize, string placeCode, string placeName, Guid companyId, int profession,
            Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency);

        /// <summary>
        /// 根据条件获取指定站点的运营商共享列表
        /// </summary>
        /// <param name="operatorsSharingId">运营商共享Id</param>
        /// <param name="remodelingId">改造Id</param>
        /// <returns>运营商共享列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetOperatorsSharingsByPlace(Guid operatorsSharingId, Guid remodelingId);

        /// <summary>
        /// 新增或者修改运营商共享
        /// </summary>
        /// <param name="operatorsSharingMaintObject">要新增或者修改的运营商共享维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateOperatorsSharing(OperatorsSharingMaintObject operatorsSharingMaintObject);

        /// <summary>
        /// 运营商共享关联改造
        /// </summary>
        /// <param name="remodelingId">改造Id</param>
        /// <param name="remodelingCreateUserId">改造创建人用户Id</param>
        /// <param name="currentUserId">当前操作人用户Id</param>
        /// <param name="operatorsSharingMaintObjects">要关联的运营商共享维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void Associate(Guid remodelingId, Guid remodelingCreateUserId, Guid currentUserId, IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects);

        /// <summary>
        /// 删除运营商共享
        /// </summary>
        /// <param name="operatorsSharingMaintObjects">要删除的运营商共享维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveOperatorsSharings(IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects);
    }
}
