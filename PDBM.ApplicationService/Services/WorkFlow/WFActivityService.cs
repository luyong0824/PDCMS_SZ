using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Domain.Models;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.WorkFlow;

namespace PDBM.ApplicationService.Services.WorkFlow
{
    /// <summary>
    /// 工作流活动应用层服务
    /// </summary>
    public class WFActivityService : DataService, IWFActivityService
    {
        private readonly IRepository<WFActivity> wfActivityRepository;

        public WFActivityService(IRepositoryContext context,
            IRepository<WFActivity> wfActivityRepository)
            : base(context)
        {
            this.wfActivityRepository = wfActivityRepository;
        }

        /// <summary>
        /// 根据工作流活动Id获取工作流活动
        /// </summary>
        /// <param name="id">工作流活动Id</param>
        /// <returns>工作流活动维护对象</returns>
        public WFActivityMaintObject GetWFActivityById(Guid id)
        {
            WFActivity wfActivity = wfActivityRepository.FindByKey(id);
            if (wfActivity != null)
            {
                WFActivityMaintObject wfActivityMaintObject = MapperHelper.Map<WFActivity, WFActivityMaintObject>(wfActivity);
                return wfActivityMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的流程步骤在系统中不存在");
            }
        }

        /// <summary>
        /// 根据工作流过程Id获取工作流活动列表
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <returns>工作流活动列表的Json字符串</returns>
        public string GetWFActivitys(Guid wfProcessId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "WFProcessId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryWFActivitys", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据工作流过程Id获取工作流活动列表，用于发送工作流实例
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="entityId">发送单据实体Id</param>
        /// <returns>工作流活动列表的Json字符串</returns>
        public string GetWFActivitysBySend(Guid wfProcessId, Guid userId, Guid entityId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "WFProcessId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessId });
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            parameters.Add(new Parameter() { Name = "EntityId", Type = SqlDbType.UniqueIdentifier, Value = entityId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetWFActivitysBySend", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者修改工作流活动
        /// </summary>
        /// <param name="wfActivityMaintObjects">要新增或者修改的工作流活动维护对象列表</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        public void AddOrUpdateWFActivitys(IList<WFActivityMaintObject> wfActivityMaintObjects, Guid wfProcessId)
        {
            IEnumerable<WFActivity> wfActivitys = wfActivityRepository.FindAll(Specification<WFActivity>.Eval(entity => entity.WFProcessId == wfProcessId));
            List<WFActivity> wfActivityAddedList = wfActivitys.ToList();
            List<WFActivity> wfActivityOrderList = wfActivitys.ToList();

            foreach (WFActivityMaintObject wfActivityMaintObject in wfActivityMaintObjects)
            {
                if (wfActivityMaintObject.Id == Guid.Empty)
                {
                    WFActivity wfActivity = AggregateFactory.CreateWFActivity(wfActivityMaintObject.WFProcessId, wfActivityMaintObject.WFActivityName, (WFActivityOperate)wfActivityMaintObject.WFActivityOperate, wfActivityMaintObject.WFActivityEditorId,
                        (WFActivityOrder)wfActivityMaintObject.WFActivityOrder, wfActivityMaintObject.SerialId, wfActivityMaintObject.RowId, wfActivityMaintObject.Timelimit, wfActivityMaintObject.CompanyId, wfActivityMaintObject.DepartmentId,
                        wfActivityMaintObject.UserId, wfActivityMaintObject.PostId, (Bool)wfActivityMaintObject.IsMustEdit, (Bool)wfActivityMaintObject.IsNecessaryStep, wfActivityMaintObject.CreateUserId);
                    wfActivityOrderList.Add(wfActivity);
                }
                else
                {
                    WFActivity wfActivity = wfActivityRepository.FindByKey(wfActivityMaintObject.Id);
                    if (wfActivity != null)
                    {
                        wfActivityOrderList.Find(entity => entity.Id == wfActivity.Id).Modify(wfActivityMaintObject.WFActivityName, (WFActivityOperate)wfActivityMaintObject.WFActivityOperate, wfActivityMaintObject.WFActivityEditorId, (WFActivityOrder)wfActivityMaintObject.WFActivityOrder,
                            wfActivityMaintObject.SerialId, wfActivityMaintObject.RowId, wfActivityMaintObject.Timelimit, wfActivityMaintObject.CompanyId, wfActivityMaintObject.DepartmentId,
                            wfActivityMaintObject.UserId, wfActivityMaintObject.PostId, (Bool)wfActivityMaintObject.IsMustEdit, (Bool)wfActivityMaintObject.IsNecessaryStep, wfActivityMaintObject.ModifyUserId);
                    }
                }
            }

            int serialId = 1;
            var wfActivityOrderedList = from el in wfActivityOrderList orderby el.SerialId, el.RowId, el.CreateDate select el;
            for (int i = 0; i < wfActivityOrderedList.Count(); i++)
            {
                WFActivity currentWFActivity = wfActivityOrderedList.ElementAt(i);
                WFActivity previousWFActivity = i == 0 ? null : wfActivityOrderedList.ElementAt(i - 1);

                currentWFActivity.ModifyRowId(i + 1);
                if (i == 0)
                {
                    currentWFActivity.ModifySerialId(1);
                }
                else
                {
                    if (previousWFActivity.WFActivityOrder == WFActivityOrder.顺序)
                    {
                        serialId++;
                    }
                    currentWFActivity.ModifySerialId(serialId);
                }

                if (wfActivityAddedList.Find(entity => entity.Id == currentWFActivity.Id) != null)
                {
                    wfActivityRepository.Update(currentWFActivity);
                }
                else
                {
                    wfActivityRepository.Add(currentWFActivity);
                }
            }

            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_WFProcess_WFProcessId"))
                {
                    throw new ApplicationFault("选择的流程在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_Company_CompanyId"))
                {
                    throw new ApplicationFault("选择的公司在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_Department_DepartmentId"))
                {
                    throw new ApplicationFault("选择的部门在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("选择的用户在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_Post_PostId"))
                {
                    throw new ApplicationFault("选择的岗位在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_WFActivityEditor_WFActivityEditorId"))
                {
                    throw new ApplicationFault("选择的编辑类型在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除工作流活动
        /// </summary>
        /// <param name="wfActivityMaintObjects">要删除的工作流活动维护对象列表</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        public void RemoveWFActivitys(IList<WFActivityMaintObject> wfActivityMaintObjects, Guid wfProcessId)
        {
            IEnumerable<WFActivity> wfActivitys = wfActivityRepository.FindAll(Specification<WFActivity>.Eval(entity => entity.WFProcessId == wfProcessId));
            List<WFActivity> wfActivityOrderList = wfActivitys.ToList();

            foreach (WFActivityMaintObject wfActivityMaintObject in wfActivityMaintObjects)
            {
                WFActivity wfActivity = wfActivityRepository.FindByKey(wfActivityMaintObject.Id);
                if (wfActivity != null)
                {
                    wfActivityOrderList.Remove(wfActivityOrderList.Find(entity => entity.Id == wfActivity.Id));
                    wfActivityRepository.Remove(wfActivity);
                }
            }

            int serialId = 1;
            var wfActivityOrderedList = from el in wfActivityOrderList orderby el.SerialId, el.RowId, el.CreateDate select el;
            for (int i = 0; i < wfActivityOrderedList.Count(); i++)
            {
                WFActivity currentWFActivity = wfActivityOrderedList.ElementAt(i);
                WFActivity previousWFActivity = i == 0 ? null : wfActivityOrderedList.ElementAt(i - 1);

                currentWFActivity.ModifyRowId(i + 1);
                if (i == 0)
                {
                    currentWFActivity.ModifySerialId(1);
                }
                else
                {
                    if (previousWFActivity.WFActivityOrder == WFActivityOrder.顺序)
                    {
                        serialId++;
                    }
                    currentWFActivity.ModifySerialId(serialId);
                }
                wfActivityRepository.Update(currentWFActivity);
            }

            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
