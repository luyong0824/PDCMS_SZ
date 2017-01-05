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
    /// 地质勘探历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AddressExplorLogService : IAddressExplorLogService
    {
        private readonly IAddressExplorLogService addressExplorLogServiceImpl = ServiceLocator.Instance.GetService<IAddressExplorLogService>();

        public void AddOrUpdateAddressExplorLog(AddressExplorMaintObject addressExplorMaintObject)
        {
            try
            {
                addressExplorLogServiceImpl.AddOrUpdateAddressExplorLog(addressExplorMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAddressExplorLog(int propertyType, Guid parentId)
        {
            try
            {
                return addressExplorLogServiceImpl.GetAddressExplorLog(propertyType, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            addressExplorLogServiceImpl.Dispose();
        }
    }
}
