using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class NoticeService : DataService, INoticeService
    {
        private readonly IRepository<Notice> noticeRepository;

        public NoticeService(IRepositoryContext context,
            IRepository<Notice> noticeRepository)
            : base(context)
        {
            this.noticeRepository = noticeRepository;
        }

        /// <summary>
        /// 新增或者修改通知
        /// </summary>
        /// <param name="noticeMaintObject">要新增或者修改的通知维护对象</param>
        public void AddOrUpdateNotice(NoticeMaintObject noticeMaintObject)
        {
            if (noticeMaintObject.Id != Guid.Empty)
            {
                Notice notice = noticeRepository.FindByKey(noticeMaintObject.Id);
                if (notice != null)
                {
                    notice.Modify((NoticeState)noticeMaintObject.NoticeState, noticeMaintObject.ModifyUserId);
                    noticeRepository.Update(notice);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
        public string GetNoticesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string noticeContent, int noticeState, Guid receiveUserId)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "NoticeContent", Type = SqlDbType.NVarChar, Value = noticeContent });
            parameters.Add(new Parameter() { Name = "NoticeState", Type = SqlDbType.Int, Value = noticeState });
            parameters.Add(new Parameter() { Name = "ReceiveUserId", Type = SqlDbType.UniqueIdentifier, Value = receiveUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryNoticesPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 标记为已阅
        /// </summary>
        /// <param name="noticeMaintObject">通知维护对象</param>
        public void SaveNoticeState(NoticeMaintObject noticeMaintObject)
        {
            Notice notice = noticeRepository.FindByKey(noticeMaintObject.Id);
            if (notice != null)
            {
                notice.Modify(NoticeState.已阅, noticeMaintObject.ModifyUserId);
                noticeRepository.Update(notice);
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量标记为已阅
        /// </summary>
        /// <param name="noticeMaintObjects">通知维护对象</param>
        public void SaveNoticeStates(IList<NoticeMaintObject> noticeMaintObjects)
        {
            foreach (NoticeMaintObject noticeMaintObject in noticeMaintObjects)
            {
                Notice notice = noticeRepository.FindByKey(noticeMaintObject.Id);
                if (notice != null)
                {
                    notice.Modify(NoticeState.已阅, noticeMaintObject.ModifyUserId);
                    noticeRepository.Update(notice);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
