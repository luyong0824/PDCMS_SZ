using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    public class PlaceOwner : AggregateRoot
    {
        protected PlaceOwner()
        {
        }

        /// <summary>
        /// 构造产权实体
        /// </summary>
        /// <param name="placeOwnerCode">产权编码</param>
        /// <param name="placeOwnerName">产权名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">产权状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal PlaceOwner(string placeOwnerCode, string placeOwnerName, string remarks, State state, Guid createUserId)
        {
            placeOwnerCode.IsNullOrEmptyOrTooLong("产权编码", true, 50);
            placeOwnerName.IsNullOrEmptyOrTooLong("产权名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("产权状态");

            this.Id = Guid.NewGuid();
            this.PlaceOwnerCode = placeOwnerCode;
            this.PlaceOwnerName = placeOwnerName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 产权编码
        /// </summary>
        public string PlaceOwnerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 产权名称
        /// </summary>
        public string PlaceOwnerName
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
        /// 产权状态
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
        /// 修改产权实体
        /// </summary>
        /// <param name="placeOwnerCode">产权编码</param>
        /// <param name="placeOwnerName">产权名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">产权状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeOwnerCode, string placeOwnerName, string remarks, State state, Guid modifyUserId)
        {
            placeOwnerCode.IsNullOrEmptyOrTooLong("产权编码", true, 50);
            placeOwnerName.IsNullOrEmptyOrTooLong("产权名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("产权状态");

            this.PlaceOwnerCode = placeOwnerCode;
            this.PlaceOwnerName = placeOwnerName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
