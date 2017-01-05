using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 通知实体
    /// </summary>
    public class Notice : AggregateRoot
    {
        protected Notice()
        { 
        }

        /// <summary>
        /// 构造通知实体
        /// </summary>
        /// <param name="noticeType">通知类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="receiveUserId">接收人用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Notice(NoticeType noticeType, Guid parentId, decimal lng, decimal lat, string noticeContent, Guid receiveUserId, Guid createUserId)
        {
            noticeType.IsInvalid("通知类型");
            noticeContent.IsNullOrEmptyOrTooLong("通知内容", true, 150);
            receiveUserId.IsEmpty("接收人用户Id");

            this.Id = Guid.NewGuid();
            this.NoticeType = noticeType;
            this.ParentId = parentId;
            this.Lng = lng;
            this.Lat = lat;
            this.NoticeContent = noticeContent;
            this.ReceiveUserId = receiveUserId;
            this.NoticeState = NoticeState.未阅;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 通知类型
        /// </summary>
        public NoticeType NoticeType
        {
            get;
            set;
        }

        /// <summary>
        /// 父表Id
        /// </summary>
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 原规划经度
        /// </summary>
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 原规划纬度
        /// </summary>
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string NoticeContent
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人用户Id
        /// </summary>
        public Guid ReceiveUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 通知状态
        /// </summary>
        public NoticeState NoticeState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改通知状态
        /// </summary>
        /// <param name="noticeState">通知状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(NoticeState noticeState, Guid modifyUserId)
        {
            noticeState.IsInvalid("通知状态");

            this.NoticeState = noticeState;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
