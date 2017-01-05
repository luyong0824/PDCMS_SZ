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
    /// 物资名称服务接口
    /// </summary>
    [ServiceContract]
    public interface IMaterialService : IDistributedService
    {
        /// <summary>
        /// 根据物资名称Id获取物资名称
        /// </summary>
        /// <param name="id">物资名称Id</param>
        /// <returns>物资名称维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        MaterialMaintObject GetMaterialById(Guid id);

        /// <summary>
        /// 根据物资类别Id获取物资名称列表
        /// </summary>
        /// <param name="materialCategoryId">物资类别Id</param>
        /// <returns>物资名称维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<MaterialMaintObject> GetMaterials(Guid materialCategoryId);

        /// <summary>
        /// 获取状态为使用的物资名称列表
        /// </summary>
        /// <returns>物资名称选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<MaterialSelectObject> GetUsedMaterials(Guid materialCategoryId);

        /// <summary>
        /// 获取所有状态为使用的物资名称列表
        /// </summary>
        /// <returns>物资名称列表Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAllUsedMaterials();

        /// <summary>
        /// 根据物资名称Id获取相同设计规格下所有状态为使用的物资名称列表
        /// </summary>
        /// <param name="id">物资名称Id</param>
        /// <returns>物资名称维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<MaterialMaintObject> GetUsedMaterialsBySelf(Guid id);

        /// <summary>
        /// 新增或者修改物资名称
        /// </summary>
        /// <param name="materialMaintObject">要新增或者修改的物资名称维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateMaterial(MaterialMaintObject materialMaintObject);

        /// <summary>
        /// 删除物资名称
        /// </summary>
        /// <param name="materialMaintObjects">要删除的物资名称维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveMaterials(IList<MaterialMaintObject> materialMaintObjects);
    }
}
