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
    /// 地质勘探分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AddressExplorService : IAddressExplorService
    {
        private readonly IAddressExplorService addressExplorServiceImpl = ServiceLocator.Instance.GetService<IAddressExplorService>();

        public AddressExplorMaintObject GetAddressExplorById(Guid id)
        {
            try
            {
                return addressExplorServiceImpl.GetAddressExplorById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateAddressExplor(AddressExplorMaintObject addressExplorMaintObject)
        {
            try
            {
                addressExplorServiceImpl.AddOrUpdateAddressExplor(addressExplorMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveAddressExplor(IList<AddressExplorMaintObject> addressExplorMaintObjects)
        {
            try
            {
                addressExplorServiceImpl.RemoveAddressExplor(addressExplorMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            addressExplorServiceImpl.Dispose();
        }
    }
}
