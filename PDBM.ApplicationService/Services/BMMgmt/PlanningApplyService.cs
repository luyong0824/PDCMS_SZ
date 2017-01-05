using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using System.Collections;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 建设申请应用层服务
    /// </summary>
    public class PlanningApplyService : DataService, IPlanningApplyService
    {
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<PlanningApplyHeader> planningApplyHeaderRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;
        private readonly ICodeSeedRepository codeSeedRepository;

        public PlanningApplyService(IRepositoryContext context,
            IRepository<Planning> planningRepository,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<PlanningApplyHeader> planningApplyHeaderRepository,
            IRepository<Area> areaRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository,
            ICodeSeedRepository codeSeedRepository)
            : base(context)
        {
            this.planningRepository = planningRepository;
            this.planningApplyRepository = planningApplyRepository;
            this.planningApplyHeaderRepository = planningApplyHeaderRepository;
            this.areaRepository = areaRepository;
            this.reseauRepository = reseauRepository;
            this.placeOwnerRepository = placeOwnerRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
            this.codeSeedRepository = codeSeedRepository;
        }

        /// <summary>
        /// 根据建设申请Id获取建设申请
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <returns>建设申请维护对象</returns>
        public PlanningApplyMaintObject GetPlanningApplyById(Guid id)
        {
            PlanningApply planningApply = planningApplyRepository.FindByKey(id);
            if (planningApply != null)
            {
                PlanningApplyMaintObject planningApplyMaintObject = MapperHelper.Map<PlanningApply, PlanningApplyMaintObject>(planningApply);
                planningApplyMaintObject.CreateDateText = planningApply.CreateDate.ToShortDateString();
                Reseau reseau = reseauRepository.GetByKey(planningApplyMaintObject.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);
                planningApplyMaintObject.AreaId = reseau.AreaId;
                planningApplyMaintObject.PlanningCode = planningApply.PlanningCode;
                planningApplyMaintObject.PlanningName = planningApply.PlanningName;
                planningApplyMaintObject.AreaName = area.AreaName;
                planningApplyMaintObject.ReseauName = reseau.ReseauName;
                planningApplyMaintObject.Lng = planningApply.Lng;
                planningApplyMaintObject.Lat = planningApply.Lat;
                planningApplyMaintObject.DetailedAddress = planningApply.DetailedAddress;
                planningApplyMaintObject.Remarks = planningApply.Remarks;
                planningApplyMaintObject.Importance = (int)planningApply.Importance;
                planningApplyMaintObject.ImportanceText = EnumHelper.GetEnumText(typeof(Importance), planningApply.Importance);
                planningApplyMaintObject.FileIdList = "";
                planningApplyMaintObject.Count = 0;
                FileAssociation planningApplyFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planningApply.Id && entity.EntityName == "PlanningApply"));
                if (planningApplyFileAssociation != null)
                {
                    int count = 0;
                    if (planningApplyFileAssociation.FileIdList != "")
                    {
                        if (planningApplyFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = planningApplyFileAssociation.FileIdList.Split(',');
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
                    planningApplyMaintObject.Count = count;
                    planningApplyMaintObject.FileIdList = planningApplyFileAssociation.FileIdList;
                }
                else
                {
                    planningApplyMaintObject.Count = 0;
                    planningApplyMaintObject.FileIdList = "";
                }
                return planningApplyMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的建设申请在系统中不存在");
            }
        }

        /// <summary>
        /// 根据建设申请Id获取建设申请打印信息
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <returns>建设申请打印对象</returns>
        public PlanningApplyHeaderPrintObject GetPlanningApplyPrintById(Guid id)
        {
            PlanningApplyHeader planningApplyHeader = planningApplyHeaderRepository.GetByKey(id);
            if (planningApplyHeader != null)
            {
                PlanningApplyHeaderPrintObject planningApplyHeaderPrintObject = MapperHelper.Map<PlanningApplyHeader, PlanningApplyHeaderPrintObject>(planningApplyHeader);
                User user = userRepository.GetByKey(planningApplyHeader.CreateUserId);
                Department department = departmentRepository.GetByKey(user.DepartmentId);
                planningApplyHeaderPrintObject.OrderCode = planningApplyHeader.OrderCode;
                planningApplyHeaderPrintObject.DepartmentName = department.DepartmentName;
                planningApplyHeaderPrintObject.CreateFullName = user.FullName;
                planningApplyHeaderPrintObject.CreateDateText = planningApplyHeader.CreateDate.ToShortDateString();
                planningApplyHeaderPrintObject.Title = planningApplyHeader.Title;

                List<Parameter> param = new List<Parameter>(1);
                param.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = planningApplyHeader.Id });
                using (var dt = SqlHelper.ExecuteDataTable("prc_GetPlanningApplysByHeaderId", param))
                {
                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sbHeader = new StringBuilder();
                        sbHeader.Append("<table class='table' cellpadding='0' cellspacing='0' style='margin:auto;'>");
                        sbHeader.Append("<tr>");
                        sbHeader.Append("<td style='width:200px;'>规划名称</td>");
                        sbHeader.Append("<td style='width:60px;'>区域</td>");
                        sbHeader.Append("<td style='width:60px;'>网格</td>");
                        sbHeader.Append("<td style='width:60px;'>经度</td>");
                        sbHeader.Append("<td style='width:60px;'>纬度</td>");
                        sbHeader.Append("<td style='width:80px;'>重要性程度</td>");
                        sbHeader.Append("<td style='width:80px;'>规划意见</td>");
                        sbHeader.Append("<td style='width:200px;'>详细地址</td>");
                        sbHeader.Append("</tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            sbHeader.Append("<tr>");
                            sbHeader.Append("<td>" + dr["PlanningName"].ToString() + "</td>");
                            sbHeader.Append("<td>" + dr["AreaName"].ToString() + "</td>");
                            sbHeader.Append("<td>" + dr["ReseauName"].ToString() + "</td>");
                            sbHeader.Append("<td>" + dr["Lng"].ToString() + "</td>");
                            sbHeader.Append("<td>" + dr["Lat"].ToString() + "</td>");
                            sbHeader.Append("<td>" + EnumHelper.GetEnumText(typeof(Importance), int.Parse(dr["Importance"].ToString())) + "</td>");
                            sbHeader.Append("<td>" + dr["PlanningAdviceName"].ToString() + "</td>");
                            sbHeader.Append("<td>" + dr["DetailedAddress"].ToString() + "</td>");
                            sbHeader.Append("</tr>");
                        }
                        sbHeader.Append("</table>");
                        planningApplyHeaderPrintObject.PlanningApplyDetailHtml = sbHeader.ToString();
                    }
                }

                if (planningApplyHeader.OrderCode != "")
                {
                    List<Parameter> parameters = new List<Parameter>(1);
                    parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = planningApplyHeader.OrderCode });
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
                            planningApplyHeaderPrintObject.WFActivityInstancesInfoHtml = sb.ToString();
                        }
                    }
                }
                else
                {
                    planningApplyHeaderPrintObject.WFActivityInstancesInfoHtml = "";
                }
                return planningApplyHeaderPrintObject;
            }
            else
            {
                throw new ApplicationFault("选择的建设申请在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页建设申请列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="issued">是否下达</param>
        /// <param name="createUserId">申请人</param>
        /// <param name="profession">专业</param>
        /// <returns>分页建设申请列表的Json字符串</returns>
        public string GetPlanningApplysPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningName, Guid areaId, Guid reseauId,
            int issued, Guid createUserId, int profession)
        {
            List<Parameter> parameters = new List<Parameter>(18);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Issued", Type = SqlDbType.Int, Value = issued });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryPlanningApplysPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改建设申请
        /// </summary>
        /// <param name="planningMaintObject">要新增或者修改的建设申请维护对象</param>
        public void AddOrUpdatePlanningApply(PlanningApplyMaintObject planningApplyMaintObject)
        {
            if (planningApplyMaintObject.Id == Guid.Empty)
            {
                PlanningApply planningApply = AggregateFactory.CreatePlanningApply(codeSeedRepository.GenerateCode("PlanningApply"), planningApplyMaintObject.PlanningName,
                    (Profession)planningApplyMaintObject.Profession, planningApplyMaintObject.ReseauId, planningApplyMaintObject.Lng, planningApplyMaintObject.Lat,
                    (Importance)planningApplyMaintObject.Importance, planningApplyMaintObject.DetailedAddress, planningApplyMaintObject.Remarks, planningApplyMaintObject.CreateUserId);
                planningApplyRepository.Add(planningApply);
            }
            else
            {
                PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
                if (planningApply != null)
                {
                    planningApply.CheckByUpdate(planningApplyMaintObject.ModifyUserId);
                    planningApply.Modify(planningApplyMaintObject.PlanningName, planningApplyMaintObject.ReseauId, planningApplyMaintObject.Lng, planningApplyMaintObject.Lat,
                        (Importance)planningApplyMaintObject.Importance, planningApplyMaintObject.DetailedAddress, planningApplyMaintObject.Remarks, planningApplyMaintObject.ModifyUserId);
                    planningApplyRepository.Update(planningApply);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlanningApplyCode"))
                {
                    throw new ApplicationFault("规划编码重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除建设申请
        /// </summary>
        /// <param name="planningMaintObjects">要删除的建设申请维护对象列表</param>
        public void RemovePlanningApplys(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            {
                PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
                if (planningApply != null)
                {
                    planningApply.CheckByRemove(planningApplyMaintObject.ModifyUserId);
                    planningApplyRepository.Remove(planningApply);
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
        /// 保存业务审核
        /// </summary>
        /// <param name="planningMaintObject">要修改的建设申请维护对象</param>
        public void SaveBusinessAudit(PlanningApplyMaintObject planningApplyMaintObject)
        {
            PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
            if (planningApply != null)
            {
                planningApply.SaveBusinessAudit((Importance)planningApplyMaintObject.Importance);
                planningApplyRepository.Update(planningApply);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == planningApplyMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(planningApplyMaintObject.WFActivityInstanceId);
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
        /// 保存技术审核
        /// </summary>
        /// <param name="planningMaintObject">要修改的建设申请维护对象</param>
        public void SaveTechnicalAudit(PlanningApplyMaintObject planningApplyMaintObject)
        {
            PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
            if (planningApply != null)
            {
                planningApply.SaveTechnicalAudit((PlanningAdvice)planningApplyMaintObject.PlanningAdvice, planningApplyMaintObject.PlanningUserId);
                planningApplyRepository.Update(planningApply);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == planningApplyMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(planningApplyMaintObject.WFActivityInstanceId);
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
        /// 保存建设申请单
        /// </summary>
        /// <param name="planningApplyHeaderMaintObject">要新增的建设申请单维护对象</param>
        /// <param name="planningApplyMaintObjects">要修改的建设申请维护对象</param>
        public void SavePlanningApplyHeader(PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject, IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            //PlanningApplyHeader planningApplyHeader = AggregateFactory.CreatePlanningApplyHeader(planningApplyHeaderMaintObject.Title, planningApplyHeaderMaintObject.CreateUserId);
            //planningApplyHeaderRepository.Add(planningApplyHeader);
            //foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            //{
            //    PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
            //    if (planningApply != null)
            //    {
            //        if (planningApply.ParentId != Guid.Empty)
            //        {
            //            throw new ApplicationFault("选择的记录已发起建设申请");
            //        }
            //        else
            //        {
            //            planningApply.ParentId = planningApplyHeader.Id;
            //            planningApplyRepository.Update(planningApply);
            //        }
            //    }
            //}
            //try
            //{
            //    this.Context.Commit();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// 根据基站建设申请单获取相关联的基站建设申请
        /// </summary>
        /// <param name="id">基站建设申请单Id</param>
        /// <returns></returns>
        public string GetPlanningApplysByHeaderId(Guid id)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = id });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetPlanningApplysByHeaderId", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 取消关联基站建设申请
        /// </summary>
        /// <param name="planningMaintObjects">要删除的建设申请维护对象列表</param>
        public void RemovePlanningApplyDetail(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            //foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            //{
            //    PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
            //    if (planningApply != null)
            //    {
            //        Guid parentId = planningApply.ParentId;
            //        planningApply.CheckByRemoveDetail(planningApplyMaintObject.ModifyUserId);
            //        planningApply.ParentId = Guid.Empty;
            //        planningApplyRepository.Update(planningApply);
            //    }
            //}
            //try
            //{
            //    this.Context.Commit();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// 删除建设申请单
        /// </summary>
        /// <param name="planningMaintObjects">要删除的建设申请维护对象列表</param>
        public void RemovePlanningApplyHeaders(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            //ArrayList al = new ArrayList();
            //foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            //{
            //    PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
            //    if (planningApply != null)
            //    {
            //        planningApply.CheckByRemove(planningApplyMaintObject.ModifyUserId);
            //        if (!al.Contains(planningApply.ParentId))
            //        {
            //            al.Add(planningApply.ParentId);
            //        }
            //        planningApply.ParentId = Guid.Empty;
            //        planningApplyRepository.Update(planningApply);
            //    }
            //}
            //if (al.Count > 0)
            //{
            //    for (int i = 0; i < al.Count; i++)
            //    {
            //        PlanningApplyHeader planningApplyHeader = planningApplyHeaderRepository.GetByKey(Guid.Parse(al[i].ToString()));
            //        planningApplyHeaderRepository.Remove(planningApplyHeader);
            //    }
            //}
            //try
            //{
            //    this.Context.Commit();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// 保存规划建议
        /// </summary>
        /// <param name="planningApplyMaintObjects">要修改的建设申请维护对象</param>
        public void SavePlanningAdvice(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            {
                PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
                if (planningApply != null)
                {
                    planningApply.CheckByPlanningAdvice(planningApplyMaintObject.ModifyUserId);
                    planningApply.PlanningAdvice = (PlanningAdvice)planningApplyMaintObject.PlanningAdvice;
                    planningApply.DoState = DoState.已处理;
                    planningApplyRepository.Update(planningApply);

                    if ((PlanningAdvice)planningApplyMaintObject.PlanningAdvice == PlanningAdvice.列入规划)
                    {
                        Planning planning = AggregateFactory.CreatePlanning(codeSeedRepository.GenerateCode("Planning"), planningApply.PlanningName, (Profession)planningApply.Profession,
                        Guid.Empty, planningApply.ReseauId, planningApply.Lng, planningApply.Lat, planningApply.DetailedAddress, planningApply.Remarks,
                        "", "", (Importance)planningApply.Importance, Guid.Empty, planningApply.PlanningUserId);
                        planningRepository.Add(planning);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlanningCode"))
                {
                    throw new ApplicationFault("规划编码重复");
                }
                if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 指定网优人员
        /// </summary>
        /// <param name="planningMaintObjects">要修改的建设申请维护对象列表</param>
        public void AppointPlanningUser(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            {
                PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
                if (planningApply != null)
                {
                    planningApply.CheckByUpdate(planningApplyMaintObject.ModifyUserId);
                    planningApply.PlanningUserId = planningApplyMaintObject.PlanningUserId;
                    planningApplyRepository.Update(planningApply);
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
        /// 提交
        /// </summary>
        /// <param name="planningMaintObjects">要修改的建设申请维护对象列表</param>
        public void IssuePlanningApply(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            {
                PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
                if (planningApply != null)
                {
                    planningApply.CheckByIssued(planningApplyMaintObject.ModifyUserId);
                    planningApply.Issued = Bool.是;
                    planningApplyRepository.Update(planningApply);
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
        /// 取消提交
        /// </summary>
        /// <param name="planningMaintObjects">要修改的建设申请维护对象列表</param>
        public void CancelIssuePlanningApply(IList<PlanningApplyMaintObject> planningApplyMaintObjects)
        {
            foreach (PlanningApplyMaintObject planningApplyMaintObject in planningApplyMaintObjects)
            {
                PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyMaintObject.Id);
                if (planningApply != null)
                {
                    planningApply.CheckByCancelIssued(planningApplyMaintObject.ModifyUserId);
                    planningApply.Issued = Bool.否;
                    planningApply.PlanningUserId = Guid.Empty;
                    planningApplyRepository.Update(planningApply);
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
        /// 根据条件获取分页待处理建设申请列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="doState">处理状态</param>
        /// <param name="createUserId">申请人</param>
        /// <param name="planningUserId">网优人员</param>
        /// <param name="profession">专业</param>
        /// <returns>分页建设申请列表的Json字符串</returns>
        public string GetPlanningApplysWaitPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningName, Guid areaId, Guid reseauId,
            int doState, Guid createUserId, Guid planningUserId, int profession)
        {
            List<Parameter> parameters = new List<Parameter>(18);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "DoState", Type = SqlDbType.Int, Value = doState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            parameters.Add(new Parameter() { Name = "PlanningUserId", Type = SqlDbType.UniqueIdentifier, Value = planningUserId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryPlanningApplysWaitPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }
    }
}
