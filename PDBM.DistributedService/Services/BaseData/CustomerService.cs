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
    /// 往来单位分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerService customerServiceImpl = ServiceLocator.Instance.GetService<ICustomerService>();

        public CustomerMaintObject GetCustomerById(Guid id)
        {
            try
            {
                return customerServiceImpl.GetCustomerById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetCustomersPage(int pageIndex, int pageSize, int customerType, string customerCode, string customerName, string customerFullName, int state)
        {
            try
            {
                return customerServiceImpl.GetCustomersPage(pageIndex, pageSize, customerType, customerCode, customerName, customerFullName, state);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateCustomer(CustomerMaintObject customerMaintObject)
        {
            try
            {
                customerServiceImpl.AddOrUpdateCustomer(customerMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveCustomers(IList<CustomerMaintObject> customerMaintObjects)
        {
            try
            {
                customerServiceImpl.RemoveCustomers(customerMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetCustomersPageBySelect(int pageIndex, int pageSize, string customerCode, string customerName, string customerFullName, int customerType)
        {
            try
            {
                return customerServiceImpl.GetCustomersPageBySelect(pageIndex, pageSize, customerCode, customerName, customerFullName, customerType);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<CustomerMaintObject> GetCustomers()
        {
            try
            {
                return customerServiceImpl.GetCustomers();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAllUsedCustomers()
        {
            try
            {
                return customerServiceImpl.GetAllUsedCustomers();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<CustomerMaintObject> GetCustomersByType(int customerType)
        {
            try
            {
                return customerServiceImpl.GetCustomersByType(customerType);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<CustomerMaintObject> GetCustomersByAll()
        {
            try
            {
                return customerServiceImpl.GetCustomersByAll();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public CustomerMaintObject GetCustomerByUserId(Guid userId)
        {
            try
            {
                return customerServiceImpl.GetCustomerByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            customerServiceImpl.Dispose();
        }
    }
}
