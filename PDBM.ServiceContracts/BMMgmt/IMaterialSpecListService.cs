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
    /// 规格型号信息服务接口
    /// </summary>
    [ServiceContract]
    public interface IMaterialSpecListService : IDistributedService
    {
        /// <summary>
        /// 根据规格型号信息表Id获取规格型号信息
        /// </summary>
        /// <param name="id">规格型号信息表Id</param>
        /// <returns>规格型号信息维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        MaterialSpecListMaintObject GetMaterialSpecListById(Guid id);

        /// <summary>
        /// 新增或者修改规格型号信息
        /// </summary>
        /// <param name="materialSpecListMaintObject">要新增或者修改的规格型号信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateMaterialSpecList(MaterialSpecListMaintObject materialSpecListMaintObject);

        /// <summary>
        /// 获取分页导入采购清单列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">立项编号</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">型号类别</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="orderCode">订单编号</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetMaterialSpecListPage(int pageIndex, int pageSize, string projectCode, string customerName, int materialSpecType, string materialSpecName, string orderCode, Guid createUserId);

        /// <summary>
        /// 获取分页导出清单列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">型号类别</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="orderCode">订单编号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectCodeListAndMaterialSpecListPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, string customerName, int materialSpecType, string materialSpecName, string orderCode);
    }
}
