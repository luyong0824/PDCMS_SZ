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
    /// 外电引入历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ExternalElectricPowerLogService : IExternalElectricPowerLogService
    {
        private readonly IExternalElectricPowerLogService externalElectricPowerLogServiceImpl = ServiceLocator.Instance.GetService<IExternalElectricPowerLogService>();

        public void AddOrUpdateExternalElectricPowerLog(ExternalElectricPowerMaintObject externalElectricPowerMaintObject)
        {
            try
            {
                externalElectricPowerLogServiceImpl.AddOrUpdateExternalElectricPowerLog(externalElectricPowerMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetExternalElectricPowerLog(int propertyType, Guid parentId)
        {
            try
            {
                return externalElectricPowerLogServiceImpl.GetExternalElectricPowerLog(propertyType, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            externalElectricPowerLogServiceImpl.Dispose();
        }
    }
}
