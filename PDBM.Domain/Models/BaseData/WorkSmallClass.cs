using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 派工小类实体
    /// </summary>
    public class WorkSmallClass : AggregateRoot
    {
        protected WorkSmallClass()
        { 
        }

        /// <summary>
        /// 构造派工小类实体
        /// </summary>
        /// <param name="bigClassId">大类Id</param>
        /// <param name="smallClassCode">小类编码</param>
        /// <param name="smallClassName">小类名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">小类状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WorkSmallClass(Guid workBigClassId, string smallClassCode, string smallClassName, string remarks, State state, Guid createUserId)
        {
            workBigClassId.IsEmpty("派工大类Id");
            smallClassCode.IsNullOrEmptyOrTooLong("小类编码", true, 50);
            smallClassName.IsNullOrEmptyOrTooLong("小类名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("小类状态");

            this.Id = Guid.NewGuid();
            this.WorkBigClassId = workBigClassId;
            this.SmallClassCode = smallClassCode;
            this.SmallClassName = smallClassName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 大类Id
        /// </summary>
        public Guid WorkBigClassId
        {
            get;
            set;
        }

        /// <summary>
        /// 小类编码
        /// </summary>
        public string SmallClassCode
        {
            get;
            set;
        }

        /// <summary>
        /// 小类名称
        /// </summary>
        public string SmallClassName
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
        /// 小类状态
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
        /// 修改派工小类实体
        /// </summary>
        /// <param name="smallClassCode">小类编码</param>
        /// <param name="smallClassName">小类名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">小类状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string smallClassCode, string smallClassName, string remarks, State state, Guid modifyUserId)
        {
            smallClassCode.IsNullOrEmptyOrTooLong("小类编码", true, 50);
            smallClassName.IsNullOrEmptyOrTooLong("小类名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("小类状态");

            this.SmallClassCode = smallClassCode;
            this.SmallClassName = smallClassName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
