using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 物资清单分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MaterialListService : IMaterialListService
    {
        private readonly IMaterialListService materialListServiceImpl = ServiceLocator.Instance.GetService<IMaterialListService>();

        public MaterialListMaintObject GetMaterialListById(Guid id)
        {
            try
            {
                return materialListServiceImpl.GetMaterialListById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetMaterialList(Guid parentId, int propertyType)
        {
            try
            {
                return materialListServiceImpl.GetMaterialList(parentId, propertyType);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateMaterialList(MaterialListMaintObject materialListMaintObject)
        {
            try
            {
                materialListServiceImpl.AddOrUpdateMaterialList(materialListMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveMaterialList(IList<MaterialListMaintObject> materialListMaintObjects)
        {
            try
            {
                materialListServiceImpl.RemoveMaterialList(materialListMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveMaterialSpec(MaterialListMaintObject materialListMaintObject)
        {
            try
            {
                materialListServiceImpl.SaveMaterialSpec(materialListMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetMaterialPurchasePage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string materialName, int doState)
        {
            try
            {
                return materialListServiceImpl.GetMaterialPurchasePage(pageIndex, pageSize, placeName, placeCategoryId, areaId, reseauId, materialName, doState);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void DoStateConfirm(IList<MaterialListMaintObject> materialListMaintObjects)
        {
            try
            {
                materialListServiceImpl.DoStateConfirm(materialListMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            materialListServiceImpl.Dispose();
        }
    }
}
