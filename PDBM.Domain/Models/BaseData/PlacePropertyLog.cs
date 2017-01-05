using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 运营商使用情况历史记录实体
    /// </summary>
    public class PlacePropertyLog : AggregateRoot
    {
        protected PlacePropertyLog()
        {
        }

        /// <summary>
        /// 构造运营商使用情况历史记录实体
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="parentId">父级Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="companyNameId">运营商Id</param>
        /// <param name="mobileShare">移动共享</param>
        /// <param name="mobilePoleNumber">移动抱杆数量</param>
        /// <param name="mobileCabinetNumber">移动机柜数量</param>
        /// <param name="mobilePowerUsed">移动用电量</param>
        /// <param name="mobileCreateUserId">移动创建人Id</param>
        /// <param name="telecomShare">电信共享</param>
        /// <param name="telecomPoleNumber">电信抱杆数量</param>
        /// <param name="telecomCabinetNumber">电信机柜数量</param>
        /// <param name="telecomPowerUsed">电信用电量</param>
        /// <param name="telecomCreateUserId">电信创建人Id</param>
        /// <param name="unicomShare">联通共享</param>
        /// <param name="unicomPoleNumber">联通抱杆数量</param>
        /// <param name="unicomCabinetNumber">联通机柜数量</param>
        /// <param name="unicomPowerUsed">联通用电量</param>
        /// <param name="unicomCreateUserId">联通创建人Id</param>
        internal PlacePropertyLog(OperationType operationType, Guid parentId, PropertyType propertyType, CompanyNameId companyNameId, Bool mobileShare, int mobilePoleNumber, int mobileCabinetNumber, decimal mobilePowerUsed, Guid? mobileCreateUserId,
            Bool telecomShare, int telecomPoleNumber, int telecomCabinetNumber, decimal telecomPowerUsed, Guid? telecomCreateUserId, Bool unicomShare, int unicomPoleNumber, int unicomCabinetNumber,
            decimal unicomPowerUsed, Guid? unicomCreateUserId)
        {
            //placeId.IsEmpty("站点Id");
            //towerType.IsInvalid("铁塔类型");
            //machineRoomType.IsInvalid("机房类型");
            //externalElectric.IsInvalid("外电引入");
            //fireControl.IsInvalid("消防");

            this.Id = Guid.NewGuid();
            this.OperationType = operationType;
            this.ParentId = parentId;
            this.PropertyType = propertyType;
            this.CompanyNameId = companyNameId;
            this.MobileShare = mobileShare;
            this.MobilePoleNumber = mobilePoleNumber;
            this.MobileCabinetNumber = mobileCabinetNumber;
            this.MobilePowerUsed = mobilePowerUsed;
            this.MobileCreateUserId = mobileCreateUserId;
            this.TelecomShare = telecomShare;
            this.TelecomPoleNumber = telecomPoleNumber;
            this.TelecomCabinetNumber = telecomCabinetNumber;
            this.TelecomPowerUsed = telecomPowerUsed;
            this.TelecomCreateUserId = telecomCreateUserId;
            this.UnicomShare = unicomShare;
            this.UnicomPoleNumber = unicomPoleNumber;
            this.UnicomCabinetNumber = unicomCabinetNumber;
            this.UnicomPowerUsed = unicomPowerUsed;
            this.UnicomCreateUserId = unicomCreateUserId;
            this.CreateDate = DateTime.Now;
            this.MobileCreateDate = this.CreateDate;
            this.TelecomCreateDate = this.CreateDate;
            this.UnicomCreateDate = this.CreateDate;
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType
        {
            get;
            set;
        }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public PropertyType PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商Id
        /// </summary>
        public CompanyNameId CompanyNameId
        {
            get;
            set;
        }

        /// <summary>
        /// 移动共享
        /// </summary>
        public Bool MobileShare
        {
            get;
            set;
        }

        /// <summary>
        /// 移动抱杆数量
        /// </summary>
        public int MobilePoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量
        /// </summary>
        public int MobileCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动用电量
        /// </summary>
        public decimal MobilePowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记人
        /// </summary>
        public Guid? MobileCreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记日期
        /// </summary>
        public DateTime MobileCreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 电信共享
        /// </summary>
        public Bool TelecomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 电信抱杆数量
        /// </summary>
        public int TelecomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量
        /// </summary>
        public int TelecomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信用电量
        /// </summary>
        public decimal TelecomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记人
        /// </summary>
        public Guid? TelecomCreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记日期
        /// </summary>
        public DateTime TelecomCreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 联通共享
        /// </summary>
        public Bool UnicomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量
        /// </summary>
        public int UnicomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量
        /// </summary>
        public int UnicomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通用电量
        /// </summary>
        public decimal UnicomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记人
        /// </summary>
        public Guid? UnicomCreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记日期
        /// </summary>
        public DateTime UnicomCreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
