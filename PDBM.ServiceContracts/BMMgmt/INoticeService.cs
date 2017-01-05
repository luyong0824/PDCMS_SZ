using System;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 通知服务接口
    /// </summary>
    [ServiceContract]
    public interface INoticeService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改通知
        /// </summary>
        /// <param name="noticeMaintObject">要新增或者修改的通知维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateNotice(NoticeMaintObject noticeMaintObject);

        /// <summary>
        /// 获取分页待阅通知列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="noticeState">通知状态</param>
        /// <param name="receiveUserId">接收人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetNoticesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string noticeContent, int noticeState, Guid receiveUserId);

        /// <summary>
        /// 标记为已阅
        /// </summary>
        /// <param name="noticeMaintObject">通知维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveNoticeState(NoticeMaintObject noticeMaintObject);

        /// <summary>
        /// 批量标记为已阅
        /// </summary>
        /// <param name="noticeMaintObjects">通知维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveNoticeStates(IList<NoticeMaintObject> noticeMaintObjects);
    }
}
