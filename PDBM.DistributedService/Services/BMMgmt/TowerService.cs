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
    /// 铁塔资源分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TowerService : ITowerService
    {
        private readonly ITowerService towerServiceImpl = ServiceLocator.Instance.GetService<ITowerService>();

        public TowerMaintObject GetTowerById(Guid id)
        {
            try
            {
                return towerServiceImpl.GetTowerById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateTower(TowerMaintObject towerMaintObject)
        {
            try
            {
                towerServiceImpl.AddOrUpdateTower(towerMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveTower(IList<TowerMaintObject> towerMaintObjects)
        {
            try
            {
                towerServiceImpl.RemoveTower(towerMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            towerServiceImpl.Dispose();
        }
    }
}
