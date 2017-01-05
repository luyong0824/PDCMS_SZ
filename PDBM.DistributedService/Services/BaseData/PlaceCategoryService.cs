using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 站点类型分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlaceCategoryService : IPlaceCategoryService
    {
        private readonly IPlaceCategoryService placeCategoryServiceImpl = ServiceLocator.Instance.GetService<IPlaceCategoryService>();

        public PlaceCategoryMaintObject GetPlaceCategoryById(Guid id)
        {
            try
            {
                return placeCategoryServiceImpl.GetPlaceCategoryById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<PlaceCategoryMaintObject> GetPlaceCategorys(int profession)
        {
            try
            {
                return placeCategoryServiceImpl.GetPlaceCategorys(profession);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<PlaceCategorySelectObject> GetUsedPlaceCategorys(int profession)
        {
            try
            {
                return placeCategoryServiceImpl.GetUsedPlaceCategorys(profession);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePlaceCategory(PlaceCategoryMaintObject placeCategoryMaintObject)
        {
            try
            {
                placeCategoryServiceImpl.AddOrUpdatePlaceCategory(placeCategoryMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlaceCategorys(IList<PlaceCategoryMaintObject> placeCategoryMaintObjects)
        {
            try
            {
                placeCategoryServiceImpl.RemovePlaceCategorys(placeCategoryMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            placeCategoryServiceImpl.Dispose();
        }
    }
}
