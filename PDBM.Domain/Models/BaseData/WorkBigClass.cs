using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    public class WorkBigClass : AggregateRoot
    {
        /// <summary>
        /// 派工大类实体
        /// </summary>
        protected WorkBigClass()
        {
        }

        /// <summary>
        /// 构造派工大类实体
        /// </summary>
        /// <param name="bigClassCode">大类编码</param>
        /// <param name="bigClassName">大类名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">大类状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WorkBigClass(string bigClassCode, string bigClassName, string remarks, State state, Guid createUserId)
        {
            bigClassCode.IsNullOrEmptyOrTooLong("大类编码", true, 50);
            bigClassName.IsNullOrEmptyOrTooLong("大类名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("大类状态");

            this.Id = Guid.NewGuid();
            this.BigClassCode = bigClassCode;
            this.BigClassName = bigClassName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 大类编码
        /// </summary>
        public string BigClassCode
        {
            get;
            set;
        }

        /// <summary>
        /// 大类名称
        /// </summary>
        public string BigClassName
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
        /// 大类状态
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
        /// 修改派工大类实体
        /// </summary>
        /// <param name="bigClassCode">大类编码</param>
        /// <param name="bigClassName">大类名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">大类状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string bigClassCode, string bigClassName, string remarks, State state, Guid modifyUserId)
        {
            bigClassCode.IsNullOrEmptyOrTooLong("区域编码", true, 50);
            bigClassName.IsNullOrEmptyOrTooLong("区域名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("大类状态");

            this.BigClassCode = bigClassCode;
            this.BigClassName = bigClassName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
