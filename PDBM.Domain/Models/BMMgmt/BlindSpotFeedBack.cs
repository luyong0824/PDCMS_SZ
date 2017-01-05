using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 盲点反馈
    /// </summary>
    public class BlindSpotFeedBack : AggregateRoot
    {
        protected BlindSpotFeedBack()
        { 
        }

        /// <summary>
        /// 构造盲点反馈实体
        /// </summary>
        /// <param name="placeName">盲点地名</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="feedBackContent">反馈内容</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal BlindSpotFeedBack(string placeName, Guid areaId, decimal lng, decimal lat, string feedBackContent,Guid createUserId)
        {
            placeName.IsNullOrTooLong("盲点地名", true, 100);
            feedBackContent.IsNullOrTooLong("反馈内容", true, 500);

            this.Id = Guid.NewGuid();
            this.PlaceName = placeName;
            this.AreaId = areaId;
            this.Lng = lng;
            this.Lat = lat;
            this.FeedBackContent = feedBackContent;
            this.DoUserId = Guid.Empty;
            this.DoState = DoState.未处理;
            this.FeedBackResult = "";
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 盲点地名
        /// </summary>
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string FeedBackContent
        {
            get;
            set;
        }

        /// <summary>
        /// 处理人
        /// </summary>
        public Guid DoUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 处理状态
        /// </summary>
        public DoState DoState
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈结果
        /// </summary>
        public string FeedBackResult
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
        /// 修改盲点反馈
        /// </summary>
        /// <param name="placeName">盲点地名</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="feedBackContent">反馈内容</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeName, Guid areaId, decimal lng, decimal lat, string feedBackContent, Guid modifyUserId)
        {
            feedBackContent.IsNullOrTooLong("反馈内容", true, 500);

            this.PlaceName = placeName;
            this.AreaId = areaId;
            this.Lng = lng;
            this.Lat = lat;
            this.FeedBackContent = feedBackContent;
            this.DoState = DoState.未处理;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 保存反馈处理
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="feedBackResult">反馈结果</param>
        /// <param name="modifyUserId">反馈处理人</param>
        public void SaveBlindSpotHanding(decimal lng, decimal lat, string feedBackResult, Guid modifyUserId)
        {
            feedBackResult.IsNullOrTooLong("反馈内容", true, 500);

            this.Lng = lng;
            this.Lat = lat;
            this.FeedBackResult = feedBackResult;
            this.DoState = DoState.已处理;
            this.DoUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
