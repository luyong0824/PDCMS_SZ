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
    /// 往来单位用户分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CustomerUserService : ICustomerUserService
    {
        private readonly ICustomerUserService customerUserServiceImpl = ServiceLocator.Instance.GetService<ICustomerUserService>();

        public string GetCustomerUsers(Guid companyId, Guid departmentId, Guid customerId)
        {
            try
            {
                return customerUserServiceImpl.GetCustomerUsers(companyId, departmentId, customerId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrRemoveCustomerUsers(IList<CustomerUserMaintObject> customerUserMaintObjects)
        {
            try
            {
                customerUserServiceImpl.AddOrRemoveCustomerUsers(customerUserMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            customerUserServiceImpl.Dispose();
        }
    }
}
