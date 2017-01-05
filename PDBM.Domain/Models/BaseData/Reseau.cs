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
    /// 网格实体
    /// </summary>
    public class Reseau : AggregateRoot
    {
        protected Reseau()
        {
        }

        /// <summary>
        /// 构造网格实体
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauCode">网格编码</param>
        /// <param name="reseauName">网格名称</param>
        /// <param name="reseauManagerId">网格经理</param>
        /// <param name="planningManagerId">规划经理</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">网格状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Reseau(Guid areaId, string reseauCode, string reseauName, Guid? reseauManagerId, Guid? planningManagerId, string remarks, State state, Guid createUserId)
        {
            areaId.IsEmpty("区域Id");
            reseauCode.IsNullOrEmptyOrTooLong("网格编码", true, 50);
            reseauName.IsNullOrEmptyOrTooLong("网格名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("网格状态");

            this.Id = Guid.NewGuid();
            this.AreaId = areaId;
            this.ReseauCode = reseauCode;
            this.ReseauName = reseauName;
            this.ReseauManagerId = reseauManagerId;
            this.PlanningManagerId = planningManagerId;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
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
        /// 网格编码
        /// </summary>
        public string ReseauCode
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格经理
        /// </summary>
        public Guid? ReseauManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划经理
        /// </summary>
        public Guid? PlanningManagerId
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
        /// 网格状态
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
        /// 区域实体
        /// </summary>
        protected virtual Area Area
        {
            get;
            set;
        }

        /// <summary>
        /// 站点实体列表
        /// </summary>
        protected virtual ICollection<Place> Places
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
        /// 修改网格实体
        /// </summary>
        /// <param name="reseauCode">网格编码</param>
        /// <param name="reseauName">网格名称</param>
        /// <param name="reseauManagerId">网格经理</param>
        /// <param name="planningManagerId">规划经理</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">网格状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string reseauCode, string reseauName, Guid? reseauManagerId, Guid? planningManagerId, string remarks, State state, Guid modifyUserId)
        {
            reseauCode.IsNullOrEmptyOrTooLong("网格编码", true, 50);
            reseauName.IsNullOrEmptyOrTooLong("网格名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("网格状态");

            this.ReseauCode = reseauCode;
            this.ReseauName = reseauName;
            this.ReseauManagerId = reseauManagerId;
            this.PlanningManagerId = planningManagerId;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
