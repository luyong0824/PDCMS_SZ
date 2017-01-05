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
    /// 区域实体
    /// </summary>
    public class Area : AggregateRoot
    {
        protected Area()
        {
        }

        /// <summary>
        /// 构造区域实体
        /// </summary>
        /// <param name="areaCode">区域编码</param>
        /// <param name="areaName">区域名称</param>
        /// <param name="lng">参考经度</param>
        /// <param name="lat">参考纬度</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">区域状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Area(string areaCode, string areaName, decimal lng, decimal lat, Guid? areaManagerId, string remarks, State state, Guid createUserId)
        {
            areaCode.IsNullOrEmptyOrTooLong("区域编码", true, 50);
            areaName.IsNullOrEmptyOrTooLong("区域名称", true, 100);
            lng.IsNonnegative("参考经度");
            lat.IsNonnegative("参考纬度");
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("区域状态");

            this.Id = Guid.NewGuid();
            this.AreaCode = areaCode;
            this.AreaName = areaName;
            this.Lng = lng;
            this.Lat = lat;
            this.AreaManagerId = areaManagerId;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 区域编码
        /// </summary>
        public string AreaCode
        {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 参考经度
        /// </summary>
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 参考纬度
        /// </summary>
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        public Guid? AreaManagerId
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
        /// 区域状态
        /// </summary>
        public State State
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
        /// 网格实体列表
        /// </summary>
        protected virtual ICollection<Reseau> Reseaus
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商规划实体列表
        /// </summary>
        protected virtual ICollection<OperatorsPlanning> OperatorsPlannings
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改区域实体
        /// </summary>
        /// <param name="areaCode">区域编码</param>
        /// <param name="areaName">区域名称</param>
        /// <param name="lng">参考经度</param>
        /// <param name="lat">参考纬度</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">区域状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string areaCode, string areaName, decimal lng, decimal lat, Guid? areaManagerId, string remarks, State state, Guid modifyUserId)
        {
            areaCode.IsNullOrEmptyOrTooLong("区域编码", true, 50);
            areaName.IsNullOrEmptyOrTooLong("区域名称", true, 100);
            lng.IsNonnegative("参考经度");
            lat.IsNonnegative("参考纬度");;
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("区域状态");

            this.AreaCode = areaCode;
            this.AreaName = areaName;
            this.Lng = lng;
            this.Lat = lat;
            this.AreaManagerId = areaManagerId;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
