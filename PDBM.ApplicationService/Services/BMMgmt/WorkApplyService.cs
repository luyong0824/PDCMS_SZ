using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 隐患上报应用层接口
    /// </summary>
    public class WorkApplyService : DataService, IWorkApplyService
    {
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;

        public WorkApplyService(IRepositoryContext context,
            IRepository<WorkApply> workApplyRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<Customer> customerRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository)
            : base(context)
        {
            this.workApplyRepository = workApplyRepository;
            this.workOrderRepository = workOrderRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.customerRepository = customerRepository;
            this.reseauRepository = reseauRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
        }

        /// <summary>
        /// 根据隐患上报Id获取隐患上报
        /// </summary>
        /// <param name="id">隐患上报Id</param>
        /// <returns>隐患上报维护对象</returns>
        public WorkApplyMaintObject GetWorkApplyById(Guid id)
        {
            WorkApply workApply = workApplyRepository.FindByKey(id);
            if (workApply != null)
            {
                WorkApplyMaintObject workApplyMaintObject = MapperHelper.Map<WorkApply, WorkApplyMaintObject>(workApply);
                workApplyMaintObject.Id = id;
                Customer customer = customerRepository.FindByKey(workApply.CustomerId);
                workApplyMaintObject.CustomerName = customer.CustomerName;
                workApplyMaintObject.Count = 0;
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workApply.Id && entity.EntityName == "WorkApply"));
                if (fileAssociation != null)
                {
                    int count = 0;
                    if (fileAssociation.FileIdList != "")
                    {
                        if (fileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = fileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    workApplyMaintObject.Count = count;
                    workApplyMaintObject.FileIdList = fileAssociation.FileIdList;
                }
                else
                {
                    workApplyMaintObject.Count = 0;
                    workApplyMaintObject.FileIdList = "";
                }

                workApplyMaintObject.ReturnReason = workApply.ReturnReason;
                return workApplyMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的隐患上报单在系统中不存在");
            }
        }

        /// <summary>
        /// 根据隐患上报Id获取隐患上报单打印页
        /// </summary>
        /// <param name="id">隐患上报Id</param>
        /// <returns>隐患上报单打印对象</returns>
        public WorkApplyPrintObject GetWorkApplyPrintById(Guid id)
        {
            WorkApply workApply = workApplyRepository.FindByKey(id);
            if (workApply != null)
            {
                WorkApplyPrintObject workApplyPrintObject = new WorkApplyPrintObject();
                workApplyPrintObject.Id = id;
                workApplyPrintObject.OrderCode = workApply.OrderCode;
                User createUser = userRepository.FindByKey(workApply.CreateUserId);
                workApplyPrintObject.CreateFullName = createUser.FullName;
                Department department = departmentRepository.FindByKey(createUser.DepartmentId);
                workApplyPrintObject.DepartmentName = department.DepartmentName;
                User sendUser = userRepository.FindByKey(workApply.ReseauManagerId);
                workApplyPrintObject.SendFullName = sendUser.FullName;
                workApplyPrintObject.CreateDate = workApply.CreateDate.ToShortDateString();
                workApplyPrintObject.Title = workApply.Title;
                workApplyPrintObject.ApplyReason = workApply.ApplyReason;
                workApplyPrintObject.SceneContactMan = workApply.SceneContactMan;
                workApplyPrintObject.SceneContactTel = workApply.SceneContactTel;
                workApplyPrintObject.ProjectCode = workApply.ProjectCode;
                Reseau reseau = reseauRepository.FindByKey(workApply.ReseauId);
                workApplyPrintObject.ReseauName = reseau.ReseauName;
                Customer customer = customerRepository.FindByKey(workApply.CustomerId);
                workApplyPrintObject.CustomerName = customer.CustomerName;
                workApplyPrintObject.Count = 0;
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workApply.Id && entity.EntityName == "WorkApply"));
                if (fileAssociation != null)
                {
                    int count = 0;
                    if (fileAssociation.FileIdList != "")
                    {
                        if (fileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = fileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    workApplyPrintObject.Count = count;
                }
                else
                {
                    workApplyPrintObject.Count = 0;
                }

                workApplyPrintObject.WFActivityInstancesInfoHtml = "";
                if (workApply.OrderCode != "")
                {
                    List<Parameter> parameters = new List<Parameter>(1);
                    parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = workApply.OrderCode });
                    using (var ds = SqlHelper.ExecuteDataSet("prc_GetWFActivityInstancesInfo", parameters))
                    {
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<table class='table' cellpadding='0' cellspacing='0' style='margin:auto;'>");
                            sb.Append("<tr>");
                            sb.Append("<td style='width:800px;' colspan='7'>发送人：" + ds.Tables[0].Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;发送日期：" + DateTime.Parse(ds.Tables[0].Rows[0][1].ToString()).ToShortDateString() + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td style='width:60px;'>步骤顺序</td>");
                            sb.Append("<td style='width:150px;'>步骤名称</td>");
                            sb.Append("<td style='width:200px;'>用户</td>");
                            sb.Append("<td style='width:60px;'>操作类型</td>");
                            sb.Append("<td style='width:60px;'>操作结果</td>");
                            sb.Append("<td style='width:190px;'>内容</td>");
                            sb.Append("<td style='width:80px;'>操作日期</td>");
                            sb.Append("</tr>");
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                sb.Append("<tr>");
                                sb.Append("<td>" + dr["SerialId"].ToString() + "</td>");
                                sb.Append("<td>" + dr["WFActivityInstanceName"].ToString() + "</td>");
                                sb.Append("<td>" + dr["FullName"].ToString() + "</td>");
                                sb.Append("<td>" + EnumHelper.GetEnumText(typeof(WFActivityOperate), int.Parse(dr["WFActivityOperate"].ToString())) + "</td>");
                                sb.Append("<td>" + EnumHelper.GetEnumText(typeof(WFActivityInstanceResult), int.Parse(dr["WFActivityInstanceResult"].ToString())) + "</td>");
                                sb.Append("<td>" + dr["Content"].ToString() + "</td>");
                                if (dr["WFActivityInstanceResult"].ToString() != "0")
                                {
                                    sb.Append("<td>" + DateTime.Parse(dr["OperateDate"].ToString()).ToShortDateString() + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td></td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("</table>");
                            workApplyPrintObject.WFActivityInstancesInfoHtml = sb.ToString();
                        }
                    }
                }
                return workApplyPrintObject;
            }
            else
            {
                throw new ApplicationFault("选择的隐患上报单在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页隐患上报列表的Json字符串</returns>
        public string GetWorkApplysPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, int orderState, int isSoved, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "OrderState", Type = SqlDbType.Int, Value = orderState });
            parameters.Add(new Parameter() { Name = "IsSoved", Type = SqlDbType.Int, Value = isSoved });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWorkApplysPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改隐患上报
        /// </summary>
        /// <param name="workApplyMaintObject">要新增或者修改的隐患上报对象</param>
        public void AddOrUpdateWorkApply(WorkApplyMaintObject workApplyMaintObject)
        {
            Reseau reseau = reseauRepository.FindByKey(workApplyMaintObject.ReseauId);
            if (workApplyMaintObject.Id == Guid.Empty)
            {
                WorkApply workApply = AggregateFactory.CreateWorkApply(workApplyMaintObject.Title, workApplyMaintObject.CustomerId, workApplyMaintObject.ReseauId, reseau.ReseauManagerId.Value,
                    workApplyMaintObject.ApplyReason, workApplyMaintObject.SceneContactMan, workApplyMaintObject.SceneContactTel, workApplyMaintObject.CreateUserId);
                workApplyRepository.Add(workApply);

                if (workApplyMaintObject.FileIdList != "")
                {
                    FileAssociation fileAssociation = AggregateFactory.CreateFileAssociation("WorkApply", workApply.Id, workApplyMaintObject.FileIdList, workApplyMaintObject.CreateUserId);
                    fileAssociationRepository.Add(fileAssociation);
                }
            }
            else
            {
                WorkApply workApply = workApplyRepository.FindByKey(workApplyMaintObject.Id);
                if (workApply != null)
                {
                    workApply.CheckByUpdate(workApplyMaintObject.ModifyUserId);
                    workApply.Modify(workApplyMaintObject.Title, workApplyMaintObject.CustomerId, workApplyMaintObject.ReseauId, reseau.ReseauManagerId.Value, workApplyMaintObject.ApplyReason, workApplyMaintObject.SceneContactMan, workApplyMaintObject.SceneContactTel, workApplyMaintObject.ModifyUserId);
                    workApplyRepository.Update(workApply);

                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workApply.Id && entity.EntityName == "WorkApply"));
                    if (fileAssociation == null && workApplyMaintObject.FileIdList != "")
                    {
                        FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("WorkApply", workApply.Id, workApplyMaintObject.FileIdList, workApplyMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newFileAssociation);
                    }
                    else if (fileAssociation != null && workApplyMaintObject.FileIdList != fileAssociation.FileIdList)
                    {
                        fileAssociation.Modify(workApplyMaintObject.FileIdList, workApplyMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(fileAssociation);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_WorkApplyTitle"))
                {
                    throw new ApplicationFault("标题重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 发送隐患上报单
        /// </summary>
        /// <param name="workApplyMaintObjects">要发送的隐患上报维护对象</param>
        public void SendWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            foreach (WorkApplyMaintObject workApplyMaintObject in workApplyMaintObjects)
            {
                WorkApply workApply = workApplyRepository.FindByKey(workApplyMaintObject.Id);
                if (workApply != null)
                {
                    if (workApply.OrderState != WFProcessInstanceState.未发送)
                    {
                        throw new ApplicationFault("只能发送申请状态为未发送的隐患上报单");
                    }
                    workApply.OrderState = WFProcessInstanceState.流程通过;
                    workApplyRepository.Update(workApply);
                }
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

        /// <summary>
        /// 删除隐患上报
        /// </summary>
        /// <param name="workApplyMaintObjects">要删除的隐患上报维护对象</param>
        public void RemoveWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            foreach (WorkApplyMaintObject workApplyMaintObject in workApplyMaintObjects)
            {
                WorkApply workApply = workApplyRepository.FindByKey(workApplyMaintObject.Id);
                if (workApply != null)
                {
                    if (workApply.OrderState != WFProcessInstanceState.未发送)
                    {
                        throw new ApplicationFault("只能删除申请状态为未发送的隐患上报单");
                    }
                    workApplyRepository.Remove(workApply);
                }
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

        /// <summary>
        /// 根据条件获取分页待处理隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="Title">标题</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="sendUserId">派单人Id</param>
        /// <returns>分页待处理隐患上报列表的Json字符串</returns>
        public string GetWorkApplyWaitPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, int isSoved, Guid sendUserId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "IsSoved", Type = SqlDbType.Int, Value = isSoved });
            parameters.Add(new Parameter() { Name = "SendUserId", Type = SqlDbType.UniqueIdentifier, Value = sendUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWorkApplyWaitPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 关联隐患上报与零星派工单
        /// </summary>
        /// <param name="workOrderMaintObject">零星派工单维护对象</param>
        /// <param name="workApplyMaintObjects">隐患上报维护对象</param>
        public void SaveWorkApplyAssociate(WorkOrderMaintObject workOrderMaintObject, IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            WorkOrder workOrder = workOrderRepository.FindByKey(workOrderMaintObject.Id);
            if (workOrder != null)
            {
                foreach (WorkApplyMaintObject workApplyMaintObject in workApplyMaintObjects)
                {
                    WorkApply workApply = workApplyRepository.FindByKey(workApplyMaintObject.Id);
                    workApply.WorkOrderId = workOrder.Id;
                    workApply.IsSoved = Bool.是;
                    workApplyRepository.Update(workApply);
                }
            }
            else
            {
                throw new ApplicationFault("选择的零星派工单在系统中不存在");
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

        /// <summary>
        /// 根据条件获取分页隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="customerId">申请单位Id</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页隐患上报列表的Json字符串</returns>
        public string GetWorkApplysReport(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid customerId, int orderState, int isSoved, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            parameters.Add(new Parameter() { Name = "OrderState", Type = SqlDbType.Int, Value = orderState });
            parameters.Add(new Parameter() { Name = "IsSoved", Type = SqlDbType.Int, Value = isSoved });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWorkApplysReport", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 退回隐患上报
        /// </summary>
        /// <param name="workApplyMaintObject">要退回的隐患上报维护</param>
        public void ReturnWorkApply(WorkApplyMaintObject workApplyMaintObject)
        {
            WorkApply workApply = workApplyRepository.FindByKey(workApplyMaintObject.Id);
            if (workApply != null)
            {
                if (workApply.OrderState != WFProcessInstanceState.流程通过)
                {
                    throw new ApplicationFault("只能退回申请状态为流程通过的隐患上报单");
                }
                if (workApply.WorkOrderId != Guid.Empty)
                {
                    throw new ApplicationFault("该隐患上报单已发起派工申请，无法退回");
                }
                workApply.OrderState = WFProcessInstanceState.未发送;
                workApply.ReturnReason = workApplyMaintObject.ReturnReason;
                workApplyRepository.Update(workApply);
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

        /// <summary>
        /// 根据条件获取分页待立项隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="sendUserId">派单人Id</param>
        /// <returns>分页待处理隐患上报列表的Json字符串</returns>
        public string GetWorkApplyProjectPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, string projectCode, int isProject, Guid sendUserId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "IsProject", Type = SqlDbType.Int, Value = isProject });
            parameters.Add(new Parameter() { Name = "SendUserId", Type = SqlDbType.UniqueIdentifier, Value = sendUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWorkApplyProjectPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 保存项目编码
        /// </summary>
        /// <param name="workApplyMaintObject">要保存项目编码的隐患上报维护对象</param>
        public void SaveWorkApplyProjectCode(WorkApplyMaintObject workApplyMaintObject)
        {
            WorkApply workApply = workApplyRepository.FindByKey(workApplyMaintObject.Id);
            if (workApply != null)
            {
                if (workApply.IsProject == Bool.否)
                {
                    workApply.ProjectCode = workApplyMaintObject.ProjectCode;
                    workApplyRepository.Update(workApply);
                }
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

        /// <summary>
        /// 保存立项完成隐患上报
        /// </summary>
        /// <param name="workApplyMaintObjects">要保存立项是否完成的隐患上报维护对象列表</param>
        public void SaveIsProjectWorkApplys(IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            foreach (WorkApplyMaintObject workApplyMaintObject in workApplyMaintObjects)
            {
                WorkApply workApply = workApplyRepository.FindByKey(workApplyMaintObject.Id);
                if (workApply != null)
                {
                    workApply.IsProject = (Bool)workApplyMaintObject.IsProject;
                    workApplyRepository.Update(workApply);
                }
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
