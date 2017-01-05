using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 物资名称实体
    /// </summary>
    public class Material : AggregateRoot
    {
        protected Material()
        { 
        }

        /// <summary>
        /// 构造物资名称实体
        /// </summary>
        /// <param name="materialCode">物资编码</param>
        /// <param name="materialName">物资名称</param>
        /// <param name="materialCategoryId">物资类别Id</param>
        /// <param name="state">物资状态</param>
        /// <param name="createUserId">创建人Id</param>
        internal Material(string materialCode, string materialName, Guid materialCategoryId, State state, Guid createUserId)
        {
            materialCode.IsNullOrEmptyOrTooLong("物资编码", true, 50);
            materialName.IsNullOrEmptyOrTooLong("物资编码", true, 100);
            materialCategoryId.IsEmpty("物资类别Id");
            state.IsInvalid("物资状态");

            this.Id = Guid.NewGuid();
            this.MaterialCode = materialCode;
            this.MaterialName = materialName;
            this.MaterialCategoryId = materialCategoryId;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 物资编码
        /// </summary>
        public string MaterialCode
        {
            get;
            set;
        }

        /// <summary>
        /// 物资名称
        /// </summary>
        public string MaterialName
        {
            get;
            set;
        }

        /// <summary>
        /// 物资类别Id
        /// </summary>
        public Guid MaterialCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 物资状态
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
        /// 修改物资名称实体
        /// </summary>
        /// <param name="materialCode">物资编码</param>
        /// <param name="materialName">物资名称</param>
        /// <param name="state">物资状态</param>
        /// <param name="modifyUserId">修改人Id</param>
        public void Modify(string materialCode, string materialName, State state, Guid modifyUserId)
        {
            materialCode.IsNullOrEmptyOrTooLong("物资编码", true, 50);
            materialName.IsNullOrEmptyOrTooLong("物资名称", true, 100);
            state.IsInvalid("物资状态");

            this.MaterialCode = materialCode;
            this.MaterialName = materialName;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
