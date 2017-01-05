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
    /// 计量单位分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class UnitService : IUnitService
    {
        private readonly IUnitService unitServiceImpl = ServiceLocator.Instance.GetService<IUnitService>();

        public UnitMaintObject GetUnitById(Guid id)
        {
            try
            {
                return unitServiceImpl.GetUnitById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<UnitMaintObject> GetUnits()
        {
            try
            {
                return unitServiceImpl.GetUnits();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<UnitMaintObject> GetUsedUnits()
        {
            try
            {
                return unitServiceImpl.GetUsedUnits();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateUnit(UnitMaintObject unitMaintObject)
        {
            try
            {
                unitServiceImpl.AddOrUpdateUnit(unitMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveUnits(IList<UnitMaintObject> unitMaintObjects)
        {
            try
            {
                unitServiceImpl.RemoveUnits(unitMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            unitServiceImpl.Dispose();
        }
    }
}
