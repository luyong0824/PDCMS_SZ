using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 站点实体
    /// </summary>
    public class Place : AggregateRoot
    {
        protected Place()
        {
        }

        /// <summary>
        /// 构造站点实体
        /// </summary>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="addressingDepartmentId">租赁部门</param>
        /// <param name="addressingRealName">租赁人</param>
        /// <param name="ownerName">业主名称</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="placeMapState">站点地图状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Place(string placeCode, string placeName, Profession profession, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat, Guid placeOwner, Importance importance,
            Guid addressingDepartmentId, string addressingRealName, string ownerName, string ownerContact, string ownerPhoneNumber, string detailedAddress, string remarks, PlaceMapState placeMapState,
            Guid createUserId)
        {
            placeCode.IsNullOrEmptyOrTooLong("站点编码", true, 50);
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 100);
            profession.IsInvalid("专业");
            placeCategoryId.IsEmpty("站点类型Id");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            placeOwner.IsEmpty("产权");
            importance.IsInvalid("重要性程度");
            addressingDepartmentId.IsEmpty("租赁部门Id");
            addressingRealName.IsNullOrTooLong("实际租赁人", true, 50);
            ownerName.IsNullOrTooLong("业主名称", true, 100);
            ownerContact.IsNullOrTooLong("业主联系人", true, 100);
            ownerPhoneNumber.IsNullOrTooLong("业主联系电话", true, 100);
            detailedAddress.IsNullOrEmptyOrTooLong("详细地址", true, 150);
            remarks.IsNullOrTooLong("备注", true, 150);
            placeMapState.IsInvalid("站点地图状态");

            this.Id = Guid.NewGuid();
            this.PlaceCode = placeCode;
            this.PlaceName = placeName;
            this.Profession = profession;
            this.PlaceCategoryId = placeCategoryId;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.PlaceOwner = placeOwner;
            this.Importance = importance;
            this.AddressingDepartmentId = addressingDepartmentId;
            this.AddressingRealName = addressingRealName;
            this.OwnerName = ownerName;
            this.OwnerContact = ownerContact;
            this.OwnerPhoneNumber = ownerPhoneNumber;
            this.DetailedAddress = detailedAddress;
            this.Remarks = remarks;
            this.State = State.使用;
            this.G2Number = "";
            this.D2Number = "";
            this.G3Number = "";
            this.G4Number = "";
            this.G5Number = "";
            this.PlaceMapState = placeMapState;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
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
        public Guid PlaceOwner
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
        /// 租赁部门
        /// </summary>
        public Guid AddressingDepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 实际租赁人
        /// </summary>
        public string AddressingRealName
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
        /// 详细地址
        /// </summary>
        public string DetailedAddress
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
        /// 站点状态
        /// </summary>
        public State State
        {
            get;
            set;
        }

        /// <summary>
        /// 2G逻辑号
        /// </summary>
        public string G2Number
        {
            get;
            set;
        }

        /// <summary>
        /// 2D逻辑号
        /// </summary>
        public string D2Number
        {
            get;
            set;
        }

        /// <summary>
        /// 3G逻辑号
        /// </summary>
        public string G3Number
        {
            get;
            set;
        }

        /// <summary>
        /// 4G逻辑号
        /// </summary>
        public string G4Number
        {
            get;
            set;
        }

        /// <summary>
        /// 5G逻辑号
        /// </summary>
        public string G5Number
        {
            get;
            set;
        }

        /// <summary>
        /// 站点地图状态
        /// </summary>
        public PlaceMapState PlaceMapState
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
        #endregion

        /// <summary>
        /// 修改站点实体
        /// </summary>
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
        /// <param name="state">站点状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeName, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat, Guid placeOwner, Importance importance, string addressingRealName,
             string ownerName, string ownerContact, string ownerPhoneNumber, string detailedAddress, string remarks, State state, string g2Number, string d2Number, string g3Number,
            string g4Number, string g5Number, Guid modifyUserId)
        {
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 100);
            placeCategoryId.IsEmpty("站点类型Id");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            placeOwner.IsEmpty("产权");
            importance.IsInvalid("重要性程度");
            ownerName.IsNullOrTooLong("业主名称", true, 100);
            ownerContact.IsNullOrTooLong("业主联系人", true, 100);
            ownerPhoneNumber.IsNullOrTooLong("业主联系电话", true, 100);
            detailedAddress.IsNullOrEmptyOrTooLong("详细地址", true, 150);

            this.PlaceName = placeName;
            this.PlaceCategoryId = placeCategoryId;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.PlaceOwner = placeOwner;
            this.Importance = importance;
            this.AddressingRealName = addressingRealName;
            this.OwnerName = ownerName;
            this.OwnerContact = ownerContact;
            this.OwnerPhoneNumber = ownerPhoneNumber;
            this.DetailedAddress = detailedAddress;
            this.Remarks = remarks;
            this.State = state;
            this.G2Number = g2Number;
            this.D2Number = d2Number;
            this.G3Number = g3Number;
            this.G4Number = g4Number;
            this.G5Number = g5Number;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        public void OperatorShared(int companyMark, Bool isShare)
        {
            //if (companyMark == 1)
            //{
            //    this.MobileShare = isShare;
            //}
            //else if (companyMark == 2)
            //{
            //    this.TelecomShare = isShare;
            //}
            //else if (companyMark == 3)
            //{
            //    this.UnicomShare = isShare;
            //}
        }

        /// <summary>
        /// 修改逻辑号
        /// </summary>
        /// <param name="g2Number">2G逻辑号</param>
        /// <param name="d2Number">2D逻辑号</param>
        /// <param name="g3Number">3G逻辑号</param>
        /// <param name="g4Number">4G逻辑号</param>
        /// <param name="g5Number">5G逻辑号</param>
        public void ModifyLogicalNumber(string g2Number, string d2Number, string g3Number, string g4Number, string g5Number)
        {
            this.G2Number = g2Number;
            this.D2Number = d2Number;
            this.G3Number = g3Number;
            this.G4Number = g4Number;
            this.G5Number = g5Number;
        }

        /// <summary>
        /// 逻辑号维护
        /// </summary>
        /// <param name="g2Number">2G逻辑号</param>
        /// <param name="d2Number">2D逻辑号</param>
        /// <param name="g3Number">3G逻辑号</param>
        /// <param name="g4Number">4G逻辑号</param>
        public void LogicalNumberMaintain(string g2Number, string d2Number, string g3Number, string g4Number)
        {
            this.G2Number = g2Number;
            this.D2Number = d2Number;
            this.G3Number = g3Number;
            this.G4Number = g4Number;
        }

        /// <summary>
        /// 修改2G逻辑号
        /// </summary>
        /// <param name="g2Number">2G逻辑号</param>
        public void ModifyG2Number(string g2Number)
        {
            this.G2Number = g2Number;
        }

        /// <summary>
        /// 修改2D逻辑号
        /// </summary>
        /// <param name="d2Number">2D逻辑号</param>
        public void ModifyD2Number(string d2Number)
        {
            this.D2Number = d2Number;
        }

        /// <summary>
        /// 修改3G逻辑号
        /// </summary>
        /// <param name="g3Number">3G逻辑号</param>
        public void ModifyG3Number(string g3Number)
        {
            this.G3Number = g3Number;
        }

        /// <summary>
        /// 修改4G逻辑号
        /// </summary>
        /// <param name="g4Number">4G逻辑号</param>
        public void ModifyG4Number(string g4Number)
        {
            this.G4Number = g4Number;
        }

        /// <summary>
        /// 站点状态变更
        /// </summary>
        /// <param name="state">站点状态</param>
        public void ModifyState(State state)
        {
            this.State = state;
        }

        public void ModifyPlaceImport(string placeName, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat, Guid placeOwner, Importance importance,
            Guid addressingDepartmentId, string addressingRealName, string ownerName, string ownerContact, string ownerPhoneNumber, string detailedAddress, string remarks, State state, Guid modifyUserId)
        {
            placeName.IsNullOrEmptyOrTooLong("基站名称", true, 100);
            placeCategoryId.IsEmpty("基站类型Id");
            placeOwner.IsEmpty("产权");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            importance.IsInvalid("重要性程度");
            addressingDepartmentId.IsEmpty("租赁部门Id");
            ownerName.IsNullOrTooLong("业主名称", true, 100);
            addressingRealName.IsNullOrTooLong("实际租赁人", true, 50);
            ownerContact.IsNullOrTooLong("业主联系人", true, 100);
            ownerPhoneNumber.IsNullOrTooLong("业主联系电话", true, 100);
            detailedAddress.IsNullOrEmptyOrTooLong("详细地址", true, 150);
            remarks.IsNullOrTooLong("备注", true, 150);

            this.PlaceName = placeName;
            this.PlaceCategoryId = placeCategoryId;
            this.PlaceOwner = placeOwner;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.Importance = importance;
            this.AddressingDepartmentId = addressingDepartmentId;
            this.AddressingRealName = addressingRealName;
            this.OwnerName = ownerName;
            this.OwnerContact = ownerContact;
            this.OwnerPhoneNumber = ownerPhoneNumber;
            this.DetailedAddress = detailedAddress;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
