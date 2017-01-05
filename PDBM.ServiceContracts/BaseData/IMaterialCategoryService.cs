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
    /// 物资类别服务接口
    /// </summary>
    [ServiceContract]
    public interface IMaterialCategoryService : IDistributedService
    {
        /// <summary>
        /// 根据物资类别Id获取物资类别
        /// </summary>
        /// <param name="id">物资类别Id</param>
        /// <returns>物资类别维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        MaterialCategoryMaintObject GetMaterialCategoryById(Guid id);

        /// <summary>
        /// 获取物资类别列表
        /// </summary>
        /// <returns>物资类别维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<MaterialCategoryMaintObject> GetMaterialCategorys();

        /// <summary>
        /// 获取状态为使用的物资类别列表
        /// </summary>
        /// <returns>物资类别选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<MaterialCategorySelectObject> GetUsedMaterialCategorys();

        /// <summary>
        /// 新增或者修改物资类别
        /// </summary>
        /// <param name="materialCategoryMaintObject">要新增或者修改的物资类别维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateMaterialCategory(MaterialCategoryMaintObject materialCategoryMaintObject);

        /// <summary>
        /// 删除物资类别
        /// </summary>
        /// <param name="materialCategoryMaintObjects">要删除的物资类别维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveMaterialCategorys(IList<MaterialCategoryMaintObject> materialCategoryMaintObjects);
    }
}
