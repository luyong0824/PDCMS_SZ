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
    /// 物资名称分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialService materialServiceImpl = ServiceLocator.Instance.GetService<IMaterialService>();

        public MaterialMaintObject GetMaterialById(Guid id)
        {
            try
            {
                return materialServiceImpl.GetMaterialById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<MaterialMaintObject> GetMaterials(Guid materialCategoryId)
        {
            try
            {
                return materialServiceImpl.GetMaterials(materialCategoryId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<MaterialSelectObject> GetUsedMaterials(Guid materialCategoryId)
        {
            try
            {
                return materialServiceImpl.GetUsedMaterials(materialCategoryId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAllUsedMaterials()
        {
            try
            {
                return materialServiceImpl.GetAllUsedMaterials();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<MaterialMaintObject> GetUsedMaterialsBySelf(Guid id)
        {
            try
            {
                return materialServiceImpl.GetUsedMaterialsBySelf(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateMaterial(MaterialMaintObject materialMaintObject)
        {
            try
            {
                materialServiceImpl.AddOrUpdateMaterial(materialMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveMaterials(IList<MaterialMaintObject> materialMaintObjects)
        {
            try
            {
                materialServiceImpl.RemoveMaterials(materialMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            materialServiceImpl.Dispose();
        }
    }
}
