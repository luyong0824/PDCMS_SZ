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
using System.Configuration;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 规划应用层服务
    /// </summary>
    public class PlanningService : DataService, IPlanningService
    {
        private const int bufferSize = 4096;
        private string baseUploadFolder = ConfigurationManager.AppSettings["baseUploadFolder"];
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<User> userRepository;
        private readonly ICodeSeedRepository codeSeedRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IRepository<OperatorsConfirm> operatorsConfirmRepository;
        private readonly IRepository<OperatorsConfirmDetail> operatorsConfirmDetailRepository;
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly IRepository<File> fileRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<PlaceDesign> placeDesignRepository;
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;
        private readonly IRepository<Notice> noticeRepository;

        public PlanningService(IRepositoryContext context,
            IRepository<Planning> planningRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<User> userRepository,
            ICodeSeedRepository codeSeedRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IRepository<OperatorsConfirm> operatorsConfirmRepository,
            IRepository<OperatorsConfirmDetail> operatorsConfirmDetailRepository,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            IRepository<File> fileRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<PlaceDesign> placeDesignRepository,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<PlacePropertyLog> placePropertyLogRepository,
            IRepository<Notice> noticeRepository)
            : base(context)
        {
            this.planningRepository = planningRepository;
            this.reseauRepository = reseauRepository;
            this.userRepository = userRepository;
            this.codeSeedRepository = codeSeedRepository;
            this.addressingRepository = addressingRepository;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.operatorsConfirmRepository = operatorsConfirmRepository;
            this.operatorsConfirmDetailRepository = operatorsConfirmDetailRepository;
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.fileRepository = fileRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.placeDesignRepository = placeDesignRepository;
            this.placePropertyRepository = placePropertyRepository;
            this.placePropertyLogRepository = placePropertyLogRepository;
            this.noticeRepository = noticeRepository;
        }

        /// <summary>
        /// 根据规划Id获取规划
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns>规划维护对象</returns>
        public PlanningMaintObject GetPlanningById(Guid id)
        {
            Planning planning = planningRepository.FindByKey(id);
            if (planning != null)
            {
                PlanningMaintObject planningMaintObject = MapperHelper.Map<Planning, PlanningMaintObject>(planning);
                Reseau reseau = reseauRepository.GetByKey(planningMaintObject.ReseauId);
                planningMaintObject.Id = id;
                planningMaintObject.PlaceId = planning.PlaceId;
                planningMaintObject.CreateDateText = planning.CreateDate.ToShortDateString();
                planningMaintObject.PlanningCode = planning.PlanningCode;
                planningMaintObject.PlanningName = planning.PlanningName;
                planningMaintObject.PlaceCategoryId = planning.PlaceCategoryId;
                planningMaintObject.AreaId = reseau.AreaId;
                planningMaintObject.ReseauId = planning.ReseauId;
                planningMaintObject.Lng = planning.Lng;
                planningMaintObject.Lat = planning.Lat;
                planningMaintObject.Importance = (int)planning.Importance;
                planningMaintObject.PlaceOwner = planning.PlaceOwner;
                planningMaintObject.DetailedAddress = planning.DetailedAddress;
                planningMaintObject.Remarks = planning.Remarks;
                planningMaintObject.OptionalAddress = planning.OptionalAddress;
                planningMaintObject.ProposedNetwork = planning.ProposedNetwork;
                planningMaintObject.FileIdList = "";
                planningMaintObject.Count = 0;
                FileAssociation planningFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.Id && entity.EntityName == "Planning"));
                if (planningFileAssociation != null)
                {
                    int count = 0;
                    if (planningFileAssociation.FileIdList != "")
                    {
                        if (planningFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = planningFileAssociation.FileIdList.Split(',');
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
                    planningMaintObject.Count = count;
                    planningMaintObject.FileIdList = planningFileAssociation.FileIdList;
                }
                else
                {
                    planningMaintObject.Count = 0;
                    planningMaintObject.FileIdList = "";
                }
                return planningMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的规划在系统中不存在");
            }
        }

        /// <summary>
        /// 根据规划Id获取规划(移动端)
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns>规划维护对象</returns>
        public PlanningMaintObject GetPlanningByIdMobile(Guid id, string header)
        {
            Planning planning = planningRepository.FindByKey(id);
            if (planning != null)
            {
                PlanningMaintObject planningMaintObject = MapperHelper.Map<Planning, PlanningMaintObject>(planning);
                Reseau reseau = reseauRepository.GetByKey(planningMaintObject.ReseauId);
                planningMaintObject.Id = id;
                planningMaintObject.PlaceId = planning.PlaceId;
                planningMaintObject.PlanningName = planning.PlanningName;
                planningMaintObject.PlaceCategoryId = planning.PlaceCategoryId;
                planningMaintObject.AreaId = reseau.AreaId;
                planningMaintObject.ReseauId = planning.ReseauId;
                planningMaintObject.Lng = planning.Lng;
                planningMaintObject.Lat = planning.Lat;
                planningMaintObject.PlaceOwner = planning.PlaceOwner;
                planningMaintObject.Importance = (int)planning.Importance;
                planningMaintObject.OptionalAddress = planning.OptionalAddress;
                planningMaintObject.ProposedNetwork = planning.ProposedNetwork;
                planningMaintObject.DetailedAddress = planning.DetailedAddress;

                planningMaintObject.ImageUrl = "";
                FileAssociation planningFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.Id && entity.EntityName == "Planning"));
                if (planningFileAssociation != null && planningFileAssociation.FileIdList != "")
                {
                    if (planningFileAssociation.FileIdList.Contains(","))
                    {
                        string[] strFileList = planningFileAssociation.FileIdList.Split(',');
                        foreach (string fileListId in strFileList)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileListId));
                            if (file.FileExtension == ".jpeg")
                            {
                                string url = file.FilePath.Replace("\\", "/");
                                url = url.Replace(url.Substring(0, 2), header);
                                planningMaintObject.ImageUrl += url + ",";
                            }
                        }
                        planningMaintObject.ImageUrl = planningMaintObject.ImageUrl.Substring(0, planningMaintObject.ImageUrl.Length - 1);
                    }
                    else
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(planningFileAssociation.FileIdList));
                        if (file.FileExtension == ".jpeg")
                        {
                            string url = file.FilePath.Replace("\\", "/");
                            url = url.Replace(url.Substring(0, 2), header);
                            planningMaintObject.ImageUrl = url;
                        }
                    }
                }
                return planningMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的规划在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="issued">是否下达</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <returns>分页规划列表的Json字符串</returns>
        public string GetPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession,
            Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int issued, int addressingState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(18);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Importance", Type = SqlDbType.Int, Value = importance });
            parameters.Add(new Parameter() { Name = "Issued", Type = SqlDbType.Int, Value = issued });
            parameters.Add(new Parameter() { Name = "AddressingState", Type = SqlDbType.Int, Value = addressingState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryPlanningsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页租赁任务分配列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="isAppoint">是否指定租赁人</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <returns>分页租赁任务分配列表的Json字符串</returns>
        public string GetAddressingUsersPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession,
            Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int isAppoint, int addressingState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(18);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Importance", Type = SqlDbType.Int, Value = importance });
            parameters.Add(new Parameter() { Name = "IsAppoint", Type = SqlDbType.Int, Value = isAppoint });
            parameters.Add(new Parameter() { Name = "AddressingState", Type = SqlDbType.Int, Value = addressingState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryAddressingUsersPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改规划
        /// </summary>
        /// <param name="planningMaintObject">要新增或者修改的规划维护对象</param>
        public void AddOrUpdatePlanning(PlanningMaintObject planningMaintObject)
        {
            if (planningMaintObject.Id == Guid.Empty)
            {
                Planning planning = AggregateFactory.CreatePlanning(codeSeedRepository.GenerateCode("Planning"), planningMaintObject.PlanningName, (Profession)planningMaintObject.Profession,
                    planningMaintObject.PlaceCategoryId, planningMaintObject.ReseauId, planningMaintObject.Lng, planningMaintObject.Lat, planningMaintObject.DetailedAddress, planningMaintObject.Remarks,
                    planningMaintObject.ProposedNetwork, planningMaintObject.OptionalAddress, (Importance)planningMaintObject.Importance, planningMaintObject.PlaceOwner, planningMaintObject.CreateUserId);
                planningRepository.Add(planning);

                if (planningMaintObject.FileIdList != "")
                {
                    FileAssociation planningFileAssociation = AggregateFactory.CreateFileAssociation("Planning", planning.Id, planningMaintObject.FileIdList, planningMaintObject.CreateUserId);
                    fileAssociationRepository.Add(planningFileAssociation);
                }
            }
            else
            {
                Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
                if (planning != null)
                {
                    planning.CheckByUpdate(planningMaintObject.ModifyUserId);

                    if (planning.Lng != planningMaintObject.Lng || planning.Lat != planningMaintObject.Lat)
                    {
                        if (planning.AddressingUserId != Guid.Empty && planning.AddressingState == AddressingState.未寻址确认)
                        {
                            string noticeContent = "【" + planning.PlanningName + "】的经纬度有了变动，请您注意查看新的位置，谢谢配合";
                            Notice notice = AggregateFactory.CreateNotice(NoticeType.经纬度变更, planning.Id, planning.Lng, planning.Lat, noticeContent, planning.AddressingUserId, planningMaintObject.ModifyUserId);
                            noticeRepository.Add(notice);
                        }
                    }

                    planning.Modify(planningMaintObject.PlanningName, planningMaintObject.PlaceCategoryId, planningMaintObject.ReseauId, planningMaintObject.Lng, planningMaintObject.Lat,
                        planningMaintObject.DetailedAddress, planningMaintObject.Remarks, planningMaintObject.ProposedNetwork, planningMaintObject.OptionalAddress, (Importance)planningMaintObject.Importance,
                        planningMaintObject.PlaceOwner, planningMaintObject.ModifyUserId);
                    planningRepository.Update(planning);

                    FileAssociation planningFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.Id && entity.EntityName == "Planning"));
                    if (planningFileAssociation == null && planningMaintObject.FileIdList != "")
                    {
                        FileAssociation newPlanningFileAssociation = AggregateFactory.CreateFileAssociation("Planning", planning.Id, planningMaintObject.FileIdList, planningMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newPlanningFileAssociation);
                    }
                    else if (planningFileAssociation != null && planningMaintObject.FileIdList != planningFileAssociation.FileIdList)
                    {
                        planningFileAssociation.Modify(planningMaintObject.FileIdList, planningMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(planningFileAssociation);
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
                //else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                //{
                //    throw new ApplicationFault("选择的站点类型在系统中不存在");
                //}
                if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 租赁主管修改规划
        /// </summary>
        /// <param name="planningMaintObject">要修改的规划维护对象</param>
        public void UpdatePlanningAddressing(PlanningMaintObject planningMaintObject)
        {
            Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
            if (planning != null)
            {
                planning.CheckByUpdateAddressing();
                planning.Modify(planningMaintObject.PlanningName, planningMaintObject.PlaceCategoryId, planningMaintObject.ReseauId, planningMaintObject.Lng, planningMaintObject.Lat,
                    planningMaintObject.DetailedAddress, planningMaintObject.Remarks, planningMaintObject.ProposedNetwork, planningMaintObject.OptionalAddress, (Importance)planningMaintObject.Importance,
                    planningMaintObject.PlaceOwner, planningMaintObject.ModifyUserId);
                planningRepository.Update(planning);

                FileAssociation planningFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.Id && entity.EntityName == "Planning"));
                if (planningFileAssociation == null && planningMaintObject.FileIdList != "")
                {
                    FileAssociation newPlanningFileAssociation = AggregateFactory.CreateFileAssociation("Planning", planning.Id, planningMaintObject.FileIdList, planningMaintObject.ModifyUserId);
                    fileAssociationRepository.Add(newPlanningFileAssociation);
                }
                else if (planningFileAssociation != null && planningMaintObject.FileIdList != planningFileAssociation.FileIdList)
                {
                    planningFileAssociation.Modify(planningMaintObject.FileIdList, planningMaintObject.ModifyUserId);
                    fileAssociationRepository.Update(planningFileAssociation);
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
                //else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                //{
                //    throw new ApplicationFault("选择的站点类型在系统中不存在");
                //}
                if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除规划
        /// </summary>
        /// <param name="planningMaintObjects">要删除的规划维护对象列表</param>
        public void RemovePlannings(IList<PlanningMaintObject> planningMaintObjects)
        {
            foreach (PlanningMaintObject planningMaintObject in planningMaintObjects)
            {
                Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
                if (planning != null)
                {
                    planning.CheckByRemove(planningMaintObject.ModifyUserId);
                    Addressing addressing = addressingRepository.Find(Specification<Addressing>.Eval(entity => entity.PlanningId == planning.Id));
                    if (addressing != null)
                    {
                        ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == addressing.Id && entity.ProjectType == ProjectType.新建));
                        {
                            if (projectTask != null)
                            {
                                IEnumerable<EngineeringTask> engineeringTasks = engineeringTaskRepository.FindAll(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id));
                                foreach (EngineeringTask engineeringTask in engineeringTasks)
                                {
                                    engineeringTaskRepository.Remove(engineeringTask);
                                }
                                projectTaskRepository.Remove(projectTask);
                            }
                        }
                        addressingRepository.Remove(addressing);
                    }
                    planningRepository.Remove(planning);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Addressing_dbo.tbl_Planning_PlanningId"))
                {
                    throw new ApplicationFault("已生成寻址确认");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 指定租赁人
        /// </summary>
        /// <param name="planningMaintObjects">要指定租赁人的规划维护对象列表</param>
        public void AppointAddressingUser(IList<PlanningMaintObject> planningMaintObjects)
        {
            foreach (PlanningMaintObject planningMaintObject in planningMaintObjects)
            {
                Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
                if (planning != null)
                {
                    planning.CheckByUpdateAddressing();
                    planning.AppointAddressingUser(planningMaintObject.AddressingUserId, planningMaintObject.ModifyUserId);
                    planningRepository.Update(planning);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId"))
                {
                    throw new ApplicationFault("选择的租赁人在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 下达规划
        /// </summary>
        /// <param name="planningMaintObjects">要下达的规划维护对象列表</param>
        public void Issue(IList<PlanningMaintObject> planningMaintObjects)
        {
            foreach (PlanningMaintObject planningMaintObject in planningMaintObjects)
            {
                Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
                if (planning != null)
                {
                    planning.Issue(planningMaintObject.ModifyUserId);
                    planningRepository.Update(planning);
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
        /// 取消下达规划
        /// </summary>
        /// <param name="planningMaintObjects">要取消下达的规划维护对象列表</param>
        public void CancelIssue(IList<PlanningMaintObject> planningMaintObjects)
        {
            foreach (PlanningMaintObject planningMaintObject in planningMaintObjects)
            {
                Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
                if (planning != null)
                {
                    planning.CancelIssue(planningMaintObject.ModifyUserId);
                    planningRepository.Update(planning);
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
        /// 根据条件获取分页规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="telecomDemand">电信需求</param>
        /// <param name="mobileDemand">移动需求</param>
        /// <param name="unicomDemand">联通需求</param>
        /// <param name="demandState">确认状态</param>
        /// <param name="issued">是否下达</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <param name="projectManagerId">工程经理</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <returns>分页规划列表的Json字符串</returns>
        public string GetConstructionPlanningsReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency, int telecomDemand, int mobileDemand, int unicomDemand, int demandState, int issued, int addressingState, Guid createUserId, string placeName, Guid addressingUserId, Guid projectManagerId, int constructionProgress)
        {
            List<Parameter> parameters = new List<Parameter>(22);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Urgency", Type = SqlDbType.Int, Value = urgency });
            parameters.Add(new Parameter() { Name = "TelecomDemand", Type = SqlDbType.Int, Value = telecomDemand });
            parameters.Add(new Parameter() { Name = "MobileDemand", Type = SqlDbType.Int, Value = mobileDemand });
            parameters.Add(new Parameter() { Name = "UnicomDemand", Type = SqlDbType.Int, Value = unicomDemand });
            parameters.Add(new Parameter() { Name = "DemandState", Type = SqlDbType.Int, Value = demandState });
            parameters.Add(new Parameter() { Name = "Issued", Type = SqlDbType.Int, Value = issued });
            parameters.Add(new Parameter() { Name = "AddressingState", Type = SqlDbType.Int, Value = addressingState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AddressingUserId", Type = SqlDbType.UniqueIdentifier, Value = addressingUserId });
            parameters.Add(new Parameter() { Name = "ProjectManagerId", Type = SqlDbType.UniqueIdentifier, Value = projectManagerId });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryConstructionTaskPlanningsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据规划Id获取规划
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns>新模式规划维护对象</returns>
        public NewPlanningMaintObject GetNewPlanningById(Guid id)
        {
            Planning planning = planningRepository.FindByKey(id);
            if (planning != null)
            {
                NewPlanningMaintObject newPlanningMaintObject = MapperHelper.Map<Planning, NewPlanningMaintObject>(planning);
                newPlanningMaintObject.CreateDateText = planning.CreateDate.ToShortDateString();
                Reseau reseau = reseauRepository.GetByKey(newPlanningMaintObject.ReseauId);
                newPlanningMaintObject.AreaId = reseau.AreaId;
                if (planning.AddressingUserId != null)
                {
                    User user = userRepository.GetByKey((Guid)planning.AddressingUserId);
                    newPlanningMaintObject.AddressingUserFullName = user.FullName;
                }

                Addressing addressing = addressingRepository.Find(Specification<Addressing>.Eval(entity => entity.PlanningId == id));
                if (addressing != null)
                {
                    newPlanningMaintObject.OwnerName = addressing.OwnerName;
                    newPlanningMaintObject.OwnerContact = addressing.OwnerContact;
                    newPlanningMaintObject.OwnerPhoneNumber = addressing.OwnerPhoneNumber;

                    FileAssociation addressingFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressing.Id && entity.EntityName == "Addressing"));
                    if (addressingFileAssociation != null)
                    {
                        int count = 0;
                        if (addressingFileAssociation.FileIdList != "")
                        {
                            if (addressingFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = addressingFileAssociation.FileIdList.Split(',');
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
                        newPlanningMaintObject.Count = count;
                        newPlanningMaintObject.FileIdList = addressingFileAssociation.FileIdList;
                    }
                    else
                    {
                        newPlanningMaintObject.Count = 0;
                        newPlanningMaintObject.FileIdList = "";
                    }
                }
                else
                {
                    newPlanningMaintObject.SceneId = Guid.Empty; ;
                    newPlanningMaintObject.OwnerName = "";
                    newPlanningMaintObject.OwnerContact = "";
                    newPlanningMaintObject.OwnerPhoneNumber = "";
                    newPlanningMaintObject.DetailedAddress = "";
                    newPlanningMaintObject.Count = 0;
                    newPlanningMaintObject.FileIdList = "";
                }

                OperatorsConfirmDetail operatorsConfirmDetail = operatorsConfirmDetailRepository.Find(Specification<OperatorsConfirmDetail>.Eval(entity => entity.PlanningId == id));

                OperatorsPlanning operatorsPlanningMobile = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == id && entity.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb"));
                if (operatorsPlanningMobile != null)
                {
                    newPlanningMaintObject.MobileDemand = (int)Bool.是;
                    newPlanningMaintObject.MobileAntennaHeight = operatorsPlanningMobile.AntennaHeight;
                    newPlanningMaintObject.MobilePoleNumber = operatorsPlanningMobile.PoleNumber;
                    newPlanningMaintObject.MobileCabinetNumber = operatorsPlanningMobile.CabinetNumber;
                    newPlanningMaintObject.MobileUserId = operatorsConfirmDetail.MobileConfirmUserId;
                    User mobileUser = userRepository.FindByKey(operatorsConfirmDetail.MobileConfirmUserId);
                    newPlanningMaintObject.MobileUserFullName = mobileUser.FullName;
                }
                else
                {
                    newPlanningMaintObject.MobileDemand = (int)Bool.否;
                    newPlanningMaintObject.MobileAntennaHeight = 0;
                    newPlanningMaintObject.MobilePoleNumber = 0;
                    newPlanningMaintObject.MobileCabinetNumber = 0;
                    newPlanningMaintObject.MobileUserId = Guid.Empty;
                    newPlanningMaintObject.MobileUserFullName = "请选择";
                }
                OperatorsPlanning operatorsPlanningTelecom = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == id && entity.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53"));
                if (operatorsPlanningTelecom != null)
                {
                    newPlanningMaintObject.TelecomDemand = (int)Bool.是;
                    newPlanningMaintObject.TelecomAntennaHeight = operatorsPlanningTelecom.AntennaHeight;
                    newPlanningMaintObject.TelecomPoleNumber = operatorsPlanningTelecom.PoleNumber;
                    newPlanningMaintObject.TelecomCabinetNumber = operatorsPlanningTelecom.CabinetNumber;
                    newPlanningMaintObject.TelecomUserId = operatorsConfirmDetail.TelecomConfirmUserId;
                    User telecomUser = userRepository.FindByKey(operatorsConfirmDetail.TelecomConfirmUserId);
                    newPlanningMaintObject.TelecomUserFullName = telecomUser.FullName;
                }
                else
                {
                    newPlanningMaintObject.TelecomDemand = (int)Bool.否;
                    newPlanningMaintObject.TelecomAntennaHeight = 0;
                    newPlanningMaintObject.TelecomPoleNumber = 0;
                    newPlanningMaintObject.TelecomCabinetNumber = 0;
                    newPlanningMaintObject.TelecomUserId = Guid.Empty;
                    newPlanningMaintObject.TelecomUserFullName = "请选择";
                }
                OperatorsPlanning operatorsPlanningUnicom = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == id && entity.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"));
                if (operatorsPlanningUnicom != null)
                {
                    newPlanningMaintObject.UnicomDemand = (int)Bool.是;
                    newPlanningMaintObject.UnicomAntennaHeight = operatorsPlanningUnicom.AntennaHeight;
                    newPlanningMaintObject.UnicomPoleNumber = operatorsPlanningUnicom.PoleNumber;
                    newPlanningMaintObject.UnicomCabinetNumber = operatorsPlanningUnicom.CabinetNumber;
                    newPlanningMaintObject.UnicomUserId = operatorsConfirmDetail.UnicomConfirmUserId;
                    User unicomUser = userRepository.FindByKey(operatorsConfirmDetail.UnicomConfirmUserId);
                    newPlanningMaintObject.UnicomUserFullName = unicomUser.FullName;
                }
                else
                {
                    newPlanningMaintObject.UnicomDemand = (int)Bool.否;
                    newPlanningMaintObject.UnicomAntennaHeight = 0;
                    newPlanningMaintObject.UnicomPoleNumber = 0;
                    newPlanningMaintObject.UnicomCabinetNumber = 0;
                    newPlanningMaintObject.UnicomUserId = Guid.Empty;
                    newPlanningMaintObject.UnicomUserFullName = "请选择";
                }

                return newPlanningMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的规划在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改规划
        /// </summary>
        /// <param name="newPlanningMaintObject">要新增或者修改的新模式规划维护对象</param>
        public void AddOrUpdateNewPlanning(NewPlanningMaintObject newPlanningMaintObject)
        {
            Demand mobileDemand = (Bool)newPlanningMaintObject.MobileDemand == Bool.是 ? Demand.需要 : Demand.不需要;
            Demand telecomDemand = (Bool)newPlanningMaintObject.TelecomDemand == Bool.是 ? Demand.需要 : Demand.不需要;
            Demand unicomDemand = (Bool)newPlanningMaintObject.UnicomDemand == Bool.是 ? Demand.需要 : Demand.不需要;

            Reseau reseau = reseauRepository.FindByKey(newPlanningMaintObject.ReseauId);
            if (newPlanningMaintObject.Id == Guid.Empty)
            {
                //新增规划记录
                Planning planning = AggregateFactory.CreatePlanning(codeSeedRepository.GenerateCode("Planning"), newPlanningMaintObject.PlanningName, (Profession)newPlanningMaintObject.Profession,
                    newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.ReseauId, newPlanningMaintObject.Lng, newPlanningMaintObject.Lat, "", "", "", "", Importance.C,
                    Guid.Empty, newPlanningMaintObject.CreateUserId);
                planningRepository.Add(planning);

                if (mobileDemand == Demand.需要)
                {
                    //新增运营商规划记录
                    OperatorsPlanning opMobile = AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), newPlanningMaintObject.PlanningName,
                    (Profession)newPlanningMaintObject.Profession, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                    newPlanningMaintObject.Lat, newPlanningMaintObject.MobileAntennaHeight, newPlanningMaintObject.MobilePoleNumber, newPlanningMaintObject.MobileCabinetNumber,
                    Urgency.一级, Bool.是, "", Guid.Parse("6365f3de-0fc5-4930-a321-2350ee6269bb"), planning.Id, newPlanningMaintObject.MobileUserId, CompanyNature.运营商);
                    operatorsPlanningRepository.Add(opMobile);
                }
                if (telecomDemand == Demand.需要)
                {
                    OperatorsPlanning opTelecom = AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), newPlanningMaintObject.PlanningName,
                    (Profession)newPlanningMaintObject.Profession, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                    newPlanningMaintObject.Lat, newPlanningMaintObject.TelecomAntennaHeight, newPlanningMaintObject.TelecomPoleNumber, newPlanningMaintObject.TelecomCabinetNumber,
                    Urgency.一级, Bool.是, "", Guid.Parse("2e0ffe5f-c03a-4767-9915-9683f0db0b53"), planning.Id, newPlanningMaintObject.TelecomUserId, CompanyNature.运营商);
                    operatorsPlanningRepository.Add(opTelecom);
                }
                if (unicomDemand == Demand.需要)
                {
                    OperatorsPlanning opUnicom = AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), newPlanningMaintObject.PlanningName,
                    (Profession)newPlanningMaintObject.Profession, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                    newPlanningMaintObject.Lat, newPlanningMaintObject.UnicomAntennaHeight, newPlanningMaintObject.UnicomPoleNumber, newPlanningMaintObject.UnicomCabinetNumber,
                    Urgency.一级, Bool.是, "", Guid.Parse("0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"), planning.Id, newPlanningMaintObject.UnicomUserId, CompanyNature.运营商);
                    operatorsPlanningRepository.Add(opUnicom);
                }

                //请求运营商确认
                OperatorsConfirm operatorsConfirm = AggregateFactory.CreateOperatorsConfirm(newPlanningMaintObject.CreateUserId);
                operatorsConfirmRepository.Add(operatorsConfirm);
                OperatorsConfirmDetail operatorsConfirmDetail = AggregateFactory.CreateOperatorsConfirmDetail(operatorsConfirm.Id, planning.Id, mobileDemand, telecomDemand,
                    unicomDemand, newPlanningMaintObject.MobileUserId, newPlanningMaintObject.TelecomUserId, newPlanningMaintObject.UnicomUserId, newPlanningMaintObject.CreateUserId);
                operatorsConfirmDetailRepository.Add(operatorsConfirmDetail);

                //Bool telecomShare = (Demand)planning.TelecomDemand == Demand.需要 ? Bool.是 : Bool.否;
                //Bool mobileShare = (Demand)planning.MobileDemand == Demand.需要 ? Bool.是 : Bool.否;
                //Bool unicomShare = (Demand)planning.UnicomDemand == Demand.需要 ? Bool.是 : Bool.否;
                //新增寻址确认记录
                Addressing addressing = AggregateFactory.CreateAddressing(planning.Id, newPlanningMaintObject.PlanningName, Guid.Empty, "",
                    newPlanningMaintObject.OwnerName, newPlanningMaintObject.OwnerContact, newPlanningMaintObject.OwnerPhoneNumber,
                    "", newPlanningMaintObject.CreateUserId);
                addressingRepository.Add(addressing);
                PlaceDesign placeDesign = AggregateFactory.CreatePlaceDesign(addressing.Id, PropertyType.寻址设计, newPlanningMaintObject.CreateUserId);
                placeDesignRepository.Add(placeDesign);
                PlaceProperty placeProperty = AggregateFactory.CreatePlaceProperty(addressing.Id, PropertyType.寻址设计, Bool.否, newPlanningMaintObject.MobilePoleNumber, newPlanningMaintObject.MobileCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.TelecomPoleNumber, newPlanningMaintObject.TelecomCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.UnicomPoleNumber, newPlanningMaintObject.UnicomCabinetNumber, 0, Guid.Empty);
                placePropertyRepository.Add(placeProperty);

                PlacePropertyLog mobilePlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, addressing.Id, PropertyType.寻址设计, CompanyNameId.移动, Bool.否, newPlanningMaintObject.MobilePoleNumber, newPlanningMaintObject.MobileCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.TelecomPoleNumber, newPlanningMaintObject.TelecomCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.UnicomPoleNumber, newPlanningMaintObject.UnicomCabinetNumber, 0, Guid.Empty);
                placePropertyLogRepository.Add(mobilePlacePropertyLog);

                PlacePropertyLog telecomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, addressing.Id, PropertyType.寻址设计, CompanyNameId.电信, Bool.否, newPlanningMaintObject.MobilePoleNumber, newPlanningMaintObject.MobileCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.TelecomPoleNumber, newPlanningMaintObject.TelecomCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.UnicomPoleNumber, newPlanningMaintObject.UnicomCabinetNumber, 0, Guid.Empty);
                placePropertyLogRepository.Add(telecomPlacePropertyLog);

                PlacePropertyLog unicomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, addressing.Id, PropertyType.寻址设计, CompanyNameId.联通, Bool.否, newPlanningMaintObject.MobilePoleNumber, newPlanningMaintObject.MobileCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.TelecomPoleNumber, newPlanningMaintObject.TelecomCabinetNumber, 0, Guid.Empty, Bool.否, newPlanningMaintObject.UnicomPoleNumber, newPlanningMaintObject.UnicomCabinetNumber, 0, Guid.Empty);
                placePropertyLogRepository.Add(unicomPlacePropertyLog);

                if (newPlanningMaintObject.FileIdList != "")
                {
                    FileAssociation addressingFileAssociation = AggregateFactory.CreateFileAssociation("Addressing", addressing.Id, newPlanningMaintObject.FileIdList, newPlanningMaintObject.CreateUserId);
                    fileAssociationRepository.Add(addressingFileAssociation);
                }
            }
            else
            {
                Planning planning = planningRepository.FindByKey(newPlanningMaintObject.Id);
                if (planning != null)
                {
                    //验证是否可修改
                    //planning.CheckByAddAddressingNew(newPlanningMaintObject.ModifyUserId);
                    //修改规划
                    planning.Modify(newPlanningMaintObject.PlanningName, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.ReseauId, newPlanningMaintObject.Lng, newPlanningMaintObject.Lat,
                        "", "", "", "", Importance.C, Guid.Empty, newPlanningMaintObject.ModifyUserId);
                    planningRepository.Update(planning);

                    Addressing addressing = addressingRepository.Find(Specification<Addressing>.Eval(entity => entity.PlanningId == planning.Id));
                    if (addressing != null)
                    {
                        //修改寻址确认
                        addressing.Modify(newPlanningMaintObject.PlanningName, Guid.Empty, "",
                            newPlanningMaintObject.OwnerName, newPlanningMaintObject.OwnerContact, newPlanningMaintObject.OwnerPhoneNumber, "",
                            newPlanningMaintObject.ModifyUserId);
                        addressingRepository.Update(addressing);

                        FileAssociation addressingFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressing.Id && entity.EntityName == "Addressing"));
                        if (addressingFileAssociation == null && newPlanningMaintObject.FileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("Addressing", addressing.Id, newPlanningMaintObject.FileIdList, newPlanningMaintObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (addressingFileAssociation != null && newPlanningMaintObject.FileIdList != addressingFileAssociation.FileIdList)
                        {
                            addressingFileAssociation.Modify(newPlanningMaintObject.FileIdList, newPlanningMaintObject.ModifyUserId);
                            fileAssociationRepository.Update(addressingFileAssociation);
                        }
                    }

                    OperatorsConfirmDetail operatorsConfirmDetail = operatorsConfirmDetailRepository.Find(Specification<OperatorsConfirmDetail>.Eval(entity => entity.PlanningId == planning.Id));
                    if (operatorsConfirmDetail != null)
                    {
                        if (operatorsConfirmDetail.MobileDemand != mobileDemand)
                        {
                            if (mobileDemand == Demand.需要)
                            {
                                //新增运营商规划记录
                                OperatorsPlanning opMobile = AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), newPlanningMaintObject.PlanningName,
                                (Profession)newPlanningMaintObject.Profession, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                                newPlanningMaintObject.Lat, newPlanningMaintObject.MobileAntennaHeight, newPlanningMaintObject.MobilePoleNumber, newPlanningMaintObject.MobileCabinetNumber,
                                Urgency.一级, Bool.是, "", Guid.Parse("6365f3de-0fc5-4930-a321-2350ee6269bb"), planning.Id, newPlanningMaintObject.MobileUserId, CompanyNature.运营商);
                                operatorsPlanningRepository.Add(opMobile);
                                operatorsConfirmDetail.Confirm(newPlanningMaintObject.MobileUserId, Guid.Parse("6365f3de-0fc5-4930-a321-2350ee6269bb"), CompanyNature.运营商, mobileDemand);
                                operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                            }
                            else
                            {
                                if (operatorsConfirmDetail.MobileDemand != Demand.未确认)
                                {
                                    OperatorsPlanning opMobile = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb"));
                                    operatorsPlanningRepository.Remove(opMobile);
                                    operatorsConfirmDetail.Confirm(operatorsConfirmDetail.MobileConfirmUserId, Guid.Parse("6365f3de-0fc5-4930-a321-2350ee6269bb"), CompanyNature.运营商, mobileDemand);
                                    operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                                }
                            }
                            //planning.ModifyDemand(mobileDemand, 1);
                        }
                        else
                        {
                            if (mobileDemand == Demand.需要)
                            {
                                OperatorsPlanning opMobile = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb"));
                                if (opMobile != null)
                                {
                                    opMobile.Modify(newPlanningMaintObject.PlanningName, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                                        newPlanningMaintObject.Lat, newPlanningMaintObject.MobileAntennaHeight, newPlanningMaintObject.MobilePoleNumber, newPlanningMaintObject.MobileCabinetNumber,
                                        Urgency.一级, "", newPlanningMaintObject.ModifyUserId);
                                    operatorsPlanningRepository.Update(opMobile);
                                    operatorsConfirmDetail.Confirm(operatorsConfirmDetail.MobileConfirmUserId, Guid.Parse("6365f3de-0fc5-4930-a321-2350ee6269bb"), CompanyNature.运营商, mobileDemand);
                                    operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                                }
                            }
                        }
                        if (operatorsConfirmDetail.TelecomDemand != telecomDemand)
                        {
                            if (telecomDemand == Demand.需要)
                            {
                                OperatorsPlanning opTelecom = AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), newPlanningMaintObject.PlanningName,
                                (Profession)newPlanningMaintObject.Profession, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                                newPlanningMaintObject.Lat, newPlanningMaintObject.TelecomAntennaHeight, newPlanningMaintObject.TelecomPoleNumber, newPlanningMaintObject.TelecomCabinetNumber,
                                Urgency.一级, Bool.是, "", Guid.Parse("2e0ffe5f-c03a-4767-9915-9683f0db0b53"), planning.Id, newPlanningMaintObject.TelecomUserId, CompanyNature.运营商);
                                operatorsPlanningRepository.Add(opTelecom);
                                operatorsConfirmDetail.Confirm(newPlanningMaintObject.TelecomUserId, Guid.Parse("2e0ffe5f-c03a-4767-9915-9683f0db0b53"), CompanyNature.运营商, telecomDemand);
                                operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                            }
                            else
                            {
                                if (operatorsConfirmDetail.TelecomDemand != Demand.未确认)
                                {
                                    OperatorsPlanning opTelecom = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53"));
                                    operatorsPlanningRepository.Remove(opTelecom);
                                    operatorsConfirmDetail.Confirm(operatorsConfirmDetail.TelecomConfirmUserId, Guid.Parse("2e0ffe5f-c03a-4767-9915-9683f0db0b53"), CompanyNature.运营商, telecomDemand);
                                    operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                                }
                            }
                            //planning.ModifyDemand(telecomDemand, 2);
                        }
                        else
                        {
                            if (telecomDemand == Demand.需要)
                            {
                                OperatorsPlanning opTelecom = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53"));
                                if (opTelecom != null)
                                {
                                    opTelecom.Modify(newPlanningMaintObject.PlanningName, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                                        newPlanningMaintObject.Lat, newPlanningMaintObject.TelecomAntennaHeight, newPlanningMaintObject.TelecomPoleNumber, newPlanningMaintObject.TelecomCabinetNumber,
                                        Urgency.一级, "", newPlanningMaintObject.ModifyUserId);
                                    operatorsPlanningRepository.Update(opTelecom);
                                    operatorsConfirmDetail.Confirm(operatorsConfirmDetail.TelecomConfirmUserId, Guid.Parse("2e0ffe5f-c03a-4767-9915-9683f0db0b53"), CompanyNature.运营商, telecomDemand);
                                    operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                                }
                            }
                        }
                        if (operatorsConfirmDetail.UnicomDemand != unicomDemand)
                        {
                            if (unicomDemand == Demand.需要)
                            {
                                OperatorsPlanning opUnicom = AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), newPlanningMaintObject.PlanningName,
                                (Profession)newPlanningMaintObject.Profession, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                                newPlanningMaintObject.Lat, newPlanningMaintObject.UnicomAntennaHeight, newPlanningMaintObject.UnicomPoleNumber, newPlanningMaintObject.UnicomCabinetNumber,
                                Urgency.一级, Bool.是, "", Guid.Parse("0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"), planning.Id, newPlanningMaintObject.UnicomUserId, CompanyNature.运营商);
                                operatorsPlanningRepository.Add(opUnicom);
                                operatorsConfirmDetail.Confirm(newPlanningMaintObject.UnicomUserId, Guid.Parse("0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"), CompanyNature.运营商, unicomDemand);
                                operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                            }
                            else
                            {
                                if (operatorsConfirmDetail.UnicomDemand != Demand.未确认)
                                {
                                    OperatorsPlanning opUnicom = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"));
                                    operatorsPlanningRepository.Remove(opUnicom);
                                    operatorsConfirmDetail.Confirm(operatorsConfirmDetail.UnicomConfirmUserId, Guid.Parse("0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"), CompanyNature.运营商, unicomDemand);
                                    operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                                }
                            }
                            //planning.ModifyDemand(unicomDemand, 3);
                        }
                        else
                        {
                            if (unicomDemand == Demand.需要)
                            {
                                OperatorsPlanning opUnicom = operatorsPlanningRepository.Find(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"));
                                if (opUnicom != null)
                                {
                                    opUnicom.Modify(newPlanningMaintObject.PlanningName, newPlanningMaintObject.PlaceCategoryId, newPlanningMaintObject.AreaId, newPlanningMaintObject.Lng,
                                        newPlanningMaintObject.Lat, newPlanningMaintObject.UnicomAntennaHeight, newPlanningMaintObject.UnicomPoleNumber, newPlanningMaintObject.UnicomCabinetNumber,
                                        Urgency.一级, "", newPlanningMaintObject.ModifyUserId);
                                    operatorsPlanningRepository.Update(opUnicom);
                                    operatorsConfirmDetail.Confirm(operatorsConfirmDetail.UnicomConfirmUserId, Guid.Parse("0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"), CompanyNature.运营商, unicomDemand);
                                    operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                                }
                            }
                        }
                        planningRepository.Update(planning);
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的规划记录在系统中不存在");
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
                else if (ex.Message.Contains("FK_dbo.tbl_Addressing_dbo.tbl_Planning_PlanningId"))
                {
                    throw new ApplicationFault("选择的寻址确认在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除规划
        /// </summary>
        /// <param name="planningMaintObjects">要删除的新模式规划维护对象列表</param>
        public void RemoveNewPlannings(IList<NewPlanningMaintObject> newPlanningMaintObjects)
        {
            foreach (NewPlanningMaintObject newPlanningMaintObject in newPlanningMaintObjects)
            {
                Planning planning = planningRepository.FindByKey(newPlanningMaintObject.Id);
                if (planning != null)
                {
                    planning.CheckRemoveAddressing(newPlanningMaintObject.ModifyUserId);
                    //删除规划
                    planningRepository.Remove(planning);

                    IEnumerable<OperatorsPlanning> operatorsPlannings = operatorsPlanningRepository.FindAll(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id));
                    if (operatorsPlannings != null)
                    {
                        foreach (OperatorsPlanning operatorsPlanning in operatorsPlannings)
                        {
                            //删除运营商规划
                            operatorsPlanningRepository.Remove(operatorsPlanning);
                        }
                    }

                    //删除寻址确认
                    Addressing addressing = addressingRepository.Find(Specification<Addressing>.Eval(entity => entity.PlanningId == planning.Id));
                    if (addressing != null)
                    {
                        addressingRepository.Remove(addressing);
                    }

                    IEnumerable<OperatorsConfirmDetail> operatorsConfirmDetails = operatorsConfirmDetailRepository.FindAll(Specification<OperatorsConfirmDetail>.Eval(entity => entity.PlanningId == planning.Id));
                    if (operatorsConfirmDetails != null)
                    {
                        foreach (OperatorsConfirmDetail operatorsConfirmDetail in operatorsConfirmDetails)
                        {
                            OperatorsConfirm operatorsConfirm = operatorsConfirmRepository.FindByKey(operatorsConfirmDetail.OperatorsConfirmId);
                            if (operatorsConfirm != null)
                            {
                                //删除需求确认单
                                operatorsConfirmRepository.Remove(operatorsConfirm);
                            }
                            //删除运营商需求确认
                            operatorsConfirmDetailRepository.Remove(operatorsConfirmDetail);
                        }
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Planning_PlanningId"))
                {
                    throw new ApplicationFault("已关联运营商规划");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_Planning_PlanningId"))
                {
                    throw new ApplicationFault("已发起运营商确认");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Addressing_dbo.tbl_Planning_PlanningId"))
                {
                    throw new ApplicationFault("已生成寻址确认");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 根据条件获取分页规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="sceneId">周边环境</param>
        /// <param name="telecomDemand">电信需求</param>
        /// <param name="mobileDemand">移动需求</param>
        /// <param name="unicomDemand">联通需求</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <returns>分页规划列表的Json字符串</returns>
        public string GetNewPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid sceneId, int telecomDemand, int mobileDemand, int unicomDemand, int addressingState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(18);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "SceneId", Type = SqlDbType.UniqueIdentifier, Value = sceneId });
            parameters.Add(new Parameter() { Name = "TelecomDemand", Type = SqlDbType.Int, Value = telecomDemand });
            parameters.Add(new Parameter() { Name = "MobileDemand", Type = SqlDbType.Int, Value = mobileDemand });
            parameters.Add(new Parameter() { Name = "UnicomDemand", Type = SqlDbType.Int, Value = unicomDemand });
            parameters.Add(new Parameter() { Name = "AddressingState", Type = SqlDbType.Int, Value = addressingState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryNewPlanningsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取规划列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <param name="planningName">规划点名称</param>
        /// <returns>分页规划点列表的Json字符串</returns>
        public string GetPlanningsMobile(int pageIndex, int pageSize, string professionListSql, string planningName, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProfessionListSql", Type = SqlDbType.VarChar, Value = professionListSql });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetPlanningsMobile", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取规划列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <returns>分页规划点列表的Json字符串</returns>
        public string GetPlanningsPageMobile(int pageIndex, int pageSize, string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProfessionListSql", Type = SqlDbType.VarChar, Value = professionListSql });
            parameters.Add(new Parameter() { Name = "Lng", Type = SqlDbType.Decimal, Value = lng });
            parameters.Add(new Parameter() { Name = "Lat", Type = SqlDbType.Decimal, Value = lat });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetPlanningsPageMobile", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 更新规划点方位(移动端)
        /// </summary>
        /// <param name="placeMaintObject">要修改的规划点点维护对象</param>
        public void SavePlanningPositionMobile(PlanningMaintObject planningMaintObject)
        {
            if (planningMaintObject.Id != Guid.Empty)
            {
                Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
                if (planning != null)
                {
                    planning.Lng = planningMaintObject.Lng;
                    planning.Lat = planningMaintObject.Lat;
                    planningRepository.Update(planning);
                }
                else
                {
                    throw new ApplicationFault("该规划点在系统中不存在");
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
        /// 规划点修改(移动端)
        /// </summary>
        /// <param name="planningMaintObject">规划点维护对象</param>
        public void SavePlanningMobile(PlanningMaintObject planningMaintObject)
        {
            if (planningMaintObject.Id != Guid.Empty)
            {
                Planning planning = planningRepository.FindByKey(planningMaintObject.Id);
                if (planning != null)
                {
                    if (planning.CreateUserId != planningMaintObject.ModifyUserId)
                    {
                        throw new ApplicationFault("无法修改他人的规划点");
                    }
                    planning.PlanningName = planningMaintObject.PlanningName;
                    planning.PlaceCategoryId = planningMaintObject.PlaceCategoryId;
                    planning.ReseauId = planningMaintObject.ReseauId;
                    planning.Lng = planningMaintObject.Lng;
                    planning.Lat = planningMaintObject.Lat;
                    planning.Importance = (Importance)planningMaintObject.Importance;
                    planning.ProposedNetwork = planningMaintObject.ProposedNetwork;
                    planning.OptionalAddress = planningMaintObject.OptionalAddress;
                    planning.DetailedAddress = planningMaintObject.DetailedAddress;
                    planningRepository.Update(planning);

                    if (planningMaintObject.Base64String.Length > 0)
                    {
                        string fileIdList = "";
                        foreach (string base64 in planningMaintObject.Base64String)
                        {
                            if (base64.Contains(","))
                            {
                                string base64Content = base64.Split(',')[1];
                                byte[] bt = Convert.FromBase64String(base64Content);
                                using (System.IO.MemoryStream stream = new System.IO.MemoryStream(bt))
                                {
                                    Guid fileId = Guid.NewGuid();
                                    DateTime today = DateTime.Now;
                                    string uploadFolder = System.IO.Path.Combine(baseUploadFolder, string.Format("{0}-{1}-{2}", today.Year, today.Month, today.Day));
                                    FileHelper.CreateDirectory(uploadFolder);
                                    string filePath = System.IO.Path.Combine(uploadFolder, Guid.NewGuid() + ".jpeg");
                                    FileHelper.UploadFile(stream, filePath, bufferSize);
                                    string fileName = fileId.ToString().Replace("-", "");
                                    File file = AggregateFactory.CreateFile(fileId, fileName, "application/octet-stream",
                                        ".jpeg", bt.Length, filePath, planningMaintObject.ModifyUserId);
                                    fileRepository.Add(file);

                                    fileIdList += fileId + ",";
                                }
                            }
                        }
                        FileAssociation planningFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.Id && entity.EntityName == "Planning"));
                        if (planningFileAssociation == null && fileIdList != "")
                        {
                            FileAssociation newGeneralDesignFileAssociation = AggregateFactory.CreateFileAssociation("Planning", planning.Id, fileIdList.Substring(0, fileIdList.Length - 1), planningMaintObject.ModifyUserId);
                            fileAssociationRepository.Add(newGeneralDesignFileAssociation);
                        }
                        else if (planningFileAssociation != null && fileIdList != "")
                        {
                            if (planningFileAssociation.FileIdList != "")
                            {
                                fileIdList = planningFileAssociation.FileIdList + "," + fileIdList.Substring(0, fileIdList.Length - 1);
                            }
                            else
                            {
                                fileIdList = fileIdList.Substring(0, fileIdList.Length - 1);
                            }
                            planningFileAssociation.Modify(fileIdList, planningMaintObject.ModifyUserId);
                            fileAssociationRepository.Update(planningFileAssociation);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("该规划点在系统中不存在");
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
