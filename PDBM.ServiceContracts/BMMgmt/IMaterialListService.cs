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
    /// 物资清单服务接口
    /// </summary>
    [ServiceContract]
    public interface IMaterialListService : IDistributedService
    {
        /// <summary>
        /// 根据物资清单Id获取任务
        /// </summary>
        /// <param name="id">物资清单Id</param>
        /// <returns>物资清单维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        MaterialListMaintObject GetMaterialListById(Guid id);

        /// <summary>
        /// 获取物资清单
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetMaterialList(Guid parentId, int propertyType);

        /// <summary>
        /// 新增或者修改物资清单
        /// </summary>
        /// <param name="materialListMaintObject">要新增或者修改的物资清单维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateMaterialList(MaterialListMaintObject materialListMaintObject);

        /// <summary>
        /// 删除物资清单
        /// </summary>
        /// <param name="towerMaintObjects">要删除的物资清单维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveMaterialList(IList<MaterialListMaintObject> materialListMaintObjects);

        /// <summary>
        /// 指定供应商
        /// </summary>
        /// <param name="materialListMaintObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveMaterialSpec(MaterialListMaintObject materialListMaintObject);

        /// <summary>
        /// 获取申购清单
        /// </summary>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="materialName">物资名称</param>
        /// <param name="doState">申购状态</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetMaterialPurchasePage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string materialName, int doState);

        /// <summary>
        /// 申购确认
        /// </summary>
        /// <param name="materialListMaintObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void DoStateConfirm(IList<MaterialListMaintObject> materialListMaintObjects);
    }
}
