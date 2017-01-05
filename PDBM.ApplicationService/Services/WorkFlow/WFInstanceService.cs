using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.WorkFlow;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.ApplicationService.Services.WorkFlow
{
    /// <summary>
    /// 工作流实例应用层服务
    /// </summary>
    public class WFInstanceService : DataService, IWFInstanceService
    {
        private readonly IRepository<WFCategory> wfCategoryRepository;
        private readonly IRepository<WFProcess> wfProcessRepository;
        private readonly IRepository<WFProcessInstance> wfProcessInstanceRepository;
        private readonly IRepository<WFActivityInstance> wfActivityInstanceRepository;
        private readonly IRepository<WFActivityEditor> wfActivityEditorRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<PlanningApplyHeader> planningApplyHeaderRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;

        public WFInstanceService(IRepositoryContext context,
            IRepository<WFCategory> wfCategoryRepository,
            IRepository<WFProcess> wfProcessRepository,
            IRepository<WFProcessInstance> wfProcessInstanceRepository,
            IRepository<WFActivityInstance> wfActivityInstanceRepository,
            IRepository<WFActivityEditor> wfActivityEditorRepository,
            IRepository<User> userRepository,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<PlanningApplyHeader> planningApplyHeaderRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<Place> placeRepository,
            IRepository<WorkApply> workApplyRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository)
            : base(context)
        {
            this.wfCategoryRepository = wfCategoryRepository;
            this.wfProcessRepository = wfProcessRepository;
            this.wfProcessInstanceRepository = wfProcessInstanceRepository;
            this.wfActivityInstanceRepository = wfActivityInstanceRepository;
            this.wfActivityEditorRepository = wfActivityEditorRepository;
            this.userRepository = userRepository;
            this.planningApplyRepository = planningApplyRepository;
            this.planningApplyHeaderRepository = planningApplyHeaderRepository;
            this.addressingRepository = addressingRepository;
            this.remodelingRepository = remodelingRepository;
            this.placeRepository = placeRepository;
            this.workApplyRepository = workApplyRepository;
            this.workOrderRepository = workOrderRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
        }

        /// <summary>
        /// 根据工作流活动实例Id获取工作流活动实例
        /// </summary>
        /// <param name="id">工作流活动实例Id</param>
        /// <returns>工作流活动实例选择对象</returns>
        public WFActivityInstanceSelectObject GetWFActivityInstanceById(Guid id)
        {
            WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(id);
            if (wfActivityInstance != null)
            {
                WFActivityInstanceSelectObject wfActivityInstanceSelectObject = MapperHelper.Map<WFActivityInstance, WFActivityInstanceSelectObject>(wfActivityInstance);
                WFProcessInstance wfProcessInstance = wfProcessInstanceRepository.GetByKey(wfActivityInstance.WFProcessInstanceId);
                WFProcess wfProcess = wfProcessRepository.GetByKey(wfProcessInstance.WFProcessId);
                WFCategory wfCategory = wfCategoryRepository.GetByKey(wfProcess.WFCategoryId);

                if (wfActivityInstance.WFActivityEditorId != null)
                {
                    WFActivityEditor wfActivityEditor = wfActivityEditorRepository.FindByKey(wfActivityInstance.WFActivityEditorId.Value);
                    wfActivityInstanceSelectObject.EditorUrl = wfActivityEditor.EditorUrl;
                }
                else
                {
                    wfActivityInstanceSelectObject.EditorUrl = "";
                }
                //IList<WFActivityEditorSelectObject> wfActivityEditorSelectObjects = new List<WFActivityEditorSelectObject>();
                //IEnumerable<WFActivityEditor> wfActivityEditors = wfActivityEditorRepository.FindAll(Specification<WFActivityEditor>.Eval(entity => entity.Id == wfActivityInstance.WFActivityEditorId && entity.State == State.使用), "WFActivityEditorCode");
                //if (wfActivityEditors != null)
                //{
                //    foreach (var wfActivityEditor in wfActivityEditors)
                //    {
                //        wfActivityEditorSelectObjects.Add(MapperHelper.Map<WFActivityEditor, WFActivityEditorSelectObject>(wfActivityEditor));
                //    }
                //}

                User user = userRepository.GetByKey(wfProcessInstance.CreateUserId);
                wfActivityInstanceSelectObject.WFCategoryId = wfProcess.WFCategoryId;
                wfActivityInstanceSelectObject.WFProcessName = wfProcess.WFProcessName;
                wfActivityInstanceSelectObject.WFProcessInstanceCode = wfProcessInstance.WFProcessInstanceCode;
                wfActivityInstanceSelectObject.WFProcessInstanceName = wfProcessInstance.WFProcessInstanceName;
                wfActivityInstanceSelectObject.FullName = user.FullName;
                wfActivityInstanceSelectObject.CreateDate = wfProcessInstance.CreateDate.ToShortDateString();
                wfActivityInstanceSelectObject.WFProcessInstanceContent = wfProcessInstance.Content;
                wfActivityInstanceSelectObject.WFProcessInstanceId = wfProcessInstance.Id;
                wfActivityInstanceSelectObject.PrintUrl = wfCategory.PrintUrl + "/" + wfProcessInstance.EntityId.ToString();
                wfActivityInstanceSelectObject.EntityId = wfProcessInstance.EntityId;
                wfActivityInstanceSelectObject.WFActivityInstanceId = id;
                wfActivityInstanceSelectObject.WFActivityInstanceState = (int)wfActivityInstance.WFActivityInstanceState;
                wfActivityInstanceSelectObject.WFActivityEditorId = wfActivityInstance.WFActivityEditorId;
                return wfActivityInstanceSelectObject;
            }
            else
            {
                throw new ApplicationFault("选择的公文处理在系统中不存在");
            }
        }

        /// <summary>
        /// 发送工作流实例
        /// </summary>
        /// <param name="wfProcessInstanceSendObject">要发送的工作流过程实例发送对象</param>
        /// <param name="wfActivityInstanceSendObjects">要发送的工作流活动实例发送对象列表</param>
        public void SendWFInstance(WFProcessInstanceSendObject wfProcessInstanceSendObject, IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects)
        {
            WFProcess wfProcess = wfProcessRepository.FindByKey(wfProcessInstanceSendObject.WFProcessId);
            if (wfProcess == null)
            {
                throw new ApplicationFault("选择的流程在系统中不存在");
            }
            if (wfActivityInstanceSendObjects.Count == 0)
            {
                throw new ApplicationFault("请添加流程步骤");
            }
            if (wfActivityInstanceSendObjects.Where(o => o.WFActivityOperate != (int)WFActivityOperate.阅).Count() == 0)
            {
                throw new ApplicationFault("流程步骤中的操作类型不能都为\"阅\"");
            }

            WFCategory wfCategory = wfCategoryRepository.GetByKey(wfProcess.WFCategoryId);
            WFProcessInstance wfProcessInstance = AggregateFactory.CreateWFProcessInstance(wfProcessInstanceSendObject.WFProcessId, wfProcessInstanceSendObject.EntityId,
                orderCodeSeedRepository.GenerateOrderCode(wfCategory.EntityName, DateTime.Now, (int)wfCategory.Profession), wfProcessInstanceSendObject.WFProcessInstanceName, wfProcessInstanceSendObject.Content,
                wfProcessInstanceSendObject.CreateUserId);
            wfProcessInstance.Start();
            wfProcessInstanceRepository.Add(wfProcessInstance);

            if (wfProcessInstanceSendObject.FileIdList != "")
            {
                FileAssociation fileAssociation = AggregateFactory.CreateFileAssociation("WFProcessInstance", wfProcessInstance.Id, wfProcessInstanceSendObject.FileIdList, wfProcessInstance.CreateUserId);
                fileAssociationRepository.Add(fileAssociation);
            }

            int serialId = 1;
            bool started = false;
            bool hadNonRead = false;
            var wfActivityInstanceSendOrderedObjects = from el in wfActivityInstanceSendObjects orderby el.RowId select el;
            for (int i = 0; i < wfActivityInstanceSendOrderedObjects.Count(); i++)
            {
                var wfActivityInstanceSendObject = wfActivityInstanceSendOrderedObjects.ElementAt(i);
                WFActivityOrder previousWFActivityOrder = i == 0 ? WFActivityOrder.并发 : (WFActivityOrder)wfActivityInstanceSendOrderedObjects.ElementAt(i - 1).WFActivityOrder;
                if (previousWFActivityOrder == WFActivityOrder.顺序)
                {
                    serialId++;
                }
                WFActivityInstance wfActivityInstance = AggregateFactory.CreateWFActivityInstance(wfProcessInstance.Id, wfActivityInstanceSendObject.WFActivityInstanceName, (WFActivityOperate)wfActivityInstanceSendObject.WFActivityOperate,
                    wfActivityInstanceSendObject.WFActivityEditorId, (WFActivityOrder)wfActivityInstanceSendObject.WFActivityOrder, serialId, wfActivityInstanceSendObject.RowId, wfActivityInstanceSendObject.Timelimit, wfActivityInstanceSendObject.UserId,
                    (Bool)wfActivityInstanceSendObject.IsMustEdit, wfProcessInstanceSendObject.CreateUserId);
                if (!hadNonRead)
                {
                    hadNonRead = wfActivityInstance.WFActivityOperate != WFActivityOperate.阅;
                }
                if (!started)
                {
                    wfActivityInstance.Start();
                    if (wfActivityInstance.WFActivityOrder == WFActivityOrder.顺序)
                    {
                        if (wfActivityInstance.WFActivityOperate != WFActivityOperate.阅)
                        {
                            started = true;
                        }
                        else
                        {
                            if (hadNonRead)
                            {
                                started = true;
                            }
                        }
                    }
                }
                wfActivityInstanceRepository.Add(wfActivityInstance);
            }

            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_WFProcessInstance_dbo.tbl_WFProcess_WFProcessId"))
                {
                    throw new ApplicationFault("选择的流程在系统中不存在");
                }
                else if (ex.Message.Contains("IX_UQ_WFProcessInstanceCode"))
                {
                    throw new ApplicationFault("单据编码重复，请重新点击发送");
                }
                else if (ex.Message.Contains("IX_UQ_WFProcessInstance_EntityId"))
                {
                    throw new ApplicationFault("选择的单据已经发送过流程");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivityInstance_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("流程步骤中的用户在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivityInstance_dbo.tbl_WFActivityEditor_WFActivityEditorId"))
                {
                    throw new ApplicationFault("流程步骤中的编辑类型在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 处理工作流实例
        /// </summary>
        /// <param name="wfActivityInstanceDoObject">要处理的工作流活动实例处理对象</param>
        /// <param name="wfActivityInstanceSendObjects">要发送的工作流活动实例发送对象列表</param>
        public void DoWFInstance(WFActivityInstanceDoObject wfActivityInstanceDoObject, IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects)
        {
            WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.GetByKey(wfActivityInstanceDoObject.Id);
            IList<WFActivityInstance> forwardWFActivityInstances = new List<WFActivityInstance>();
            if (wfActivityInstanceDoObject.WFActivityInstanceFlow == 2)
            {
                foreach (WFActivityInstanceSendObject wfActivityInstanceSendObject in wfActivityInstanceSendObjects)
                {
                    forwardWFActivityInstances.Add(AggregateFactory.CreateWFActivityInstance(wfActivityInstance.WFProcessInstanceId, wfActivityInstanceSendObject.WFActivityInstanceName, (WFActivityOperate)wfActivityInstanceSendObject.WFActivityOperate,
                        wfActivityInstanceSendObject.WFActivityEditorId, WFActivityOrder.并发, wfActivityInstance.SerialId, wfActivityInstanceSendObject.RowId, 24, wfActivityInstanceSendObject.UserId, (Bool)wfActivityInstanceSendObject.IsMustEdit, wfActivityInstance.UserId));
                }
            }
            wfActivityInstance.Do((WFActivityInstanceFlow)wfActivityInstanceDoObject.WFActivityInstanceFlow, wfActivityInstanceDoObject.Content, forwardWFActivityInstances);
            wfActivityInstanceRepository.Update(wfActivityInstance);
            if (wfActivityInstanceDoObject.FileIdList != "")
            {
                FileAssociation fileAssociation = AggregateFactory.CreateFileAssociation("WFActivityInstance", wfActivityInstance.Id, wfActivityInstanceDoObject.FileIdList, wfActivityInstance.UserId);
                fileAssociationRepository.Add(fileAssociation);
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlaceCode"))
                {
                    throw new ApplicationFault("站点编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_PlaceName"))
                {
                    throw new ApplicationFault("站点名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户待办工作流实例列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户待办工作流实例列表的Json字符串</returns>
        public string GetWFInstancesToDo(Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetWFInstancesToDo", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取用户待办任务工作流实例列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户待办工作流实例列表的Json字符串</returns>
        public string GetTaskToDo(Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetTaskToDo", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取用户待办任务工作流实例列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户待办工作流实例列表的Json字符串</returns>
        public string GetTaskToDoMobile(Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetTaskToDoMobile", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取报表列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>报表列表的Json字符串</returns>
        public string GetReports(Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetReports", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据工作流过程实例Id获取工作流活动实例列表
        /// </summary>
        /// <param name="wfProcessInstanceId">工作流过程实例Id</param>
        /// <returns>工作流活动实例列表的Json字符串</returns>
        public string GetWFActivityInstances(Guid wfProcessInstanceId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "WFProcessInstanceId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessInstanceId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetWFActivityInstances", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据条件获取分页工作流过程实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="wfProcessInstanceState">工作流过程实例状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页工作流过程实例列表的Json字符串</returns>
        public string GetWFProcessInstancesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, int wfProcessInstanceState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = wfProcessInstanceCode });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceName", Type = SqlDbType.NVarChar, Value = wfProcessInstanceName });
            parameters.Add(new Parameter() { Name = "WFProcessId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessId });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceState", Type = SqlDbType.Int, Value = wfProcessInstanceState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWFProcessInstancesPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页待处理工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>分页待处理工作流实例列表的Json字符串</returns>
        public string GetWFInstancesDoingPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = wfProcessInstanceCode });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceName", Type = SqlDbType.NVarChar, Value = wfProcessInstanceName });
            parameters.Add(new Parameter() { Name = "WFProcessId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessId });
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWFInstancesDoingPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页已处理工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>分页已处理工作流实例列表的Json字符串</returns>
        public string GetWFInstancesDoedPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = wfProcessInstanceCode });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceName", Type = SqlDbType.NVarChar, Value = wfProcessInstanceName });
            parameters.Add(new Parameter() { Name = "WFProcessId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessId });
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWFInstancesDoedPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页已发送(待处理)工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页已发送(待处理)工作流实例列表的Json字符串</returns>
        public string GetWFInstancesSendedToDoingPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = wfProcessInstanceCode });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceName", Type = SqlDbType.NVarChar, Value = wfProcessInstanceName });
            parameters.Add(new Parameter() { Name = "WFProcessId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessId });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWFInstancesSendedToDoingPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页已发送(已处理)工作流实例列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="wfProcessInstanceState">工作流过程实例状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页已发送(已处理)工作流实例列表的Json字符串</returns>
        public string GetWFInstancesSendedToDoedPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string wfProcessInstanceCode, string wfProcessInstanceName, Guid wfProcessId, int wfProcessInstanceState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = wfProcessInstanceCode });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceName", Type = SqlDbType.NVarChar, Value = wfProcessInstanceName });
            parameters.Add(new Parameter() { Name = "WFProcessId", Type = SqlDbType.UniqueIdentifier, Value = wfProcessId });
            parameters.Add(new Parameter() { Name = "WFProcessInstanceState", Type = SqlDbType.Int, Value = wfProcessInstanceState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWFInstancesSendedToDoedPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 获取公文标题
        /// </summary>
        /// <param name="wfCategoryId">公文类型Id</param>
        /// <param name="entityId">发送实体Id</param>
        /// <returns></returns>
        public string GetWFProcessInstanceName(Guid wfCategoryId, Guid entityId)
        {
            string wfProcessInstanceName = "";
            WFCategory wfCategory = wfCategoryRepository.FindByKey(wfCategoryId);
            if (wfCategory != null)
            {
                if (wfCategory.EntityName == "PlanningApplyHeader")
                {
                    PlanningApplyHeader planningApplyHeader = planningApplyHeaderRepository.FindByKey(entityId);
                    wfProcessInstanceName = planningApplyHeader.Title;
                }
                else if (wfCategory.EntityName == "Addressing")
                {
                    Addressing addressing = addressingRepository.FindByKey(entityId);
                    wfProcessInstanceName = addressing.PlaceName + wfCategory.WFCategoryName + "审批";
                }
                else if (wfCategory.EntityName == "Remodeling")
                {
                    Remodeling remodeling = remodelingRepository.FindByKey(entityId);
                    Place place = placeRepository.FindByKey(remodeling.PlaceId);
                    wfProcessInstanceName = place.PlaceName + wfCategory.WFCategoryName + "审批";
                }
                else if (wfCategory.EntityName == "WorkApply")
                {
                    WorkApply workApply = workApplyRepository.FindByKey(entityId);
                    wfProcessInstanceName = workApply.Title;
                }
                else if (wfCategory.EntityName == "WorkOrder")
                {
                    WorkOrder workOrder = workOrderRepository.FindByKey(entityId);
                    wfProcessInstanceName = workOrder.Title;
                }
                else if (wfCategory.EntityName == "AddressingID")
                {
                    Addressing addressing = addressingRepository.FindByKey(entityId);
                    wfProcessInstanceName = addressing.PlaceName + wfCategory.WFCategoryName + "审批";
                }
            }
            return wfProcessInstanceName;
        }
    }
}
