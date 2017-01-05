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
    /// 购置站点服务接口
    /// </summary>
    [ServiceContract]
    public interface IPurchaseService : IDistributedService
    {
        /// <summary>
        /// 根据购置站点Id获取购置站点
        /// </summary>
        /// <param name="id">购置站点Id</param>
        /// <returns>购置站点维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PurchaseMaintObject GetPurchaseById(Guid id);

        /// <summary>
        /// 根据条件获取分页购置站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="propertyRightSql">产权列表Sql语句</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="telecomShare">是否电信共享</param>
        /// <param name="mobileShare">是否移动共享</param>
        /// <param name="unicomShare">是否联通共享</param>
        /// <returns>分页购置站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPurchasesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate,
            string groupPlaceCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId,
            string propertyRightSql, int importance, int telecomShare, int mobileShare, int unicomShare);

        /// <summary>
        /// 新增或者修改购置站点
        /// </summary>
        /// <param name="purchaseMaintObject">要新增或者修改的购置站点维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdatePurchase(PurchaseMaintObject purchaseMaintObject);

        /// <summary>
        /// 删除购置站点
        /// </summary>
        /// <param name="purchaseMaintObjects">要删除的购置站点维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePurchases(IList<PurchaseMaintObject> purchaseMaintObjects);
    }
}
