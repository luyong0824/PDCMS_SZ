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
    /// 铁塔基础历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TowerBaseLogService : ITowerBaseLogService
    {
        private readonly ITowerBaseLogService towerBaseLogServiceImpl = ServiceLocator.Instance.GetService<ITowerBaseLogService>();

        public void AddOrUpdateTowerBaseLog(TowerBaseMaintObject towerBaseMaintObject)
        {
            try
            {
                towerBaseLogServiceImpl.AddOrUpdateTowerBaseLog(towerBaseMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetTowerBaseLog(int propertyType, Guid parentId)
        {
            try
            {
                return towerBaseLogServiceImpl.GetTowerBaseLog(propertyType, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            towerBaseLogServiceImpl.Dispose();
        }
    }
}
