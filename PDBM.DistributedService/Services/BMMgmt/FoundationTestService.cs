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
    /// 桩基动测分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class FoundationTestService : IFoundationTestService
    {
        private readonly IFoundationTestService foundationTestServiceImpl = ServiceLocator.Instance.GetService<IFoundationTestService>();

        public FoundationTestMaintObject GetFoundationTestById(Guid id)
        {
            try
            {
                return foundationTestServiceImpl.GetFoundationTestById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateFoundationTest(FoundationTestMaintObject foundationTestMaintObject)
        {
            try
            {
                foundationTestServiceImpl.AddOrUpdateFoundationTest(foundationTestMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveFoundationTest(IList<FoundationTestMaintObject> foundationTestMaintObjects)
        {
            try
            {
                foundationTestServiceImpl.RemoveFoundationTest(foundationTestMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            foundationTestServiceImpl.Dispose();
        }
    }
}
