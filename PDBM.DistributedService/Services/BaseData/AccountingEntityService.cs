using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 会计主体分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AccountingEntityService : IAccountingEntityService
    {
        private readonly IAccountingEntityService accountingEntityServiceImpl = ServiceLocator.Instance.GetService<IAccountingEntityService>();

        public AccountingEntityMaintObject GetAccountingEntityById(Guid id)
        {
            try
            {
                return accountingEntityServiceImpl.GetAccountingEntityById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<AccountingEntityMaintObject> GetAccountingEntitys()
        {
            try
            {
                return accountingEntityServiceImpl.GetAccountingEntitys();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<AccountingEntitySelectObject> GetUsedAccountingEntitys()
        {
            try
            {
                return accountingEntityServiceImpl.GetUsedAccountingEntitys();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateAccountingEntity(AccountingEntityMaintObject accountingEntityMaintObject)
        {
            try
            {
                accountingEntityServiceImpl.AddOrUpdateAccountingEntity(accountingEntityMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveAccountingEntitys(IList<AccountingEntityMaintObject> accountingEntityMaintObjects)
        {
            try
            {
                accountingEntityServiceImpl.RemoveAccountingEntitys(accountingEntityMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            accountingEntityServiceImpl.Dispose();
        }
    }
}
