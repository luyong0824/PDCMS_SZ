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
    /// 桩基动测历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class FoundationTestLogService : IFoundationTestLogService
    {
        private readonly IFoundationTestLogService foundationTestLogServiceImpl = ServiceLocator.Instance.GetService<IFoundationTestLogService>();

        public void AddOrUpdateFoundationTestLog(FoundationTestMaintObject foundationTestMaintObject)
        {
            try
            {
                foundationTestLogServiceImpl.AddOrUpdateFoundationTestLog(foundationTestMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetFoundationTestLog(int propertyType, Guid parentId)
        {
            try
            {
                return foundationTestLogServiceImpl.GetFoundationTestLog(propertyType, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            foundationTestLogServiceImpl.Dispose();
        }
    }
}
