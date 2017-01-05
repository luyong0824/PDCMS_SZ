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
    /// 区域分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AreaService : IAreaService
    {
        private readonly IAreaService areaServiceImpl = ServiceLocator.Instance.GetService<IAreaService>();

        public AreaMaintObject GetAreaById(Guid id)
        {
            try
            {
                return areaServiceImpl.GetAreaById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<AreaMaintObject> GetAreas()
        {
            try
            {
                return areaServiceImpl.GetAreas();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<AreaSelectObject> GetUsedAreas()
        {
            try
            {
                return areaServiceImpl.GetUsedAreas();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateArea(AreaMaintObject areaMaintObject)
        {
            try
            {
                areaServiceImpl.AddOrUpdateArea(areaMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveAreas(IList<AreaMaintObject> areaMaintObjects)
        {
            try
            {
                areaServiceImpl.RemoveAreas(areaMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAllAreas(int state)
        {
            try
            {
                return areaServiceImpl.GetAllAreas(state);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            areaServiceImpl.Dispose();
        }
    }
}
