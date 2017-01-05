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
    /// 通知分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class NoticeService:INoticeService
    {
        private readonly INoticeService noticeServiceImpl = ServiceLocator.Instance.GetService<INoticeService>();

        public void AddOrUpdateNotice(NoticeMaintObject noticeMaintObject)
        {
            try
            {
                noticeServiceImpl.AddOrUpdateNotice(noticeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetNoticesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string noticeContent, int noticeState, Guid receiveUserId)
        {
            try
            {
                return noticeServiceImpl.GetNoticesPage(pageIndex, pageSize, beginDate, endDate, noticeContent, noticeState,receiveUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveNoticeState(NoticeMaintObject noticeMaintObject)
        {
            try
            {
                noticeServiceImpl.SaveNoticeState(noticeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveNoticeStates(IList<NoticeMaintObject> noticeMaintObjects)
        {
            try
            {
                noticeServiceImpl.SaveNoticeStates(noticeMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            noticeServiceImpl.Dispose();
        }
    }
}
