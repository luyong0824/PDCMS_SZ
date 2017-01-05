using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 物资类别分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MaterialCategoryService : IMaterialCategoryService
    {
        private readonly IMaterialCategoryService materialCategoryServiceImpl = ServiceLocator.Instance.GetService<IMaterialCategoryService>();

        public MaterialCategoryMaintObject GetMaterialCategoryById(Guid id)
        {
            try
            {
                return materialCategoryServiceImpl.GetMaterialCategoryById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<MaterialCategoryMaintObject> GetMaterialCategorys()
        {
            try
            {
                return materialCategoryServiceImpl.GetMaterialCategorys();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<MaterialCategorySelectObject> GetUsedMaterialCategorys()
        {
            try
            {
                return materialCategoryServiceImpl.GetUsedMaterialCategorys();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateMaterialCategory(MaterialCategoryMaintObject materialCategoryMaintObject)
        {
            try
            {
                materialCategoryServiceImpl.AddOrUpdateMaterialCategory(materialCategoryMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveMaterialCategorys(IList<MaterialCategoryMaintObject> materialCategoryMaintObjects)
        {
            try
            {
                materialCategoryServiceImpl.RemoveMaterialCategorys(materialCategoryMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            materialCategoryServiceImpl.Dispose();
        }
    }
}
