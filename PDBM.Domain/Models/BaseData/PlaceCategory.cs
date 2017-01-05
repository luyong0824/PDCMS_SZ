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
    /// 站点类型实体
    /// </summary>
    public class PlaceCategory : AggregateRoot
    {
        protected PlaceCategory()
        {
        }

        /// <summary>
        /// 构造站点类型实体
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryCode">站点类型编码</param>
        /// <param name="placeCategoryName">站点类型名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">站点类型状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal PlaceCategory(Profession profession, string placeCategoryCode, string placeCategoryName, string remarks, State state, Guid createUserId)
        {
            profession.IsInvalid("专业");
            placeCategoryCode.IsNullOrEmptyOrTooLong("站点类型编码", true, 50);
            placeCategoryName.IsNullOrEmptyOrTooLong("站点类型名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("站点类型状态");

            this.Id = Guid.NewGuid();
            this.Profession = profession;
            this.PlaceCategoryCode = placeCategoryCode;
            this.PlaceCategoryName = placeCategoryName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
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
        /// 站点类型编码
        /// </summary>
        public string PlaceCategoryCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型名称
        /// </summary>
        public string PlaceCategoryName
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
        /// 站点类型状态
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
        /// 站点实体列表
        /// </summary>
        protected virtual ICollection<Place> Places
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

        /// <summary>
        /// 规划站点实体列表
        /// </summary>
        protected virtual ICollection<Planning> Plannings
        {
            get;
            set;
        }

        /// <summary>
        /// 购置站点实体列表
        /// </summary>
        protected virtual ICollection<Purchase> Purchases
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改站点类型实体
        /// </summary>
        /// <param name="placeCategoryCode">站点类型编码</param>
        /// <param name="placeCategoryName">站点类型名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">站点类型状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeCategoryCode, string placeCategoryName, string remarks, State state, Guid modifyUserId)
        {
            placeCategoryCode.IsNullOrEmptyOrTooLong("站点类型编码", true, 50);
            placeCategoryName.IsNullOrEmptyOrTooLong("站点类型名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("站点类型状态");

            this.PlaceCategoryCode = placeCategoryCode;
            this.PlaceCategoryName = placeCategoryName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
