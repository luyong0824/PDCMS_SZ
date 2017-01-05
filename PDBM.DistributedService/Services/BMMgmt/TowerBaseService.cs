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
    /// 铁塔基础分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TowerBaseService : ITowerBaseService
    {
        private readonly ITowerBaseService towerBaseServiceImpl = ServiceLocator.Instance.GetService<ITowerBaseService>();

        public TowerBaseMaintObject GetTowerBaseById(Guid id)
        {
            try
            {
                return towerBaseServiceImpl.GetTowerBaseById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateTowerBase(TowerBaseMaintObject towerBaseMaintObject)
        {
            try
            {
                towerBaseServiceImpl.AddOrUpdateTowerBase(towerBaseMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveTowerBase(IList<TowerBaseMaintObject> towerBaseMaintObjects)
        {
            try
            {
                towerBaseServiceImpl.RemoveTowerBase(towerBaseMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            towerBaseServiceImpl.Dispose();
        }
    }
}
