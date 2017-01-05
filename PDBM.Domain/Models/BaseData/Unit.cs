using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 计量单位实体
    /// </summary>
    public class Unit : AggregateRoot
    {
        protected Unit()
        { 
        }

        /// <summary>
        /// 构造计量单位实体
        /// </summary>
        /// <param name="unitName">单位名称</param>
        /// <param name="state">状态</param>
        /// <param name="createUserId">创建人Id</param>
        internal Unit(string unitName, State state, Guid createUserId)
        {
            unitName.IsNullOrEmptyOrTooLong("单位名称", true, 100);
            state.IsInvalid("区域状态");

            this.Id = Guid.NewGuid();
            this.UnitName = unitName;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 计量单位名称
        /// </summary>
        public string UnitName
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
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

        /// <summary>
        /// 修改计量单位实体
        /// </summary>
        /// <param name="unitName">单位名称</param>
        /// <param name="state">状态</param>
        /// <param name="modifyUserId">修改人Id</param>
        public void Modify(string unitName, State state, Guid modifyUserId)
        {
            unitName.IsNullOrEmptyOrTooLong("单位名称", true, 100);
            state.IsInvalid("状态");

            this.UnitName = unitName;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
