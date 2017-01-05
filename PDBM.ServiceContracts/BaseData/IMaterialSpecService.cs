using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BaseData
{
    /// <summary>
    /// 设计规格服务接口
    /// </summary>
    [ServiceContract]
    public interface IMaterialSpecService : IDistributedService
    {
        /// <summary>
        /// 根据设计规格Id获取设计规格
        /// </summary>
        /// <param name="id">设计规格Id</param>
        /// <returns>设计规格维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        MaterialSpecMaintObject GetMaterialSpecById(Guid id);

        /// <summary>
        /// 根据区域Id获取设计规格列表
        /// </summary>
        /// <param name="materialId">区域Id</param>
        /// <returns>设计规格维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetMaterialSpecs(Guid materialId);

        /// <summary>
        /// 获取状态为使用的设计规格列表
        /// </summary>
        /// <returns>设计规格选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<MaterialSpecMaintObject> GetUsedMaterialSpecs(Guid materialId);

        /// <summary>
        /// 新增或者修改设计规格
        /// </summary>
        /// <param name="materialMaintObject">要新增或者修改的设计规格维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateMaterialSpec(MaterialSpecMaintObject materialMaintObject);

        /// <summary>
        /// 删除设计规格
        /// </summary>
        /// <param name="materialMaintObjects">要删除的设计规格维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveMaterialSpecs(IList<MaterialSpecMaintObject> materialMaintObjects);

        /// <summary>
        /// 根据设计规格Id获取供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        MaterialSpecMaintObject GetSupplierCustomerNameByMaterialSpecId(Guid id);
    }
}
