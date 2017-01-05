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
    /// 职务用户分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class DutyUserService : IDutyUserService
    {
        private readonly IDutyUserService dutyUserServiceImpl = ServiceLocator.Instance.GetService<IDutyUserService>();

        public string GetDutyUsers(Guid companyId, Guid departmentId, int duty)
        {
            try
            {
                return dutyUserServiceImpl.GetDutyUsers(companyId, departmentId, duty);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrRemoveDutyUsers(IList<DutyUserMaintObject> dutyUserMaintObjects)
        {
            try
            {
                dutyUserServiceImpl.AddOrRemoveDutyUsers(dutyUserMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            dutyUserServiceImpl.Dispose();
        }
    }
}
