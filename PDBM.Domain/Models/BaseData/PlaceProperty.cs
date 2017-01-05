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
    /// 运营商使用情况实体
    /// </summary>
    public class PlaceProperty : AggregateRoot
    {
        protected PlaceProperty()
        {
        }

        /// <summary>
        /// 构造运营商使用情况实体
        /// </summary>
        /// <param name="parentId">父级Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="mobilePoleNumber">移动抱杆数量</param>
        /// <param name="mobileCabinetNumber">移动机柜数量</param>
        /// <param name="mobilePowerUsed">移动用电量</param>
        /// <param name="mobileCreateUserId">移动登记人</param>
        /// <param name="telecomPoleNumber">电信抱杆数量</param>
        /// <param name="telecomCabinetNumber">电信机柜数量</param>
        /// <param name="telecomPowerUsed">电信用电量</param>
        /// <param name="telecomCreateUserId">电信登记人</param>
        /// <param name="unicomPoleNumber">联通抱杆数量</param>
        /// <param name="unicomCabinetNumber">联通机柜数量</param>
        /// <param name="unicomPowerUsed">联通用电量</param>
        /// <param name="unicomCreateUserId">联通登记人</param>
        internal PlaceProperty(Guid parentId, PropertyType propertyType, Bool mobileShare, int mobilePoleNumber, int mobileCabinetNumber, decimal mobilePowerUsed, Guid? mobileCreateUserId,
            Bool telecomShare, int telecomPoleNumber, int telecomCabinetNumber, decimal telecomPowerUsed, Guid? telecomCreateUserId, Bool unicomShare, int unicomPoleNumber, int unicomCabinetNumber,
            decimal unicomPowerUsed, Guid? unicomCreateUserId)
        {
            //placeId.IsEmpty("站点Id");
            //towerType.IsInvalid("铁塔类型");
            //machineRoomType.IsInvalid("机房类型");
            //externalElectric.IsInvalid("外电引入");
            //fireControl.IsInvalid("消防");

            this.Id = Guid.NewGuid();
            this.ParentId = parentId;
            this.PropertyType = propertyType;
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

        /// <summary>
        /// 移动再次确认
        /// </summary>
        /// <param name="poleNumber">抱杆数量</param>
        /// <param name="cabinetNumber">机柜数量</param>
        /// <param name="powerUsed">用电量</param>
        /// <param name="createUserId">确认用户Id</param>
        public void ModifyMobile(Bool isShre, int poleNumber, int cabinetNumber, decimal powerUsed, Guid createUserId)
        {
            poleNumber.IsNonnegative("移动抱杆数量");
            cabinetNumber.IsNonnegative("移动机柜数量");
            powerUsed.IsNonnegative("移动用电量");
            this.MobileShare = isShre;
            this.MobilePoleNumber = poleNumber;
            this.MobileCabinetNumber = cabinetNumber;
            this.MobilePowerUsed = powerUsed;
            this.MobileCreateUserId = createUserId;
            this.MobileCreateDate = DateTime.Now;
        }

        /// <summary>
        /// 电信再次确认
        /// </summary>
        /// <param name="poleNumber">抱杆数量</param>
        /// <param name="cabinetNumber">机柜数量</param>
        /// <param name="powerUsed">用电量</param>
        /// <param name="createUserId">确认用户Id</param>
        public void ModifyTelecom(Bool isShre, int poleNumber, int cabinetNumber, decimal powerUsed, Guid createUserId)
        {
            poleNumber.IsNonnegative("电信抱杆数量");
            cabinetNumber.IsNonnegative("电信机柜数量");
            powerUsed.IsNonnegative("电信用电量");
            this.TelecomShare = isShre;
            this.TelecomPoleNumber = poleNumber;
            this.TelecomCabinetNumber = cabinetNumber;
            this.TelecomPowerUsed = powerUsed;
            this.TelecomCreateUserId = createUserId;
            this.TelecomCreateDate = DateTime.Now;
        }

        /// <summary>
        /// 联通再次确认
        /// </summary>
        /// <param name="poleNumber">抱杆数量</param>
        /// <param name="cabinetNumber">机柜数量</param>
        /// <param name="powerUsed">用电量</param>
        /// <param name="createUserId">确认用户Id</param>
        public void ModifyUnicom(Bool isShre, int poleNumber, int cabinetNumber, decimal powerUsed, Guid createUserId)
        {
            poleNumber.IsNonnegative("联通抱杆数量");
            cabinetNumber.IsNonnegative("联通机柜数量");
            powerUsed.IsNonnegative("联通用电量");
            this.UnicomShare = isShre;
            this.UnicomPoleNumber = poleNumber;
            this.UnicomCabinetNumber = cabinetNumber;
            this.UnicomPowerUsed = powerUsed;
            this.UnicomCreateUserId = createUserId;
            this.UnicomCreateDate = DateTime.Now;
        }
    }
}
