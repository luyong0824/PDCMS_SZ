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
    /// 盲点反馈分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class BlindSpotFeedBackService : IBlindSpotFeedBackService
    {
        private readonly IBlindSpotFeedBackService blindSpotFeedBackServiceImpl = ServiceLocator.Instance.GetService<IBlindSpotFeedBackService>();

        public BlindSpotFeedBackMaintObject GetBlindSpotFeedBackById(Guid id)
        {
            try
            {
                return blindSpotFeedBackServiceImpl.GetBlindSpotFeedBackById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateBlindSpotFeedBack(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject)
        {
            try
            {
                blindSpotFeedBackServiceImpl.AddOrUpdateBlindSpotFeedBack(blindSpotFeedBackMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveBlindSpotFeedBacks(IList<BlindSpotFeedBackMaintObject> blindSpotFeedBackMaintObjects)
        {
            try
            {
                blindSpotFeedBackServiceImpl.RemoveBlindSpotFeedBacks(blindSpotFeedBackMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBlindSpotFeedBacksPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, Guid areaId, string placeName, int doState, Guid createUserId, Guid companyId)
        {
            try
            {
                return blindSpotFeedBackServiceImpl.GetBlindSpotFeedBacksPage(pageIndex, pageSize, beginDate, endDate, areaId, placeName, doState, createUserId, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveBlindSpotHanding(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject)
        {
            try
            {
                blindSpotFeedBackServiceImpl.SaveBlindSpotHanding(blindSpotFeedBackMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveBlindSpotFeedBackMobile(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject)
        {
            try
            {
                blindSpotFeedBackServiceImpl.SaveBlindSpotFeedBackMobile(blindSpotFeedBackMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }
        public void Dispose()
        {
            blindSpotFeedBackServiceImpl.Dispose();
        }
    }
}
