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
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories.BaseData;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 改造应用层服务
    /// </summary>
    public class RemodelingService : DataService, IRemodelingService
    {
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Scene> sceneRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<PlaceCategory> placeCategoryRepository;
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<OperatorsSharing> operatorsSharingRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<PlaceDesign> placeDesignRepository;
        private readonly IRepository<ConstructionTask> constructionTaskRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;

        public RemodelingService(IRepositoryContext context,
            IRepository<Remodeling> remodelingRepository,
            IRepository<Place> placeRepository,
            IRepository<Area> areaRepository,
            IRepository<Scene> sceneRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<PlaceCategory> placeCategoryRepository,
            IRepository<Project> projectRepository,
            IRepository<OperatorsSharing> operatorsSharingRepository,
            IRepository<User> userRepository,
            IRepository<PlaceDesign> placeDesignRepository,
            IRepository<ConstructionTask> constructionTaskRepository,
            IRepository<Customer> customerRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository)
            : base(context)
        {
            this.remodelingRepository = remodelingRepository;
            this.placeRepository = placeRepository;
            this.areaRepository = areaRepository;
            this.sceneRepository = sceneRepository;
            this.reseauRepository = reseauRepository;
            this.placeCategoryRepository = placeCategoryRepository;
            this.projectRepository = projectRepository;
            this.operatorsSharingRepository = operatorsSharingRepository;
            this.userRepository = userRepository;
            this.placeDesignRepository = placeDesignRepository;
            this.constructionTaskRepository = constructionTaskRepository;
            this.customerRepository = customerRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
            this.placeOwnerRepository = placeOwnerRepository;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
        }

        /// <summary>
        /// 根据改造Id获取改造
        /// </summary>
        /// <param name="id">改造Id</param>
        /// <returns>改造维护对象</returns>
        public RemodelingMaintObject GetRemodelingById(Guid id)
        {
            Remodeling remodeling = remodelingRepository.FindByKey(id);
            if (remodeling != null)
            {
                RemodelingMaintObject remodelingMaintObject = MapperHelper.Map<Remodeling, RemodelingMaintObject>(remodeling);
                remodelingMaintObject.CreateDateText = remodeling.CreateDate.ToShortDateString();
                Place place = placeRepository.GetByKey(remodeling.PlaceId);
                remodelingMaintObject.PlaceName = place.PlaceName;
                remodelingMaintObject.ProposedNetwork = remodeling.ProposedNetwork;
                remodelingMaintObject.Remarks = remodeling.Remarks;

                remodelingMaintObject.FileIdList = "";
                remodelingMaintObject.Count = 0;
                FileAssociation remodelingFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == remodeling.Id && entity.EntityName == "Remodeling"));
                if (remodelingFileAssociation != null)
                {
                    int count = 0;
                    if (remodelingFileAssociation.FileIdList != "")
                    {
                        if (remodelingFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = remodelingFileAssociation.FileIdList.Split(',');
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
                    remodelingMaintObject.Count = count;
                    remodelingMaintObject.FileIdList = remodelingFileAssociation.FileIdList;
                }
                else
                {
                    remodelingMaintObject.Count = 0;
                    remodelingMaintObject.FileIdList = "";
                }

                ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == remodeling.Id && entity.ProjectType != ProjectType.新建));
                if (projectTask != null)
                {
                    remodelingMaintObject.ProjectType = (int)projectTask.ProjectType;
                }
                else
                {
                    remodelingMaintObject.ProjectType = 0;
                }
                return remodelingMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点改造在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页改造列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectType">项目类型</param>
        /// <param name="issued">是否下达</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页改造列表的Json字符串</returns>
        public string GetRemodelingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int projectType, int orderState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(12);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectType", Type = SqlDbType.Int, Value = projectType });
            parameters.Add(new Parameter() { Name = "OrderState", Type = SqlDbType.Int, Value = orderState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryRemodelingsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改改造
        /// </summary>
        /// <param name="remodelingMaintObject">要新增或者修改的改造维护对象</param>
        public void AddOrUpdateRemodeling(RemodelingMaintObject remodelingMaintObject)
        {
            Place place = placeRepository.FindByKey(remodelingMaintObject.PlaceId);

            if (remodelingMaintObject.Id == Guid.Empty)
            {
                Remodeling remodeling = AggregateFactory.CreateRemodeling((Profession)remodelingMaintObject.Profession, place.PlaceCode, remodelingMaintObject.PlaceId, remodelingMaintObject.ProposedNetwork,
                    remodelingMaintObject.Remarks, remodelingMaintObject.CreateUserId);
                remodelingRepository.Add(remodeling);

                ProjectTask projectTask = AggregateFactory.CreateProjectTask((ProjectType)remodelingMaintObject.ProjectType, remodeling.Id, place.Id, "", remodelingMaintObject.CreateUserId);
                projectTaskRepository.Add(projectTask);

                if ((Profession)remodelingMaintObject.Profession == Profession.基站)
                {
                    EngineeringTask et1 = AggregateFactory.CreateEngineeringTask(TaskModel.天桅, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et1);
                    EngineeringTask et2 = AggregateFactory.CreateEngineeringTask(TaskModel.天桅基础, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et2);
                    EngineeringTask et3 = AggregateFactory.CreateEngineeringTask(TaskModel.机房, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et3);
                    EngineeringTask et4 = AggregateFactory.CreateEngineeringTask(TaskModel.外电引入, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et4);
                    EngineeringTask et5 = AggregateFactory.CreateEngineeringTask(TaskModel.设备安装, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et5);
                    EngineeringTask et6 = AggregateFactory.CreateEngineeringTask(TaskModel.线路, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et6);
                }
                else if ((Profession)remodelingMaintObject.Profession == Profession.室分)
                {
                    EngineeringTask et5 = AggregateFactory.CreateEngineeringTask(TaskModel.设备安装, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et5);
                    EngineeringTask et6 = AggregateFactory.CreateEngineeringTask(TaskModel.线路, projectTask.Id, remodelingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et6);
                }
            }
            else
            {
                Remodeling remodeling = remodelingRepository.FindByKey(remodelingMaintObject.Id);
                if (remodeling != null)
                {
                    remodeling.CheckByUpdate(remodelingMaintObject.ModifyUserId);
                    remodeling.Modify(place.PlaceCode, remodelingMaintObject.PlaceId, remodelingMaintObject.ProposedNetwork, remodelingMaintObject.Remarks, remodelingMaintObject.ModifyUserId);
                    remodelingRepository.Update(remodeling);

                    ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == remodeling.Id && entity.ProjectType != ProjectType.新建));
                    if (projectTask != null)
                    {
                        projectTask.ProjectType = (ProjectType)remodelingMaintObject.ProjectType;
                        projectTaskRepository.Update(projectTask);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Remodeling_dbo.tbl_Place_PlaceId"))
                {
                    throw new ApplicationFault("选择的站点在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除改造
        /// </summary>
        /// <param name="remodelingMaintObjects">要删除的改造维护对象列表</param>
        public void RemoveRemodelings(IList<RemodelingMaintObject> remodelingMaintObjects)
        {
            foreach (RemodelingMaintObject remodelingMaintObject in remodelingMaintObjects)
            {
                Remodeling remodeling = remodelingRepository.FindByKey(remodelingMaintObject.Id);
                if (remodeling != null)
                {
                    remodeling.CheckByRemove(remodelingMaintObject.ModifyUserId);
                    if (remodeling.OrderState != WFProcessInstanceState.未发送)
                    {
                        throw new ApplicationFault("{0}<br>该基站改造已发送公文", remodeling.PlaceCode);
                    }
                    ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == remodeling.Id && entity.ProjectType != ProjectType.新建));
                    if (projectTask != null)
                    {
                        IEnumerable<EngineeringTask> engineeringTasks = engineeringTaskRepository.FindAll(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id));
                        if (engineeringTasks != null)
                        {
                            foreach (EngineeringTask engineeringTask in engineeringTasks)
                            {
                                engineeringTaskRepository.Remove(engineeringTask);
                            }
                        }
                        projectTaskRepository.Remove(projectTask);
                    }
                    remodelingRepository.Remove(remodeling);
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
        /// 根据改造确认Id获取改造确认打印信息
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <returns>改造确认打印对象</returns>
        public RemodelingPrintObject GetRemodelingPrintById(Guid id)
        {
            Remodeling remodeling = remodelingRepository.FindByKey(id);
            if (remodeling != null)
            {
                RemodelingPrintObject remodelingPrintObject = new RemodelingPrintObject();
                Place place = placeRepository.GetByKey(remodeling.PlaceId);
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);
                PlaceCategory placeCategory = placeCategoryRepository.GetByKey(place.PlaceCategoryId);
                PlaceOwner placeOwner = placeOwnerRepository.GetByKey(place.PlaceOwner);

                remodelingPrintObject.Id = remodeling.Id;
                remodelingPrintObject.PlaceId = remodeling.PlaceId;
                remodelingPrintObject.OrderCode = remodeling.OrderCode;
                remodelingPrintObject.CreateDateText = remodeling.CreateDate.ToShortDateString();
                remodelingPrintObject.PlaceName = place.PlaceName;
                remodelingPrintObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                remodelingPrintObject.ImportanceName = EnumHelper.GetEnumText(typeof(Importance), place.Importance);
                remodelingPrintObject.AreaName = area.AreaName;
                remodelingPrintObject.ReseauName = reseau.ReseauName;
                remodelingPrintObject.Lng = place.Lng;
                remodelingPrintObject.Lat = place.Lat;
                remodelingPrintObject.PlaceOwnerName = placeOwner.PlaceOwnerName;
                remodelingPrintObject.ProposedNetwork = remodeling.ProposedNetwork;
                remodelingPrintObject.OwnerName = place.OwnerName;
                remodelingPrintObject.OwnerContact = place.OwnerContact;
                remodelingPrintObject.OwnerPhoneNumber = place.OwnerPhoneNumber;
                remodelingPrintObject.DetailedAddress = place.DetailedAddress;
                remodelingPrintObject.Remarks = remodeling.Remarks;
                remodelingPrintObject.PlaceState = (int)place.State;
                remodelingPrintObject.FileIdList = "";
                remodelingPrintObject.Count = 0;
                FileAssociation remodelingFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == remodeling.PlaceId && entity.EntityName == "Place"));
                if (remodelingFileAssociation != null)
                {
                    int count = 0;
                    if (remodelingFileAssociation.FileIdList != "")
                    {
                        if (remodelingFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = remodelingFileAssociation.FileIdList.Split(',');
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
                    remodelingPrintObject.Count = count;
                    remodelingPrintObject.FileIdList = remodelingFileAssociation.FileIdList;
                }
                else
                {
                    remodelingPrintObject.Count = 0;
                    remodelingPrintObject.FileIdList = "";
                }

                remodelingPrintObject.WFActivityInstancesInfoHtml = "";
                if (remodeling.OrderCode != "")
                {
                    List<Parameter> parameters = new List<Parameter>(1);
                    parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = remodeling.OrderCode });
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
                            remodelingPrintObject.WFActivityInstancesInfoHtml = sb.ToString();
                        }
                    }
                }
                return remodelingPrintObject;
            }
            else
            {
                throw new ApplicationFault("选择的改造确认在系统中不存在");
            }
        }
    }
}
