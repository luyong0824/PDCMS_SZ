using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 物资类别实体
    /// </summary>
    public class MaterialCategory : AggregateRoot
    {
        protected MaterialCategory()
        { 
        }

        /// <summary>
        /// 构造物资类别实体
        /// </summary>
        /// <param name="materialCategoryCode">类别编码</param>
        /// <param name="materialCategoryName">类别名称</param>
        /// <param name="state">类别状态</param>
        /// <param name="createUserId">创建人Id</param>
        internal MaterialCategory(string materialCategoryCode,string materialCategoryName, State state, Guid createUserId)
        {
            materialCategoryCode.IsNullOrEmptyOrTooLong("类别编码", true, 50);
            materialCategoryName.IsNullOrEmptyOrTooLong("类别名称", true, 100);
            state.IsInvalid("类别状态");

            this.Id = Guid.NewGuid();
            this.MaterialCategoryCode = materialCategoryCode;
            this.MaterialCategoryName = materialCategoryName;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 物资类别编码
        /// </summary>
        public string MaterialCategoryCode
        {
            get;
            set;
        }

        /// <summary>
        /// 物资类别名称
        /// </summary>
        public string MaterialCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 类别状态
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
        /// 修改物资类别实体
        /// </summary>
        /// <param name="materialCategoryCode">类别编码</param>
        /// <param name="materialCategoryName">类别名称</param>
        /// <param name="state">类别状态</param>
        /// <param name="modifyUserId">修改人Id</param>
        public void Modify(string materialCategoryCode, string materialCategoryName, State state, Guid modifyUserId)
        {
            materialCategoryCode.IsNullOrEmptyOrTooLong("类别编码", true, 50);
            materialCategoryName.IsNullOrEmptyOrTooLong("类别名称", true, 100);
            state.IsInvalid("类别状态");

            this.MaterialCategoryCode = materialCategoryCode;
            this.MaterialCategoryName = materialCategoryName;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
