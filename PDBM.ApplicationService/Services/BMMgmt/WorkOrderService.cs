using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
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
    /// 派工单应用层服务
    /// </summary>
    public class WorkOrderService : DataService, IWorkOrderService
    {
        private readonly IRepository<WorkBigClass> workBigClassRepository;
        private readonly IRepository<WorkSmallClass> workSmallClassRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;

        public WorkOrderService(IRepositoryContext context,
            IRepository<WorkBigClass> workBigClassRepository,
            IRepository<WorkSmallClass> workSmallClassRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<WorkApply> workApplyRepository,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<Customer> customerRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<Area> areaRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository)
            : base(context)
        {
            this.workBigClassRepository = workBigClassRepository;
            this.workSmallClassRepository = workSmallClassRepository;
            this.workOrderRepository = workOrderRepository;
            this.workApplyRepository = workApplyRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.customerRepository = customerRepository;
            this.reseauRepository = reseauRepository;
            this.areaRepository = areaRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
        }

        /// <summary>
        /// 根据派工单Id获取派工单
        /// </summary>
        /// <param name="id">派工单Id</param>
        /// <returns>派工单维护对象</returns>
        public WorkOrderMaintObject GetWorkOrderById(Guid id)
        {
            WorkOrder workOrder = workOrderRepository.FindByKey(id);
            if (workOrder != null)
            {
                WorkOrderMaintObject workOrderMaintObject = MapperHelper.Map<WorkOrder, WorkOrderMaintObject>(workOrder);
                WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(workOrder.WorkSmallClassId);
                Customer customer = customerRepository.FindByKey(workOrder.CustomerId);
                workOrderMaintObject.Id = id;
                workOrderMaintObject.PlaceName = workOrder.PlaceName;
                workOrderMaintObject.Title = workOrder.Title;
                workOrderMaintObject.WorkBigClassId = workSmallClass.WorkBigClassId;
                workOrderMaintObject.WorkSmallClassId = workOrder.WorkSmallClassId;
                workOrderMaintObject.CreateDate = workOrder.CreateDate;
                workOrderMaintObject.CreateDateText = workOrder.CreateDate.ToShortDateString();
                workOrderMaintObject.RequireSendDate = workOrder.RequireSendDate;
                workOrderMaintObject.RequireSendDateText = workOrder.RequireSendDate.ToShortDateString();
                workOrderMaintObject.SceneContactMan = workOrder.SceneContactMan;
                workOrderMaintObject.SceneContactTel = workOrder.SceneContactTel;
                workOrderMaintObject.CustomerType = (int)customer.CustomerType;
                workOrderMaintObject.CustomerId = workOrder.CustomerId;
                workOrderMaintObject.CustomerName = customer.CustomerName;
                workOrderMaintObject.Days = workOrder.Days;
                workOrderMaintObject.CustomerUserId = workOrder.CustomerUserId;
                User customerUser = userRepository.FindByKey(workOrder.CustomerUserId);
                workOrderMaintObject.MaintainContactTel = customerUser.PhoneNumber;
                workOrderMaintObject.WorkContent = workOrder.WorkContent;
                workOrderMaintObject.HumanRequire = workOrder.HumanRequire;
                workOrderMaintObject.CarRequire = workOrder.CarRequire;
                workOrderMaintObject.MaterialRequire = workOrder.MaterialRequire;
                workOrderMaintObject.Count = 0;
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrder"));
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
                    workOrderMaintObject.Count = count;
                    workOrderMaintObject.FileIdList = fileAssociation.FileIdList;
                }
                else
                {
                    workOrderMaintObject.Count = 0;
                    workOrderMaintObject.FileIdList = "";
                }
                return workOrderMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的派工单在系统中不存在");
            }
        }

        /// <summary>
        /// 根据零星派工单Id获取零星派工单审批维护对象
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <returns>零星派工单审批维护对象</returns>
        public WorkOrderEditorObject GetWorkOrderEditorById(Guid id)
        {
            WorkOrder workOrder = workOrderRepository.FindByKey(id);
            if (workOrder != null)
            {
                WorkOrderEditorObject workOrderEditorObject = MapperHelper.Map<WorkOrder, WorkOrderEditorObject>(workOrder);
                WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(workOrder.WorkSmallClassId);
                Customer customer = customerRepository.FindByKey(workOrder.CustomerId);
                workOrderEditorObject.Id = id;
                workOrderEditorObject.WorkBeginDate = workOrder.WorkBeginDate;
                workOrderEditorObject.WorkBeginDateText = workOrder.WorkBeginDate.ToShortDateString();
                workOrderEditorObject.WorkEndDate = workOrder.WorkBeginDate;
                workOrderEditorObject.WorkEndDateText = workOrder.WorkEndDate.ToShortDateString();
                workOrderEditorObject.BeginHour = workOrder.BeginHour;
                workOrderEditorObject.BeginMinute = workOrder.BeginMinute;
                workOrderEditorObject.EndHour = workOrder.EndHour;
                workOrderEditorObject.EndMinute = workOrder.EndMinute;
                workOrderEditorObject.ExecuteSituation = workOrder.ExecuteSituation;
                workOrderEditorObject.MaterialConsumption = workOrder.MaterialConsumption;
                workOrderEditorObject.PersonnelNumber = workOrder.PersonnelNumber;
                workOrderEditorObject.CarType = workOrder.CarType;
                workOrderEditorObject.IsFinish = (int)workOrder.IsFinish;
                workOrderEditorObject.WFCount = 0;
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrderWF"));
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
                    workOrderEditorObject.WFCount = count;
                    workOrderEditorObject.WFFileIdList = fileAssociation.FileIdList;
                }
                else
                {
                    workOrderEditorObject.WFCount = 0;
                    workOrderEditorObject.WFFileIdList = "";
                }
                return workOrderEditorObject;
            }
            else
            {
                throw new ApplicationFault("选择的派工单在系统中不存在");
            }
        }

        /// <summary>
        /// 根据零星派工单Id获取零星派工单打印页
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <returns>零星派工单打印对象</returns>
        public WorkOrderPrintObject GetWorkOrderPrintById(Guid id)
        {
            WorkOrder workOrder = workOrderRepository.FindByKey(id);
            if (workOrder != null)
            {
                WorkOrderPrintObject workOrderPrintObject = new WorkOrderPrintObject();
                workOrderPrintObject.OrderCode = workOrder.OrderCode;
                User createUser = userRepository.FindByKey(workOrder.CreateUserId);
                Department department = departmentRepository.FindByKey(createUser.DepartmentId);
                workOrderPrintObject.FullName = department.DepartmentName + "/" + createUser.FullName;
                workOrderPrintObject.CreateDate = workOrder.CreateDate.ToShortDateString();
                workOrderPrintObject.Title = workOrder.Title;
                Reseau reseau = reseauRepository.FindByKey(workOrder.ReseauId);
                workOrderPrintObject.ReseauName = reseau.ReseauName;
                WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(workOrder.WorkSmallClassId);
                WorkBigClass workBigClass = workBigClassRepository.FindByKey(workSmallClass.WorkBigClassId);
                workOrderPrintObject.ClassName = workBigClass.BigClassName + "/" + workSmallClass.SmallClassName;
                workOrderPrintObject.RequireSendDate = workOrder.RequireSendDate.ToShortDateString();
                workOrderPrintObject.SceneContactMan = workOrder.SceneContactMan;
                workOrderPrintObject.SceneContactTel = workOrder.SceneContactTel;
                Customer customer = customerRepository.FindByKey(workOrder.CustomerId);
                workOrderPrintObject.CustomerName = customer.CustomerFullName;
                workOrderPrintObject.Days = workOrder.Days.ToString();
                User customerUser = userRepository.FindByKey(workOrder.CustomerUserId);
                workOrderPrintObject.MaintainContactMan = customerUser.FullName;
                workOrderPrintObject.MainTainContactTel = customerUser.PhoneNumber;
                workOrderPrintObject.WorkContent = workOrder.WorkContent;
                workOrderPrintObject.HumanRequire = workOrder.HumanRequire;
                workOrderPrintObject.CarRequire = workOrder.CarRequire;
                workOrderPrintObject.MaterialRequire = workOrder.MaterialRequire;
                workOrderPrintObject.WorkBeginDate = workOrder.WorkBeginDate.AddHours(workOrder.BeginHour).AddMinutes(workOrder.BeginMinute).ToString();
                workOrderPrintObject.WorkEndDate = workOrder.WorkEndDate.AddHours(workOrder.EndHour).AddMinutes(workOrder.EndMinute).ToString();
                workOrderPrintObject.IsFinish = EnumHelper.GetEnumText(typeof(Bool), workOrder.IsFinish);
                if (workOrder.RegisterUserId.Value != Guid.Empty)
                {
                    User registerUser = userRepository.FindByKey(workOrder.RegisterUserId.Value);
                    workOrderPrintObject.RegisterFullName = registerUser.FullName;
                    workOrderPrintObject.RegisterDate = workOrder.RegisterDate.ToShortDateString();
                }
                else
                {
                    workOrderPrintObject.RegisterFullName = "";
                    workOrderPrintObject.RegisterDate = "";
                }
                workOrderPrintObject.ExecuteSituation = workOrder.ExecuteSituation;
                workOrderPrintObject.MaterialConsumption = workOrder.MaterialConsumption;
                workOrderPrintObject.PersonnelNumber = workOrder.PersonnelNumber;
                workOrderPrintObject.CarType = workOrder.CarType;

                workOrderPrintObject.Count = 0;
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrder"));
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
                    workOrderPrintObject.Count = count;
                }
                else
                {
                    workOrderPrintObject.Count = 0;
                }

                workOrderPrintObject.SettlementCount = 0;
                FileAssociation fileAssociationSettlement = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrderWF"));
                if (fileAssociationSettlement != null)
                {
                    int count = 0;
                    if (fileAssociationSettlement.FileIdList != "")
                    {
                        if (fileAssociationSettlement.FileIdList.Contains(","))
                        {
                            string[] strFileList = fileAssociationSettlement.FileIdList.Split(',');
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
                    workOrderPrintObject.SettlementCount = count;
                }
                else
                {
                    workOrderPrintObject.SettlementCount = 0;
                }

                workOrderPrintObject.WorkOrderDetailInfoHtml = "";
                List<Parameter> pars = new List<Parameter>(1);
                pars.Add(new Parameter() { Name = "WorkOrderId", Type = SqlDbType.UniqueIdentifier, Value = workOrder.Id });
                using (var ds = SqlHelper.ExecuteDataSet("prc_GetWorkOrderDetail", pars))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table class='table' cellpadding='0' cellspacing='0' style='margin:auto; width: 100%'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='width:105px;'>工作开始时间</td>");
                        sb.Append("<td style='width:105px;'>工作结束时间</td>");
                        sb.Append("<td style='width:150px;'>执行情况</td>");
                        sb.Append("<td style='width:150px;'>材料消耗</td>");
                        sb.Append("<td style='width:150px;'>人员数量</td>");
                        sb.Append("<td style='width:150px;'>车辆类型</td>");
                        sb.Append("<td style='width:60px;'>是否完成</td>");
                        sb.Append("</tr>");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>" + dr["WorkBeginDate"].ToString() + "</td>");
                            sb.Append("<td>" + dr["WorkEndDate"].ToString() + "</td>");
                            sb.Append("<td>" + dr["ExecuteSituation"].ToString() + "</td>");
                            sb.Append("<td>" + dr["MaterialConsumption"].ToString() + "</td>");
                            sb.Append("<td>" + dr["PersonnelNumber"].ToString() + "</td>");
                            sb.Append("<td>" + dr["CarType"].ToString() + "</td>");
                            sb.Append("<td>" + dr["IsFinishName"].ToString() + "</td>");
                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                        workOrderPrintObject.WorkOrderDetailInfoHtml = sb.ToString();
                    }
                }

                workOrderPrintObject.WFActivityInstancesInfoHtml = "";
                if (workOrder.OrderCode != "")
                {
                    List<Parameter> parameters = new List<Parameter>(1);
                    parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = workOrder.OrderCode });
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
                            workOrderPrintObject.WFActivityInstancesInfoHtml = sb.ToString();
                        }
                    }
                }
                return workOrderPrintObject;
            }
            else
            {
                throw new ApplicationFault("选择的零星派工单在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页零星派工单列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="workBigClassId">工单大类Id</param>
        /// <param name="workSmallClassId">工单小类Id</param>
        /// <param name="CustomerId">代维单位Id</param>
        /// <param name="maintainContactMan">代维人员</param>
        /// <param name="isFinish">是否完成</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public string GetWorkOrdersPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid workBigClassId, Guid workSmallClassId, Guid customerId, string maintainContactMan, Guid sendUserId, int isFinish, int orderState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(14);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "WorkBigClassId", Type = SqlDbType.UniqueIdentifier, Value = workBigClassId });
            parameters.Add(new Parameter() { Name = "WorkSmallClassId", Type = SqlDbType.UniqueIdentifier, Value = workSmallClassId });
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            parameters.Add(new Parameter() { Name = "MaintainContactMan", Type = SqlDbType.NVarChar, Value = maintainContactMan });
            parameters.Add(new Parameter() { Name = "SendUserId", Type = SqlDbType.UniqueIdentifier, Value = sendUserId });
            parameters.Add(new Parameter() { Name = "IsFinish", Type = SqlDbType.Int, Value = isFinish });
            parameters.Add(new Parameter() { Name = "OrderState", Type = SqlDbType.Int, Value = orderState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryWorkOrdersPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改派工单
        /// </summary>
        /// <param name="workOrderMaintObject">要新增或者修改的派工单对象</param>
        public void AddOrUpdateWorkOrder(WorkOrderMaintObject workOrderMaintObject)
        {
            Reseau reseau = reseauRepository.FindByKey(workOrderMaintObject.ReseauId);
            Area area = areaRepository.FindByKey(reseau.AreaId);
            WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(workOrderMaintObject.WorkSmallClassId);
            WorkBigClass workBigClass = workBigClassRepository.FindByKey(workSmallClass.WorkBigClassId);
            string title = area.AreaName + "_" + workOrderMaintObject.PlaceName + "_" + workBigClass.BigClassName + "_" + workSmallClass.SmallClassName + "工单";
            if (workOrderMaintObject.Id == Guid.Empty)
            {
                WorkOrder workOrder = AggregateFactory.CreateWorkOrder(workOrderMaintObject.PlaceName, title, workOrderMaintObject.ReseauId, workOrderMaintObject.WorkSmallClassId, workOrderMaintObject.SceneContactMan,
                    workOrderMaintObject.SceneContactTel, workOrderMaintObject.RequireSendDate, workOrderMaintObject.Days, workOrderMaintObject.CustomerId, workOrderMaintObject.CustomerUserId,
                    workOrderMaintObject.WorkContent, workOrderMaintObject.HumanRequire, workOrderMaintObject.CarRequire, workOrderMaintObject.MaterialRequire,
                    workOrderMaintObject.CreateUserId);
                workOrderRepository.Add(workOrder);

                if (workOrderMaintObject.FileIdList != "")
                {
                    FileAssociation fileAssociation = AggregateFactory.CreateFileAssociation("WorkOrder", workOrder.Id, workOrderMaintObject.FileIdList, workOrderMaintObject.CreateUserId);
                    fileAssociationRepository.Add(fileAssociation);
                }
            }
            else
            {
                WorkOrder workOrder = workOrderRepository.FindByKey(workOrderMaintObject.Id);
                if (workOrder != null)
                {
                    workOrder.CheckByUpdate(workOrderMaintObject.ModifyUserId);
                    workOrder.Modify(workOrderMaintObject.PlaceName, title, workOrderMaintObject.ReseauId, workOrderMaintObject.WorkSmallClassId, workOrderMaintObject.SceneContactMan, workOrderMaintObject.SceneContactTel,
                        workOrderMaintObject.RequireSendDate, workOrderMaintObject.Days, workOrderMaintObject.CustomerId, workOrderMaintObject.CustomerUserId,
                        workOrderMaintObject.WorkContent, workOrderMaintObject.HumanRequire, workOrderMaintObject.CarRequire, workOrderMaintObject.MaterialRequire, workOrderMaintObject.ModifyUserId);
                    workOrderRepository.Update(workOrder);

                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrder"));
                    if (fileAssociation == null && workOrderMaintObject.FileIdList != "")
                    {
                        FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("WorkOrder", workOrder.Id, workOrderMaintObject.FileIdList, workOrderMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newFileAssociation);
                    }
                    else if (fileAssociation != null && workOrderMaintObject.FileIdList != fileAssociation.FileIdList)
                    {
                        fileAssociation.Modify(workOrderMaintObject.FileIdList, workOrderMaintObject.ModifyUserId);
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
                if (ex.Message.Contains("IX_UQ_WorkOrderTitle"))
                {
                    throw new ApplicationFault("标题重复");
                }
                if (ex.Message.Contains("FK_dbo.tbl_WorkOrder_dbo.tbl_WorkSmallClass_WorkSmallClassId"))
                {
                    throw new ApplicationFault("工单小类在系统中不存在");
                }
                if (ex.Message.Contains("FK_dbo.tbl_WorkOrder_dbo.tbl_Customer_CustomerId"))
                {
                    throw new ApplicationFault("代维单位在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除派工单
        /// </summary>
        /// <param name="workOrderMaintObjects">要删除的派工单维护实体</param>
        public void RemoveWorkOrder(IList<WorkOrderMaintObject> workOrderMaintObjects)
        {
            foreach (WorkOrderMaintObject workOrderMaintObject in workOrderMaintObjects)
            {
                WorkOrder workOrder = workOrderRepository.FindByKey(workOrderMaintObject.Id);
                if (workOrder != null)
                {
                    if (workOrder.OrderState != WFProcessInstanceState.未发送)
                    {
                        throw new ApplicationFault("只能删除状态为未发送的派工单");
                    }

                    IEnumerable<WorkApply> workApplys = workApplyRepository.FindAll(Specification<WorkApply>.Eval(entity => entity.WorkOrderId == workOrder.Id));
                    foreach (WorkApply workApply in workApplys)
                    {
                        workApply.WorkOrderId = Guid.Empty;
                        workApplyRepository.Update(workApply);
                    }
                    workOrderRepository.Remove(workOrder);
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
        /// 登记结算
        /// </summary>
        /// <param name="workOrderEditorObject">零星派工单审批对象</param>
        public void SaveWorkOrderWF(WorkOrderEditorObject workOrderEditorObject)
        {
            WorkOrder workOrder = workOrderRepository.FindByKey(workOrderEditorObject.Id);
            if (workOrder != null)
            {
                workOrder.SaveWorkOrderWF(workOrderEditorObject.WorkBeginDate, workOrderEditorObject.BeginHour, workOrderEditorObject.BeginMinute, workOrderEditorObject.WorkEndDate,
                    workOrderEditorObject.EndHour, workOrderEditorObject.EndMinute, workOrderEditorObject.ExecuteSituation, workOrderEditorObject.MaterialConsumption, workOrderEditorObject.PersonnelNumber,
                    workOrderEditorObject.CarType, (Bool)workOrderEditorObject.IsFinish, workOrderEditorObject.ModifyUserId);
                workOrderRepository.Update(workOrder);

                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrderWF"));
                if (fileAssociation == null && workOrderEditorObject.WFFileIdList != "")
                {
                    FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("WorkOrderWF", workOrder.Id, workOrderEditorObject.WFFileIdList, workOrderEditorObject.ModifyUserId);
                    fileAssociationRepository.Add(newFileAssociation);
                }
                else if (fileAssociation != null && workOrderEditorObject.WFFileIdList != fileAssociation.FileIdList)
                {
                    fileAssociation.Modify(workOrderEditorObject.WFFileIdList, workOrderEditorObject.ModifyUserId);
                    fileAssociationRepository.Update(fileAssociation);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == workOrderEditorObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(workOrderEditorObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
        /// 保存通过派单申请新增的零星派工单
        /// </summary>
        /// <param name="workOrderMaintObject">零星派工单维护对象</param>
        /// <param name="workApplyMaintObjects">隐患上报维护对象</param>
        public void SaveWorkOrderByWorkApply(WorkOrderMaintObject workOrderMaintObject, IList<WorkApplyMaintObject> workApplyMaintObjects)
        {
            Reseau reseau = reseauRepository.FindByKey(workOrderMaintObject.ReseauId);
            Area area = areaRepository.FindByKey(reseau.AreaId);
            WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(workOrderMaintObject.WorkSmallClassId);
            WorkBigClass workBigClass = workBigClassRepository.FindByKey(workSmallClass.WorkBigClassId);
            string title = area.AreaName + "_" + workOrderMaintObject.PlaceName + "_" + workBigClass.BigClassName + "_" + workSmallClass.SmallClassName + "工单";
            if (workOrderMaintObject.Id == Guid.Empty)
            {
                WorkOrder workOrder = AggregateFactory.CreateWorkOrder(workOrderMaintObject.PlaceName, title, workOrderMaintObject.ReseauId, workOrderMaintObject.WorkSmallClassId, workOrderMaintObject.SceneContactMan,
                    workOrderMaintObject.SceneContactTel, workOrderMaintObject.RequireSendDate, workOrderMaintObject.Days, workOrderMaintObject.CustomerId, workOrderMaintObject.CustomerUserId,
                    workOrderMaintObject.WorkContent, workOrderMaintObject.HumanRequire, workOrderMaintObject.CarRequire, workOrderMaintObject.MaterialRequire,
                    workOrderMaintObject.CreateUserId);
                workOrderRepository.Add(workOrder);

                FileAssociation fileAssociationWorkApply = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrderMaintObject.WorkApplyId && entity.EntityName == "WorkApply"));

                if (workOrderMaintObject.FileIdList != "")
                {
                    if (fileAssociationWorkApply != null)
                    {
                        workOrderMaintObject.FileIdList = workOrderMaintObject.FileIdList + "," + fileAssociationWorkApply.FileIdList;
                    }
                    FileAssociation fileAssociation = AggregateFactory.CreateFileAssociation("WorkOrder", workOrder.Id, workOrderMaintObject.FileIdList, workOrderMaintObject.CreateUserId);
                    fileAssociationRepository.Add(fileAssociation);
                }
                else
                {
                    if (fileAssociationWorkApply != null)
                    {
                        FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("WorkOrder", workOrder.Id, fileAssociationWorkApply.FileIdList, workOrderMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newFileAssociation);
                    }
                }

                //foreach (WorkApplyMaintObject workApplyMaintObject in workApplyMaintObjects)
                //{
                WorkApply workApply = workApplyRepository.FindByKey(workOrderMaintObject.WorkApplyId);
                workApply.WorkOrderId = workOrder.Id;
                workApplyRepository.Update(workApply);
                //}
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
