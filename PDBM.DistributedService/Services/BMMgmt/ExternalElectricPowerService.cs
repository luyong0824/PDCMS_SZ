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
    /// 外电引入分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ExternalElectricPowerService : IExternalElectricPowerService
    {
        private readonly IExternalElectricPowerService externalElectricPowerServiceImpl = ServiceLocator.Instance.GetService<IExternalElectricPowerService>();

        public ExternalElectricPowerMaintObject GetExternalElectricPowerById(Guid id)
        {
            try
            {
                return externalElectricPowerServiceImpl.GetExternalElectricPowerById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateExternalElectricPower(ExternalElectricPowerMaintObject externalElectricPowerMaintObject)
        {
            try
            {
                externalElectricPowerServiceImpl.AddOrUpdateExternalElectricPower(externalElectricPowerMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveExternalElectricPower(IList<ExternalElectricPowerMaintObject> externalElectricPowerMaintObjects)
        {
            try
            {
                externalElectricPowerServiceImpl.RemoveExternalElectricPower(externalElectricPowerMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            externalElectricPowerServiceImpl.Dispose();
        }
    }
}
