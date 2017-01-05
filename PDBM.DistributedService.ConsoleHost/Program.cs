using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DistributedService.Services.BaseData;
using PDBM.DistributedService.Services.BMMgmt;
using PDBM.DistributedService.Services.DataImport;
using PDBM.DistributedService.Services.DataOutput;
using PDBM.DistributedService.Services.FileMgmt;
using PDBM.DistributedService.Services.Map;
using PDBM.DistributedService.Services.WorkFlow;
using PDBM.Infrastructure.Data.EntityFramework;
using PDBM.Infrastructure.Utils;

namespace PDBM.DistributedService.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("PDBM服务正在启动...");
                LogHelper.InitConfigure();
                PDBMDbContextInitializer.Initialize();
                using (ServiceHost companyService = new ServiceHost(typeof(CompanyService)))
                using (ServiceHost departmentService = new ServiceHost(typeof(DepartmentService)))
                using (ServiceHost userService = new ServiceHost(typeof(UserService)))
                using (ServiceHost menuService = new ServiceHost(typeof(MenuService)))
                using (ServiceHost roleService = new ServiceHost(typeof(RoleService)))
                using (ServiceHost roleMenuItemService = new ServiceHost(typeof(RoleMenuItemService)))
                using (ServiceHost roleUserService = new ServiceHost(typeof(RoleUserService)))
                using (ServiceHost postService = new ServiceHost(typeof(PostService)))
                using (ServiceHost postUserService = new ServiceHost(typeof(PostUserService)))
                using (ServiceHost accountingEntityService = new ServiceHost(typeof(AccountingEntityService)))
                using (ServiceHost projectService = new ServiceHost(typeof(ProjectService)))
                using (ServiceHost areaService = new ServiceHost(typeof(AreaService)))
                using (ServiceHost reseauService = new ServiceHost(typeof(ReseauService)))
                using (ServiceHost placeCategoryService = new ServiceHost(typeof(PlaceCategoryService)))
                using (ServiceHost sceneService = new ServiceHost(typeof(SceneService)))
                using (ServiceHost placeService = new ServiceHost(typeof(PlaceService)))
                using (ServiceHost operatorsPlanningService = new ServiceHost(typeof(OperatorsPlanningService)))
                using (ServiceHost planningService = new ServiceHost(typeof(PlanningService)))
                using (ServiceHost addressingService = new ServiceHost(typeof(AddressingService)))
                using (ServiceHost purchaseService = new ServiceHost(typeof(PurchaseService)))
                using (ServiceHost operatorsConfirmService = new ServiceHost(typeof(OperatorsConfirmService)))
                using (ServiceHost operatorsSharingService = new ServiceHost(typeof(OperatorsSharingService)))
                using (ServiceHost remodelingService = new ServiceHost(typeof(RemodelingService)))
                using (ServiceHost fileService = new ServiceHost(typeof(FileService)))
                using (ServiceHost mapService = new ServiceHost(typeof(MapService)))
                using (ServiceHost dataImportService = new ServiceHost(typeof(DataImportService)))
                using (ServiceHost dataOutputService = new ServiceHost(typeof(DataOutputService)))
                using (ServiceHost wfProcessService = new ServiceHost(typeof(WFProcessService)))
                using (ServiceHost wfCategoryService = new ServiceHost(typeof(WFCategoryService)))
                using (ServiceHost wfActivityService = new ServiceHost(typeof(WFActivityService)))
                using (ServiceHost wfActivityEditorService = new ServiceHost(typeof(WFActivityEditorService)))
                using (ServiceHost wfInstanceService = new ServiceHost(typeof(WFInstanceService)))
                using (ServiceHost constructionTaskService = new ServiceHost(typeof(ConstructionTaskService)))
                using (ServiceHost placePropertyService = new ServiceHost(typeof(PlacePropertyService)))
                using (ServiceHost wfActivityInstanceEditorService = new ServiceHost(typeof(WFActivityInstanceEditorService)))
                using (ServiceHost unitService = new ServiceHost(typeof(UnitService)))
                using (ServiceHost materialCategoryService = new ServiceHost(typeof(MaterialCategoryService)))
                using (ServiceHost materialService = new ServiceHost(typeof(MaterialService)))
                using (ServiceHost materialSpecService = new ServiceHost(typeof(MaterialSpecService)))
                using (ServiceHost customerService = new ServiceHost(typeof(CustomerService)))
                using (ServiceHost operatorsPlanningDemandService = new ServiceHost(typeof(OperatorsPlanningDemandService)))
                using (ServiceHost towerService = new ServiceHost(typeof(TowerService)))
                using (ServiceHost towerBaseService = new ServiceHost(typeof(TowerBaseService)))
                using (ServiceHost machineRoomService = new ServiceHost(typeof(MachineRoomService)))
                using (ServiceHost externalElectricPowerService = new ServiceHost(typeof(ExternalElectricPowerService)))
                using (ServiceHost equipmentInstallService = new ServiceHost(typeof(EquipmentInstallService)))
                using (ServiceHost addressExplorService = new ServiceHost(typeof(AddressExplorService)))
                using (ServiceHost foundationTestService = new ServiceHost(typeof(FoundationTestService)))
                using (ServiceHost placeDesignService = new ServiceHost(typeof(PlaceDesignService)))
                using (ServiceHost materialListService = new ServiceHost(typeof(MaterialListService)))
                using (ServiceHost taskPropertyService = new ServiceHost(typeof(TaskPropertyService)))
                using (ServiceHost towerLogService = new ServiceHost(typeof(TowerLogService)))
                using (ServiceHost towerBaseLogService = new ServiceHost(typeof(TowerBaseLogService)))
                using (ServiceHost machineRoomLogService = new ServiceHost(typeof(MachineRoomLogService)))
                using (ServiceHost externalElectricPowerLogService = new ServiceHost(typeof(ExternalElectricPowerLogService)))
                using (ServiceHost equipmentInstallLogService = new ServiceHost(typeof(EquipmentInstallLogService)))
                using (ServiceHost addressExplorLogService = new ServiceHost(typeof(AddressExplorLogService)))
                using (ServiceHost foundationTestLogService = new ServiceHost(typeof(FoundationTestLogService)))
                using (ServiceHost taskPropertyLogService = new ServiceHost(typeof(TaskPropertyLogService)))
                using (ServiceHost placePropertyLogService = new ServiceHost(typeof(PlacePropertyLogService)))
                using (ServiceHost workBigClassService = new ServiceHost(typeof(WorkBigClassService)))
                using (ServiceHost workSmallClassService = new ServiceHost(typeof(WorkSmallClassService)))
                using (ServiceHost workApplyService = new ServiceHost(typeof(WorkApplyService)))
                using (ServiceHost workOrderService = new ServiceHost(typeof(WorkOrderService)))
                using (ServiceHost workOrderDetailService = new ServiceHost(typeof(WorkOrderDetailService)))
                using (ServiceHost customerUserService = new ServiceHost(typeof(CustomerUserService)))
                using (ServiceHost delayApplyService = new ServiceHost(typeof(DelayApplyService)))
                using (ServiceHost projectCodeListService = new ServiceHost(typeof(ProjectCodeListService)))
                using (ServiceHost materialSpecListService = new ServiceHost(typeof(MaterialSpecListService)))
                using (ServiceHost planningApplyService = new ServiceHost(typeof(PlanningApplyService)))
                using (ServiceHost placeOwnerService = new ServiceHost(typeof(PlaceOwnerService)))
                using (ServiceHost projectTaskService = new ServiceHost(typeof(ProjectTaskService)))
                using (ServiceHost engineeringTaskService = new ServiceHost(typeof(EngineeringTaskService)))
                using (ServiceHost businessVolumeService = new ServiceHost(typeof(BusinessVolumeService)))
                using (ServiceHost noticeService = new ServiceHost(typeof(NoticeService)))
                using (ServiceHost placeBusinessVolumeService = new ServiceHost(typeof(PlaceBusinessVolumeService)))
                using (ServiceHost blindSpotFeedBackService = new ServiceHost(typeof(BlindSpotFeedBackService)))
                using (ServiceHost planningApplyHeaderService = new ServiceHost(typeof(PlanningApplyHeaderService)))
                using (ServiceHost dutyUserService = new ServiceHost(typeof(DutyUserService)))
                {
                    companyService.Open();
                    departmentService.Open();
                    userService.Open();
                    menuService.Open();
                    roleService.Open();
                    roleMenuItemService.Open();
                    roleUserService.Open();
                    postService.Open();
                    postUserService.Open();
                    accountingEntityService.Open();
                    projectService.Open();
                    areaService.Open();
                    reseauService.Open();
                    placeCategoryService.Open();
                    sceneService.Open();
                    placeService.Open();
                    operatorsPlanningService.Open();
                    planningService.Open();
                    addressingService.Open();
                    purchaseService.Open();
                    operatorsConfirmService.Open();
                    operatorsSharingService.Open();
                    remodelingService.Open();
                    fileService.Open();
                    mapService.Open();
                    dataImportService.Open();
                    dataOutputService.Open();
                    wfProcessService.Open();
                    wfCategoryService.Open();
                    wfActivityService.Open();
                    wfActivityEditorService.Open();
                    wfInstanceService.Open();
                    constructionTaskService.Open();
                    placePropertyService.Open();
                    wfActivityInstanceEditorService.Open();
                    unitService.Open();
                    materialCategoryService.Open();
                    materialService.Open();
                    materialSpecService.Open();
                    customerService.Open();
                    operatorsPlanningDemandService.Open();
                    towerService.Open();
                    towerBaseService.Open();
                    machineRoomService.Open();
                    externalElectricPowerService.Open();
                    equipmentInstallService.Open();
                    addressExplorService.Open();
                    foundationTestService.Open();
                    placeDesignService.Open();
                    materialListService.Open();
                    taskPropertyService.Open();
                    towerLogService.Open();
                    towerBaseLogService.Open();
                    machineRoomLogService.Open();
                    externalElectricPowerLogService.Open();
                    equipmentInstallLogService.Open();
                    addressExplorLogService.Open();
                    foundationTestLogService.Open();
                    taskPropertyLogService.Open();
                    placePropertyLogService.Open();
                    workBigClassService.Open();
                    workSmallClassService.Open();
                    workApplyService.Open();
                    workOrderService.Open();
                    workOrderDetailService.Open();
                    customerUserService.Open();
                    delayApplyService.Open();
                    projectCodeListService.Open();
                    materialSpecListService.Open();
                    planningApplyService.Open();
                    placeOwnerService.Open();
                    projectTaskService.Open();
                    engineeringTaskService.Open();
                    businessVolumeService.Open();
                    noticeService.Open();
                    placeBusinessVolumeService.Open();
                    blindSpotFeedBackService.Open();
                    planningApplyHeaderService.Open();
                    dutyUserService.Open();
                    Console.WriteLine("PDBM服务已启动：{0}.", DateTime.Now);
                    while (true)
                    {
                        string input = Console.ReadLine().Trim().ToLower();
                        if (input == "clear")
                        {
                            Console.Clear();
                        }
                        else if (input == "exit")
                        {
                            break;
                        }
                    }
                    Console.WriteLine("PDBM服务正在关闭...");
                    companyService.Close();
                    departmentService.Close();
                    userService.Close();
                    menuService.Close();
                    roleService.Close();
                    roleMenuItemService.Close();
                    roleUserService.Close();
                    postService.Close();
                    postUserService.Close();
                    accountingEntityService.Close();
                    projectService.Close();
                    areaService.Close();
                    reseauService.Close();
                    placeCategoryService.Close();
                    sceneService.Close();
                    placeService.Close();
                    operatorsPlanningService.Close();
                    planningService.Close();
                    addressingService.Close();
                    purchaseService.Close();
                    operatorsConfirmService.Close();
                    operatorsSharingService.Close();
                    remodelingService.Close();
                    fileService.Close();
                    mapService.Close();
                    dataImportService.Close();
                    dataOutputService.Close();
                    wfProcessService.Close();
                    wfCategoryService.Close();
                    wfActivityService.Close();
                    wfActivityEditorService.Close();
                    wfInstanceService.Close();
                    constructionTaskService.Close();
                    placePropertyService.Close();
                    wfActivityInstanceEditorService.Close();
                    unitService.Close();
                    materialCategoryService.Close();
                    materialService.Close();
                    materialSpecService.Close();
                    customerService.Close();
                    operatorsPlanningDemandService.Close();
                    towerService.Close();
                    towerBaseService.Close();
                    machineRoomService.Close();
                    externalElectricPowerService.Close();
                    equipmentInstallService.Close();
                    addressExplorService.Close();
                    foundationTestService.Close();
                    placeDesignService.Close();
                    materialListService.Close();
                    taskPropertyService.Close();
                    towerLogService.Close();
                    towerBaseLogService.Close();
                    machineRoomLogService.Close();
                    externalElectricPowerLogService.Close();
                    equipmentInstallLogService.Close();
                    addressExplorLogService.Close();
                    foundationTestLogService.Close();
                    taskPropertyLogService.Close();
                    placePropertyLogService.Close();
                    workBigClassService.Close();
                    workSmallClassService.Close();
                    workApplyService.Close();
                    workOrderService.Close();
                    workOrderDetailService.Close();
                    customerUserService.Close();
                    delayApplyService.Close();
                    projectCodeListService.Close();
                    materialSpecListService.Close();
                    planningApplyService.Close();
                    placeOwnerService.Close();
                    projectTaskService.Close();
                    engineeringTaskService.Close();
                    businessVolumeService.Close();
                    noticeService.Close();
                    placeBusinessVolumeService.Close();
                    blindSpotFeedBackService.Close();
                    planningApplyHeaderService.Close();
                    dutyUserService.Close();
                    Console.WriteLine("PDBM服务已关闭：{0}.", DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
