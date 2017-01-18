using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.DataImport;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Services;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.DataImport;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.DataTransferObjects.BaseData;

namespace PDBM.ApplicationService.Services.DataImport
{
    /// <summary>
    /// 数据导入应用层服务
    /// </summary>
    public class DataImportService : DataService, IDataImportService
    {
        private readonly IRepository<File> fileRepository;
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly IRepository<OperatorsSharing> operatorsSharingRepository;
        private readonly IRepository<Purchase> purchaseRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<PlaceCategory> placeCategoryRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<PlaceDesign> placeDesignRepository;
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;
        private readonly IRepository<Scene> sceneRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<OperatorsConfirm> operatorsConfirmRepository;
        private readonly IRepository<OperatorsConfirmDetail> operatorsConfirmDetailRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<TowerLog> towerLogRepository;
        private readonly IRepository<TowerBaseLog> towerBaseLogRepository;
        private readonly IRepository<MachineRoomLog> machineRoomLogRepository;
        private readonly IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository;
        private readonly IRepository<EquipmentInstallLog> equipmentInstallLogRepository;
        private readonly IRepository<AddressExplorLog> addressExplorLogRepository;
        private readonly IRepository<FoundationTestLog> foundationTestLogRepository;
        private readonly IRepository<ProjectCodeList> projectCodeListRepository;
        private readonly IRepository<MaterialSpecList> materialSpecListRepository;
        private readonly ICodeSeedRepository codeSeedRepository;
        private readonly IBMMgmtService bmMgmtService;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<BusinessVolume> businessVolumeRepository;
        private readonly IRepository<PlaceBusinessVolume> placeBusinessVolumeRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;

        public DataImportService(IRepositoryContext context,
            IRepository<File> fileRepository,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            IRepository<OperatorsSharing> operatorsSharingRepository,
            IRepository<Purchase> purchaseRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<Area> areaRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<PlaceCategory> placeCategoryRepository,
            IRepository<Place> placeRepository,
            IRepository<PlaceDesign> placeDesignRepository,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<PlacePropertyLog> placePropertyLogRepository,
            IRepository<Scene> sceneRepository,
            IRepository<User> userRepository,
            IRepository<Planning> planningRepository,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<OperatorsConfirm> operatorsConfirmRepository,
            IRepository<OperatorsConfirmDetail> operatorsConfirmDetailRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<TowerLog> towerLogRepository,
            IRepository<TowerBaseLog> towerBaseLogRepository,
            IRepository<MachineRoomLog> machineRoomLogRepository,
            IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository,
            IRepository<EquipmentInstallLog> equipmentInstallLogRepository,
            IRepository<AddressExplorLog> addressExplorLogRepository,
            IRepository<FoundationTestLog> foundationTestLogRepository,
            IRepository<ProjectCodeList> projectCodeListRepository,
            IRepository<MaterialSpecList> materialSpecListRepository,
            ICodeSeedRepository codeSeedRepository,
            IBMMgmtService bmMgmtService,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<Department> departmentRepository,
            IRepository<BusinessVolume> businessVolumeRepository,
            IRepository<PlaceBusinessVolume> placeBusinessVolumeRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository)
            : base(context)
        {
            this.fileRepository = fileRepository;
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.operatorsSharingRepository = operatorsSharingRepository;
            this.purchaseRepository = purchaseRepository;
            this.remodelingRepository = remodelingRepository;
            this.areaRepository = areaRepository;
            this.reseauRepository = reseauRepository;
            this.placeCategoryRepository = placeCategoryRepository;
            this.placeRepository = placeRepository;
            this.placeDesignRepository = placeDesignRepository;
            this.placePropertyRepository = placePropertyRepository;
            this.placePropertyLogRepository = placePropertyLogRepository;
            this.sceneRepository = sceneRepository;
            this.userRepository = userRepository;
            this.planningRepository = planningRepository;
            this.planningApplyRepository = planningApplyRepository;
            this.addressingRepository = addressingRepository;
            this.operatorsConfirmRepository = operatorsConfirmRepository;
            this.operatorsConfirmDetailRepository = operatorsConfirmDetailRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.towerLogRepository = towerLogRepository;
            this.towerBaseLogRepository = towerBaseLogRepository;
            this.machineRoomLogRepository = machineRoomLogRepository;
            this.externalElectricPowerLogRepository = externalElectricPowerLogRepository;
            this.equipmentInstallLogRepository = equipmentInstallLogRepository;
            this.addressExplorLogRepository = addressExplorLogRepository;
            this.foundationTestLogRepository = foundationTestLogRepository;
            this.projectCodeListRepository = projectCodeListRepository;
            this.materialSpecListRepository = materialSpecListRepository;
            this.codeSeedRepository = codeSeedRepository;
            this.bmMgmtService = bmMgmtService;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.placeOwnerRepository = placeOwnerRepository;
            this.departmentRepository = departmentRepository;
            this.businessVolumeRepository = businessVolumeRepository;
            this.placeBusinessVolumeRepository = placeBusinessVolumeRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
        }

        /// <summary>
        /// 导入运营商基站规划
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="currentCompanyNature">创建人所在公司性质</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportOperatorsPlanningBS(Guid excelFileId, Guid createUserId, Guid companyId, int currentCompanyNature)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 10)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为10列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "规划名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为规划名称"));
                    }
                    if (dt.Columns[1].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为区域"));
                    }
                    if (dt.Columns[2].ColumnName != "基站类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为基站类型"));
                    }
                    if (dt.Columns[3].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为经度"));
                    }
                    if (dt.Columns[4].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为纬度"));
                    }
                    if (dt.Columns[5].ColumnName != "天线挂高")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为天线挂高"));
                    }
                    if (dt.Columns[6].ColumnName != "抱杆数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为抱杆数量"));
                    }
                    if (dt.Columns[7].ColumnName != "机柜数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为机柜数量"));
                    }
                    if (dt.Columns[8].ColumnName != "紧要程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为紧要程度"));
                    }
                    if (dt.Columns[9].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("规划名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "规划名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string planningName = "";
                        Guid areaId = Guid.Empty;
                        Guid placeCategoryId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        decimal antennaHeight = 0;
                        int poleNumber = 0;
                        int cabinetNumber = 0;
                        Urgency urgency = Urgency.一级;
                        string remarks = "";
                        IList<OperatorsPlanning> operatorsPlannings = new List<OperatorsPlanning>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规划名称验证
                            if (dr["规划名称"].ToString().Trim() != "")
                            {
                                planningName = dr["规划名称"].ToString().Trim();
                                if (operatorsPlanningRepository.Exists(Specification<OperatorsPlanning>.Eval(entity => entity.PlanningName == planningName && entity.CompanyId == companyId)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", planningName + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //基站类型验证
                            if (dr["基站类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["基站类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.基站));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //天线挂高验证
                            if (dr["天线挂高"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["天线挂高"].ToString().Trim(), out antennaHeight))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "天线挂高", "必须为数字"));
                                }
                            }
                            else
                            {
                                antennaHeight = 0;
                            }

                            //抱杆数量验证
                            if (dr["抱杆数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["抱杆数量"].ToString().Trim(), out poleNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "抱杆数量", "必须为数字"));
                                }
                            }
                            else
                            {
                                poleNumber = 0;
                            }

                            //机柜数量验证
                            if (dr["机柜数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["机柜数量"].ToString().Trim(), out cabinetNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "机柜数量", "必须为数字"));
                                }
                            }
                            else
                            {
                                cabinetNumber = 0;
                            }

                            //紧要程度验证
                            if (dr["紧要程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Urgency>(dr["紧要程度"].ToString().Trim(), out urgency))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "紧要程度", dr["紧要程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (urgency != Urgency.一级 && urgency != Urgency.二级)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "紧要程度", dr["紧要程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "紧要程度", "不能为空"));
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                operatorsPlannings.Add(AggregateFactory.CreateOperatorsPlanning("-1", planningName, Profession.基站, placeCategoryId, areaId, lng, lat, antennaHeight, poleNumber, cabinetNumber, urgency, Bool.否, remarks, companyId, null, createUserId, (CompanyNature)currentCompanyNature));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            //生成规划编码
                            IList<string> codes = codeSeedRepository.GenerateCodes("OperatorsPlanning", operatorsPlannings.Count);

                            for (int i = 0; i < operatorsPlannings.Count; i++)
                            {
                                operatorsPlannings[i].ModifyPlanningCode(codes[i]);
                                operatorsPlanningRepository.Add(operatorsPlannings[i]);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_OperatorsPlanningCode"))
                                {
                                    throw new ApplicationFault("规划编码重复");
                                }
                                else if (ex.Message.Contains("IX_UQ_CompanyIdOperatorsPlanningName"))
                                {
                                    throw new ApplicationFault("规划名称重复");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("基站类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId"))
                                {
                                    throw new ApplicationFault("区域在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入运营商共享基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="currentCompanyNature">创建人所在公司性质</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportOperatorsSharingBS(Guid excelFileId, Guid createUserId, Guid companyId, int currentCompanyNature)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 6)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为6列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "基站名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为基站名称"));
                    }
                    if (dt.Columns[1].ColumnName != "天线挂高")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为天线挂高"));
                    }
                    if (dt.Columns[2].ColumnName != "抱杆数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为抱杆数量"));
                    }
                    if (dt.Columns[3].ColumnName != "机柜数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为机柜数量"));
                    }
                    if (dt.Columns[4].ColumnName != "紧要程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为紧要程度"));
                    }
                    if (dt.Columns[5].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证基站名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("基站名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "基站名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //基站名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string placeCode = "";
                        string placeName = "";
                        Guid placeId = Guid.Empty;
                        decimal antennaHeight = 0;
                        int poleNumber = 0;
                        int cabinetNumber = 0;
                        Urgency urgency = Urgency.一级;
                        string remarks = "";
                        IList<OperatorsSharing> operatorsSharings = new List<OperatorsSharing>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //基站名称验证
                            if (dr["基站名称"].ToString().Trim() != "")
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName && entity.Profession == Profession.基站));
                                if (place != null)
                                {
                                    placeId = place.Id;
                                    placeCode = place.PlaceCode;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", placeName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "不能为空"));
                            }

                            //天线挂高验证
                            if (dr["天线挂高"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["天线挂高"].ToString().Trim(), out antennaHeight))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "天线挂高", "必须为数字"));
                                }
                            }
                            else
                            {
                                antennaHeight = 0;
                            }

                            //抱杆数量验证
                            if (dr["抱杆数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["抱杆数量"].ToString().Trim(), out poleNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "抱杆数量", "必须为数字"));
                                }
                            }
                            else
                            {
                                poleNumber = 0;
                            }

                            //机柜数量验证
                            if (dr["机柜数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["机柜数量"].ToString().Trim(), out cabinetNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "机柜数量", "必须为数字"));
                                }
                            }
                            else
                            {
                                cabinetNumber = 0;
                            }

                            //紧要程度验证
                            if (dr["紧要程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Urgency>(dr["紧要程度"].ToString().Trim(), out urgency))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "紧要程度", dr["紧要程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (urgency != Urgency.一级 && urgency != Urgency.二级)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "紧要程度", dr["紧要程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "紧要程度", "不能为空"));
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                operatorsSharings.Add(AggregateFactory.CreateOperatorsSharing(Profession.基站, placeCode, placeId, antennaHeight, poleNumber, cabinetNumber, urgency, Bool.否, remarks, companyId, null, Guid.Empty, createUserId, (CompanyNature)currentCompanyNature));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            foreach (var operatorsSharing in operatorsSharings)
                            {
                                operatorsSharingRepository.Add(operatorsSharing);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("FK_dbo.tbl_OperatorsSharing_dbo.tbl_Place_PlaceId"))
                                {
                                    throw new ApplicationFault("基站在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入购置基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportPurchaseBS(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 18)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为18列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "站点编码")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为站点编码"));
                    }
                    if (dt.Columns[1].ColumnName != "基站名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为基站名称"));
                    }
                    if (dt.Columns[2].ColumnName != "基站类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为基站类型"));
                    }
                    if (dt.Columns[3].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为区域"));
                    }
                    if (dt.Columns[4].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为网格"));
                    }
                    if (dt.Columns[5].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为经度"));
                    }
                    if (dt.Columns[6].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为纬度"));
                    }
                    if (dt.Columns[7].ColumnName != "产权")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为产权"));
                    }
                    if (dt.Columns[8].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[9].ColumnName != "周边场景")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为周边场景"));
                    }
                    if (dt.Columns[10].ColumnName != "移动共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为移动共享"));
                    }
                    if (dt.Columns[11].ColumnName != "电信共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为电信共享"));
                    }
                    if (dt.Columns[12].ColumnName != "联通共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为联通共享"));
                    }
                    if (dt.Columns[13].ColumnName != "业主名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为业主名称"));
                    }
                    if (dt.Columns[14].ColumnName != "联系人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为联系人"));
                    }
                    if (dt.Columns[15].ColumnName != "联系方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十六列", "列名必须为联系方式"));
                    }
                    if (dt.Columns[16].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十七列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[17].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十八列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证基站名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("基站名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "基站名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //基站名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string groupPlaceCode = "";
                        string placeName = "";
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        Guid placeCategoryId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        PropertyRight propertyRight = PropertyRight.购电信;
                        Importance importance = Importance.A;
                        Guid sceneId = Guid.Empty;
                        Bool telecomShare = Bool.否;
                        Bool mobileShare = Bool.否;
                        Bool unicomShare = Bool.否;
                        string ownerName = "";
                        string ownerContact = "";
                        string ownerPhoneNumber = "";
                        string detailedAddress = "";
                        string remarks = "";
                        IList<Purchase> purchases = new List<Purchase>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //站点编码验证
                            if (dr["站点编码"].ToString().Trim() != "")
                            {
                                groupPlaceCode = dr["站点编码"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "站点编码", "不能为空"));
                            }

                            //基站名称验证
                            if (dr["基站名称"].ToString().Trim() != "")
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.PlaceName == placeName)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", placeName + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "不能为空"));
                            }

                            //基站类型验证
                            if (dr["基站类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["基站类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.基站));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //产权验证
                            if (dr["产权"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<PropertyRight>(dr["产权"].ToString().Trim(), out propertyRight))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", dr["产权"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (propertyRight != PropertyRight.铁塔 && propertyRight != PropertyRight.购电信 && propertyRight != PropertyRight.购移动 && propertyRight != PropertyRight.购联通)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", dr["产权"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", "不能为空"));
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //周边场景验证
                            if (dr["周边场景"].ToString().Trim() != "")
                            {
                                string sceneName = dr["周边场景"].ToString().Trim();
                                Scene scene = sceneRepository.Find(Specification<Scene>.Eval(entity => entity.SceneName == sceneName));
                                if (scene != null)
                                {
                                    sceneId = scene.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "周边场景", sceneName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "周边场景", "不能为空"));
                            }

                            //移动共享验证
                            if (dr["移动共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["移动共享"].ToString().Trim(), out mobileShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", dr["移动共享"].ToString().Trim() + "-只能为是或者否"));
                                }
                                else
                                {
                                    if (mobileShare != Bool.否 && mobileShare != Bool.是)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", dr["移动共享"].ToString().Trim() + "-只能为是或者否"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", "不能为空"));
                            }

                            //电信共享验证
                            if (dr["电信共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["电信共享"].ToString().Trim(), out telecomShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", dr["电信共享"].ToString().Trim() + "-只能为是或者否"));
                                }
                                else
                                {
                                    if (telecomShare != Bool.否 && telecomShare != Bool.是)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", dr["电信共享"].ToString().Trim() + "-只能为是或者否"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", "不能为空"));
                            }

                            //联通共享验证
                            if (dr["联通共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["联通共享"].ToString().Trim(), out unicomShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", dr["联通共享"].ToString().Trim() + "-只能为是或者否"));
                                }
                                else
                                {
                                    if (unicomShare != Bool.否 && unicomShare != Bool.是)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", dr["联通共享"].ToString().Trim() + "-只能为是或者否"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", "不能为空"));
                            }

                            ownerName = dr["业主名称"].ToString().Trim();
                            ownerContact = dr["联系人"].ToString().Trim();
                            ownerPhoneNumber = dr["联系方式"].ToString().Trim();

                            //详细地址验证
                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailedAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                purchases.Add(AggregateFactory.CreatePurchase(DateTime.Now, groupPlaceCode, placeName, Profession.基站, placeCategoryId, reseauId, lng, lat, propertyRight, importance, sceneId, detailedAddress, ownerName, ownerContact, ownerPhoneNumber, telecomShare, mobileShare, unicomShare, remarks, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            //生成基站编码
                            IList<string> codes = codeSeedRepository.GenerateCodes("Place", purchases.Count);

                            for (int i = 0; i < purchases.Count; i++)
                            {
                                purchaseRepository.Add(purchases[i]);
                                Place place = bmMgmtService.CreatePlace(purchases[i], codes[i]);
                                placeRepository.Add(place);
                                PlaceProperty placeProperty = AggregateFactory.CreatePlaceProperty(purchases[i].PlaceId, PropertyType.站点参数, purchases[i].MobileShare, 0, 0, 0, Guid.Empty, purchases[i].TelecomShare, 0, 0, 0, Guid.Empty, purchases[i].UnicomShare, 0, 0, 0, Guid.Empty);
                                placePropertyRepository.Add(placeProperty);
                                //if (purchases[i].MobileShare == Bool.是)
                                //{
                                PlacePropertyLog mobilePlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.导入, purchases[i].PlaceId, PropertyType.站点参数, CompanyNameId.移动, purchases[i].MobileShare, 0, 0, 0, Guid.Empty, purchases[i].TelecomShare, 0, 0, 0, Guid.Empty, purchases[i].UnicomShare, 0, 0, 0, Guid.Empty);
                                placePropertyLogRepository.Add(mobilePlacePropertyLog);
                                //}
                                //if (purchases[i].TelecomShare == Bool.是)
                                //{
                                PlacePropertyLog telecomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.导入, purchases[i].PlaceId, PropertyType.站点参数, CompanyNameId.电信, purchases[i].MobileShare, 0, 0, 0, Guid.Empty, purchases[i].TelecomShare, 0, 0, 0, Guid.Empty, purchases[i].UnicomShare, 0, 0, 0, Guid.Empty);
                                placePropertyLogRepository.Add(telecomPlacePropertyLog);
                                //}
                                //if (purchases[i].UnicomShare == Bool.是)
                                //{
                                PlacePropertyLog unicomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.导入, purchases[i].PlaceId, PropertyType.站点参数, CompanyNameId.联通, purchases[i].MobileShare, 0, 0, 0, Guid.Empty, purchases[i].TelecomShare, 0, 0, 0, Guid.Empty, purchases[i].UnicomShare, 0, 0, 0, Guid.Empty);
                                placePropertyLogRepository.Add(unicomPlacePropertyLog);
                                //}
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_GroupPlaceCode"))
                                {
                                    throw new ApplicationFault("站点编码重复");
                                }
                                if (ex.Message.Contains("IX_UQ_PlaceCode"))
                                {
                                    throw new ApplicationFault("站点编码重复");
                                }
                                else if (ex.Message.Contains("IX_UQ_PlaceName"))
                                {
                                    throw new ApplicationFault("站点名称重复");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_Scene_SceneId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId"))
                                {
                                    throw new ApplicationFault("选择的周边场景在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入基站规划
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportPlanning(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 9)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为9列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "规划名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为规划名称"));
                    }
                    if (dt.Columns[1].ColumnName != "基站类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为基站类型"));
                    }
                    if (dt.Columns[2].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为区域"));
                    }
                    if (dt.Columns[3].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为网格"));
                    }
                    if (dt.Columns[4].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为经度"));
                    }
                    if (dt.Columns[5].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为纬度"));
                    }
                    if (dt.Columns[6].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[7].ColumnName != "拟建网络")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为拟建网络"));
                    }
                    if (dt.Columns[8].ColumnName != "可选位置")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为可选位置"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("规划名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "规划名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string planningName = "";
                        Guid placeCategoryId = Guid.Empty;
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        string proposedNetwork = "";
                        string optionalAddress = "";
                        IList<Planning> plannings = new List<Planning>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规划名称验证
                            if (dr["规划名称"].ToString().Trim() != "")
                            {
                                planningName = dr["规划名称"].ToString().Trim();
                                if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.PlanningName == planningName)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", planningName + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", "不能为空"));
                            }

                            //基站类型验证
                            if (dr["基站类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["基站类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.基站));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //验证拟建网络
                            if (dr["拟建网络"].ToString().Trim() != "")
                            {
                                proposedNetwork = dr["拟建网络"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "拟建网络", "不能为空"));
                            }

                            //验证可选位置
                            if (dr["可选位置"].ToString().Trim() != "")
                            {
                                optionalAddress = dr["可选位置"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "可选位置", "不能为空"));
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                plannings.Add(AggregateFactory.CreatePlanning(codeSeedRepository.GenerateCode("Planning"), planningName, Profession.基站, placeCategoryId, reseauId, lng, lat, "", "", proposedNetwork, optionalAddress, importance, Guid.Empty, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < plannings.Count; i++)
                            {
                                planningRepository.Add(plannings[i]);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_PlanningCode"))
                                {
                                    throw new ApplicationFault("站点编码重复");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入基站改造
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportRemodeling(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<RemodelingImportObject> remodelingImportObjects = new List<RemodelingImportObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 3)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为3列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "基站名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为基站名称"));
                    }
                    if (dt.Columns[1].ColumnName != "建设方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为建设方式"));
                    }
                    if (dt.Columns[2].ColumnName != "拟建网络")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为拟建网络"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证基站名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("基站名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "基站名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string placeCode = "";
                        string placeName = "";
                        Guid placeId = Guid.Empty;
                        ProjectType projectType = ProjectType.改造;
                        string proposedNetwork = "";
                        IList<Remodeling> remodelings = new List<Remodeling>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //基站名称验证
                            if (dr["基站名称"].ToString().Trim() != "")
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName && entity.Profession == Profession.基站));
                                if (place != null)
                                {
                                    placeId = place.Id;
                                    placeCode = place.PlaceCode;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", placeName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "不能为空"));
                            }

                            //建设方式验证
                            if (dr["建设方式"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<ProjectType>(dr["建设方式"].ToString().Trim(), out projectType))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", dr["建设方式"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (projectType != ProjectType.改造 && projectType != ProjectType.部分拆除 && projectType != ProjectType.全部拆除)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", dr["建设方式"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", "不能为空"));
                            }

                            //验证拟建网络
                            if (dr["拟建网络"].ToString().Trim() != "")
                            {
                                proposedNetwork = dr["拟建网络"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "拟建网络", "不能为空"));
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                remodelingImportObjects.Add(BuildRemodelingImportObject((int)projectType));
                                remodelings.Add(AggregateFactory.CreateRemodeling(Profession.基站, placeCode, placeId, proposedNetwork, "", createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < remodelings.Count; i++)
                            {
                                remodelingRepository.Add(remodelings[i]);

                                ProjectTask projectTask = AggregateFactory.CreateProjectTask((ProjectType)remodelingImportObjects[i].ProjectType, remodelings[i].Id, remodelings[i].PlaceId, "", createUserId);
                                projectTaskRepository.Add(projectTask);

                                EngineeringTask et1 = AggregateFactory.CreateEngineeringTask(TaskModel.天桅, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et1);
                                EngineeringTask et2 = AggregateFactory.CreateEngineeringTask(TaskModel.天桅基础, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et2);
                                EngineeringTask et3 = AggregateFactory.CreateEngineeringTask(TaskModel.机房, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et3);
                                EngineeringTask et4 = AggregateFactory.CreateEngineeringTask(TaskModel.外电引入, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et4);
                                EngineeringTask et5 = AggregateFactory.CreateEngineeringTask(TaskModel.设备安装, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et5);
                                EngineeringTask et6 = AggregateFactory.CreateEngineeringTask(TaskModel.线路, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et6);
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
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportPlace(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 15)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为15列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "基站名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为基站名称"));
                    }
                    if (dt.Columns[1].ColumnName != "基站类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为基站类型"));
                    }
                    if (dt.Columns[2].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为区域"));
                    }
                    if (dt.Columns[3].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为网格"));
                    }
                    if (dt.Columns[4].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为经度"));
                    }
                    if (dt.Columns[5].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为纬度"));
                    }
                    if (dt.Columns[6].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[7].ColumnName != "产权")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为产权"));
                    }
                    if (dt.Columns[8].ColumnName != "租赁部门")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为租赁部门"));
                    }
                    if (dt.Columns[9].ColumnName != "实际租赁人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为实际租赁人"));
                    }
                    if (dt.Columns[10].ColumnName != "业主名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为业主名称"));
                    }
                    if (dt.Columns[11].ColumnName != "联系人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为联系人"));
                    }
                    if (dt.Columns[12].ColumnName != "联系方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为联系方式"));
                    }
                    if (dt.Columns[13].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[14].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 2000)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为2000行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("基站名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "基站名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string placeName = "";
                        Guid placeCategoryId = Guid.Empty;
                        Guid placeOwnerId = Guid.Empty;
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        Guid addressingDepartmentId = Guid.Empty;
                        string addressingRealName = "";
                        string ownerName = "";
                        string ownerContact = "";
                        string ownerPhoneNumber = "";
                        string detailedAddress = "";
                        string remarks = "";
                        IList<Place> places = new List<Place>(dt.Rows.Count);
                        IList<PlaceBusinessVolume> placeBusinessVolumes = new List<PlaceBusinessVolume>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规基站称验证
                            if (dr["基站名称"].ToString().Trim() != "")
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.PlaceName == placeName)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", placeName + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "不能为空"));
                            }

                            //基站类型验证
                            if (dr["基站类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["基站类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.基站));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //产权验证
                            if (dr["产权"].ToString().Trim() != "")
                            {
                                string placeOwnerName = dr["产权"].ToString().Trim();
                                PlaceOwner placeOwner = placeOwnerRepository.Find(Specification<PlaceOwner>.Eval(entity => entity.PlaceOwnerName == placeOwnerName));
                                if (placeOwner != null)
                                {
                                    placeOwnerId = placeOwner.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", placeOwnerName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", "不能为空"));
                            }

                            //租赁部门验证
                            if (dr["租赁部门"].ToString().Trim() != "")
                            {
                                string addressingDepartmentName = dr["租赁部门"].ToString().Trim();
                                Department department = departmentRepository.Find(Specification<Department>.Eval(entity => entity.DepartmentName == addressingDepartmentName));
                                if (department != null)
                                {
                                    addressingDepartmentId = department.Id;
                                }
                                //else
                                //{
                                //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "租赁部门", addressingDepartmentName + "-在系统中不存在"));
                                //}
                            }
                            //else
                            //{
                            //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "租赁部门", "不能为空"));
                            //}

                            //验证实际租赁人
                            if (dr["实际租赁人"].ToString().Trim() != "")
                            {
                                addressingRealName = dr["实际租赁人"].ToString().Trim();
                            }
                            //else
                            //{
                            //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "实际租赁人", "不能为空"));
                            //}

                            ownerName = dr["业主名称"].ToString().Trim();
                            ownerContact = dr["联系人"].ToString().Trim();
                            ownerPhoneNumber = dr["联系方式"].ToString().Trim();

                            //详细地址验证
                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailedAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                places.Add(AggregateFactory.CreatePlace(codeSeedRepository.GenerateCode("Place"), placeName, Profession.基站, placeCategoryId, reseauId, lng,
                                    lat, placeOwnerId, importance, addressingDepartmentId, addressingRealName, ownerName, ownerContact, ownerPhoneNumber, detailedAddress,
                                    remarks, PlaceMapState.项目开通, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            User user = userRepository.GetByKey(createUserId);
                            Department department = departmentRepository.GetByKey(user.DepartmentId);
                            for (int i = 0; i < places.Count; i++)
                            {
                                placeRepository.Add(places[i]);

                                PlaceBusinessVolume placeBusinessVolume = AggregateFactory.CreatePlaceBusinessVolume(places[i].Id, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, department.CompanyId);
                                placeBusinessVolumeRepository.Add(placeBusinessVolume);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_PlaceCode"))
                                {
                                    throw new ApplicationFault("基站编码重复");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 更新基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> UpdatePlace(Guid excelFileId, Guid modifyUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<PlaceMaintObject> placeMaintObjects = new List<PlaceMaintObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 15)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为15列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "基站名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为基站名称"));
                    }
                    if (dt.Columns[1].ColumnName != "基站类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为基站类型"));
                    }
                    if (dt.Columns[2].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为区域"));
                    }
                    if (dt.Columns[3].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为网格"));
                    }
                    if (dt.Columns[4].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为经度"));
                    }
                    if (dt.Columns[5].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为纬度"));
                    }
                    if (dt.Columns[6].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[7].ColumnName != "产权")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为产权"));
                    }
                    if (dt.Columns[8].ColumnName != "租赁部门")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为租赁部门"));
                    }
                    if (dt.Columns[9].ColumnName != "实际租赁人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为实际租赁人"));
                    }
                    if (dt.Columns[10].ColumnName != "业主名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为业主名称"));
                    }
                    if (dt.Columns[11].ColumnName != "联系人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为联系人"));
                    }
                    if (dt.Columns[12].ColumnName != "联系方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为联系方式"));
                    }
                    if (dt.Columns[13].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[14].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 2000)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为2000行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("基站名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "基站名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        Guid placeId = Guid.Empty;
                        string placeName = "";
                        Guid placeCategoryId = Guid.Empty;
                        Guid placeOwnerId = Guid.Empty;
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        Guid addressingDepartmentId = Guid.Empty;
                        string addressingRealName = "";
                        string ownerName = "";
                        string ownerContact = "";
                        string ownerPhoneNumber = "";
                        string detailedAddress = "";
                        string remarks = "";
                        IList<Place> places = new List<Place>(dt.Rows.Count);
                        IList<PlaceBusinessVolume> placeBusinessVolumes = new List<PlaceBusinessVolume>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规基站称验证
                            if (dr["基站名称"].ToString().Trim() != "")
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName));
                                if (place != null)
                                {
                                    placeId = place.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", placeName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "不能为空"));
                            }

                            //基站类型验证
                            if (dr["基站类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["基站类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.基站));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //产权验证
                            if (dr["产权"].ToString().Trim() != "")
                            {
                                string placeOwnerName = dr["产权"].ToString().Trim();
                                PlaceOwner placeOwner = placeOwnerRepository.Find(Specification<PlaceOwner>.Eval(entity => entity.PlaceOwnerName == placeOwnerName));
                                if (placeOwner != null)
                                {
                                    placeOwnerId = placeOwner.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", placeOwnerName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", "不能为空"));
                            }

                            //租赁部门验证
                            if (dr["租赁部门"].ToString().Trim() != "")
                            {
                                string addressingDepartmentName = dr["租赁部门"].ToString().Trim();
                                Department department = departmentRepository.Find(Specification<Department>.Eval(entity => entity.DepartmentName == addressingDepartmentName));
                                if (department != null)
                                {
                                    addressingDepartmentId = department.Id;
                                }
                            }

                            addressingRealName = dr["实际租赁人"].ToString().Trim();
                            ownerName = dr["业主名称"].ToString().Trim();
                            ownerContact = dr["联系人"].ToString().Trim();
                            ownerPhoneNumber = dr["联系方式"].ToString().Trim();

                            //详细地址验证
                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailedAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                Place placeMaint = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName));
                                placeMaintObjects.Add(BuildUpdatePlaceObject(placeMaint.Id, placeCategoryId, reseauId, lng, lat, importance, placeOwnerId, addressingDepartmentId, addressingRealName,
                                    ownerName, ownerContact, ownerPhoneNumber, detailedAddress, remarks, modifyUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < placeMaintObjects.Count; i++)
                            {
                                Place place = placeRepository.GetByKey(placeMaintObjects[i].Id);
                                place.UpdatePlace(placeMaintObjects[i].PlaceCategoryId, placeMaintObjects[i].ReseauId, placeMaintObjects[i].Lng, placeMaintObjects[i].Lat,
                                    (Importance)placeMaintObjects[i].Importance, placeMaintObjects[i].PlaceOwner, placeMaintObjects[i].AddressingDepartmentId,
                                    placeMaintObjects[i].AddressingRealName, placeMaintObjects[i].OwnerName, placeMaintObjects[i].OwnerContact,
                                    placeMaintObjects[i].OwnerPhoneNumber, placeMaintObjects[i].DetailedAddress, placeMaintObjects[i].Remarks,
                                    placeMaintObjects[i].ModifyUserId);
                                placeRepository.Update(place);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入逻辑号
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportLogicalNumber(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<PlaceMaintObject> placeMaintObjects = new List<PlaceMaintObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 5)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为5列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "站点名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为站点名称"));
                    }
                    if (dt.Columns[1].ColumnName != "2G逻辑号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为2G逻辑号"));
                    }
                    if (dt.Columns[2].ColumnName != "2D逻辑号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为2D逻辑号"));
                    }
                    if (dt.Columns[3].ColumnName != "3G逻辑号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为3G逻辑号"));
                    }
                    if (dt.Columns[4].ColumnName != "4G逻辑号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为4G逻辑号"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 2000)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为2000行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证站点名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("站点名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "站点名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        Guid placeId = Guid.Empty;
                        string placeName = "";
                        string g2Number = "";
                        string d2Number = "";
                        string g3Number = "";
                        string g4Number = "";
                        IList<Place> places = new List<Place>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规站点称验证
                            if (dr["站点名称"].ToString().Trim() != "")
                            {
                                placeName = dr["站点名称"].ToString().Trim();
                                if (!placeRepository.Exists(Specification<Place>.Eval(entity => entity.PlaceName == placeName)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "站点名称", placeName + "-在系统中不存在"));
                                }
                                else
                                {
                                    Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName));
                                    placeId = place.Id;
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "站点名称", "不能为空"));
                            }

                            g2Number = dr["2G逻辑号"].ToString().Trim();
                            d2Number = dr["2D逻辑号"].ToString().Trim();
                            g3Number = dr["3G逻辑号"].ToString().Trim();
                            g4Number = dr["4G逻辑号"].ToString().Trim();


                            if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G2Number == g2Number && entity.Id != placeId && entity.G2Number != "")))
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "2G逻辑号", "在系统中已存在"));
                            }
                            if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.D2Number == d2Number && entity.Id != placeId && entity.D2Number != "")))
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "2D逻辑号", "在系统中已存在"));
                            }
                            if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G3Number == g3Number && entity.Id != placeId && entity.G3Number != "")))
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "3G逻辑号", "在系统中已存在"));
                            }
                            if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G4Number == g4Number && entity.Id != placeId && entity.G4Number != "")))
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "4G逻辑号", "在系统中已存在"));
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                placeMaintObjects.Add(BuildLogicalNumberImportObject(placeId, g2Number, d2Number, g3Number, g4Number));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < placeMaintObjects.Count; i++)
                            {
                                Place place = placeRepository.GetByKey(placeMaintObjects[i].Id);
                                if (placeMaintObjects[i].G2Number != "")
                                {
                                    place.ModifyG2Number(placeMaintObjects[i].G2Number);
                                }
                                if (placeMaintObjects[i].D2Number != "")
                                {
                                    place.ModifyD2Number(placeMaintObjects[i].D2Number);
                                }
                                if (placeMaintObjects[i].G3Number != "")
                                {
                                    place.ModifyG3Number(placeMaintObjects[i].G3Number);
                                }
                                if (placeMaintObjects[i].G4Number != "")
                                {
                                    place.ModifyG4Number(placeMaintObjects[i].G4Number);
                                }
                                placeRepository.Update(place);
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
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入业务量
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="profession">专业</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportBusinessVolume(Guid excelFileId, int logicalType, int profession, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 4)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为4列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "逻辑号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为逻辑号"));
                    }
                    if (dt.Columns[1].ColumnName != "话务量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为话务量"));
                    }
                    if (dt.Columns[2].ColumnName != "业务量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为业务量"));
                    }
                    if (dt.Columns[3].ColumnName != "导入日期")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为业务量"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 10000)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为10000行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证逻辑号是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("逻辑号") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "逻辑号", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //逻辑号有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string logicalNumber = "";
                        decimal trafficVolumes = 0;
                        decimal businessVolumes = 0;
                        User user = userRepository.GetByKey(createUserId);
                        Department department = departmentRepository.GetByKey(user.DepartmentId);
                        Guid companyId = department.CompanyId;

                        IList<BusinessVolume> businessVolumeList = new List<BusinessVolume>(dt.Rows.Count);

                        //DateTime datetimeNow = DateTime.Parse(DateTime.Now.ToShortDateString());
                        DateTime datetimeImport = DateTime.Parse(DateTime.Now.ToShortDateString());

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            if (dr["导入日期"].ToString().Trim() != "")
                            {
                                try
                                {
                                    datetimeImport = DateTime.Parse(DateTime.Parse(dr["导入日期"].ToString().Trim()).ToShortDateString());
                                }
                                catch
                                {
                                    throw new ApplicationFault("导入日期格式不正确");
                                }
                                if (datetimeImport > DateTime.Now)
                                {
                                    throw new ApplicationFault("只能导入当天或之前的业务量");
                                }

                                //逻辑号验证
                                if (dr["逻辑号"].ToString().Trim() != "")
                                {
                                    logicalNumber = dr["逻辑号"].ToString().Trim();
                                    if (logicalType == 1)
                                    {
                                        if (!placeRepository.Exists(Specification<Place>.Eval(entity => entity.G2Number == logicalNumber)))
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-在系统中不存在"));
                                        }
                                        else
                                        {
                                            if (businessVolumeRepository.Exists(Specification<BusinessVolume>.Eval(entity => entity.LogicalType == (LogicalType)logicalType && entity.LogicalNumber == logicalNumber && entity.CreateDate == datetimeImport)))
                                            {
                                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-该日期已导入"));
                                            }
                                        }
                                    }
                                    else if (logicalType == 2)
                                    {
                                        if (!placeRepository.Exists(Specification<Place>.Eval(entity => entity.D2Number == logicalNumber)))
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-在系统中不存在"));
                                        }
                                        else
                                        {
                                            if (businessVolumeRepository.Exists(Specification<BusinessVolume>.Eval(entity => entity.LogicalType == (LogicalType)logicalType && entity.LogicalNumber == logicalNumber && entity.CreateDate == datetimeImport)))
                                            {
                                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-该日期已导入"));
                                            }
                                        }
                                    }
                                    else if (logicalType == 3)
                                    {
                                        if (!placeRepository.Exists(Specification<Place>.Eval(entity => entity.G3Number == logicalNumber)))
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-在系统中不存在"));
                                        }
                                        else
                                        {
                                            if (businessVolumeRepository.Exists(Specification<BusinessVolume>.Eval(entity => entity.LogicalType == (LogicalType)logicalType && entity.LogicalNumber == logicalNumber && entity.CreateDate == datetimeImport)))
                                            {
                                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-该日期已导入"));
                                            }
                                        }
                                    }
                                    else if (logicalType == 4)
                                    {
                                        if (!placeRepository.Exists(Specification<Place>.Eval(entity => entity.G4Number == logicalNumber)))
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-在系统中不存在"));
                                        }
                                        else
                                        {
                                            if (businessVolumeRepository.Exists(Specification<BusinessVolume>.Eval(entity => entity.LogicalType == (LogicalType)logicalType && entity.LogicalNumber == logicalNumber && entity.CreateDate == datetimeImport)))
                                            {
                                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", logicalNumber + "-该日期已导入"));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "逻辑号", "不能为空"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "导入日期", "不能为空"));
                            }

                            try
                            {
                                trafficVolumes = decimal.Parse(dr["话务量"].ToString().Trim());
                            }
                            catch
                            {
                                trafficVolumes = 0;
                            }

                            try
                            {
                                businessVolumes = decimal.Parse(dr["业务量"].ToString().Trim());
                            }
                            catch
                            {
                                businessVolumes = 0;
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                businessVolumeList.Add(AggregateFactory.CreateBusinessVolume((LogicalType)logicalType, logicalNumber, trafficVolumes, businessVolumes, (Profession)profession, companyId, datetimeImport, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < businessVolumeList.Count; i++)
                            {
                                businessVolumeRepository.Add(businessVolumeList[i]);

                                string number = businessVolumeList[i].LogicalNumber;
                                DateTime createDate = businessVolumeList[i].CreateDate;
                                if (businessVolumeList[i].LogicalType == LogicalType.G2)
                                {
                                    Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.G2Number == number));
                                    if (place != null)
                                    {
                                        PlaceBusinessVolume placeBusinessVolume = placeBusinessVolumeRepository.Find(Specification<PlaceBusinessVolume>.Eval(entity => entity.PlaceId == place.Id && entity.CreateDate == createDate));
                                        if (placeBusinessVolume != null)
                                        {
                                            placeBusinessVolume.Modify(LogicalType.G2, businessVolumeList[i].Id);
                                            placeBusinessVolumeRepository.Update(placeBusinessVolume);
                                        }
                                    }
                                }
                                if (businessVolumeList[i].LogicalType == LogicalType.D2)
                                {
                                    Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.D2Number == number));
                                    if (place != null)
                                    {
                                        PlaceBusinessVolume placeBusinessVolume = placeBusinessVolumeRepository.Find(Specification<PlaceBusinessVolume>.Eval(entity => entity.PlaceId == place.Id && entity.CreateDate == createDate));
                                        if (placeBusinessVolume != null)
                                        {
                                            placeBusinessVolume.Modify(LogicalType.D2, businessVolumeList[i].Id);
                                            placeBusinessVolumeRepository.Update(placeBusinessVolume);
                                        }
                                    }
                                }
                                if (businessVolumeList[i].LogicalType == LogicalType.G3)
                                {
                                    Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.G3Number == number));
                                    if (place != null)
                                    {
                                        PlaceBusinessVolume placeBusinessVolume = placeBusinessVolumeRepository.Find(Specification<PlaceBusinessVolume>.Eval(entity => entity.PlaceId == place.Id && entity.CreateDate == createDate));
                                        if (placeBusinessVolume != null)
                                        {
                                            placeBusinessVolume.Modify(LogicalType.G3, businessVolumeList[i].Id);
                                            placeBusinessVolumeRepository.Update(placeBusinessVolume);
                                        }
                                    }
                                }
                                if (businessVolumeList[i].LogicalType == LogicalType.G4)
                                {
                                    Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.G4Number == number));
                                    if (place != null)
                                    {
                                        PlaceBusinessVolume placeBusinessVolume = placeBusinessVolumeRepository.Find(Specification<PlaceBusinessVolume>.Eval(entity => entity.PlaceId == place.Id && entity.CreateDate == createDate));
                                        if (placeBusinessVolume != null)
                                        {
                                            placeBusinessVolume.Modify(LogicalType.G4, businessVolumeList[i].Id);
                                            placeBusinessVolumeRepository.Update(placeBusinessVolume);
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
                                if (ex.Message.Contains("IX_UQ_BusinessVolumeLogicalTypeLogicalNumberCreateDate"))
                                {
                                    throw new ApplicationFault("逻辑号重复");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入新增基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportNewPlanningBS(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<NewPlanningImportObject> newPlanningImportObjects = new List<NewPlanningImportObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 27)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为27列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "规划名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为规划名称"));
                    }
                    if (dt.Columns[1].ColumnName != "基站类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为基站类型"));
                    }
                    if (dt.Columns[2].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为区域"));
                    }
                    if (dt.Columns[3].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为网格"));
                    }
                    if (dt.Columns[4].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为经度"));
                    }
                    if (dt.Columns[5].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为纬度"));
                    }
                    if (dt.Columns[6].ColumnName != "周边场景")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为周边场景"));
                    }
                    if (dt.Columns[7].ColumnName != "业主名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为业主名称"));
                    }
                    if (dt.Columns[8].ColumnName != "联系人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为联系人"));
                    }
                    if (dt.Columns[9].ColumnName != "联系方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为联系方式"));
                    }
                    if (dt.Columns[10].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[11].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为备注"));
                    }
                    if (dt.Columns[12].ColumnName != "移动共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为移动共享"));
                    }
                    if (dt.Columns[13].ColumnName != "移动天线挂高(米)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为移动天线挂高(米)"));
                    }
                    if (dt.Columns[14].ColumnName != "移动抱杆数量(根)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为移动抱杆数量(根)"));
                    }
                    if (dt.Columns[15].ColumnName != "移动机柜数量(个)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十六列", "列名必须为移动机柜数量(个)"));
                    }
                    if (dt.Columns[16].ColumnName != "移动确认人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十七列", "列名必须为移动确认人"));
                    }
                    if (dt.Columns[17].ColumnName != "电信共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十八列", "列名必须为电信共享"));
                    }
                    if (dt.Columns[18].ColumnName != "电信天线挂高(米)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十九列", "列名必须为电信天线挂高(米)"));
                    }
                    if (dt.Columns[19].ColumnName != "电信抱杆数量(根)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十列", "列名必须为电信抱杆数量(根)"));
                    }
                    if (dt.Columns[20].ColumnName != "电信机柜数量(个)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十一列", "列名必须为电信机柜数量(个)"));
                    }
                    if (dt.Columns[21].ColumnName != "电信确认人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十二列", "列名必须为电信确认人"));
                    }
                    if (dt.Columns[22].ColumnName != "联通共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十三列", "列名必须为联通共享"));
                    }
                    if (dt.Columns[23].ColumnName != "联通天线挂高(米)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十四列", "列名必须为联通天线挂高(米)"));
                    }
                    if (dt.Columns[24].ColumnName != "联通抱杆数量(根)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十五列", "列名必须为联通抱杆数量(根)"));
                    }
                    if (dt.Columns[25].ColumnName != "联通机柜数量(个)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十六列", "列名必须为联通机柜数量(个)"));
                    }
                    if (dt.Columns[26].ColumnName != "联通确认人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十七列", "列名必须为联通确认人"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("规划名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "规划名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string planningName = "";
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        Guid placeCategoryId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Urgency urgency = Urgency.一级;
                        Guid sceneId = Guid.Empty;
                        string ownerName = "";
                        string ownerContact = "";
                        string ownerPhoneNumber = "";
                        string detailAddress = "";
                        string remarks = "";
                        Bool mobileShare = Bool.否;
                        decimal mobileAntennaHeight = 0;
                        int mobilePoleNumber = 0;
                        int mobileCabinetNumber = 0;
                        Guid mobileUserId = Guid.Empty;
                        Bool telecomShare = Bool.否;
                        decimal telecomAntennaHeight = 0;
                        int telecomPoleNumber = 0;
                        int telecomCabinetNumber = 0;
                        Guid telecomUserId = Guid.Empty;
                        Bool unicomShare = Bool.否;
                        decimal unicomAntennaHeight = 0;
                        int unicomPoleNumber = 0;
                        int unicomCabinetNumber = 0;
                        Guid unicomUserId = Guid.Empty;
                        IList<Planning> plannings = new List<Planning>(dt.Rows.Count);
                        IList<OperatorsPlanning> operatorsPlannings = new List<OperatorsPlanning>(dt.Rows.Count);
                        IList<OperatorsConfirm> operatorsConfirms = new List<OperatorsConfirm>(dt.Rows.Count);
                        IList<OperatorsConfirmDetail> operatorsConfirmDetails = new List<OperatorsConfirmDetail>(dt.Rows.Count);
                        IList<Addressing> addressings = new List<Addressing>(dt.Rows.Count);
                        IList<PlaceDesign> placeDesigns = new List<PlaceDesign>(dt.Rows.Count);
                        IList<PlaceProperty> placePropertys = new List<PlaceProperty>(dt.Rows.Count);
                        IList<PlacePropertyLog> placePropertyLogs = new List<PlacePropertyLog>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规划名称验证
                            if (dr["规划名称"].ToString().Trim() != "")
                            {
                                planningName = dr["规划名称"].ToString().Trim();
                                if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.PlanningName == planningName)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", planningName + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", "不能为空"));
                            }

                            //基站类型验证
                            if (dr["基站类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["基站类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.基站));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //周边场景验证
                            if (dr["周边场景"].ToString().Trim() != "")
                            {
                                string sceneName = dr["周边场景"].ToString().Trim();
                                Scene scene = sceneRepository.Find(Specification<Scene>.Eval(entity => entity.SceneName == sceneName));
                                if (scene != null)
                                {
                                    sceneId = scene.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "周边场景", sceneName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "周边场景", "不能为空"));
                            }

                            ownerName = dr["业主名称"].ToString().Trim();
                            ownerContact = dr["联系人"].ToString().Trim();
                            ownerPhoneNumber = dr["联系方式"].ToString().Trim();

                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }
                            remarks = dr["备注"].ToString().Trim();

                            //移动共享验证
                            if (dr["移动共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["移动共享"].ToString().Trim(), out mobileShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", dr["移动共享"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (mobileShare != Bool.是 && mobileShare != Bool.否)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", dr["移动共享"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                mobileShare = Bool.否;
                            }

                            if (mobileShare == Bool.是)
                            {
                                //移动天线挂高(米)验证
                                if (dr["移动天线挂高(米)"].ToString().Trim() != "")
                                {
                                    if (!decimal.TryParse(dr["移动天线挂高(米)"].ToString().Trim(), out mobileAntennaHeight))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动天线挂高(米)", "必须为数字"));
                                    }
                                }
                                else
                                {
                                    mobileAntennaHeight = 0;
                                }

                                //移动抱杆数量(根)验证
                                if (dr["移动抱杆数量(根)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["移动抱杆数量(根)"].ToString().Trim(), out mobilePoleNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动抱杆数量(根)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    mobilePoleNumber = 0;
                                }

                                //移动机柜数量(个)验证
                                if (dr["移动机柜数量(个)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["移动机柜数量(个)"].ToString().Trim(), out mobileCabinetNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动机柜数量(个)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    mobileCabinetNumber = 0;
                                }

                                //移动确认人验证
                                if (dr["移动确认人"].ToString().Trim() != "")
                                {
                                    string mobileUserName = dr["移动确认人"].ToString().Trim();
                                    User mobileUser = userRepository.Find(Specification<User>.Eval(entity => entity.UserName == mobileUserName));
                                    if (mobileUser != null)
                                    {
                                        mobileUserId = mobileUser.Id;
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动确认人", mobileUserName + "-在系统中不存在"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动确认人", "不能为空"));
                                }
                            }

                            //电信共享验证
                            if (dr["电信共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["电信共享"].ToString().Trim(), out telecomShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", dr["电信共享"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (telecomShare != Bool.是 && telecomShare != Bool.否)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", dr["电信共享"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                telecomShare = Bool.否;
                            }

                            if (telecomShare == Bool.是)
                            {
                                //电信天线挂高(米)验证
                                if (dr["电信天线挂高(米)"].ToString().Trim() != "")
                                {
                                    if (!decimal.TryParse(dr["电信天线挂高(米)"].ToString().Trim(), out telecomAntennaHeight))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信天线挂高(米)", "必须为数字"));
                                    }
                                }
                                else
                                {
                                    telecomAntennaHeight = 0;
                                }

                                //电信抱杆数量(根)验证
                                if (dr["电信抱杆数量(根)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["电信抱杆数量(根)"].ToString().Trim(), out telecomPoleNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信抱杆数量(根)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    telecomPoleNumber = 0;
                                }

                                //电信机柜数量(个)验证
                                if (dr["电信机柜数量(个)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["电信机柜数量(个)"].ToString().Trim(), out telecomCabinetNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信机柜数量(个)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    telecomCabinetNumber = 0;
                                }

                                //电信确认人验证
                                if (dr["电信确认人"].ToString().Trim() != "")
                                {
                                    string telecomUserName = dr["电信确认人"].ToString().Trim();
                                    User telecomUser = userRepository.Find(Specification<User>.Eval(entity => entity.UserName == telecomUserName));
                                    if (telecomUser != null)
                                    {
                                        telecomUserId = telecomUser.Id;
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信确认人", telecomUserName + "-在系统中不存在"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信确认人", "不能为空"));
                                }
                            }

                            //联通共享验证
                            if (dr["联通共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["联通共享"].ToString().Trim(), out unicomShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", dr["联通共享"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (unicomShare != Bool.是 && unicomShare != Bool.否)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", dr["联通共享"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                unicomShare = Bool.否;
                            }

                            if (unicomShare == Bool.是)
                            {
                                //联通天线挂高(米)验证
                                if (dr["联通天线挂高(米)"].ToString().Trim() != "")
                                {
                                    if (!decimal.TryParse(dr["联通天线挂高(米)"].ToString().Trim(), out unicomAntennaHeight))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通天线挂高(米)", "必须为数字"));
                                    }
                                }
                                else
                                {
                                    unicomAntennaHeight = 0;
                                }

                                //联通抱杆数量(根)验证
                                if (dr["联通抱杆数量(根)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["联通抱杆数量(根)"].ToString().Trim(), out unicomPoleNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通抱杆数量(根)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    unicomPoleNumber = 0;
                                }

                                //联通机柜数量(个)验证
                                if (dr["联通机柜数量(个)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["联通机柜数量(个)"].ToString().Trim(), out unicomCabinetNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通机柜数量(个)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    unicomCabinetNumber = 0;
                                }

                                //联通确认人验证
                                if (dr["联通确认人"].ToString().Trim() != "")
                                {
                                    string unicomUserName = dr["联通确认人"].ToString().Trim();
                                    User unicomUser = userRepository.Find(Specification<User>.Eval(entity => entity.UserName == unicomUserName));
                                    if (unicomUser != null)
                                    {
                                        unicomUserId = unicomUser.Id;
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通确认人", unicomUserName + "-在系统中不存在"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通确认人", "不能为空"));
                                }
                            }

                            Demand mobileDemand = mobileShare == Bool.是 ? Demand.需要 : Demand.不需要;
                            Demand telecomDemand = telecomShare == Bool.是 ? Demand.需要 : Demand.不需要;
                            Demand unicomDemand = unicomShare == Bool.是 ? Demand.需要 : Demand.不需要;

                            if (importErrorObjects.Count == 0)
                            {
                                newPlanningImportObjects.Add(this.BuildNewPlanningImportObject((int)mobileDemand, (int)telecomDemand, (int)unicomDemand, planningName, (int)Profession.基站,
                                   placeCategoryId, areaId, reseauId, lng, lat, sceneId, ownerName, ownerContact, ownerPhoneNumber, detailAddress, remarks, mobileAntennaHeight, mobilePoleNumber,
                                   mobileCabinetNumber, mobileUserId, telecomAntennaHeight, telecomPoleNumber, telecomCabinetNumber, telecomUserId, unicomAntennaHeight, unicomPoleNumber,
                                   unicomCabinetNumber, unicomUserId));
                                plannings.Add(AggregateFactory.CreatePlanning(codeSeedRepository.GenerateCode("Planning"), planningName, Profession.基站, placeCategoryId, reseauId, lng, lat, "", "", "", "", Importance.C, Guid.Empty, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < plannings.Count; i++)
                            {
                                planningRepository.Add(plannings[i]);

                                //if (plannings[i].MobileDemand == Demand.需要)
                                //{
                                //    operatorsPlanningRepository.Add(AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), plannings[i].PlanningName,
                                //    Profession.基站, plannings[i].PlaceCategoryId, newPlanningImportObjects[i].AreaId, plannings[i].Lng, plannings[i].Lat, newPlanningImportObjects[i].MobileAntennaHeight,
                                //    newPlanningImportObjects[i].MobilePoleNumber, newPlanningImportObjects[i].MobileCabinetNumber, Urgency.一级, Bool.是, "", Guid.Parse("6365f3de-0fc5-4930-a321-2350ee6269bb"),
                                //    plannings[i].Id, newPlanningImportObjects[i].MobileUserId, CompanyNature.运营商));
                                //}
                                //if (plannings[i].TelecomDemand == Demand.需要)
                                //{
                                //    operatorsPlanningRepository.Add(AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), plannings[i].PlanningName,
                                //    Profession.基站, plannings[i].PlaceCategoryId, newPlanningImportObjects[i].AreaId, plannings[i].Lng, plannings[i].Lat, newPlanningImportObjects[i].TelecomAntennaHeight,
                                //    newPlanningImportObjects[i].TelecomPoleNumber, newPlanningImportObjects[i].TelecomCabinetNumber, Urgency.一级, Bool.是, "", Guid.Parse("2e0ffe5f-c03a-4767-9915-9683f0db0b53"),
                                //    plannings[i].Id, newPlanningImportObjects[i].TelecomUserId, CompanyNature.运营商));
                                //}
                                //if (plannings[i].UnicomDemand == Demand.需要)
                                //{
                                //    operatorsPlanningRepository.Add(AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), plannings[i].PlanningName,
                                //    Profession.基站, plannings[i].PlaceCategoryId, newPlanningImportObjects[i].AreaId, plannings[i].Lng, plannings[i].Lat, newPlanningImportObjects[i].UnicomAntennaHeight,
                                //    newPlanningImportObjects[i].UnicomPoleNumber, newPlanningImportObjects[i].UnicomCabinetNumber, Urgency.一级, Bool.是, "", Guid.Parse("0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"),
                                //    plannings[i].Id, newPlanningImportObjects[i].UnicomUserId, CompanyNature.运营商));
                                //}

                                OperatorsConfirm operatorsConfirm = AggregateFactory.CreateOperatorsConfirm(plannings[i].CreateUserId);
                                operatorsConfirmRepository.Add(operatorsConfirm);
                                //OperatorsConfirmDetail operatorsConfirmDetail = AggregateFactory.CreateOperatorsConfirmDetail(operatorsConfirm.Id, plannings[i].Id, plannings[i].MobileDemand, plannings[i].TelecomDemand,
                                //    plannings[i].UnicomDemand, newPlanningImportObjects[i].MobileUserId, newPlanningImportObjects[i].TelecomUserId, newPlanningImportObjects[i].UnicomUserId, plannings[i].CreateUserId);
                                //operatorsConfirmDetailRepository.Add(operatorsConfirmDetail);

                                Bool mShare = (Demand)newPlanningImportObjects[i].MobileDemand == Demand.需要 ? Bool.是 : Bool.否;
                                Bool tShare = (Demand)newPlanningImportObjects[i].TelecomDemand == Demand.需要 ? Bool.是 : Bool.否;
                                Bool uShare = (Demand)newPlanningImportObjects[i].UnicomDemand == Demand.需要 ? Bool.是 : Bool.否;

                                Reseau reseau = reseauRepository.FindByKey(plannings[i].ReseauId);

                                Addressing addressing = AggregateFactory.CreateAddressing(plannings[i].Id, plannings[i].PlanningName, Guid.Empty, "",
                                    newPlanningImportObjects[i].OwnerName, newPlanningImportObjects[i].OwnerContact, newPlanningImportObjects[i].OwnerPhoneNumber, "", plannings[i].CreateUserId);
                                addressingRepository.Add(addressing);
                                PlaceDesign placeDesign = AggregateFactory.CreatePlaceDesign(addressing.Id, PropertyType.寻址设计, plannings[i].CreateUserId);
                                placeDesignRepository.Add(placeDesign);
                                PlaceProperty placeProperty = AggregateFactory.CreatePlaceProperty(addressing.Id, PropertyType.寻址设计, mShare, newPlanningImportObjects[i].MobilePoleNumber, newPlanningImportObjects[i].MobileCabinetNumber, 0, Guid.Empty, tShare, newPlanningImportObjects[i].TelecomPoleNumber, newPlanningImportObjects[i].TelecomCabinetNumber, 0, Guid.Empty, uShare, newPlanningImportObjects[i].UnicomPoleNumber, newPlanningImportObjects[i].UnicomCabinetNumber, 0, Guid.Empty);
                                placePropertyRepository.Add(placeProperty);

                                PlacePropertyLog mobilePlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, addressing.Id, PropertyType.寻址设计, CompanyNameId.移动, mShare, newPlanningImportObjects[i].MobilePoleNumber, newPlanningImportObjects[i].MobileCabinetNumber, 0, Guid.Empty, tShare, newPlanningImportObjects[i].TelecomPoleNumber, newPlanningImportObjects[i].TelecomCabinetNumber, 0, Guid.Empty, uShare, newPlanningImportObjects[i].UnicomPoleNumber, newPlanningImportObjects[i].UnicomCabinetNumber, 0, Guid.Empty);
                                placePropertyLogRepository.Add(mobilePlacePropertyLog);

                                PlacePropertyLog telecomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, addressing.Id, PropertyType.寻址设计, CompanyNameId.电信, mShare, newPlanningImportObjects[i].MobilePoleNumber, newPlanningImportObjects[i].MobileCabinetNumber, 0, Guid.Empty, tShare, newPlanningImportObjects[i].TelecomPoleNumber, newPlanningImportObjects[i].TelecomCabinetNumber, 0, Guid.Empty, uShare, newPlanningImportObjects[i].UnicomPoleNumber, newPlanningImportObjects[i].UnicomCabinetNumber, 0, Guid.Empty);
                                placePropertyLogRepository.Add(telecomPlacePropertyLog);

                                PlacePropertyLog unicomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, addressing.Id, PropertyType.寻址设计, CompanyNameId.联通, mShare, newPlanningImportObjects[i].MobilePoleNumber, newPlanningImportObjects[i].MobileCabinetNumber, 0, Guid.Empty, tShare, newPlanningImportObjects[i].TelecomPoleNumber, newPlanningImportObjects[i].TelecomCabinetNumber, 0, Guid.Empty, uShare, newPlanningImportObjects[i].UnicomPoleNumber, newPlanningImportObjects[i].UnicomCabinetNumber, 0, Guid.Empty);
                                placePropertyLogRepository.Add(unicomPlacePropertyLog);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_PlanningCode"))
                                {
                                    throw new ApplicationFault("站点编码重复");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入改造基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportNewRemodelingBS(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<NewRemodelingImportObject> newRemodelingImportObjects = new List<NewRemodelingImportObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 17)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为17列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "基站名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为基站名称"));
                    }
                    if (dt.Columns[1].ColumnName != "移动共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为移动共享"));
                    }
                    if (dt.Columns[2].ColumnName != "移动用电量(KW)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为移动用电量(KW)"));
                    }
                    if (dt.Columns[3].ColumnName != "移动抱杆数量(根)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为移动抱杆数量(根)"));
                    }
                    if (dt.Columns[4].ColumnName != "移动机柜数量(个)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为移动机柜数量(个)"));
                    }
                    if (dt.Columns[5].ColumnName != "移动确认人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为移动确认人"));
                    }
                    if (dt.Columns[6].ColumnName != "电信共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为电信共享"));
                    }
                    if (dt.Columns[7].ColumnName != "电信用电量(KW)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为电信用电量(KW)"));
                    }
                    if (dt.Columns[8].ColumnName != "电信抱杆数量(根)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为电信抱杆数量(根)"));
                    }
                    if (dt.Columns[9].ColumnName != "电信机柜数量(个)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为电信机柜数量(个)"));
                    }
                    if (dt.Columns[10].ColumnName != "电信确认人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为电信确认人"));
                    }
                    if (dt.Columns[11].ColumnName != "联通共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为联通共享"));
                    }
                    if (dt.Columns[12].ColumnName != "联通用电量(KW)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为联通用电量(KW)"));
                    }
                    if (dt.Columns[13].ColumnName != "联通抱杆数量(根)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为联通抱杆数量(根)"));
                    }
                    if (dt.Columns[14].ColumnName != "联通机柜数量(个)")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为联通机柜数量(个)"));
                    }
                    if (dt.Columns[15].ColumnName != "联通确认人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十六列", "列名必须为联通确认人"));
                    }
                    if (dt.Columns[16].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十七列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    //var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("基站名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    //foreach (var duplicateData in duplicateDatas)
                    //{
                    //    if (duplicateData[0].ToString().Trim() != "")
                    //    {
                    //        importErrorObjects.Add(this.BuildImportError("模板", "基站名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                    //    }
                    //}

                    //基站名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string placeCode = "";
                        string placeName = "";
                        Guid placeId = Guid.Empty;
                        Bool mobileShare = Bool.否;
                        decimal mobilePowerUsed = 0;
                        int mobilePoleNumber = 0;
                        int mobileCabinetNumber = 0;
                        Guid mobileUserId = Guid.Empty;
                        Bool telecomShare = Bool.否;
                        decimal telecomPowerUsed = 0;
                        int telecomPoleNumber = 0;
                        int telecomCabinetNumber = 0;
                        Guid telecomUserId = Guid.Empty;
                        Bool unicomShare = Bool.否;
                        decimal unicomPowerUsed = 0;
                        int unicomPoleNumber = 0;
                        int unicomCabinetNumber = 0;
                        Guid unicomUserId = Guid.Empty;
                        Guid? projectId = null;
                        Guid? projectManagerId = null;
                        string remarks = "";
                        IList<Remodeling> remodelings = new List<Remodeling>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //基站名称验证
                            if (dr["基站名称"].ToString().Trim() != "")
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName && entity.Profession == Profession.基站));
                                if (place != null)
                                {
                                    placeId = place.Id;
                                    placeCode = place.PlaceCode;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", placeName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "不能为空"));
                            }

                            //移动共享验证
                            if (dr["移动共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["移动共享"].ToString().Trim(), out mobileShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", dr["移动共享"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (mobileShare != Bool.是 && mobileShare != Bool.否)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", dr["移动共享"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                mobileShare = Bool.否;
                            }

                            if (mobileShare == Bool.是)
                            {
                                //移动用电量(KW)验证
                                if (dr["移动用电量(KW)"].ToString().Trim() != "")
                                {
                                    if (!decimal.TryParse(dr["移动用电量(KW)"].ToString().Trim(), out mobilePowerUsed))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动用电量(KW)", "必须为数字"));
                                    }
                                }
                                else
                                {
                                    mobilePowerUsed = 0;
                                }

                                //移动抱杆数量(根)验证
                                if (dr["移动抱杆数量(根)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["移动抱杆数量(根)"].ToString().Trim(), out mobilePoleNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动抱杆数量(根)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    mobilePoleNumber = 0;
                                }

                                //移动机柜数量(个)验证
                                if (dr["移动机柜数量(个)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["移动机柜数量(个)"].ToString().Trim(), out mobileCabinetNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动机柜数量(个)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    mobileCabinetNumber = 0;
                                }

                                //移动确认人验证
                                if (dr["移动确认人"].ToString().Trim() != "")
                                {
                                    string mobileUserName = dr["移动确认人"].ToString().Trim();
                                    User mobileUser = userRepository.Find(Specification<User>.Eval(entity => entity.UserName == mobileUserName));
                                    if (mobileUser != null)
                                    {
                                        mobileUserId = mobileUser.Id;
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动确认人", mobileUserName + "-在系统中不存在"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动确认人", "不能为空"));
                                }
                            }

                            //电信共享验证
                            if (dr["电信共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["电信共享"].ToString().Trim(), out telecomShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", dr["电信共享"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (telecomShare != Bool.是 && telecomShare != Bool.否)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", dr["电信共享"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                telecomShare = Bool.否;
                            }

                            if (telecomShare == Bool.是)
                            {
                                //电信用电量(KW)验证
                                if (dr["电信用电量(KW)"].ToString().Trim() != "")
                                {
                                    if (!decimal.TryParse(dr["电信用电量(KW)"].ToString().Trim(), out telecomPowerUsed))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信用电量(KW)", "必须为数字"));
                                    }
                                }
                                else
                                {
                                    telecomPowerUsed = 0;
                                }

                                //电信抱杆数量(根)验证
                                if (dr["电信抱杆数量(根)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["电信抱杆数量(根)"].ToString().Trim(), out telecomPoleNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信抱杆数量(根)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    telecomPoleNumber = 0;
                                }

                                //电信机柜数量(个)验证
                                if (dr["电信机柜数量(个)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["电信机柜数量(个)"].ToString().Trim(), out telecomCabinetNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信机柜数量(个)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    telecomCabinetNumber = 0;
                                }

                                //电信确认人验证
                                if (dr["电信确认人"].ToString().Trim() != "")
                                {
                                    string telecomUserName = dr["电信确认人"].ToString().Trim();
                                    User telecomUser = userRepository.Find(Specification<User>.Eval(entity => entity.UserName == telecomUserName));
                                    if (telecomUser != null)
                                    {
                                        telecomUserId = telecomUser.Id;
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信确认人", telecomUserName + "-在系统中不存在"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信确认人", "不能为空"));
                                }
                            }

                            //联通共享验证
                            if (dr["联通共享"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Bool>(dr["联通共享"].ToString().Trim(), out unicomShare))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", dr["联通共享"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (unicomShare != Bool.是 && unicomShare != Bool.否)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", dr["联通共享"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                unicomShare = Bool.否;
                            }

                            if (unicomShare == Bool.是)
                            {
                                //联通用电量(KW)验证
                                if (dr["联通用电量(KW)"].ToString().Trim() != "")
                                {
                                    if (!decimal.TryParse(dr["联通用电量(KW)"].ToString().Trim(), out unicomPowerUsed))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通用电量(KW)", "必须为数字"));
                                    }
                                }
                                else
                                {
                                    unicomPowerUsed = 0;
                                }

                                //联通抱杆数量(根)验证
                                if (dr["联通抱杆数量(根)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["联通抱杆数量(根)"].ToString().Trim(), out unicomPoleNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通抱杆数量(根)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    unicomPoleNumber = 0;
                                }

                                //联通机柜数量(个)验证
                                if (dr["联通机柜数量(个)"].ToString().Trim() != "")
                                {
                                    if (!int.TryParse(dr["联通机柜数量(个)"].ToString().Trim(), out unicomCabinetNumber))
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通机柜数量(个)", "必须为整数"));
                                    }
                                }
                                else
                                {
                                    unicomCabinetNumber = 0;
                                }

                                //联通确认人验证
                                if (dr["联通确认人"].ToString().Trim() != "")
                                {
                                    string unicomUserName = dr["联通确认人"].ToString().Trim();
                                    User unicomUser = userRepository.Find(Specification<User>.Eval(entity => entity.UserName == unicomUserName));
                                    if (unicomUser != null)
                                    {
                                        unicomUserId = unicomUser.Id;
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通确认人", unicomUserName + "-在系统中不存在"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通确认人", "不能为空"));
                                }
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                newRemodelingImportObjects.Add(this.BuildNewRemodelingImportObject((int)mobileShare, (int)telecomShare, (int)unicomShare, placeName, (int)Profession.基站,
                                mobilePowerUsed, mobilePoleNumber, mobileCabinetNumber, mobileUserId, telecomPowerUsed, telecomPoleNumber, telecomCabinetNumber, telecomUserId, unicomPowerUsed,
                                unicomPoleNumber, unicomCabinetNumber, unicomUserId));
                                Place place = placeRepository.FindByKey(placeId);
                                Reseau reseau = reseauRepository.FindByKey(place.ReseauId);
                                projectManagerId = reseau.ReseauManagerId;
                                remodelings.Add(AggregateFactory.CreateRemodeling(Profession.基站, placeCode, placeId, "", remarks, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < remodelings.Count; i++)
                            {
                                remodelingRepository.Add(remodelings[i]);

                                PlaceDesign placeDesign = AggregateFactory.CreatePlaceDesign(remodelings[i].Id, PropertyType.改造设计, remodelings[i].CreateUserId);
                                placeDesignRepository.Add(placeDesign);

                                if ((Bool)newRemodelingImportObjects[i].MobileDemand == Bool.是)
                                {
                                    OperatorsSharing osMobile = AggregateFactory.CreateOperatorsSharing((Profession)newRemodelingImportObjects[i].Profession, remodelings[i].PlaceCode, remodelings[i].PlaceId, newRemodelingImportObjects[i].MobilePowerUsed,
                                    newRemodelingImportObjects[i].MobilePoleNumber, newRemodelingImportObjects[i].MobileCabinetNumber, Urgency.一级, Bool.是, "", Guid.Parse("6365f3de-0fc5-4930-a321-2350ee6269bb"),
                                    remodelings[i].Id, Guid.Empty, newRemodelingImportObjects[i].MobileUserId, CompanyNature.运营商);
                                    operatorsSharingRepository.Add(osMobile);
                                }
                                if ((Bool)newRemodelingImportObjects[i].TelecomDemand == Bool.是)
                                {
                                    OperatorsSharing osTelecom = AggregateFactory.CreateOperatorsSharing((Profession)newRemodelingImportObjects[i].Profession, remodelings[i].PlaceCode, remodelings[i].PlaceId, newRemodelingImportObjects[i].TelecomPowerUsed,
                                    newRemodelingImportObjects[i].TelecomPoleNumber, newRemodelingImportObjects[i].TelecomCabinetNumber, Urgency.一级, Bool.是, "", Guid.Parse("2e0ffe5f-c03a-4767-9915-9683f0db0b53"),
                                    remodelings[i].Id, Guid.Empty, newRemodelingImportObjects[i].TelecomUserId, CompanyNature.运营商);
                                    operatorsSharingRepository.Add(osTelecom);
                                }
                                if ((Bool)newRemodelingImportObjects[i].UnicomDemand == Bool.是)
                                {
                                    OperatorsSharing osUnicom = AggregateFactory.CreateOperatorsSharing((Profession)newRemodelingImportObjects[i].Profession, remodelings[i].PlaceCode, remodelings[i].PlaceId, newRemodelingImportObjects[i].UnicomPowerUsed,
                                    newRemodelingImportObjects[i].UnicomPoleNumber, newRemodelingImportObjects[i].UnicomCabinetNumber, Urgency.一级, Bool.是, "", Guid.Parse("0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600"),
                                    remodelings[i].Id, Guid.Empty, newRemodelingImportObjects[i].UnicomUserId, CompanyNature.运营商);
                                    operatorsSharingRepository.Add(osUnicom);
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
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入资源信息
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportResources(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 24)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为24列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "基站名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为基站名称"));
                    }
                    if (dt.Columns[1].ColumnName != "铁塔类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为铁塔类型"));
                    }
                    if (dt.Columns[2].ColumnName != "铁塔高度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为铁塔高度"));
                    }
                    if (dt.Columns[3].ColumnName != "平台数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为平台数量"));
                    }
                    if (dt.Columns[4].ColumnName != "抱杆数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为抱杆数量"));
                    }
                    if (dt.Columns[5].ColumnName != "基础类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为基础类型"));
                    }
                    if (dt.Columns[6].ColumnName != "机房类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为机房类型"));
                    }
                    if (dt.Columns[7].ColumnName != "机房面积")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为机房面积"));
                    }
                    if (dt.Columns[8].ColumnName != "引入方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为引入方式"));
                    }
                    if (dt.Columns[9].ColumnName != "开关电源")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为开关电源"));
                    }
                    if (dt.Columns[10].ColumnName != "蓄电池")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为蓄电池"));
                    }
                    if (dt.Columns[11].ColumnName != "机柜数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为机柜数量"));
                    }
                    if (dt.Columns[12].ColumnName != "移动共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为移动共享"));
                    }
                    if (dt.Columns[13].ColumnName != "移动抱杆数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为移动抱杆数量"));
                    }
                    if (dt.Columns[14].ColumnName != "移动机柜数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为移动机柜数量"));
                    }
                    if (dt.Columns[15].ColumnName != "移动用电量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十六列", "列名必须为移动用电量"));
                    }
                    if (dt.Columns[16].ColumnName != "电信共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十七列", "列名必须为电信共享"));
                    }
                    if (dt.Columns[17].ColumnName != "电信抱杆数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十八列", "列名必须为电信抱杆数量"));
                    }
                    if (dt.Columns[18].ColumnName != "电信机柜数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十九列", "列名必须为电信机柜数量"));
                    }
                    if (dt.Columns[19].ColumnName != "电信用电量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十列", "列名必须为电信用电量"));
                    }
                    if (dt.Columns[20].ColumnName != "联通共享")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十一列", "列名必须为联通共享"));
                    }
                    if (dt.Columns[21].ColumnName != "联通抱杆数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十二列", "列名必须为联通抱杆数量"));
                    }
                    if (dt.Columns[22].ColumnName != "联通机柜数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十三列", "列名必须为联通机柜数量"));
                    }
                    if (dt.Columns[23].ColumnName != "联通用电量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二十四列", "列名必须为联通用电量"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证基站名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("基站名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "基站名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //基站名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;

                        string placeName = "";
                        Guid placeId = Guid.Empty;
                        int towerMark = 0;
                        int towerBaseMark = 0;
                        int machineRoomMark = 0;
                        int externalMark = 0;
                        int equipmentMark = 0;
                        TowerType towerType = TowerType.抱杆;
                        decimal towerHeight = 0;
                        int platFormNumber = 0;
                        int poleNumber = 0;
                        TowerBaseType towerBaseType = TowerBaseType.独立桩基;
                        MachineRoomType machineRoomType = MachineRoomType.其他;
                        decimal machineRoomArea = 0;
                        ExternalElectric externalElectric = ExternalElectric.专变;
                        decimal switchPower = 0;
                        decimal battery = 0;
                        int cabinetNumber = 0;
                        int mobileShare = 2;
                        int mobilePoleNumber = 0;
                        int mobileCabinetNumber = 0;
                        decimal mobilePowerUsed = 0;
                        int telecomShare = 2;
                        int telecomPoleNumber = 0;
                        int telecomCabinetNumber = 0;
                        decimal telecomPowerUsed = 0;
                        int unicomShare = 2;
                        int unicomPoleNumber = 0;
                        int unicomCabinetNumber = 0;
                        decimal unicomPowerUsed = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //基站名称验证
                            if (dr["基站名称"].ToString().Trim() != "")
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.PlaceName == placeName)))
                                {
                                    Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName));
                                    placeId = place.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", placeName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "不能为空"));
                            }

                            if (!purchaseRepository.Exists(Specification<Purchase>.Eval(entity => entity.PlaceId == placeId)))
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "只能导入购置基站的资源"));
                            }
                            if (remodelingRepository.Exists(Specification<Remodeling>.Eval(entity => entity.PlaceId == placeId)))
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基站名称", "该基站已进行改造，无法导入资源"));
                            }

                            //铁塔类型验证
                            if (dr["铁塔类型"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<TowerType>(dr["铁塔类型"].ToString().Trim(), out towerType))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "铁塔类型", dr["铁塔类型"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (towerType != TowerType.抱杆 && towerType != TowerType.插接式单管塔 && towerType != TowerType.灯杆景观塔 && towerType != TowerType.仿生树 && towerType != TowerType.角钢塔 && towerType != TowerType.路灯杆塔 && towerType != TowerType.落地拉线塔 && towerType != TowerType.三管塔 && towerType != TowerType.双轮景观塔 && towerType != TowerType.屋顶拉线塔 && towerType != TowerType.增高架 && towerType != TowerType.支撑杆)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "铁塔类型", dr["铁塔类型"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }

                            //铁塔高度验证
                            if (dr["铁塔高度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["铁塔高度"].ToString().Trim(), out towerHeight))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "铁塔高度", "必须为数字"));
                                }
                            }

                            //平台数量验证
                            if (dr["平台数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["平台数量"].ToString().Trim(), out platFormNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "平台数量", "必须为整数"));
                                }
                            }

                            //抱杆数量验证
                            if (dr["抱杆数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["抱杆数量"].ToString().Trim(), out poleNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "抱杆数量", "必须为整数"));
                                }
                            }

                            //基础类型验证
                            if (dr["基础类型"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<TowerBaseType>(dr["基础类型"].ToString().Trim(), out towerBaseType))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基础类型", dr["基础类型"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (towerBaseType != TowerBaseType.独立桩基 && towerBaseType != TowerBaseType.开挖式基础 && towerBaseType != TowerBaseType.拉线塔基础 && towerBaseType != TowerBaseType.楼顶塔基础)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "基础类型", dr["基础类型"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }

                            //机房类型验证
                            if (dr["机房类型"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<MachineRoomType>(dr["机房类型"].ToString().Trim(), out machineRoomType))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "机房类型", dr["机房类型"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (machineRoomType != MachineRoomType.其他 && machineRoomType != MachineRoomType.一体化机柜 && machineRoomType != MachineRoomType.自建彩钢板机房 && machineRoomType != MachineRoomType.自建砖混机房 && machineRoomType != MachineRoomType.租用砖混机房)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "机房类型", dr["机房类型"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }

                            //机房面积验证
                            if (dr["机房面积"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["机房面积"].ToString().Trim(), out machineRoomArea))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "机房面积", "必须为数字"));
                                }
                            }

                            //引入方式验证
                            if (dr["引入方式"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<ExternalElectric>(dr["引入方式"].ToString().Trim(), out externalElectric))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "引入方式", dr["引入方式"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (externalElectric != ExternalElectric.专变 && externalElectric != ExternalElectric.专线 && externalElectric != ExternalElectric.转供)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "引入方式", dr["引入方式"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }

                            //开关电源验证
                            if (dr["开关电源"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["开关电源"].ToString().Trim(), out switchPower))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "开关电源", "必须为数字"));
                                }
                            }

                            //蓄电池验证
                            if (dr["蓄电池"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["蓄电池"].ToString().Trim(), out battery))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "蓄电池", "必须为数字"));
                                }
                            }

                            //机柜数量验证
                            if (dr["机柜数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["机柜数量"].ToString().Trim(), out cabinetNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "机柜数量", "必须为整数"));
                                }
                            }

                            //移动共享验证
                            if (dr["移动共享"].ToString().Trim() != "")
                            {
                                if (dr["移动共享"].ToString().Trim() != "是" && dr["移动共享"].ToString().Trim() != "否")
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动共享", "必须为是或者否"));
                                }
                            }

                            //移动抱杆数量验证
                            if (dr["移动抱杆数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["移动抱杆数量"].ToString().Trim(), out mobilePoleNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动抱杆数量", "必须为整数"));
                                }
                            }

                            //移动机柜数量验证
                            if (dr["移动机柜数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["移动机柜数量"].ToString().Trim(), out mobileCabinetNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动机柜数量", "必须为整数"));
                                }
                            }

                            //移动用电量验证
                            if (dr["移动用电量"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["移动用电量"].ToString().Trim(), out mobilePowerUsed))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "移动用电量", "必须为数字"));
                                }
                            }

                            //电信共享验证
                            if (dr["电信共享"].ToString().Trim() != "")
                            {
                                if (dr["电信共享"].ToString().Trim() != "是" && dr["电信共享"].ToString().Trim() != "否")
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信共享", "必须为是或者否"));
                                }
                            }

                            //电信抱杆数量验证
                            if (dr["电信抱杆数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["电信抱杆数量"].ToString().Trim(), out telecomPoleNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信抱杆数量", "必须为整数"));
                                }
                            }

                            //电信机柜数量验证
                            if (dr["电信机柜数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["电信机柜数量"].ToString().Trim(), out telecomCabinetNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信机柜数量", "必须为整数"));
                                }
                            }

                            //电信用电量验证
                            if (dr["电信用电量"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["电信用电量"].ToString().Trim(), out telecomPowerUsed))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "电信用电量", "必须为数字"));
                                }
                            }

                            //联通共享验证
                            if (dr["联通共享"].ToString().Trim() != "")
                            {
                                if (dr["联通共享"].ToString().Trim() != "是" && dr["联通共享"].ToString().Trim() != "否")
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通共享", "必须为是或者否"));
                                }
                            }

                            //联通抱杆数量验证
                            if (dr["联通抱杆数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["联通抱杆数量"].ToString().Trim(), out unicomPoleNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通抱杆数量", "必须为整数"));
                                }
                            }

                            //联通机柜数量验证
                            if (dr["联通机柜数量"].ToString().Trim() != "")
                            {
                                if (!int.TryParse(dr["联通机柜数量"].ToString().Trim(), out unicomCabinetNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通机柜数量", "必须为整数"));
                                }
                            }

                            //联通用电量验证
                            if (dr["联通用电量"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["联通用电量"].ToString().Trim(), out unicomPowerUsed))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "联通用电量", "必须为数字"));
                                }
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                placeName = dr["基站名称"].ToString().Trim();
                                Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName));
                                placeId = place.Id;
                                PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == placeId && entity.PropertyType == PropertyType.站点参数));

                                //铁塔类型
                                if (dr["铁塔类型"].ToString().Trim() != "")
                                {
                                    System.Enum.TryParse<TowerType>(dr["铁塔类型"].ToString().Trim(), out towerType);
                                    towerMark = 1;
                                }
                                else
                                {
                                    towerMark = 0;
                                }

                                //铁塔高度
                                if (dr["铁塔高度"].ToString().Trim() != "")
                                {
                                    towerHeight = decimal.Parse(dr["铁塔高度"].ToString().Trim());
                                }
                                else
                                {
                                    towerHeight = 0;
                                }

                                //平台数量
                                if (dr["平台数量"].ToString().Trim() != "")
                                {
                                    platFormNumber = int.Parse(dr["平台数量"].ToString().Trim());
                                }
                                else
                                {
                                    platFormNumber = 0;
                                }

                                //抱杆数量
                                if (dr["抱杆数量"].ToString().Trim() != "")
                                {
                                    poleNumber = int.Parse(dr["抱杆数量"].ToString().Trim());
                                }
                                else
                                {
                                    poleNumber = 0;
                                }

                                //基础类型
                                if (dr["基础类型"].ToString().Trim() != "")
                                {
                                    System.Enum.TryParse<TowerBaseType>(dr["基础类型"].ToString().Trim(), out towerBaseType);
                                    towerBaseMark = 1;
                                }
                                else
                                {
                                    towerBaseMark = 0;
                                }

                                //机房类型
                                if (dr["机房类型"].ToString().Trim() != "")
                                {
                                    System.Enum.TryParse<MachineRoomType>(dr["机房类型"].ToString().Trim(), out machineRoomType);
                                    machineRoomMark = 1;
                                }
                                else
                                {
                                    machineRoomMark = 0;
                                }

                                //机房面积
                                if (dr["机房面积"].ToString().Trim() != "")
                                {
                                    machineRoomArea = decimal.Parse(dr["机房面积"].ToString().Trim());
                                }
                                else
                                {
                                    machineRoomArea = 0;
                                }

                                //引入方式
                                if (dr["引入方式"].ToString().Trim() != "")
                                {
                                    System.Enum.TryParse<ExternalElectric>(dr["引入方式"].ToString().Trim(), out externalElectric);
                                    externalMark = 1;
                                }
                                else
                                {
                                    externalMark = 0;
                                }

                                equipmentMark = 0;

                                //开关电源
                                if (dr["开关电源"].ToString().Trim() != "")
                                {
                                    equipmentMark = 1;
                                    switchPower = decimal.Parse(dr["开关电源"].ToString().Trim());
                                }
                                else
                                {
                                    switchPower = 0;
                                }

                                //蓄电池
                                if (dr["蓄电池"].ToString().Trim() != "")
                                {
                                    equipmentMark = 1;
                                    battery = decimal.Parse(dr["蓄电池"].ToString().Trim());
                                }
                                else
                                {
                                    battery = 0;
                                }

                                //机柜数量
                                if (dr["机柜数量"].ToString().Trim() != "")
                                {
                                    equipmentMark = 1;
                                    cabinetNumber = int.Parse(dr["机柜数量"].ToString().Trim());
                                }
                                else
                                {
                                    cabinetNumber = 0;
                                }

                                //移动共享
                                if (dr["移动共享"].ToString().Trim() != "")
                                {
                                    if (dr["移动共享"].ToString().Trim() == "是")
                                    {
                                        mobileShare = 1;
                                    }
                                    else
                                    {
                                        mobileShare = 2;
                                    }
                                }
                                else
                                {
                                    mobileShare = 2;
                                }

                                //移动抱杆数量
                                if (dr["移动抱杆数量"].ToString().Trim() != "")
                                {
                                    mobilePoleNumber = int.Parse(dr["移动抱杆数量"].ToString().Trim());
                                }
                                else
                                {
                                    mobilePoleNumber = 0;
                                }

                                //移动机柜数量
                                if (dr["移动机柜数量"].ToString().Trim() != "")
                                {
                                    mobileCabinetNumber = int.Parse(dr["移动机柜数量"].ToString().Trim());
                                }
                                else
                                {
                                    mobileCabinetNumber = 0;
                                }

                                //移动用电量
                                if (dr["移动用电量"].ToString().Trim() != "")
                                {
                                    mobilePowerUsed = decimal.Parse(dr["移动用电量"].ToString().Trim());
                                }
                                else
                                {
                                    mobilePowerUsed = 0;
                                }

                                //电信共享
                                if (dr["电信共享"].ToString().Trim() != "")
                                {
                                    if (dr["电信共享"].ToString().Trim() == "是")
                                    {
                                        telecomShare = 1;
                                    }
                                    else
                                    {
                                        telecomShare = 2;
                                    }
                                }
                                else
                                {
                                    telecomShare = 2;
                                }

                                //电信抱杆数量
                                if (dr["电信抱杆数量"].ToString().Trim() != "")
                                {
                                    telecomPoleNumber = int.Parse(dr["电信抱杆数量"].ToString().Trim());
                                }
                                else
                                {
                                    telecomPoleNumber = 0;
                                }

                                //电信机柜数量
                                if (dr["电信机柜数量"].ToString().Trim() != "")
                                {
                                    telecomCabinetNumber = int.Parse(dr["电信机柜数量"].ToString().Trim());
                                }
                                else
                                {
                                    telecomCabinetNumber = 0;
                                }

                                //电信用电量
                                if (dr["电信用电量"].ToString().Trim() != "")
                                {
                                    telecomPowerUsed = decimal.Parse(dr["电信用电量"].ToString().Trim());
                                }
                                else
                                {
                                    telecomPowerUsed = 0;
                                }

                                //联通共享
                                if (dr["联通共享"].ToString().Trim() != "")
                                {
                                    if (dr["联通共享"].ToString().Trim() == "是")
                                    {
                                        unicomShare = 1;
                                    }
                                    else
                                    {
                                        unicomShare = 2;
                                    }
                                }
                                else
                                {
                                    unicomShare = 2;
                                }

                                //联通抱杆数量
                                if (dr["联通抱杆数量"].ToString().Trim() != "")
                                {
                                    unicomPoleNumber = int.Parse(dr["联通抱杆数量"].ToString().Trim());
                                }
                                else
                                {
                                    unicomPoleNumber = 0;
                                }

                                //联通机柜数量
                                if (dr["联通机柜数量"].ToString().Trim() != "")
                                {
                                    unicomCabinetNumber = int.Parse(dr["联通机柜数量"].ToString().Trim());
                                }
                                else
                                {
                                    unicomCabinetNumber = 0;
                                }

                                //联通用电量
                                if (dr["联通用电量"].ToString().Trim() != "")
                                {
                                    unicomPowerUsed = decimal.Parse(dr["联通用电量"].ToString().Trim());
                                }
                                else
                                {
                                    unicomPowerUsed = 0;
                                }

                                if (towerMark == 1)
                                {
                                    if (!towerRepository.Exists(Specification<Tower>.Eval(entity => entity.ParentId == placeId && entity.PropertyType == PropertyType.站点参数)))
                                    {
                                        Tower tower = AggregateFactory.CreateTower(placeId, PropertyType.站点参数, towerType, towerHeight, platFormNumber, poleNumber, 0, 0, "", createUserId);
                                        towerRepository.Add(tower);
                                        TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.导入, placeId, PropertyType.站点参数, towerType, towerHeight, platFormNumber, poleNumber, 0, 0, "", createUserId);
                                        towerLogRepository.Add(towerLog);
                                    }
                                }
                                if (towerBaseMark == 1)
                                {
                                    if (!towerBaseRepository.Exists(Specification<TowerBase>.Eval(entity => entity.ParentId == placeId && entity.PropertyType == PropertyType.站点参数)))
                                    {
                                        TowerBase towerBase = AggregateFactory.CreateTowerBase(placeId, PropertyType.站点参数, towerBaseType, 0, 0, "", createUserId);
                                        towerBaseRepository.Add(towerBase);
                                        TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.导入, placeId, PropertyType.站点参数, towerBaseType, 0, 0, "", createUserId);
                                        towerBaseLogRepository.Add(towerBaseLog);
                                    }
                                }
                                if (machineRoomMark == 1)
                                {
                                    if (!machineRoomRepository.Exists(Specification<MachineRoom>.Eval(entity => entity.ParentId == placeId && entity.PropertyType == PropertyType.站点参数)))
                                    {
                                        MachineRoom machineRoom = AggregateFactory.CreateMachineRoom(placeId, PropertyType.站点参数, machineRoomType, machineRoomArea, 0, 0, "", createUserId);
                                        machineRoomRepository.Add(machineRoom);
                                        MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.导入, placeId, PropertyType.站点参数, machineRoomType, machineRoomArea, 0, 0, "", createUserId);
                                        machineRoomLogRepository.Add(machineRoomLog);
                                    }
                                }
                                if (externalMark == 1)
                                {
                                    if (!externalElectricPowerRepository.Exists(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == placeId && entity.PropertyType == PropertyType.站点参数)))
                                    {
                                        ExternalElectricPower external = AggregateFactory.CreateExternalElectricPower(placeId, PropertyType.站点参数, externalElectric, 0, 0, "", createUserId);
                                        externalElectricPowerRepository.Add(external);
                                        ExternalElectricPowerLog externalLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.导入, placeId, PropertyType.站点参数, externalElectric, 0, 0, "", createUserId);
                                        externalElectricPowerLogRepository.Add(externalLog);
                                    }
                                }
                                if (equipmentMark == 1)
                                {
                                    if (!equipmentInstallRepository.Exists(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == placeId && entity.PropertyType == PropertyType.站点参数)))
                                    {
                                        EquipmentInstall equipment = AggregateFactory.CreateEquipmentInstall(placeId, PropertyType.站点参数, switchPower, battery, cabinetNumber, 0, 0, "", createUserId);
                                        equipmentInstallRepository.Add(equipment);
                                        EquipmentInstallLog equipmentLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.导入, placeId, PropertyType.站点参数, switchPower, battery, cabinetNumber, 0, 0, "", createUserId);
                                        equipmentInstallLogRepository.Add(equipmentLog);
                                    }
                                }

                                if (placeProperty != null)
                                {
                                    if (placeProperty.MobileShare != (Bool)mobileShare || placeProperty.MobilePoleNumber != mobilePoleNumber || placeProperty.MobileCabinetNumber != mobileCabinetNumber || placeProperty.MobilePowerUsed != mobilePowerUsed)
                                    {
                                        place.OperatorShared(1, (Bool)mobileShare);
                                        placeRepository.Update(place);
                                        placeProperty.ModifyMobile((Bool)mobileShare, mobilePoleNumber, mobileCabinetNumber, mobilePowerUsed, createUserId);
                                        placePropertyRepository.Update(placeProperty);
                                        PlacePropertyLog mobilePlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.导入, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.移动, (Bool)mobileShare, mobilePoleNumber, mobileCabinetNumber, mobilePowerUsed, createUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                                        placePropertyLogRepository.Add(mobilePlacePropertyLog);
                                    }
                                    if (placeProperty.TelecomShare != (Bool)telecomShare || placeProperty.TelecomPoleNumber != telecomPoleNumber || placeProperty.TelecomCabinetNumber != telecomCabinetNumber || placeProperty.TelecomPowerUsed != telecomPowerUsed)
                                    {
                                        place.OperatorShared(2, (Bool)telecomShare);
                                        placeRepository.Update(place);
                                        placeProperty.ModifyTelecom((Bool)telecomShare, telecomPoleNumber, telecomCabinetNumber, telecomPowerUsed, createUserId);
                                        placePropertyRepository.Update(placeProperty);
                                        PlacePropertyLog telcomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.导入, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.电信, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, (Bool)telecomShare, telecomPoleNumber, telecomCabinetNumber, telecomPowerUsed, createUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                                        placePropertyLogRepository.Add(telcomPlacePropertyLog);
                                    }
                                    if (placeProperty.UnicomShare != (Bool)unicomShare || placeProperty.UnicomPoleNumber != unicomPoleNumber || placeProperty.UnicomCabinetNumber != unicomCabinetNumber || placeProperty.UnicomPowerUsed != unicomPowerUsed)
                                    {
                                        place.OperatorShared(3, (Bool)unicomShare);
                                        placeRepository.Update(place);
                                        placeProperty.ModifyUnicom((Bool)unicomShare, unicomPoleNumber, unicomCabinetNumber, unicomPowerUsed, createUserId);
                                        placePropertyRepository.Update(placeProperty);
                                        PlacePropertyLog unicomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.导入, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.联通, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, (Bool)unicomShare, unicomPoleNumber, unicomCabinetNumber, unicomPowerUsed, createUserId);
                                        placePropertyLogRepository.Add(unicomPlacePropertyLog);
                                    }
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
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入立项信息
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportProjectCodeList(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 6)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为6列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "立项编号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为立项编号"));
                    }
                    if (dt.Columns[1].ColumnName != "建设方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为建设方式"));
                    }
                    if (dt.Columns[2].ColumnName != "立项时间")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为立项时间"));
                    }
                    if (dt.Columns[3].ColumnName != "站点名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为站点名称"));
                    }
                    if (dt.Columns[4].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为网格"));
                    }
                    if (dt.Columns[5].ColumnName != "工程经理")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为工程经理"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证立项编号是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("立项编号") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "立项编号", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //立项编号有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string projectCode = "";
                        ProjectType projectType = ProjectType.新建;
                        DateTime projectDate = DateTime.Now;
                        string placeName = "";
                        Guid reseauId = Guid.Empty;
                        Guid projectManagerId = Guid.Empty;

                        IList<ProjectCodeList> projectCodeLists = new List<ProjectCodeList>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //立项编号验证
                            if (dr["立项编号"].ToString().Trim() != "")
                            {
                                projectCode = dr["立项编号"].ToString().Trim();
                                ProjectCodeList projectCodeList = projectCodeListRepository.Find(Specification<ProjectCodeList>.Eval(entity => entity.ProjectCode == projectCode));
                                if (projectCodeList != null)
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "立项编号", projectCode + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "立项编号", "不能为空"));
                            }

                            //建设方式验证
                            if (dr["建设方式"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<ProjectType>(dr["建设方式"].ToString().Trim(), out projectType))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", dr["建设方式"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (projectType != ProjectType.新建 && projectType != ProjectType.改造 && projectType != ProjectType.部分拆除 && projectType != ProjectType.全部拆除)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", dr["建设方式"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", "不能为空"));
                            }

                            //立项时间验证
                            if (dr["立项时间"].ToString().Trim() != "")
                            {
                                if (!DateTime.TryParse(dr["立项时间"].ToString().Trim(), out projectDate))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "立项时间", "必须为日期"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "立项时间", "不能为空"));
                            }

                            //站点名称验证
                            if (dr["站点名称"].ToString().Trim() != "")
                            {
                                placeName = dr["站点名称"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "站点名称", "不能为空"));
                            }

                            //网格验证
                            if (dr["网格"].ToString().Trim() != "")
                            {
                                string reseauName = dr["网格"].ToString().Trim();
                                Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName));
                                if (reseau != null)
                                {
                                    reseauId = reseau.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                            }

                            //工程经理验证
                            if (dr["工程经理"].ToString().Trim() != "")
                            {
                                string fullName = dr["工程经理"].ToString().Trim();
                                User user = userRepository.Find(Specification<User>.Eval(entity => entity.FullName == fullName && entity.State == State.使用 && entity.IsCurrentUsed == Bool.是));
                                if (user != null)
                                {
                                    projectManagerId = user.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "工程经理", fullName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "工程经理", "不能为空"));
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                projectCodeLists.Add(AggregateFactory.CreateProjectCodeList(projectCode, (ProjectType)projectType, projectDate, placeName, reseauId, projectManagerId, State.使用, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            foreach (var projectCodeList in projectCodeLists)
                            {
                                projectCodeListRepository.Add(projectCodeList);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_ProjectCodeListProjectCode"))
                                {
                                    throw new ApplicationFault("立项编号重复");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入采购清单
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportMaterialSpecList(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<ImportErrorObject> repeatImportErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 8)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为8列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "立项编号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为立项编号"));
                    }
                    if (dt.Columns[1].ColumnName != "供应商")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为供应商"));
                    }
                    if (dt.Columns[2].ColumnName != "型号类别")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为型号类别"));
                    }
                    if (dt.Columns[3].ColumnName != "规格型号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为规格型号"));
                    }
                    if (dt.Columns[4].ColumnName != "单价")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为单价"));
                    }
                    if (dt.Columns[5].ColumnName != "数量")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为数量"));
                    }
                    if (dt.Columns[6].ColumnName != "金额")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为金额"));
                    }
                    if (dt.Columns[7].ColumnName != "订单编号")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为订单编号"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证立项编号是否重复
                    //var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("立项编号") into g where g.Count() > 1 select g.FirstOrDefault());
                    //foreach (var duplicateData in duplicateDatas)
                    //{
                    //    if (duplicateData[0].ToString().Trim() != "")
                    //    {
                    //        importErrorObjects.Add(this.BuildImportError("模板", "立项编号", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                    //    }
                    //}

                    //立项编号有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string projectCode = "";
                        string customerName = "";
                        MaterialSpecType materialSpecType = MaterialSpecType.铁塔;
                        string materialSpecName = "";
                        decimal unitPrice = 0;
                        decimal specNumber = 0;
                        decimal totalPrice = 0;
                        string orderCode = "";

                        IList<MaterialSpecList> materialSpecLists = new List<MaterialSpecList>(dt.Rows.Count);
                        MaterialSpecList repeatMaterialSpecList = null;

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //立项编号验证
                            if (dr["立项编号"].ToString().Trim() != "")
                            {
                                projectCode = dr["立项编号"].ToString().Trim();
                                ProjectCodeList projectCodeList = projectCodeListRepository.Find(Specification<ProjectCodeList>.Eval(entity => entity.ProjectCode == projectCode));
                                if (projectCodeList == null)
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "立项编号", projectCode + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "立项编号", "不能为空"));
                            }

                            //供应商验证
                            if (dr["供应商"].ToString().Trim() != "")
                            {
                                customerName = dr["供应商"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "供应商", "不能为空"));
                            }

                            //型号类别验证
                            if (dr["型号类别"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<MaterialSpecType>(dr["型号类别"].ToString().Trim(), out materialSpecType))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "型号类别", dr["型号类别"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (materialSpecType != MaterialSpecType.地勘 && materialSpecType != MaterialSpecType.电力线缆 && materialSpecType != MaterialSpecType.监理 && materialSpecType != MaterialSpecType.开关电源 && materialSpecType != MaterialSpecType.美化外罩 && materialSpecType != MaterialSpecType.设计 && materialSpecType != MaterialSpecType.施工 && materialSpecType != MaterialSpecType.室外机柜 && materialSpecType != MaterialSpecType.铁塔 && materialSpecType != MaterialSpecType.土建 && materialSpecType != MaterialSpecType.外电引入 && materialSpecType != MaterialSpecType.蓄电池 && materialSpecType != MaterialSpecType.桩基检测)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "型号类别", dr["型号类别"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "型号类别", "不能为空"));
                            }

                            //规格型号验证
                            if (dr["规格型号"].ToString().Trim() != "")
                            {
                                customerName = dr["规格型号"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规格型号", "不能为空"));
                            }

                            //单价验证
                            if (dr["单价"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["单价"].ToString().Trim(), out unitPrice))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "单价", "必须为数字"));
                                }
                            }
                            else
                            {
                                unitPrice = 0;
                            }

                            //数量验证
                            if (dr["数量"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["数量"].ToString().Trim(), out specNumber))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "数量", "必须为数字"));
                                }
                            }
                            else
                            {
                                specNumber = 0;
                            }

                            //金额验证
                            if (dr["金额"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["金额"].ToString().Trim(), out totalPrice))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "金额", "必须为数字"));
                                }
                            }
                            else
                            {
                                totalPrice = 0;
                            }

                            //订单编号验证
                            if (dr["订单编号"].ToString().Trim() != "")
                            {
                                orderCode = dr["订单编号"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "订单编号", "不能为空"));
                            }

                            repeatMaterialSpecList = materialSpecListRepository.Find(Specification<MaterialSpecList>.Eval(entity => entity.ProjectCode == projectCode && entity.CustomerName == customerName && entity.MaterialSpecName == materialSpecName));
                            if (repeatMaterialSpecList != null)
                            {
                                repeatImportErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "同一立项编号同一供应商同一规格型号", "在系统中已存在"));
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                materialSpecLists.Add(AggregateFactory.CreateMaterialSpecList(projectCode, customerName, (MaterialSpecType)materialSpecType, materialSpecName, unitPrice, specNumber, totalPrice, orderCode, State.使用, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            foreach (var materialSpecList in materialSpecLists)
                            {
                                materialSpecListRepository.Add(materialSpecList);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("FK_dbo.tbl_MaterialSpecList_dbo.tbl_ProjectCodeList_ProjectCode"))
                                {
                                    throw new ApplicationFault("立项编号在立项信息表中不存在");
                                }
                                throw ex;
                            }
                        }
                        if (repeatImportErrorObjects.Count > 0)
                        {
                            importErrorObjects = repeatImportErrorObjects;
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入基站建设
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportPlanningApply(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 8)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为8列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "规划名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为规划名称"));
                    }
                    if (dt.Columns[1].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为区域"));
                    }
                    if (dt.Columns[2].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为网格"));
                    }
                    if (dt.Columns[3].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为经度"));
                    }
                    if (dt.Columns[4].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为纬度"));
                    }
                    if (dt.Columns[5].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[6].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[7].ColumnName != "建设理由")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为建设理由"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("规划名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "规划名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string planningName = "";
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        string detailedAddress = "";
                        string remarks = "";
                        IList<PlanningApply> planningApplys = new List<PlanningApply>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规划名称验证
                            if (dr["规划名称"].ToString().Trim() != "")
                            {
                                planningName = dr["规划名称"].ToString().Trim();
                                //if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.PlanningName == planningName)))
                                //{
                                //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", planningName + "-在系统中已存在"));
                                //}
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //验证详细地址
                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailedAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }

                            remarks = dr["建设理由"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                planningApplys.Add(AggregateFactory.CreatePlanningApply(codeSeedRepository.GenerateCode("PlanningApply"), planningName, Profession.基站, reseauId, lng, lat, importance, detailedAddress, remarks, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < planningApplys.Count; i++)
                            {
                                planningApplyRepository.Add(planningApplys[i]);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入室分建设
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportPlanningApplyID(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 8)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为8列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "规划名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为规划名称"));
                    }
                    if (dt.Columns[1].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为区域"));
                    }
                    if (dt.Columns[2].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为网格"));
                    }
                    if (dt.Columns[3].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为经度"));
                    }
                    if (dt.Columns[4].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为纬度"));
                    }
                    if (dt.Columns[5].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[6].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[7].ColumnName != "建设理由")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为建设理由"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("规划名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "规划名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string planningName = "";
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        string detailedAddress = "";
                        string remarks = "";
                        IList<PlanningApply> planningApplys = new List<PlanningApply>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规划名称验证
                            if (dr["规划名称"].ToString().Trim() != "")
                            {
                                planningName = dr["规划名称"].ToString().Trim();
                                //if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.PlanningName == planningName)))
                                //{
                                //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", planningName + "-在系统中已存在"));
                                //}
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //验证详细地址
                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailedAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }

                            remarks = dr["建设理由"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                planningApplys.Add(AggregateFactory.CreatePlanningApply(codeSeedRepository.GenerateCode("PlanningApply"), planningName, Profession.室分, reseauId, lng, lat, importance, detailedAddress, remarks, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < planningApplys.Count; i++)
                            {
                                planningApplyRepository.Add(planningApplys[i]);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入室分规划
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportPlanningID(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 8)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为8列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "规划名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为规划名称"));
                    }
                    if (dt.Columns[1].ColumnName != "室分类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为室分类型"));
                    }
                    if (dt.Columns[2].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为区域"));
                    }
                    if (dt.Columns[3].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为网格"));
                    }
                    if (dt.Columns[4].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为经度"));
                    }
                    if (dt.Columns[5].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为纬度"));
                    }
                    if (dt.Columns[6].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[7].ColumnName != "拟建网络")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为拟建网络"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("规划名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "规划名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string planningName = "";
                        Guid placeCategoryId = Guid.Empty;
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        string proposedNetwork = "";
                        IList<Planning> plannings = new List<Planning>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规划名称验证
                            if (dr["规划名称"].ToString().Trim() != "")
                            {
                                planningName = dr["规划名称"].ToString().Trim();
                                if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.PlanningName == planningName)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", planningName + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "规划名称", "不能为空"));
                            }

                            //室分类型验证
                            if (dr["室分类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["室分类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.室分));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //验证拟建网络
                            if (dr["拟建网络"].ToString().Trim() != "")
                            {
                                proposedNetwork = dr["拟建网络"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "拟建网络", "不能为空"));
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                plannings.Add(AggregateFactory.CreatePlanning(codeSeedRepository.GenerateCode("Planning"), planningName, Profession.室分, placeCategoryId, reseauId, lng, lat, "", "", proposedNetwork, "", importance, Guid.Empty, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < plannings.Count; i++)
                            {
                                planningRepository.Add(plannings[i]);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_PlanningCode"))
                                {
                                    throw new ApplicationFault("站点编码重复");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入室分改造
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportRemodelingID(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<RemodelingImportObject> remodelingImportObjects = new List<RemodelingImportObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 3)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为3列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "室分名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为室分名称"));
                    }
                    if (dt.Columns[1].ColumnName != "建设方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为建设方式"));
                    }
                    if (dt.Columns[2].ColumnName != "拟建网络")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为拟建网络"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 200)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为200行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证室分名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("室分名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "室分名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string placeCode = "";
                        string placeName = "";
                        Guid placeId = Guid.Empty;
                        ProjectType projectType = ProjectType.改造;
                        string proposedNetwork = "";
                        IList<Remodeling> remodelings = new List<Remodeling>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //室分名称验证
                            if (dr["室分名称"].ToString().Trim() != "")
                            {
                                placeName = dr["室分名称"].ToString().Trim();
                                Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName && entity.Profession == Profession.室分));
                                if (place != null)
                                {
                                    placeId = place.Id;
                                    placeCode = place.PlaceCode;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分名称", placeName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分名称", "不能为空"));
                            }

                            //建设方式验证
                            if (dr["建设方式"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<ProjectType>(dr["建设方式"].ToString().Trim(), out projectType))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", dr["建设方式"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (projectType != ProjectType.改造 && projectType != ProjectType.部分拆除 && projectType != ProjectType.全部拆除)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", dr["建设方式"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "建设方式", "不能为空"));
                            }

                            //验证拟建网络
                            if (dr["拟建网络"].ToString().Trim() != "")
                            {
                                proposedNetwork = dr["拟建网络"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "拟建网络", "不能为空"));
                            }

                            if (importErrorObjects.Count == 0)
                            {
                                remodelingImportObjects.Add(BuildRemodelingImportObject((int)projectType));
                                remodelings.Add(AggregateFactory.CreateRemodeling(Profession.室分, placeCode, placeId, proposedNetwork, "", createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < remodelings.Count; i++)
                            {
                                remodelingRepository.Add(remodelings[i]);

                                ProjectTask projectTask = AggregateFactory.CreateProjectTask((ProjectType)remodelingImportObjects[i].ProjectType, remodelings[i].Id, remodelings[i].PlaceId, "", createUserId);
                                projectTaskRepository.Add(projectTask);

                                EngineeringTask et5 = AggregateFactory.CreateEngineeringTask(TaskModel.设备安装, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et5);
                                EngineeringTask et6 = AggregateFactory.CreateEngineeringTask(TaskModel.线路, projectTask.Id, createUserId);
                                engineeringTaskRepository.Add(et6);
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
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 导入室分
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> ImportPlaceID(Guid excelFileId, Guid createUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 15)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为15列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "室分名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为室分名称"));
                    }
                    if (dt.Columns[1].ColumnName != "室分类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为室分类型"));
                    }
                    if (dt.Columns[2].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为区域"));
                    }
                    if (dt.Columns[3].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为网格"));
                    }
                    if (dt.Columns[4].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为经度"));
                    }
                    if (dt.Columns[5].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为纬度"));
                    }
                    if (dt.Columns[6].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[7].ColumnName != "产权")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为产权"));
                    }
                    if (dt.Columns[8].ColumnName != "租赁部门")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为租赁部门"));
                    }
                    if (dt.Columns[9].ColumnName != "实际租赁人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为实际租赁人"));
                    }
                    if (dt.Columns[10].ColumnName != "业主名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为业主名称"));
                    }
                    if (dt.Columns[11].ColumnName != "联系人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为联系人"));
                    }
                    if (dt.Columns[12].ColumnName != "联系方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为联系方式"));
                    }
                    if (dt.Columns[13].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[14].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 2000)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为2000行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("室分名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "室分名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        string placeName = "";
                        Guid placeCategoryId = Guid.Empty;
                        Guid placeOwnerId = Guid.Empty;
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        Guid addressingDepartmentId = Guid.Empty;
                        string addressingRealName = "";
                        string ownerName = "";
                        string ownerContact = "";
                        string ownerPhoneNumber = "";
                        string detailedAddress = "";
                        string remarks = "";
                        IList<Place> places = new List<Place>(dt.Rows.Count);
                        IList<PlaceBusinessVolume> placeBusinessVolumes = new List<PlaceBusinessVolume>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规室分称验证
                            if (dr["室分名称"].ToString().Trim() != "")
                            {
                                placeName = dr["室分名称"].ToString().Trim();
                                if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.PlaceName == placeName)))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分名称", placeName + "-在系统中已存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分名称", "不能为空"));
                            }

                            //室分类型验证
                            if (dr["室分类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["室分类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.室分));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //产权验证
                            if (dr["产权"].ToString().Trim() != "")
                            {
                                string placeOwnerName = dr["产权"].ToString().Trim();
                                PlaceOwner placeOwner = placeOwnerRepository.Find(Specification<PlaceOwner>.Eval(entity => entity.PlaceOwnerName == placeOwnerName));
                                if (placeOwner != null)
                                {
                                    placeOwnerId = placeOwner.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", placeOwnerName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", "不能为空"));
                            }

                            //租赁部门验证
                            if (dr["租赁部门"].ToString().Trim() != "")
                            {
                                string addressingDepartmentName = dr["租赁部门"].ToString().Trim();
                                Department department = departmentRepository.Find(Specification<Department>.Eval(entity => entity.DepartmentName == addressingDepartmentName));
                                if (department != null)
                                {
                                    addressingDepartmentId = department.Id;
                                }
                                //else
                                //{
                                //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "租赁部门", addressingDepartmentName + "-在系统中不存在"));
                                //}
                            }
                            //else
                            //{
                            //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "租赁部门", "不能为空"));
                            //}

                            //验证实际租赁人
                            if (dr["实际租赁人"].ToString().Trim() != "")
                            {
                                addressingRealName = dr["实际租赁人"].ToString().Trim();
                            }
                            //else
                            //{
                            //    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "实际租赁人", "不能为空"));
                            //}

                            ownerName = dr["业主名称"].ToString().Trim();
                            ownerContact = dr["联系人"].ToString().Trim();
                            ownerPhoneNumber = dr["联系方式"].ToString().Trim();

                            //详细地址验证
                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailedAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                places.Add(AggregateFactory.CreatePlace(codeSeedRepository.GenerateCode("Place"), placeName, Profession.室分, placeCategoryId, reseauId, lng,
                                    lat, placeOwnerId, importance, addressingDepartmentId, addressingRealName, ownerName, ownerContact, ownerPhoneNumber, detailedAddress,
                                    remarks, PlaceMapState.项目开通, createUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            User user = userRepository.GetByKey(createUserId);
                            Department department = departmentRepository.GetByKey(user.DepartmentId);
                            for (int i = 0; i < places.Count; i++)
                            {
                                placeRepository.Add(places[i]);

                                PlaceBusinessVolume placeBusinessVolume = AggregateFactory.CreatePlaceBusinessVolume(places[i].Id, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, department.CompanyId);
                                placeBusinessVolumeRepository.Add(placeBusinessVolume);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("IX_UQ_PlaceCode"))
                                {
                                    throw new ApplicationFault("室分编码重复");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 更新室分
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        public IList<ImportErrorObject> UpdatePlaceID(Guid excelFileId, Guid modifyUserId)
        {
            IList<ImportErrorObject> importErrorObjects = new List<ImportErrorObject>();
            IList<PlaceMaintObject> placeMaintObjects = new List<PlaceMaintObject>();
            File file = fileRepository.FindByKey(excelFileId);
            if (file != null)
            {
                if (!FileHelper.IsExistFile(file.FilePath))
                {
                    throw new ApplicationFault("导入的Excel文件在系统中不存在");
                }

                DataTable dt = ExcelHelper.ExcelToDataTable(file.FilePath, "Sheet1");

                //列验证
                if (dt.Columns.Count != 15)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "列数", "导入的模板列数为" + dt.Columns.Count.ToString() + "列，正确的模板列数应为15列"));
                }
                else
                {
                    if (dt.Columns[0].ColumnName != "室分名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第一列", "列名必须为室分名称"));
                    }
                    if (dt.Columns[1].ColumnName != "室分类型")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第二列", "列名必须为室分类型"));
                    }
                    if (dt.Columns[2].ColumnName != "区域")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第三列", "列名必须为区域"));
                    }
                    if (dt.Columns[3].ColumnName != "网格")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第四列", "列名必须为网格"));
                    }
                    if (dt.Columns[4].ColumnName != "经度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第五列", "列名必须为经度"));
                    }
                    if (dt.Columns[5].ColumnName != "纬度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第六列", "列名必须为纬度"));
                    }
                    if (dt.Columns[6].ColumnName != "重要性程度")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第七列", "列名必须为重要性程度"));
                    }
                    if (dt.Columns[7].ColumnName != "产权")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第八列", "列名必须为产权"));
                    }
                    if (dt.Columns[8].ColumnName != "租赁部门")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第九列", "列名必须为租赁部门"));
                    }
                    if (dt.Columns[9].ColumnName != "实际租赁人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十列", "列名必须为实际租赁人"));
                    }
                    if (dt.Columns[10].ColumnName != "业主名称")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十一列", "列名必须为业主名称"));
                    }
                    if (dt.Columns[11].ColumnName != "联系人")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十二列", "列名必须为联系人"));
                    }
                    if (dt.Columns[12].ColumnName != "联系方式")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十三列", "列名必须为联系方式"));
                    }
                    if (dt.Columns[13].ColumnName != "详细地址")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十四列", "列名必须为详细地址"));
                    }
                    if (dt.Columns[14].ColumnName != "备注")
                    {
                        importErrorObjects.Add(this.BuildImportError("模板", "第十五列", "列名必须为备注"));
                    }
                }

                //行数验证
                if (dt.Rows.Count == 0)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "导入的模板不存在数据行"));
                }
                if (dt.Rows.Count > 2000)
                {
                    importErrorObjects.Add(this.BuildImportError("模板", "行数", "每次导入的模板数据最多为2000行"));
                }

                //列或者行有错，直接返回
                if (importErrorObjects.Count() > 0)
                {
                    return importErrorObjects;
                }
                else
                {
                    //验证规划名称是否重复
                    var duplicateDatas = (from r in dt.AsEnumerable() group r by r.Field<object>("室分名称") into g where g.Count() > 1 select g.FirstOrDefault());
                    foreach (var duplicateData in duplicateDatas)
                    {
                        if (duplicateData[0].ToString().Trim() != "")
                        {
                            importErrorObjects.Add(this.BuildImportError("模板", "室分名称", "" + duplicateData[0].ToString() + "-在导入的模板中存在重复数据"));
                        }
                    }

                    //规划名称有重复，直接返回
                    if (importErrorObjects.Count() > 0)
                    {
                        return importErrorObjects;
                    }
                    else
                    {
                        int rowIndex = 1;
                        Guid placeId = Guid.Empty;
                        string placeName = "";
                        Guid placeCategoryId = Guid.Empty;
                        Guid placeOwnerId = Guid.Empty;
                        Guid areaId = Guid.Empty;
                        Guid reseauId = Guid.Empty;
                        decimal lng = 0;
                        decimal lat = 0;
                        Importance importance = Importance.C;
                        Guid addressingDepartmentId = Guid.Empty;
                        string addressingRealName = "";
                        string ownerName = "";
                        string ownerContact = "";
                        string ownerPhoneNumber = "";
                        string detailedAddress = "";
                        string remarks = "";
                        IList<Place> places = new List<Place>(dt.Rows.Count);
                        IList<PlaceBusinessVolume> placeBusinessVolumes = new List<PlaceBusinessVolume>(dt.Rows.Count);

                        foreach (DataRow dr in dt.Rows)
                        {
                            rowIndex++;

                            //规室分称验证
                            if (dr["室分名称"].ToString().Trim() != "")
                            {
                                placeName = dr["室分名称"].ToString().Trim();
                                Place place = placeRepository.Find(Specification<Place>.Eval(entity => entity.PlaceName == placeName));
                                if (place != null)
                                {
                                    placeId = place.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分名称", placeName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分名称", "不能为空"));
                            }

                            //室分类型验证
                            if (dr["室分类型"].ToString().Trim() != "")
                            {
                                string placeCategoryName = dr["室分类型"].ToString().Trim();
                                PlaceCategory placeCategory = placeCategoryRepository.Find(Specification<PlaceCategory>.Eval(entity => entity.PlaceCategoryName == placeCategoryName && entity.Profession == Profession.室分));
                                if (placeCategory != null)
                                {
                                    placeCategoryId = placeCategory.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分类型", placeCategoryName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "室分类型", "不能为空"));
                            }

                            //区域验证
                            if (dr["区域"].ToString().Trim() != "")
                            {
                                string areaName = dr["区域"].ToString().Trim();
                                Area area = areaRepository.Find(Specification<Area>.Eval(entity => entity.AreaName == areaName));
                                if (area != null)
                                {
                                    areaId = area.Id;
                                    //网格验证
                                    if (dr["网格"].ToString().Trim() != "")
                                    {
                                        string reseauName = dr["网格"].ToString().Trim();
                                        Reseau reseau = reseauRepository.Find(Specification<Reseau>.Eval(entity => entity.ReseauName == reseauName && entity.AreaId == areaId));
                                        if (reseau != null)
                                        {
                                            reseauId = reseau.Id;
                                        }
                                        else
                                        {
                                            importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", reseauName + "-在系统中不存在"));
                                        }
                                    }
                                    else
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "网格", "不能为空"));
                                    }
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", areaName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "区域", "不能为空"));
                            }

                            //经度验证
                            if (dr["经度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["经度"].ToString().Trim(), out lng))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "经度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lng = 0;
                            }

                            //纬度验证
                            if (dr["纬度"].ToString().Trim() != "")
                            {
                                if (!decimal.TryParse(dr["纬度"].ToString().Trim(), out lat))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "纬度", "必须为数字"));
                                }
                            }
                            else
                            {
                                lat = 0;
                            }

                            //重要性程度验证
                            if (dr["重要性程度"].ToString().Trim() != "")
                            {
                                if (!System.Enum.TryParse<Importance>(dr["重要性程度"].ToString().Trim(), out importance))
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                }
                                else
                                {
                                    if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
                                    {
                                        importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", dr["重要性程度"].ToString().Trim() + "-在系统中不存在"));
                                    }
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "重要性程度", "不能为空"));
                            }

                            //产权验证
                            if (dr["产权"].ToString().Trim() != "")
                            {
                                string placeOwnerName = dr["产权"].ToString().Trim();
                                PlaceOwner placeOwner = placeOwnerRepository.Find(Specification<PlaceOwner>.Eval(entity => entity.PlaceOwnerName == placeOwnerName));
                                if (placeOwner != null)
                                {
                                    placeOwnerId = placeOwner.Id;
                                }
                                else
                                {
                                    importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", placeOwnerName + "-在系统中不存在"));
                                }
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "产权", "不能为空"));
                            }

                            //租赁部门验证
                            if (dr["租赁部门"].ToString().Trim() != "")
                            {
                                string addressingDepartmentName = dr["租赁部门"].ToString().Trim();
                                Department department = departmentRepository.Find(Specification<Department>.Eval(entity => entity.DepartmentName == addressingDepartmentName));
                                if (department != null)
                                {
                                    addressingDepartmentId = department.Id;
                                }
                            }

                            addressingRealName = dr["实际租赁人"].ToString().Trim();
                            ownerName = dr["业主名称"].ToString().Trim();
                            ownerContact = dr["联系人"].ToString().Trim();
                            ownerPhoneNumber = dr["联系方式"].ToString().Trim();

                            //详细地址验证
                            if (dr["详细地址"].ToString().Trim() != "")
                            {
                                detailedAddress = dr["详细地址"].ToString().Trim();
                            }
                            else
                            {
                                importErrorObjects.Add(this.BuildImportError("第" + rowIndex + "行", "详细地址", "不能为空"));
                            }

                            remarks = dr["备注"].ToString().Trim();

                            if (importErrorObjects.Count == 0)
                            {
                                placeMaintObjects.Add(BuildUpdatePlaceObject(placeId, placeCategoryId, reseauId, lng, lat, importance, placeOwnerId, addressingDepartmentId,
                                    addressingRealName, ownerName, ownerContact, ownerPhoneNumber, detailedAddress, remarks, modifyUserId));
                            }
                        }

                        //存在验证失败，直接返回
                        if (importErrorObjects.Count > 0)
                        {
                            return importErrorObjects;
                        }
                        else
                        {
                            for (int i = 0; i < placeMaintObjects.Count; i++)
                            {
                                Place place = placeRepository.GetByKey(placeMaintObjects[i].Id);
                                place.UpdatePlace(placeMaintObjects[i].PlaceCategoryId, placeMaintObjects[i].ReseauId, placeMaintObjects[i].Lng, placeMaintObjects[i].Lat,
                                    (Importance)placeMaintObjects[i].Importance, placeMaintObjects[i].PlaceOwner, placeMaintObjects[i].AddressingDepartmentId,
                                    placeMaintObjects[i].AddressingRealName, placeMaintObjects[i].OwnerName, placeMaintObjects[i].OwnerContact,
                                    placeMaintObjects[i].OwnerPhoneNumber, placeMaintObjects[i].DetailedAddress, placeMaintObjects[i].Remarks,
                                    placeMaintObjects[i].ModifyUserId);
                                placeRepository.Update(place);
                            }
                            try
                            {
                                this.Context.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                                {
                                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                                }
                                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                                {
                                    throw new ApplicationFault("选择的网格在系统中不存在");
                                }
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("导入的Excel文件在系统中不存在");
            }
            return importErrorObjects;
        }

        /// <summary>
        /// 生成导入错误对象
        /// </summary>
        /// <param name="objectName">对象名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>导入错误对象</returns>
        private ImportErrorObject BuildImportError(string objectName, string propertyName, string errorMessage)
        {
            return new ImportErrorObject()
            {
                ObjectName = objectName,
                PropertyName = propertyName,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// 新增基站导入对象
        /// </summary>
        /// <param name="mobileDemand">移动需求</param>
        /// <param name="telecomDemand">电信需求</param>
        /// <param name="unicomDemand">联通需求</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业属性</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="sceneId">周边场景Id</param>
        /// <param name="ownerName">业主</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系方式</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="mobileAntennaHeight">移动天线挂高</param>
        /// <param name="mobilePoleNumber">移动平台数量</param>
        /// <param name="mobileCabinetNumber">移动机柜数量</param>
        /// <param name="mobileUserId">移动确认人</param>
        /// <param name="telecomAntennaHeight">电信天线挂高</param>
        /// <param name="telecomPoleNumber">电信平台数量</param>
        /// <param name="telecomCabinetNumber">电信机柜数量</param>
        /// <param name="telecomUserId">电信确认人</param>
        /// <param name="unicomAntennaHeight">联通天线挂高</param>
        /// <param name="unicomPoleNumber">联通平台数量</param>
        /// <param name="unicomCabinetNumber">联通机柜数量</param>
        /// <param name="unicomUserId">联通确认人</param>
        /// <returns></returns>
        private NewPlanningImportObject BuildNewPlanningImportObject(int mobileDemand, int telecomDemand, int unicomDemand, string planningName, int profession, Guid placeCategoryId,
            Guid areaId, Guid reseauId, decimal lng, decimal lat, Guid sceneId, string ownerName, string ownerContact, string ownerPhoneNumber, string detailedAddress, string remarks,
            decimal mobileAntennaHeight, int mobilePoleNumber, int mobileCabinetNumber, Guid mobileUserId, decimal telecomAntennaHeight, int telecomPoleNumber, int telecomCabinetNumber,
            Guid telecomUserId, decimal unicomAntennaHeight, int unicomPoleNumber, int unicomCabinetNumber, Guid unicomUserId)
        {
            return new NewPlanningImportObject()
            {
                MobileDemand = mobileDemand,
                TelecomDemand = telecomDemand,
                UnicomDemand = unicomDemand,
                PlanningName = planningName,
                Profession = profession,
                PlaceCategoryId = placeCategoryId,
                AreaId = areaId,
                ReseauId = reseauId,
                Lng = lng,
                Lat = lat,
                SceneId = sceneId,
                OwnerName = ownerName,
                OwnerContact = ownerContact,
                OwnerPhoneNumber = ownerPhoneNumber,
                DetailedAddress = detailedAddress,
                Remarks = remarks,
                MobileAntennaHeight = mobileAntennaHeight,
                MobilePoleNumber = mobilePoleNumber,
                MobileCabinetNumber = mobileCabinetNumber,
                MobileUserId = mobileUserId,
                TelecomAntennaHeight = telecomAntennaHeight,
                TelecomPoleNumber = telecomPoleNumber,
                TelecomCabinetNumber = telecomCabinetNumber,
                TelecomUserId = telecomUserId,
                UnicomAntennaHeight = unicomAntennaHeight,
                UnicomPoleNumber = unicomPoleNumber,
                UnicomCabinetNumber = unicomCabinetNumber,
                UnicomUserId = unicomUserId
            };
        }

        /// <summary>
        /// 改造基站导入对象
        /// </summary>
        /// <param name="mobileDemand">移动需求</param>
        /// <param name="telecomDemand">电信需求</param>
        /// <param name="unicomDemand">联通需求</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业属性</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="sceneId">周边场景Id</param>
        /// <param name="ownerName">业主</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系方式</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="mobilePowerUsed">移动用电量</param>
        /// <param name="mobilePoleNumber">移动平台数量</param>
        /// <param name="mobileCabinetNumber">移动机柜数量</param>
        /// <param name="mobileUserId">移动确认人</param>
        /// <param name="telecomPowerUsed">电信用电量</param>
        /// <param name="telecomPoleNumber">电信平台数量</param>
        /// <param name="telecomCabinetNumber">电信机柜数量</param>
        /// <param name="telecomUserId">电信确认人</param>
        /// <param name="unicomPowerUsed">联通用电量</param>
        /// <param name="unicomPoleNumber">联通平台数量</param>
        /// <param name="unicomCabinetNumber">联通机柜数量</param>
        /// <param name="unicomUserId">联通确认人</param>
        /// <returns></returns>
        private NewRemodelingImportObject BuildNewRemodelingImportObject(int mobileDemand, int telecomDemand, int unicomDemand, string placeName, int profession, decimal mobilePowerUsed,
            int mobilePoleNumber, int mobileCabinetNumber, Guid mobileUserId, decimal telecomPowerUsed, int telecomPoleNumber, int telecomCabinetNumber, Guid telecomUserId, decimal unicomPowerUsed,
            int unicomPoleNumber, int unicomCabinetNumber, Guid unicomUserId)
        {
            return new NewRemodelingImportObject()
            {
                MobileDemand = mobileDemand,
                TelecomDemand = telecomDemand,
                UnicomDemand = unicomDemand,
                PlaceName = placeName,
                Profession = profession,
                MobilePowerUsed = mobilePowerUsed,
                MobilePoleNumber = mobilePoleNumber,
                MobileCabinetNumber = mobileCabinetNumber,
                MobileUserId = mobileUserId,
                TelecomPowerUsed = telecomPowerUsed,
                TelecomPoleNumber = telecomPoleNumber,
                TelecomCabinetNumber = telecomCabinetNumber,
                TelecomUserId = telecomUserId,
                UnicomPowerUsed = unicomPowerUsed,
                UnicomPoleNumber = unicomPoleNumber,
                UnicomCabinetNumber = unicomCabinetNumber,
                UnicomUserId = unicomUserId
            };
        }

        private RemodelingImportObject BuildRemodelingImportObject(int projectType)
        {
            return new RemodelingImportObject()
            {
                ProjectType = projectType
            };
        }

        /// <summary>
        /// 逻辑号导入对象
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="g2Number">2G逻辑号</param>
        /// <param name="d2Number">2D逻辑号</param>
        /// <param name="g3Number">3G逻辑号</param>
        /// <param name="g4Number">4G逻辑号</param>
        /// <returns></returns>
        private PlaceMaintObject BuildLogicalNumberImportObject(Guid placeId, string g2Number, string d2Number, string g3Number, string g4Number)
        {
            return new PlaceMaintObject()
            {
                Id = placeId,
                G2Number = g2Number,
                D2Number = d2Number,
                G3Number = g3Number,
                G4Number = g4Number
            };
        }

        /// <summary>
        /// 业务量导入对象
        /// </summary>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="trafficVolumes">话务量</param>
        /// <param name="businessVolumes">业务量</param>
        /// <returns></returns>
        private BusinessVolumeMaintObject BuildBusinessVolumeImportObject(int logicalType, string logicalNumber, decimal trafficVolumes, int businessVolumes)
        {
            return new BusinessVolumeMaintObject()
            {
                LogicalType = logicalType,
                LogicalNumber = logicalNumber,
                TrafficVolumes = trafficVolumes,
                BusinessVolumes = businessVolumes
            };
        }

        /// <summary>
        /// 基站更新对象
        /// </summary>
        /// <param name="placeId">基站Id</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="addressingDepartmentId">租赁部门Id</param>
        /// <param name="addressingRealName">实际租赁人</param>
        /// <param name="ownerName">业主名称</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        /// <returns></returns>
        private PlaceMaintObject BuildUpdatePlaceObject(Guid placeId, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat, Importance importance, Guid placeOwner, Guid addressingDepartmentId,
            string addressingRealName, string ownerName, string ownerContact, string ownerPhoneNumber, string detailedAddress, string remarks, Guid modifyUserId)
        {
            return new PlaceMaintObject()
            {
                Id = placeId,
                PlaceCategoryId = placeCategoryId,
                ReseauId = reseauId,
                Lng = lng,
                Lat = lat,
                Importance = (int)importance,
                PlaceOwner = placeOwner,
                AddressingDepartmentId = addressingDepartmentId,
                AddressingRealName = addressingRealName,
                OwnerName = ownerName,
                OwnerContact = ownerContact,
                OwnerPhoneNumber = ownerPhoneNumber,
                DetailedAddress = detailedAddress,
                Remarks = remarks,
                ModifyUserId = modifyUserId
            };
        }
    }
}
