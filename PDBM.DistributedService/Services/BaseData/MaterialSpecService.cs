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
    /// 设计规格分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MaterialSpecService : IMaterialSpecService
    {
        private readonly IMaterialSpecService materialSpecServiceImpl = ServiceLocator.Instance.GetService<IMaterialSpecService>();

        public MaterialSpecMaintObject GetMaterialSpecById(Guid id)
        {
            try
            {
                return materialSpecServiceImpl.GetMaterialSpecById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetMaterialSpecs(Guid materialSpecId)
        {
            try
            {
                return materialSpecServiceImpl.GetMaterialSpecs(materialSpecId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<MaterialSpecMaintObject> GetUsedMaterialSpecs(Guid materialSpecId)
        {
            try
            {
                return materialSpecServiceImpl.GetUsedMaterialSpecs(materialSpecId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateMaterialSpec(MaterialSpecMaintObject materialSpecMaintObject)
        {
            try
            {
                materialSpecServiceImpl.AddOrUpdateMaterialSpec(materialSpecMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveMaterialSpecs(IList<MaterialSpecMaintObject> materialSpecMaintObjects)
        {
            try
            {
                materialSpecServiceImpl.RemoveMaterialSpecs(materialSpecMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public MaterialSpecMaintObject GetSupplierCustomerNameByMaterialSpecId(Guid id)
        {
            try
            {
                return materialSpecServiceImpl.GetSupplierCustomerNameByMaterialSpecId(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            materialSpecServiceImpl.Dispose();
        }
    }
}
