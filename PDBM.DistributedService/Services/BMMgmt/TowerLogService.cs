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
    /// 铁塔历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TowerLogService : ITowerLogService
    {
        private readonly ITowerLogService towerLogServiceImpl = ServiceLocator.Instance.GetService<ITowerLogService>();

        public void AddOrUpdateTowerLog(TowerMaintObject towerMaintObject)
        {
            try
            {
                towerLogServiceImpl.AddOrUpdateTowerLog(towerMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetTowerLog(int propertyType, Guid parentId)
        {
            try
            {
                return towerLogServiceImpl.GetTowerLog(propertyType, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            towerLogServiceImpl.Dispose();
        }
    }
}
