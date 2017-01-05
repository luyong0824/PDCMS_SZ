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
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.Domain.Models.WorkFlow;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 寻址确认应用层服务
    /// </summary>
    public class AddressingService : DataService, IAddressingService
    {
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Scene> sceneRepository;
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<PlaceCategory> placeCategoryRepository;
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<PlaceDesign> placeDesignRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;

        public AddressingService(IRepositoryContext context,
            IRepository<Addressing> addressingRepository,
            IRepository<Planning> planningRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<Area> areaRepository,
            IRepository<Scene> sceneRepository,
            IRepository<Project> projectRepository,
            IRepository<PlaceCategory> placeCategoryRepository,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<PlacePropertyLog> placePropertyLogRepository,
            IRepository<Place> placeRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<PlaceDesign> placeDesignRepository,
            IRepository<Customer> customerRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository)
            : base(context)
        {
            this.addressingRepository = addressingRepository;
            this.planningRepository = planningRepository;
            this.reseauRepository = reseauRepository;
            this.areaRepository = areaRepository;
            this.sceneRepository = sceneRepository;
            this.projectRepository = projectRepository;
            this.placeCategoryRepository = placeCategoryRepository;
            this.placePropertyRepository = placePropertyRepository;
            this.placePropertyLogRepository = placePropertyLogRepository;
            this.placeRepository = placeRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.placeOwnerRepository = placeOwnerRepository;
            this.placeDesignRepository = placeDesignRepository;
            this.customerRepository = customerRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
        }

        /// <summary>
        /// 根据寻址确认Id和规划Id获取寻址确认
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="planningId">规划Id</param>
        /// <returns>寻址确认维护对象</returns>
        public AddressingMaintObject GetAddressingById(Guid id, Guid planningId)
        {
            Addressing addressing = addressingRepository.FindByKey(id);
            if (addressing != null)
            {
                AddressingMaintObject addressingMaintObject = MapperHelper.Map<Addressing, AddressingMaintObject>(addressing);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                Reseau reseau = reseauRepository.GetByKey(planning.ReseauId);

                addressingMaintObject.Id = addressing.Id;
                addressingMaintObject.PlanningId = planning.Id;
                if (planning.PlaceId == Guid.Empty)
                {
                    addressingMaintObject.PlaceCode = "";
                    addressingMaintObject.PlaceId = planning.PlaceId;
                }
                else
                {
                    Place place = placeRepository.GetByKey(planning.PlaceId);
                    addressingMaintObject.PlaceCode = place.PlaceCode;
                    addressingMaintObject.PlaceId = planning.PlaceId;
                }
                addressingMaintObject.PlanningName = planning.PlanningName;
                addressingMaintObject.PlaceName = addressing.PlaceName;
                addressingMaintObject.Profession = (int)planning.Profession;
                addressingMaintObject.PlaceCategoryId = planning.PlaceCategoryId;
                addressingMaintObject.Importance = (int)planning.Importance;
                addressingMaintObject.AreaId = reseau.AreaId;
                addressingMaintObject.ReseauId = planning.ReseauId;
                addressingMaintObject.Lng = planning.Lng;
                addressingMaintObject.Lat = planning.Lat;
                addressingMaintObject.PlaceOwner = planning.PlaceOwner;
                addressingMaintObject.ProposedNetwork = planning.ProposedNetwork;
                addressingMaintObject.DetailedAddress = planning.DetailedAddress;
                addressingMaintObject.AddressingState = (int)planning.AddressingState;
                if (addressing.AreaManagerId != Guid.Empty)
                {
                    User areaManager = userRepository.GetByKey(addressing.AreaManagerId);
                    addressingMaintObject.AreaManagerText = areaManager.FullName;
                }
                else
                {
                    addressingMaintObject.AreaManagerText = "请选择";
                }
                if (addressing.DesignCustomerId != Guid.Empty)
                {
                    Customer desginCustomer = customerRepository.GetByKey(addressing.DesignCustomerId);
                    addressingMaintObject.DesignCustomerText = desginCustomer.CustomerName;
                }
                else
                {
                    addressingMaintObject.DesignCustomerText = "请选择";
                }
                addressingMaintObject.FileIdList = "";
                addressingMaintObject.Count = 0;
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
                    addressingMaintObject.Count = count;
                    addressingMaintObject.FileIdList = addressingFileAssociation.FileIdList;
                }
                else
                {
                    addressingMaintObject.Count = 0;
                    addressingMaintObject.FileIdList = "";
                }
                return addressingMaintObject;
            }
            else
            {
                Planning planning = planningRepository.FindByKey(planningId);
                if (planning != null)
                {
                    Reseau reseau = reseauRepository.GetByKey(planning.ReseauId);
                    AddressingMaintObject addressingMaintObject = new AddressingMaintObject();
                    addressingMaintObject.Id = Guid.Empty;
                    addressingMaintObject.PlanningId = planning.Id;
                    addressingMaintObject.PlaceId = planning.PlaceId;
                    addressingMaintObject.PlanningName = planning.PlanningName;
                    addressingMaintObject.PlaceCode = "";
                    addressingMaintObject.PlaceName = planning.PlanningName;
                    addressingMaintObject.Profession = (int)planning.Profession;
                    addressingMaintObject.PlaceCategoryId = planning.PlaceCategoryId;
                    addressingMaintObject.Importance = (int)planning.Importance;
                    addressingMaintObject.AreaId = reseau.AreaId;
                    addressingMaintObject.ReseauId = planning.ReseauId;
                    addressingMaintObject.Lng = planning.Lng;
                    addressingMaintObject.Lat = planning.Lat;
                    addressingMaintObject.PlaceOwner = planning.PlaceOwner;
                    addressingMaintObject.ProposedNetwork = planning.ProposedNetwork;
                    addressingMaintObject.AddressingDepartmentId = Guid.Empty;
                    addressingMaintObject.AddressingRealName = "";
                    addressingMaintObject.OwnerName = "";
                    addressingMaintObject.OwnerContact = "";
                    addressingMaintObject.OwnerPhoneNumber = "";
                    addressingMaintObject.DetailedAddress = planning.DetailedAddress;
                    addressingMaintObject.AddressingState = (int)planning.AddressingState;
                    addressingMaintObject.Remarks = "";
                    addressingMaintObject.AreaManagerId = Guid.Empty;
                    addressingMaintObject.AreaManagerText = "请选择";
                    addressingMaintObject.DesignCustomerId = Guid.Empty;
                    addressingMaintObject.DesignCustomerText = "请选择";
                    addressingMaintObject.FileIdList = "";
                    addressingMaintObject.Count = 0;
                    return addressingMaintObject;
                }
                else
                {
                    throw new ApplicationFault("选择的寻址确认在系统中不存在");
                }
            }
        }

        /// <summary>
        /// 根据寻址确认Id获取寻址确认打印信息
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <returns>寻址确认打印对象</returns>
        public AddressingPrintObject GetAddressingPrintById(Guid id)
        {
            Addressing addressing = addressingRepository.FindByKey(id);
            if (addressing != null)
            {
                AddressingPrintObject addressingPrintObject = new AddressingPrintObject();
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                Reseau reseau = reseauRepository.GetByKey(planning.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);
                PlaceCategory placeCategory = placeCategoryRepository.GetByKey(planning.PlaceCategoryId);
                PlaceOwner placeOwner = placeOwnerRepository.GetByKey(planning.PlaceOwner);
                Department department = departmentRepository.GetByKey(addressing.AddressingDepartmentId);

                //PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                //PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));

                addressingPrintObject.Id = addressing.Id;
                addressingPrintObject.PlanningId = planning.Id;
                addressingPrintObject.PlaceId = planning.PlaceId;
                addressingPrintObject.OrderCode = addressing.OrderCode;
                addressingPrintObject.CreateDateText = addressing.CreateDate.ToShortDateString();
                addressingPrintObject.PlanningName = planning.PlanningName;
                addressingPrintObject.PlaceName = addressing.PlaceName;
                addressingPrintObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                addressingPrintObject.ImportanceName = EnumHelper.GetEnumText(typeof(Importance), planning.Importance);
                addressingPrintObject.AreaName = area.AreaName;
                addressingPrintObject.ReseauName = reseau.ReseauName;
                addressingPrintObject.Lng = planning.Lng;
                addressingPrintObject.Lat = planning.Lat;
                addressingPrintObject.PlaceOwnerName = placeOwner.PlaceOwnerName;
                addressingPrintObject.ProposedNetwork = planning.ProposedNetwork;
                addressingPrintObject.AddressingDepartmentName = department.DepartmentName;
                addressingPrintObject.AddressingRealName = addressing.AddressingRealName;
                addressingPrintObject.OwnerName = addressing.OwnerName;
                addressingPrintObject.OwnerContact = addressing.OwnerContact;
                addressingPrintObject.OwnerPhoneNumber = addressing.OwnerPhoneNumber;
                addressingPrintObject.DetailedAddress = planning.DetailedAddress;
                addressingPrintObject.Remarks = addressing.Remarks;
                addressingPrintObject.FileIdList = "";
                addressingPrintObject.Count = 0;
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
                    addressingPrintObject.Count = count;
                    addressingPrintObject.FileIdList = addressingFileAssociation.FileIdList;
                }
                else
                {
                    addressingPrintObject.Count = 0;
                    addressingPrintObject.FileIdList = "";
                }

                addressingPrintObject.WFActivityInstancesInfoHtml = "";
                if (addressing.OrderCode != "")
                {
                    List<Parameter> parameters = new List<Parameter>(1);
                    parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = addressing.OrderCode });
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
                            addressingPrintObject.WFActivityInstancesInfoHtml = sb.ToString();
                        }
                    }
                }
                return addressingPrintObject;
            }
            else
            {
                throw new ApplicationFault("选择的寻址确认在系统中不存在");
            }
        }

        public AddressingEditorObject GetAddressingEditorById(Guid id)
        {
            Addressing addressing = addressingRepository.FindByKey(id);
            if (addressing != null)
            {
                AddressingEditorObject addressingEditorObject = new AddressingEditorObject();
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                Reseau reseau = reseauRepository.GetByKey(planning.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);
                PlaceCategory placeCategory = placeCategoryRepository.GetByKey(planning.PlaceCategoryId);
                PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));

                addressingEditorObject.ProjectCode = placeDesign.ProjectCode;
                addressingEditorObject.ProjectName = placeDesign.ProjectName;
                addressingEditorObject.BudgetPrice = placeDesign.ProjectMoney;
                addressingEditorObject.GroupPlaceCode = placeDesign.GroupPlaceCode;
                addressingEditorObject.ProjectIsApply = (int)placeDesign.ProjectIsApply;
                addressingEditorObject.ProjectIsDoApply = (int)placeDesign.ProjectIsDoApply;
                if (placeDesign.ProjectIsApply == Bool.是)
                {
                    addressingEditorObject.ProjectApplyDate = placeDesign.ProjectApplyDate.ToString();
                }
                else
                {
                    addressingEditorObject.ProjectApplyDate = "";
                }
                if (placeDesign.ProjectIsDoApply == Bool.是)
                {
                    addressingEditorObject.ProjectDoApplyDate = placeDesign.ProjectDoApplyDate.ToString();
                }
                else
                {
                    addressingEditorObject.ProjectDoApplyDate = "";
                }
                addressingEditorObject.Id = addressing.Id;
                addressingEditorObject.OrderCode = addressing.OrderCode;

                if (planning.PlaceId == Guid.Empty)
                {
                    addressingEditorObject.PlaceCode = "";
                }
                else
                {
                    Place place = placeRepository.GetByKey(planning.PlaceId);
                    addressingEditorObject.PlaceCode = place.PlaceCode;
                }
                addressingEditorObject.PlaceName = addressing.PlaceName;
                addressingEditorObject.PlanningName = planning.PlanningName;
                addressingEditorObject.AreaName = area.AreaName;
                addressingEditorObject.ReseauName = reseau.ReseauName;
                addressingEditorObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                addressingEditorObject.ImportanceName = EnumHelper.GetEnumText(typeof(Importance), planning.Importance);
                addressingEditorObject.Lng = planning.Lng;
                addressingEditorObject.Lat = planning.Lat;
                addressingEditorObject.AddressingStateName = EnumHelper.GetEnumText(typeof(AddressingState), planning.AddressingState);
                addressingEditorObject.OwnerName = addressing.OwnerName;
                addressingEditorObject.OwnerContact = addressing.OwnerContact;
                addressingEditorObject.OwnerPhoneNumber = addressing.OwnerPhoneNumber;
                //addressingEditorObject.TelecomDemand = (int)planning.TelecomDemand;
                //addressingEditorObject.MobileDemand = (int)planning.MobileDemand;
                //addressingEditorObject.UnicomDemand = (int)planning.UnicomDemand;
                addressingEditorObject.DetailedAddress = planning.DetailedAddress;
                addressingEditorObject.Remarks = addressing.Remarks;
                addressingEditorObject.PlaceId = planning.PlaceId;
                addressingEditorObject.PlanningId = addressing.PlanningId;
                if (placeDesign.DesignCustomerId == Guid.Empty)
                {
                    addressingEditorObject.DesignCustomerId = Guid.Empty;
                    addressingEditorObject.DesignCustomerName = "请选择";
                }
                else
                {
                    Customer customer = customerRepository.GetByKey(placeDesign.DesignCustomerId.Value);
                    addressingEditorObject.DesignCustomerId = placeDesign.DesignCustomerId.Value;
                    addressingEditorObject.DesignCustomerName = customer.CustomerName;
                }
                if (placeDesign.DesignUserId == Guid.Empty)
                {
                    addressingEditorObject.DesignUserId = Guid.Empty;
                    addressingEditorObject.DesignUserName = "请选择";
                }
                else
                {
                    User designUser = userRepository.GetByKey(placeDesign.DesignUserId.Value);
                    addressingEditorObject.DesignUserId = placeDesign.DesignUserId.Value;
                    addressingEditorObject.DesignUserName = designUser.FullName;
                }
                addressingEditorObject.PlaceDesignId = placeDesign.Id;

                if (placeDesign.TowerMark == Bool.是)
                {
                    addressingEditorObject.TowerMark = 1;
                    Tower tower = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressingEditorObject.TowerId = tower.Id;
                    addressingEditorObject.TowerType = (int)tower.TowerType;
                    addressingEditorObject.TowerHeight = tower.TowerHeight;
                    addressingEditorObject.PlatFormNumber = tower.PlatFormNumber;
                    addressingEditorObject.PoleNumber = tower.PoleNumber;
                    addressingEditorObject.TowerTimeLimit = tower.TimeLimit;
                    addressingEditorObject.TowerBudget = tower.BudgetPrice;

                    FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                    if (towerFileAssociation != null)
                    {
                        int count = 0;
                        if (towerFileAssociation.FileIdList != "")
                        {
                            if (towerFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = towerFileAssociation.FileIdList.Split(',');
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
                        addressingEditorObject.TowerCount = count;
                        addressingEditorObject.TowerFileIdList = towerFileAssociation.FileIdList;
                    }
                    else
                    {
                        addressingEditorObject.TowerCount = 0;
                        addressingEditorObject.TowerFileIdList = "";
                    }

                    if (tower.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(tower.CustomerId.Value);
                        addressingEditorObject.TowerCustomerId = tower.CustomerId;
                        addressingEditorObject.TowerCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        addressingEditorObject.TowerCustomerId = Guid.Empty;
                        addressingEditorObject.TowerCustomerName = "请选择";
                    }
                }
                else
                {
                    addressingEditorObject.TowerId = Guid.Empty;
                    addressingEditorObject.TowerMark = 0;
                    addressingEditorObject.TowerType = 0;
                    addressingEditorObject.TowerHeight = 0;
                    addressingEditorObject.PlatFormNumber = 0;
                    addressingEditorObject.PoleNumber = 0;
                    addressingEditorObject.TowerTimeLimit = 0;
                    addressingEditorObject.TowerBudget = 0;
                    addressingEditorObject.TowerCount = 0;
                    addressingEditorObject.TowerCustomerId = Guid.Empty;
                    addressingEditorObject.TowerCustomerName = "请选择";
                }
                if (placeDesign.TowerBaseMark == Bool.是)
                {
                    addressingEditorObject.TowerBaseMark = 1;
                    TowerBase towerBase = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressingEditorObject.TowerBaseId = towerBase.Id;
                    addressingEditorObject.TowerBaseType = (int)towerBase.TowerBaseType;
                    addressingEditorObject.TowerBaseTimeLimit = towerBase.TimeLimit;
                    addressingEditorObject.TowerBaseBudget = towerBase.BudgetPrice;

                    FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                    if (towerBaseFileAssociation != null)
                    {
                        int count = 0;
                        if (towerBaseFileAssociation.FileIdList != "")
                        {
                            if (towerBaseFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = towerBaseFileAssociation.FileIdList.Split(',');
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
                        addressingEditorObject.TowerBaseCount = count;
                        addressingEditorObject.TowerBaseFileIdList = towerBaseFileAssociation.FileIdList;
                    }
                    else
                    {
                        addressingEditorObject.TowerBaseCount = 0;
                        addressingEditorObject.TowerBaseFileIdList = "";
                    }

                    if (towerBase.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(towerBase.CustomerId.Value);
                        addressingEditorObject.TowerBaseCustomerId = towerBase.CustomerId;
                        addressingEditorObject.TowerBaseCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        addressingEditorObject.TowerBaseCustomerId = Guid.Empty;
                        addressingEditorObject.TowerBaseCustomerName = "请选择";
                    }
                }
                else
                {
                    addressingEditorObject.TowerBaseId = Guid.Empty;
                    addressingEditorObject.TowerBaseMark = 0;
                    addressingEditorObject.TowerBaseType = 0;
                    addressingEditorObject.TowerBaseTimeLimit = 0;
                    addressingEditorObject.TowerBaseBudget = 0;
                    addressingEditorObject.TowerBaseCount = 0;
                    addressingEditorObject.TowerBaseCustomerId = Guid.Empty;
                    addressingEditorObject.TowerBaseCustomerName = "请选择";
                }
                if (placeDesign.MachineRoomMark == Bool.是)
                {
                    addressingEditorObject.MachineRoomMark = 1;
                    MachineRoom machineRoom = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressingEditorObject.MachineRoomId = machineRoom.Id;
                    addressingEditorObject.MachineRoomType = (int)machineRoom.MachineRoomType;
                    addressingEditorObject.MachineRoomArea = machineRoom.MachineRoomArea;
                    addressingEditorObject.MachineRoomTimeLimit = machineRoom.TimeLimit;
                    addressingEditorObject.MachineRoomBudget = machineRoom.BudgetPrice;

                    FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                    if (machineRoomFileAssociation != null)
                    {
                        int count = 0;
                        if (machineRoomFileAssociation.FileIdList != "")
                        {
                            if (machineRoomFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = machineRoomFileAssociation.FileIdList.Split(',');
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
                        addressingEditorObject.MachineRoomCount = count;
                        addressingEditorObject.MachineRoomFileIdList = machineRoomFileAssociation.FileIdList;
                    }
                    else
                    {
                        addressingEditorObject.MachineRoomCount = 0;
                        addressingEditorObject.MachineRoomFileIdList = "";
                    }

                    if (machineRoom.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(machineRoom.CustomerId.Value);
                        addressingEditorObject.MachineRoomCustomerId = machineRoom.CustomerId;
                        addressingEditorObject.MachineRoomCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        addressingEditorObject.MachineRoomCustomerId = Guid.Empty;
                        addressingEditorObject.MachineRoomCustomerName = "请选择";
                    }
                }
                else
                {
                    addressingEditorObject.MachineRoomId = Guid.Empty;
                    addressingEditorObject.MachineRoomMark = 0;
                    addressingEditorObject.MachineRoomType = 0;
                    addressingEditorObject.MachineRoomArea = 0;
                    addressingEditorObject.MachineRoomTimeLimit = 0;
                    addressingEditorObject.MachineRoomBudget = 0;
                    addressingEditorObject.MachineRoomCount = 0;
                    addressingEditorObject.MachineRoomCustomerId = Guid.Empty;
                    addressingEditorObject.MachineRoomCustomerName = "请选择";
                }
                if (placeDesign.ExternalElectricPowerMark == Bool.是)
                {
                    addressingEditorObject.ExternalElectricPowerMark = 1;
                    ExternalElectricPower externalElectricPower = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressingEditorObject.ExternalElectricPowerId = externalElectricPower.Id;
                    addressingEditorObject.ExternalElectric = (int)externalElectricPower.ExternalElectric;
                    addressingEditorObject.ExternalTimeLimit = externalElectricPower.TimeLimit;
                    addressingEditorObject.ExternalBudget = externalElectricPower.BudgetPrice;

                    FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                    if (externalElectricPowerFileAssociation != null)
                    {
                        int count = 0;
                        if (externalElectricPowerFileAssociation.FileIdList != "")
                        {
                            if (externalElectricPowerFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = externalElectricPowerFileAssociation.FileIdList.Split(',');
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
                        addressingEditorObject.ExternalCount = count;
                        addressingEditorObject.ExternalFileIdList = externalElectricPowerFileAssociation.FileIdList;
                    }
                    else
                    {
                        addressingEditorObject.ExternalCount = 0;
                        addressingEditorObject.ExternalFileIdList = "";
                    }

                    if (externalElectricPower.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(externalElectricPower.CustomerId.Value);
                        addressingEditorObject.ExternalCustomerId = externalElectricPower.CustomerId;
                        addressingEditorObject.ExternalCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        addressingEditorObject.ExternalCustomerId = Guid.Empty;
                        addressingEditorObject.ExternalCustomerName = "请选择";
                    }
                }
                else
                {
                    addressingEditorObject.ExternalElectricPowerId = Guid.Empty;
                    addressingEditorObject.ExternalElectricPowerMark = 0;
                    addressingEditorObject.ExternalElectric = 0;
                    addressingEditorObject.ExternalTimeLimit = 0;
                    addressingEditorObject.ExternalBudget = 0;
                    addressingEditorObject.ExternalCount = 0;
                    addressingEditorObject.ExternalCustomerId = Guid.Empty;
                    addressingEditorObject.ExternalCustomerName = "请选择";
                }
                if (placeDesign.EquipmentInstallMark == Bool.是)
                {
                    addressingEditorObject.EquipmentInstallMark = 1;
                    EquipmentInstall equipmentInstall = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressingEditorObject.EquipmentInstallId = equipmentInstall.Id;
                    addressingEditorObject.SwitchPower = equipmentInstall.SwitchPower;
                    addressingEditorObject.Battery = equipmentInstall.Battery;
                    addressingEditorObject.CabinetNumber = equipmentInstall.CabinetNumber;
                    addressingEditorObject.EquipmentTimeLimit = equipmentInstall.TimeLimit;
                    addressingEditorObject.EquipmentBudget = equipmentInstall.BudgetPrice;

                    FileAssociation EquipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                    if (EquipmentInstallFileAssociation != null)
                    {
                        int count = 0;
                        if (EquipmentInstallFileAssociation.FileIdList != "")
                        {
                            if (EquipmentInstallFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = EquipmentInstallFileAssociation.FileIdList.Split(',');
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
                        addressingEditorObject.EquipmentInstallCount = count;
                        addressingEditorObject.EquipmentInstallFileIdList = EquipmentInstallFileAssociation.FileIdList;
                    }
                    else
                    {
                        addressingEditorObject.EquipmentInstallCount = 0;
                        addressingEditorObject.EquipmentInstallFileIdList = "";
                    }

                    if (equipmentInstall.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(equipmentInstall.CustomerId.Value);
                        addressingEditorObject.EquipmentCustomerId = equipmentInstall.CustomerId;
                        addressingEditorObject.EquipmentCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        addressingEditorObject.EquipmentCustomerId = Guid.Empty;
                        addressingEditorObject.EquipmentCustomerName = "请选择";
                    }
                }
                else
                {
                    addressingEditorObject.EquipmentInstallId = Guid.Empty;
                    addressingEditorObject.EquipmentInstallMark = 0;
                    addressingEditorObject.SwitchPower = 0;
                    addressingEditorObject.Battery = 0;
                    addressingEditorObject.CabinetNumber = 0;
                    addressingEditorObject.EquipmentTimeLimit = 0;
                    addressingEditorObject.EquipmentBudget = 0;
                    addressingEditorObject.EquipmentInstallCount = 0;
                    addressingEditorObject.EquipmentCustomerId = Guid.Empty;
                    addressingEditorObject.EquipmentCustomerName = "请选择";
                }
                if (placeDesign.AddressExplorMark == Bool.是)
                {
                    addressingEditorObject.AddressExplorMark = 1;
                    AddressExplor addressExplor = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressingEditorObject.AddressExplorId = addressExplor.Id;
                    addressingEditorObject.AddressTimeLimit = addressExplor.TimeLimit;
                    addressingEditorObject.AddressBudget = addressExplor.BudgetPrice;

                    FileAssociation AddressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                    if (AddressExplorFileAssociation != null)
                    {
                        int count = 0;
                        if (AddressExplorFileAssociation.FileIdList != "")
                        {
                            if (AddressExplorFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = AddressExplorFileAssociation.FileIdList.Split(',');
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
                        addressingEditorObject.AddressCount = count;
                        addressingEditorObject.AddressFileIdList = AddressExplorFileAssociation.FileIdList;
                    }
                    else
                    {
                        addressingEditorObject.AddressCount = 0;
                        addressingEditorObject.AddressFileIdList = "";
                    }

                    if (addressExplor.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(addressExplor.CustomerId.Value);
                        addressingEditorObject.AddressCustomerId = addressExplor.CustomerId;
                        addressingEditorObject.AddressCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        addressingEditorObject.AddressCustomerId = Guid.Empty;
                        addressingEditorObject.AddressCustomerName = "请选择";
                    }
                }
                else
                {
                    addressingEditorObject.AddressExplorId = Guid.Empty;
                    addressingEditorObject.AddressExplorMark = 0;
                    addressingEditorObject.AddressTimeLimit = 0;
                    addressingEditorObject.AddressBudget = 0;
                    addressingEditorObject.AddressCount = 0;
                    addressingEditorObject.AddressCustomerId = Guid.Empty;
                    addressingEditorObject.AddressCustomerName = "请选择";
                }
                if (placeDesign.FoundationTestMark == Bool.是)
                {
                    addressingEditorObject.FoundationTestMark = 1;
                    FoundationTest foundationTest = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressingEditorObject.FoundationTestId = foundationTest.Id;
                    addressingEditorObject.FoundationTimeLimit = foundationTest.TimeLimit;
                    addressingEditorObject.FoundationBudget = foundationTest.BudgetPrice;

                    FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                    if (foundationTestFileAssociation != null)
                    {
                        int count = 0;
                        if (foundationTestFileAssociation.FileIdList != "")
                        {
                            if (foundationTestFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = foundationTestFileAssociation.FileIdList.Split(',');
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
                        addressingEditorObject.FoundationCount = count;
                        addressingEditorObject.FoundationFileIdList = foundationTestFileAssociation.FileIdList;
                    }
                    else
                    {
                        addressingEditorObject.FoundationCount = 0;
                        addressingEditorObject.FoundationFileIdList = "";
                    }

                    if (foundationTest.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(foundationTest.CustomerId.Value);
                        addressingEditorObject.FoundationCustomerId = foundationTest.CustomerId;
                        addressingEditorObject.FoundationCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        addressingEditorObject.FoundationCustomerId = Guid.Empty;
                        addressingEditorObject.FoundationCustomerName = "请选择";
                    }
                }
                else
                {
                    addressingEditorObject.FoundationTestId = Guid.Empty;
                    addressingEditorObject.FoundationTestMark = 0;
                    addressingEditorObject.FoundationTimeLimit = 0;
                    addressingEditorObject.FoundationBudget = 0;
                    addressingEditorObject.FoundationCount = 0;
                    addressingEditorObject.FoundationCustomerId = Guid.Empty;
                    addressingEditorObject.FoundationCustomerName = "请选择";
                }

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
                    addressingEditorObject.Count = count;
                    addressingEditorObject.FileIdList = addressingFileAssociation.FileIdList;
                }
                else
                {
                    addressingEditorObject.Count = 0;
                    addressingEditorObject.FileIdList = "";
                }

                if (placeProperty.TelecomCreateUserId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    addressingEditorObject.TelecomPoleNumber = placeProperty.TelecomPoleNumber;
                    addressingEditorObject.TelecomCabinetNumber = placeProperty.TelecomCabinetNumber;
                    addressingEditorObject.TelecomPowerUsed = placeProperty.TelecomPowerUsed;
                }
                else
                {
                    IEnumerable<OperatorsPlanning> operatorsPlannings = operatorsPlanningRepository.FindAll(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53"));
                    if (operatorsPlannings != null)
                    {
                        foreach (OperatorsPlanning operatorsPlanning in operatorsPlannings)
                        {
                            addressingEditorObject.TelecomPoleNumber = operatorsPlanning.PoleNumber;
                            addressingEditorObject.TelecomCabinetNumber = operatorsPlanning.CabinetNumber;
                            addressingEditorObject.TelecomPowerUsed = 0;
                            break;
                        }
                    }
                    else
                    {
                        addressingEditorObject.TelecomPoleNumber = 0;
                        addressingEditorObject.TelecomCabinetNumber = 0;
                        addressingEditorObject.TelecomPowerUsed = 0;
                    }
                }

                if (placeProperty.MobileCreateUserId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    addressingEditorObject.MobilePoleNumber = placeProperty.MobilePoleNumber;
                    addressingEditorObject.MobileCabinetNumber = placeProperty.MobileCabinetNumber;
                    addressingEditorObject.MobilePowerUsed = placeProperty.MobilePowerUsed;
                }
                else
                {
                    IEnumerable<OperatorsPlanning> operatorsPlannings = operatorsPlanningRepository.FindAll(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb"));
                    if (operatorsPlannings != null)
                    {
                        foreach (OperatorsPlanning operatorsPlanning in operatorsPlannings)
                        {
                            addressingEditorObject.MobilePoleNumber = operatorsPlanning.PoleNumber;
                            addressingEditorObject.MobileCabinetNumber = operatorsPlanning.CabinetNumber;
                            addressingEditorObject.MobilePowerUsed = 0;
                            break;
                        }
                    }
                    else
                    {
                        addressingEditorObject.MobilePoleNumber = 0;
                        addressingEditorObject.MobileCabinetNumber = 0;
                        addressingEditorObject.MobilePowerUsed = 0;
                    }
                }

                if (placeProperty.UnicomCreateUserId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    addressingEditorObject.UnicomPoleNumber = placeProperty.UnicomPoleNumber;
                    addressingEditorObject.UnicomCabinetNumber = placeProperty.UnicomCabinetNumber;
                    addressingEditorObject.UnicomPowerUsed = placeProperty.UnicomPowerUsed;
                }
                else
                {
                    IEnumerable<OperatorsPlanning> operatorsPlannings = operatorsPlanningRepository.FindAll(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningId == planning.Id && entity.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"));
                    if (operatorsPlannings != null)
                    {
                        foreach (OperatorsPlanning operatorsPlanning in operatorsPlannings)
                        {
                            addressingEditorObject.UnicomPoleNumber = operatorsPlanning.PoleNumber;
                            addressingEditorObject.UnicomCabinetNumber = operatorsPlanning.CabinetNumber;
                            addressingEditorObject.UnicomPowerUsed = 0;
                            break;
                        }
                    }
                    else
                    {
                        addressingEditorObject.UnicomPoleNumber = 0;
                        addressingEditorObject.UnicomCabinetNumber = 0;
                        addressingEditorObject.UnicomPowerUsed = 0;
                    }
                }

                if (placeDesign.SupervisorCustomerId != Guid.Empty)
                {
                    Customer customer = customerRepository.FindByKey(placeDesign.SupervisorCustomerId.Value);
                    addressingEditorObject.SupervisorCustomerId = placeDesign.SupervisorCustomerId;
                    addressingEditorObject.SupervisorCustomerName = customer.CustomerName;
                }
                else
                {
                    addressingEditorObject.SupervisorCustomerId = Guid.Empty;
                    addressingEditorObject.SupervisorCustomerName = "请选择";
                }
                return addressingEditorObject;
            }
            else
            {
                throw new ApplicationFault("选择的寻址确认在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页寻址确认列表
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
        /// <param name="addressingState">寻址状态</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <returns>分页寻址确认列表的Json字符串</returns>
        public string GetAddressingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingUserId)
        {
            List<Parameter> parameters = new List<Parameter>(13);
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
            parameters.Add(new Parameter() { Name = "AddressingState", Type = SqlDbType.Int, Value = addressingState });
            parameters.Add(new Parameter() { Name = "AddressingUserId", Type = SqlDbType.UniqueIdentifier, Value = addressingUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryAddressingsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改寻址确认
        /// </summary>
        /// <param name="addressingMaintObject">要新增或者修改的寻址确认维护对象</param>
        public void AddOrUpdateAddressing(AddressingMaintObject addressingMaintObject)
        {
            if (addressingMaintObject.Id == Guid.Empty)
            {
                Planning planning = planningRepository.FindByKey(addressingMaintObject.PlanningId);
                Reseau reseau = reseauRepository.FindByKey(addressingMaintObject.ReseauId);
                if (planning == null)
                {
                    throw new ApplicationFault("选择的寻址确认在系统中不存在");
                }

                Addressing addressing = AggregateFactory.CreateAddressing(addressingMaintObject.PlanningId, addressingMaintObject.PlaceName, addressingMaintObject.AddressingDepartmentId,
                    addressingMaintObject.AddressingRealName, addressingMaintObject.OwnerName, addressingMaintObject.OwnerContact, addressingMaintObject.OwnerPhoneNumber,
                    addressingMaintObject.Remarks, addressingMaintObject.CreateUserId);
                addressingRepository.Add(addressing);

                planning.CheckByAddAddressing(addressingMaintObject.CreateUserId);
                planning.Modify(planning.PlanningName, addressingMaintObject.PlaceCategoryId, addressingMaintObject.ReseauId, addressingMaintObject.Lng, addressingMaintObject.Lat,
                    addressingMaintObject.DetailedAddress, planning.Remarks, addressingMaintObject.ProposedNetwork, planning.OptionalAddress, (Importance)addressingMaintObject.Importance,
                    addressingMaintObject.PlaceOwner, addressingMaintObject.CreateUserId);
                planningRepository.Update(planning);

                ProjectTask projectTask = AggregateFactory.CreateProjectTask(ProjectType.新建, addressing.Id, Guid.Empty, "", addressingMaintObject.CreateUserId);
                projectTaskRepository.Add(projectTask);

                if (planning.Profession == Profession.基站)
                {
                    EngineeringTask et1 = AggregateFactory.CreateEngineeringTask(TaskModel.天桅, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et1);
                    EngineeringTask et2 = AggregateFactory.CreateEngineeringTask(TaskModel.天桅基础, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et2);
                    EngineeringTask et3 = AggregateFactory.CreateEngineeringTask(TaskModel.机房, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et3);
                    EngineeringTask et4 = AggregateFactory.CreateEngineeringTask(TaskModel.外电引入, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et4);
                    EngineeringTask et5 = AggregateFactory.CreateEngineeringTask(TaskModel.设备安装, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et5);
                    EngineeringTask et6 = AggregateFactory.CreateEngineeringTask(TaskModel.线路, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et6);
                }
                else if (planning.Profession == Profession.室分)
                {
                    EngineeringTask et5 = AggregateFactory.CreateEngineeringTask(TaskModel.设备安装, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et5);
                    EngineeringTask et6 = AggregateFactory.CreateEngineeringTask(TaskModel.线路, projectTask.Id, addressingMaintObject.CreateUserId);
                    engineeringTaskRepository.Add(et6);
                }
                if (addressingMaintObject.FileIdList != "")
                {
                    FileAssociation planningFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.Id && entity.EntityName == "Planning"));
                    addressingMaintObject.FileIdList = addressingMaintObject.FileIdList + "," + planningFileAssociation.FileIdList;
                    FileAssociation addressingFileAssociation = AggregateFactory.CreateFileAssociation("Addressing", addressing.Id, addressingMaintObject.FileIdList, addressingMaintObject.CreateUserId);
                    fileAssociationRepository.Add(addressingFileAssociation);
                }
                else
                {
                    FileAssociation planningFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.Id && entity.EntityName == "Planning"));
                    if (planningFileAssociation != null)
                    {
                        FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("Addressing", addressing.Id, planningFileAssociation.FileIdList, addressingMaintObject.CreateUserId);
                        fileAssociationRepository.Add(newAddressingFileAssociation);
                    }
                }
            }
            else
            {
                Addressing addressing = addressingRepository.FindByKey(addressingMaintObject.Id);
                Reseau reseau = reseauRepository.FindByKey(addressingMaintObject.ReseauId);
                if (addressing != null)
                {
                    addressing.Modify(addressingMaintObject.PlaceName, addressingMaintObject.AddressingDepartmentId, addressingMaintObject.AddressingRealName,
                        addressingMaintObject.OwnerName, addressingMaintObject.OwnerContact, addressingMaintObject.OwnerPhoneNumber,
                        addressingMaintObject.Remarks, addressingMaintObject.ModifyUserId);
                    addressingRepository.Update(addressing);

                    Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                    planning.CheckByUpdateAddressing(addressingMaintObject.ModifyUserId);
                    planning.Modify(planning.PlanningName, addressingMaintObject.PlaceCategoryId, addressingMaintObject.ReseauId, addressingMaintObject.Lng, addressingMaintObject.Lat,
                    addressingMaintObject.DetailedAddress, planning.Remarks, addressingMaintObject.ProposedNetwork, planning.OptionalAddress, (Importance)addressingMaintObject.Importance,
                    addressingMaintObject.PlaceOwner, addressingMaintObject.CreateUserId);
                    planningRepository.Update(planning);

                    FileAssociation addressingFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressing.Id && entity.EntityName == "Addressing"));
                    if (addressingFileAssociation == null && addressingMaintObject.FileIdList != "")
                    {
                        FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("Addressing", addressing.Id, addressingMaintObject.FileIdList, addressingMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newAddressingFileAssociation);
                    }
                    else if (addressingFileAssociation != null && addressingMaintObject.FileIdList != addressingFileAssociation.FileIdList)
                    {
                        addressingFileAssociation.Modify(addressingMaintObject.FileIdList, addressingMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(addressingFileAssociation);
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的寻址确认在系统中不存在");
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlanningId"))
                {
                    throw new ApplicationFault("选择的寻址确认已经保存过");
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

        public void UpdateAddressingEdit(AddressingMaintObject addressingMaintObject)
        {
            Addressing addressing = addressingRepository.FindByKey(addressingMaintObject.Id);
            Planning planning = planningRepository.FindByKey(addressing.PlanningId);
            if (addressing != null)
            {
                if (addressingMaintObject.CompanyId.ToString() == "9d4a4487-2ad6-4c19-8633-00742e8f1d28")
                {
                    //addressing.UpdateAddressingEdit(addressingMaintObject.ProjectId, addressingMaintObject.ProjectManagerId, addressingMaintObject.ModifyUserId.Value);
                    //addressingRepository.Update(addressing);
                }
                else if (addressingMaintObject.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb")
                {
                    //addressing.ModifyDemand((Demand)addressingMaintObject.MobileDemand, 1);
                    //addressingRepository.Update(addressing);
                    //planning.ModifyDemand((Demand)addressingMaintObject.MobileDemand, 1);
                    //planningRepository.Update(planning);
                }
                else if (addressingMaintObject.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53")
                {
                    //addressing.ModifyDemand((Demand)addressingMaintObject.TelecomDemand, 2);
                    //addressingRepository.Update(addressing);
                    //planning.ModifyDemand((Demand)addressingMaintObject.TelecomDemand, 2);
                    //planningRepository.Update(planning);
                }
                else if (addressingMaintObject.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600")
                {
                    //addressing.ModifyDemand((Demand)addressingMaintObject.UnicomDemand, 3);
                    //addressingRepository.Update(addressing);
                    //planning.ModifyDemand((Demand)addressingMaintObject.UnicomDemand, 3);
                    //planningRepository.Update(planning);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == addressingMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(addressingMaintObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Addressing_dbo.tbl_Project_ProjectId"))
                {
                    throw new ApplicationFault("选择的项目在系统中不存在");
                }
                if (ex.Message.Contains("FK_dbo.tbl_Addressing_dbo.tbl_User_ProjectManagerId"))
                {
                    throw new ApplicationFault("选择的工程经理在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 退回寻址确认任务
        /// </summary>
        /// <param name="addressingMaintObjects">要退回任务的寻址确认维护对象列表</param>
        public void ReturnAddressings(IList<AddressingMaintObject> addressingMaintObjects)
        {
            foreach (AddressingMaintObject addressingMaintObject in addressingMaintObjects)
            {
                Planning planning = planningRepository.FindByKey(addressingMaintObject.PlanningId);
                if (planning != null)
                {
                    planning.ReturnAddressing(addressingMaintObject.ModifyUserId);
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
        /// 获取寻址确认任务
        /// </summary>
        /// <param name="addressingMaintObjects">要获取任务的寻址确认维护对象列表</param>
        public void GetAddressings(IList<AddressingMaintObject> addressingMaintObjects)
        {
            foreach (AddressingMaintObject addressingMaintObject in addressingMaintObjects)
            {
                Planning planning = planningRepository.FindByKey(addressingMaintObject.PlanningId);
                if (planning != null)
                {
                    planning.GetAddressing(addressingMaintObject.ModifyUserId);
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
        /// 保存运营商确认
        /// </summary>
        /// <param name="addressingMaintObject"></param>
        public void SaveOperatorConfirm(AddressingMaintObject addressingMaintObject)
        {
            Addressing addressing = addressingRepository.FindByKey(addressingMaintObject.Id);
            Planning planning = planningRepository.FindByKey(addressing.PlanningId);
            PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
            if (addressing != null)
            {
                if (addressingMaintObject.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb")
                {
                    //addressing.ModifyDemandMobile((Demand)addressingMaintObject.MobileDemand);
                    //addressingRepository.Update(addressing);
                    Bool isShare = Bool.否;
                    //if ((Demand)addressingMaintObject.MobileDemand == Demand.需要)
                    //{
                    //    isShare = Bool.是;
                    //}
                    //placeProperty.ModifyMobile(isShare, addressingMaintObject.MobilePoleNumber, addressingMaintObject.MobileCabinetNumber, addressingMaintObject.MobilePowerUsed, addressingMaintObject.ModifyUserId.Value);
                    //placePropertyRepository.Update(placeProperty);
                    //PlacePropertyLog placePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.移动, isShare, addressingMaintObject.MobilePoleNumber, addressingMaintObject.MobileCabinetNumber, addressingMaintObject.MobilePowerUsed, addressingMaintObject.ModifyUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                    //placePropertyLogRepository.Add(placePropertyLog);
                    //planning.ModifyDemand((Demand)addressingMaintObject.MobileDemand, 1);
                    planningRepository.Update(planning);
                }
                else if (addressingMaintObject.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53")
                {
                    //addressing.ModifyDemandTelecom((Demand)addressingMaintObject.TelecomDemand);
                    //addressingRepository.Update(addressing);
                    Bool isShare = Bool.否;
                    //if ((Demand)addressingMaintObject.TelecomDemand == Demand.需要)
                    //{
                    //    isShare = Bool.是;
                    //}
                    //placeProperty.ModifyTelecom(isShare, addressingMaintObject.TelecomPoleNumber, addressingMaintObject.TelecomCabinetNumber, addressingMaintObject.TelecomPowerUsed, addressingMaintObject.ModifyUserId.Value);
                    //placePropertyRepository.Update(placeProperty);
                    //PlacePropertyLog placePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.电信, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, isShare, addressingMaintObject.TelecomPoleNumber, addressingMaintObject.TelecomCabinetNumber, addressingMaintObject.TelecomPowerUsed, addressingMaintObject.ModifyUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                    //placePropertyLogRepository.Add(placePropertyLog);
                    //planning.ModifyDemand((Demand)addressingMaintObject.TelecomDemand, 2);
                    planningRepository.Update(planning);
                }
                else if (addressingMaintObject.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600")
                {
                    //addressing.ModifyDemandUnicom((Demand)addressingMaintObject.UnicomDemand);
                    //addressingRepository.Update(addressing);
                    Bool isShare = Bool.否;
                    //if ((Demand)addressingMaintObject.UnicomDemand == Demand.需要)
                    //{
                    //    isShare = Bool.是;
                    //}
                    //placeProperty.ModifyUnicom(isShare, addressingMaintObject.UnicomPoleNumber, addressingMaintObject.UnicomCabinetNumber, addressingMaintObject.UnicomPowerUsed, addressingMaintObject.ModifyUserId.Value);
                    //placePropertyRepository.Update(placeProperty);
                    //PlacePropertyLog placePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.联通, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, isShare, addressingMaintObject.UnicomPoleNumber, addressingMaintObject.UnicomCabinetNumber, addressingMaintObject.UnicomPowerUsed, addressingMaintObject.ModifyUserId);
                    //placePropertyLogRepository.Add(placePropertyLog);
                    //planning.ModifyDemand((Demand)addressingMaintObject.UnicomDemand, 3);
                    planningRepository.Update(planning);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == addressingMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(addressingMaintObject.WFActivityInstanceId);
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

        public void SaveProjectAndPlaceCode(AddressingMaintObject addressingMaintObject)
        {
            Addressing addressing = addressingRepository.FindByKey(addressingMaintObject.Id);
            Planning planning = planningRepository.FindByKey(addressing.PlanningId);
            PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
            if (addressing != null)
            {
                //if (addressing.ProjectId == Guid.Empty)
                //{
                //Project project = AggregateFactory.CreateProject(addressingMaintObject.ProjectCode, addressingMaintObject.ProjectName, addressingMaintObject.ProjectName, ProjectCategory.集团投资, Guid.Empty, Guid.Empty, Guid.Empty, "", ProjectProgress.在建中, State.使用, "1", addressingMaintObject.BudgetPrice, addressingMaintObject.ModifyUserId);
                //addressing.ModifyProjectId(project.Id);
                //if (projectRepository.Exists(Specification<Project>.Eval(entity => entity.ProjectCode == addressingMaintObject.ProjectCode)))
                //{
                //    throw new ApplicationFault("项目编码在系统中已存在");
                //}
                //if (projectRepository.Exists(Specification<Project>.Eval(entity => entity.ProjectName == addressingMaintObject.ProjectName)))
                //{
                //    throw new ApplicationFault("项目名称在系统中已存在");
                //}
                //if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.GroupPlaceCode == addressingMaintObject.GroupPlaceCode)))
                //{
                //    throw new ApplicationFault("站点编码在系统中已存在");
                //}
                //placeDesign.AppointProjectAndPlaceCode(addressingMaintObject.ProjectCode, addressingMaintObject.ProjectName, 0, addressingMaintObject.GroupPlaceCode);
                //projectRepository.Add(project);
                //addressingRepository.Update(addressing);
                //placeDesignRepository.Update(placeDesign);
                //}
                //else
                //{
                //    Project project = projectRepository.FindByKey(addressing.ProjectId.Value);
                //    project.ModifyProject(addressingMaintObject.ProjectCode, addressingMaintObject.ProjectName, addressingMaintObject.BudgetPrice);
                //    placeDesign.AppointProjectAndPlaceCode(addressingMaintObject.ProjectCode, addressingMaintObject.ProjectName, addressingMaintObject.BudgetPrice, addressingMaintObject.GroupPlaceCode);
                //    projectRepository.Update(project);
                //    placeDesignRepository.Update(placeDesign);
                //}

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == addressingMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(addressingMaintObject.WFActivityInstanceId);
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
        /// 项目申请立项
        /// </summary>
        /// <param name="addressingEditorObject"></param>
        public void SaveApplyProject(AddressingEditorObject addressingEditorObject)
        {
            Addressing addressing = addressingRepository.FindByKey(addressingEditorObject.Id);
            PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
            if (addressing != null)
            {
                placeDesign.ApplyProject();
                placeDesignRepository.Update(placeDesign);
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
        /// 项目完成立项
        /// </summary>
        /// <param name="addressingEditorObject"></param>
        public void SaveDoApplyProject(AddressingEditorObject addressingEditorObject)
        {
            Addressing addressing = addressingRepository.FindByKey(addressingEditorObject.Id);
            PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(entity => entity.ParentId == addressing.Id && entity.PropertyType == PropertyType.寻址设计));
            if (addressing != null)
            {
                placeDesign.DoApplyProject();
                placeDesignRepository.Update(placeDesign);
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
        /// 根据条件获取分页租赁进度表
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
        /// <param name="addressingState">寻址状态</param>
        /// <param name="addressingDepartmentId">租赁部门</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <param name="isAppoint">指定租赁</param>
        /// <returns>分页租赁进度表的Json字符串</returns>
        public string GetAddressingReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, 
                Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingDepartmentId, Guid addressingUserId, int isAppoint,
                Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(16);
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
            parameters.Add(new Parameter() { Name = "AddressingState", Type = SqlDbType.Int, Value = addressingState });
            parameters.Add(new Parameter() { Name = "AddressingDepartmentId", Type = SqlDbType.UniqueIdentifier, Value = addressingDepartmentId });
            parameters.Add(new Parameter() { Name = "AddressingUserId", Type = SqlDbType.UniqueIdentifier, Value = addressingUserId });
            parameters.Add(new Parameter() { Name = "IsAppoint", Type = SqlDbType.Int, Value = isAppoint });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryAddressingReportPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取租赁月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        public string GetAddressingMonthReseau(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryAddressingMonthReseau", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据条件获取租赁人月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        public string GetAddressingMonthUser(DateTime beginDate, DateTime endDate, Guid departmentId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryAddressingMonthUser", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
