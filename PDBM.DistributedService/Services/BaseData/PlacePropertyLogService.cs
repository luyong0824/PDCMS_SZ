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
    /// 运营商使用情况历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlacePropertyLogService : IPlacePropertyLogService
    {
        private readonly IPlacePropertyLogService placePropertyLogServiceImpl = ServiceLocator.Instance.GetService<IPlacePropertyLogService>();

        public PlacePropertyLogMaintObject GetPlacePropertyLogById(Guid id)
        {
            try
            {
                return placePropertyLogServiceImpl.GetPlacePropertyLogById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlacePropertyLog(int propertyType, Guid parentId, int companyNameId)
        {
            try
            {
                return placePropertyLogServiceImpl.GetPlacePropertyLog(propertyType, parentId, companyNameId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            placePropertyLogServiceImpl.Dispose();
        }
    }
}
