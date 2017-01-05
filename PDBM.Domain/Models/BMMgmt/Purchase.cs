using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 购置站点实体
    /// </summary>
    public class Purchase : AggregateRoot
    {
        protected Purchase()
        {
        }

        /// <summary>
        /// 构造购置站点实体
        /// </summary>
        /// <param name="purchaseDate">购置日期</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="propertyRight">产权</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="sceneId">周边场景Id</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="ownerName">业主名称</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="telecomShare">是否电信共享</param>
        /// <param name="mobileShare">是否移动共享</param>
        /// <param name="unicomShare">是否联通共享</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Purchase(DateTime purchaseDate, string groupPlaceCode, string placeName, Profession profession, Guid placeCategoryId, Guid reseauId,
            decimal lng, decimal lat, PropertyRight propertyRight, Importance importance, Guid sceneId, string detailedAddress,
            string ownerName, string ownerContact, string ownerPhoneNumber, Bool telecomShare, Bool mobileShare, Bool unicomShare,
            string remarks, Guid createUserId)
        {
            groupPlaceCode.IsNullOrEmptyOrTooLong("站点编码", true, 50);
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 100);
            profession.IsInvalid("专业");
            placeCategoryId.IsEmpty("站点类型Id");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            propertyRight.IsInvalid("产权");
            //if (propertyRight == Enum.PropertyRight.铁塔)
            //{
            //    throw new DomainFault("产权无效");
            //}
            importance.IsInvalid("重要性程度");
            sceneId.IsEmpty("周边场景Id");
            detailedAddress.IsNullOrEmptyOrTooLong("详细地址", true, 150);
            ownerName.IsNullOrTooLong("业主名称", true, 100);
            ownerContact.IsNullOrTooLong("业主联系人", true, 100);
            ownerPhoneNumber.IsNullOrTooLong("业主联系电话", true, 100);
            telecomShare.IsInvalid("是否电信共享");
            mobileShare.IsInvalid("是否移动共享");
            unicomShare.IsInvalid("是否联通共享");
            remarks.IsNullOrTooLong("备注", true, 150);

            this.Id = Guid.NewGuid();
            this.OrderCode = "";
            this.PurchaseDate = purchaseDate;
            this.PlaceCode = "";
            this.GroupPlaceCode = groupPlaceCode;
            this.PlaceName = placeName;
            this.Profession = profession;
            this.PlaceCategoryId = placeCategoryId;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.PropertyRight = propertyRight;
            this.Importance = importance;
            this.SceneId = sceneId;
            this.DetailedAddress = detailedAddress;
            this.OwnerName = ownerName;
            this.OwnerContact = ownerContact;
            this.OwnerPhoneNumber = ownerPhoneNumber;
            this.TelecomShare = telecomShare;
            this.MobileShare = mobileShare;
            this.UnicomShare = unicomShare;
            this.Remarks = remarks;
            this.PlaceId = Guid.Empty;
            this.OrderState = WFProcessInstanceState.流程通过;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 购置单编码
        /// </summary>
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 购置日期
        /// </summary>
        public DateTime PurchaseDate
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        public string PlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        public string GroupPlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        public Profession Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        public Guid ReseauId
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
        /// 产权
        /// </summary>
        public PropertyRight PropertyRight
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        public Importance Importance
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景Id
        /// </summary>
        public Guid SceneId
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系人
        /// </summary>
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系电话
        /// </summary>
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 是否电信共享
        /// </summary>
        public Bool TelecomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 是否移动共享
        /// </summary>
        public Bool MobileShare
        {
            get;
            set;
        }

        /// <summary>
        /// 是否联通共享
        /// </summary>
        public Bool UnicomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 购置单状态
        /// </summary>
        public WFProcessInstanceState OrderState
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

        #region Relations
        /// <summary>
        /// 站点类型实体
        /// </summary>
        protected virtual PlaceCategory PlaceCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 网格实体
        /// </summary>
        protected virtual Reseau Reseau
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景实体
        /// </summary>
        protected virtual Scene Scene
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改购置站点实体
        /// </summary>
        /// <param name="purchaseDate">购置日期</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="placeCategoryId">站点类型</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="propertyRight">产权</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="sceneId">周边场景Id</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="ownerName">业主名称</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="telecomShare">是否电信共享</param>
        /// <param name="mobileShare">是否移动共享</param>
        /// <param name="unicomShare">是否联通共享</param>
        /// <param name="remarks">备注</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(DateTime purchaseDate, string groupPlaceCode, string placeName, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat,
            PropertyRight propertyRight, Importance importance, Guid sceneId, string detailedAddress,
            string ownerName, string ownerContact, string ownerPhoneNumber, Bool telecomShare, Bool mobileShare,
            Bool unicomShare, string remarks, Guid modifyUserId)
        {
            groupPlaceCode.IsNullOrEmptyOrTooLong("站点编码", true, 50);
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 100);
            placeCategoryId.IsEmpty("站点类型Id");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            propertyRight.IsInvalid("产权");
            //if (propertyRight == Enum.PropertyRight.铁塔)
            //{
            //    throw new DomainFault("产权无效");
            //}
            importance.IsInvalid("重要性程度");
            sceneId.IsEmpty("周边场景Id");
            detailedAddress.IsNullOrEmptyOrTooLong("详细地址", true, 150);
            ownerName.IsNullOrTooLong("业主名称", true, 100);
            ownerContact.IsNullOrTooLong("业主联系人", true, 100);
            ownerPhoneNumber.IsNullOrTooLong("业主联系电话", true, 100);
            telecomShare.IsInvalid("是否电信共享");
            mobileShare.IsInvalid("是否移动共享");
            unicomShare.IsInvalid("是否联通共享");
            remarks.IsNullOrTooLong("备注", true, 150);

            this.PurchaseDate = purchaseDate;
            this.GroupPlaceCode = groupPlaceCode;
            this.PlaceName = placeName;
            this.PlaceCategoryId = placeCategoryId;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.PropertyRight = propertyRight;
            this.Importance = importance;
            this.SceneId = sceneId;
            this.DetailedAddress = detailedAddress;
            this.OwnerName = ownerName;
            this.OwnerContact = ownerContact;
            this.OwnerPhoneNumber = ownerPhoneNumber;
            this.TelecomShare = telecomShare;
            this.MobileShare = mobileShare;
            this.UnicomShare = unicomShare;
            this.Remarks = remarks;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改购置站点检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的购置站点");
            }
        }

        /// <summary>
        /// 删除购置站点检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByRemove(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能删除别人创建的购置站点", this.PlaceCode);
            }
        }
    }
}
