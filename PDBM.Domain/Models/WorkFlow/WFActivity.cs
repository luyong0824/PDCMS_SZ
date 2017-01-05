using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.WorkFlow
{
    /// <summary>
    /// 工作流活动实体
    /// </summary>
    public class WFActivity : AggregateRoot
    {
        protected WFActivity()
        {
        }

        /// <summary>
        /// 构造工作流活动实体
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="wfActivityName">工作流活动名称</param>
        /// <param name="wfActivityOperate">工作流活动操作类型</param>
        /// <param name="wfActivityEditorId">工作流活动编辑器Id</param>
        /// <param name="wfActivityOrder">工作流活动顺序类型</param>
        /// <param name="serialId">序号</param>
        /// <param name="rowId">行号</param>
        /// <param name="timelimit">期限</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="postId">岗位Id</param>
        /// <param name="isMustEdit">是否必须编辑</param>
        /// <param name="isNecessaryStep">是否必要步骤</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WFActivity(Guid wfProcessId, string wfActivityName, WFActivityOperate wfActivityOperate, Guid wfActivityEditorId, WFActivityOrder wfActivityOrder,
            int serialId, int rowId, int timelimit, Guid companyId, Guid departmentId, Guid userId, Guid postId, Bool isMustEdit, Bool isNecessaryStep, Guid createUserId)
        {
            wfProcessId.IsEmpty("流程Id");
            wfActivityName.IsNullOrTooLong("步骤名称", true, 100);
            wfActivityOperate.IsInvalid("操作类型");
            if (wfActivityOperate == WFActivityOperate.单据编辑)
            {
                wfActivityEditorId.IsEmpty("编辑类型");
            }
            wfActivityOrder.IsInvalid("排序方式");
            companyId.IsEmpty("公司Id");

            this.Id = Guid.NewGuid();
            this.WFProcessId = wfProcessId;
            this.WFActivityName = wfActivityName;
            this.WFActivityOperate = wfActivityOperate;
            if (wfActivityOperate != WFActivityOperate.单据编辑)
            {
                this.WFActivityEditorId = null;
                this.IsMustEdit = Bool.否;
            }
            else
            {
                this.WFActivityEditorId = wfActivityEditorId;
                this.IsMustEdit = isMustEdit;
            }
            this.WFActivityOrder = wfActivityOrder;
            this.SerialId = serialId;
            this.RowId = rowId;
            this.Timelimit = timelimit;
            this.CompanyId = companyId;
            if (departmentId == Guid.Empty)
            {
                this.DepartmentId = null;
                this.UserId = null;
            }
            else
            {
                this.DepartmentId = departmentId;
                if (userId == Guid.Empty)
                {
                    this.UserId = null;
                }
                else
                {
                    this.UserId = userId;
                }
            }
            if (postId == Guid.Empty)
            {
                this.PostId = null;
            }
            else
            {
                this.PostId = postId;
            }
            this.IsNecessaryStep = isNecessaryStep;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 工作流过程Id
        /// </summary>
        public Guid WFProcessId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动名称
        /// </summary>
        public string WFActivityName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动操作类型
        /// </summary>
        public WFActivityOperate WFActivityOperate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器Id
        /// </summary>
        public Guid? WFActivityEditorId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动顺序类型
        /// </summary>
        public WFActivityOrder WFActivityOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialId
        {
            get;
            set;
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowId
        {
            get;
            set;
        }

        /// <summary>
        /// 期限(小时)
        /// </summary>
        public int Timelimit
        {
            get;
            set;
        }

        /// <summary>
        /// 批阅公司Id
        /// </summary>
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 批阅部门Id
        /// </summary>
        public Guid? DepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 批阅用户Id
        /// </summary>
        public Guid? UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 批阅岗位Id
        /// </summary>
        public Guid? PostId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必要步骤
        /// </summary>
        public Bool IsNecessaryStep
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必须编辑
        /// </summary>
        public Bool IsMustEdit
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
        /// 工作流过程实体
        /// </summary>
        protected virtual WFProcess WFProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器实体
        /// </summary>
        protected virtual WFActivityEditor WFActivityEditor
        {
            get;
            set;
        }

        /// <summary>
        /// 公司实体
        /// </summary>
        protected virtual Company Company
        {
            get;
            set;
        }

        /// <summary>
        /// 部门实体
        /// </summary>
        protected virtual Department Department
        {
            get;
            set;
        }

        /// <summary>
        /// 用户实体
        /// </summary>
        protected virtual User User
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位实体
        /// </summary>
        protected virtual Post Post
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改工作流活动实体
        /// </summary>
        /// <param name="wfActivityName">工作流活动名称</param>
        /// <param name="wfActivityOperate">工作流活动操作类型</param>
        /// <param name="wfActivityEditorId">工作流活动编辑器Id</param>
        /// <param name="wfActivityOrder">工作流活动顺序类型</param>
        /// <param name="serialId">序号</param>
        /// <param name="rowId">行号</param>
        /// <param name="timelimit">期限</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="postId">岗位Id</param>
        /// <param name="isMustEdit">是否必须编辑</param>
        /// <param name="isNecessaryStep">是否必要步骤</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string wfActivityName, WFActivityOperate wfActivityOperate, Guid wfActivityEditorId, WFActivityOrder wfActivityOrder,
            int serialId, int rowId, int timelimit, Guid companyId, Guid departmentId, Guid userId, Guid postId, Bool isMustEdit, Bool isNecessaryStep, Guid modifyUserId)
        {
            wfActivityName.IsNullOrTooLong("步骤名称", true, 100);
            wfActivityOperate.IsInvalid("操作类型");
            if (wfActivityOperate == WFActivityOperate.单据编辑)
            {
                wfActivityEditorId.IsEmpty("编辑类型");
            }
            wfActivityOrder.IsInvalid("排序方式");
            companyId.IsEmpty("公司Id");

            this.WFActivityName = wfActivityName;
            this.WFActivityOperate = wfActivityOperate;
            if (wfActivityOperate != WFActivityOperate.单据编辑)
            {
                this.WFActivityEditorId = null;
                this.IsMustEdit = Bool.否;
            }
            else
            {
                this.WFActivityEditorId = wfActivityEditorId;
                this.IsMustEdit = isMustEdit;
            }
            this.WFActivityOrder = wfActivityOrder;
            this.SerialId = serialId;
            this.RowId = rowId;
            this.Timelimit = timelimit;
            this.CompanyId = companyId;
            if (departmentId == Guid.Empty)
            {
                this.DepartmentId = null;
                this.UserId = null;
            }
            else
            {
                this.DepartmentId = departmentId;
                if (userId == Guid.Empty)
                {
                    this.UserId = null;
                }
                else
                {
                    this.UserId = userId;
                }
            }
            if (postId == Guid.Empty)
            {
                this.PostId = null;
            }
            else
            {
                this.PostId = postId;
            }
            this.IsNecessaryStep = isNecessaryStep;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改序号
        /// </summary>
        /// <param name="serialId">序号</param>
        public void ModifySerialId(int serialId)
        {
            this.SerialId = serialId;
        }

        /// <summary>
        /// 修改行号
        /// </summary>
        /// <param name="rowId">行号</param>
        public void ModifyRowId(int rowId)
        {
            this.RowId = rowId;
        }
    }
}
