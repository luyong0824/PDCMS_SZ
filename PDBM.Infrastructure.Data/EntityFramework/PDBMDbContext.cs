using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;

namespace PDBM.Infrastructure.Data.EntityFramework
{
    /// <summary>
    /// PDBM数据库上下文
    /// </summary>
    public sealed class PDBMDbContext : DbContext
    {
        public PDBMDbContext()
            : base("name=PDCMS_SZ")
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = true;
            this.Database.Log = s => Console.WriteLine(s);
        }

        #region BaseData
        public DbSet<Company> CompanySet { get; set; }
        public DbSet<Department> DepartmentSet { get; set; }
        public DbSet<User> UserSet { get; set; }
        public DbSet<Menu> MenuSet { get; set; }
        public DbSet<MenuSub> MenuSubSet { get; set; }
        public DbSet<MenuItem> MenuItemSet { get; set; }
        public DbSet<Role> RoleSet { get; set; }
        public DbSet<RoleMenuItem> RoleMenuItemSet { get; set; }
        public DbSet<RoleUser> RoleUserSet { get; set; }
        public DbSet<Post> PostSet { get; set; }
        public DbSet<PostUser> PostUserSet { get; set; }
        public DbSet<AccountingEntity> AccountingEntitySet { get; set; }
        public DbSet<Project> ProjectSet { get; set; }
        public DbSet<ProjectProfession> ProjectProfessionSet { get; set; }
        public DbSet<Area> AreaSet { get; set; }
        public DbSet<Reseau> ReseauSet { get; set; }
        public DbSet<PlaceCategory> PlaceCategorySet { get; set; }
        public DbSet<Scene> SceneSet { get; set; }
        public DbSet<Place> PlaceSet { get; set; }
        public DbSet<CodeSeed> CodeSeedSet { get; set; }
        public DbSet<OrderCodeSeed> OrderCodeSeedSet { get; set; }
        public DbSet<Unit> UnitSet { get; set; }
        public DbSet<MaterialCategory> MaterialCategorySet { get; set; }
        public DbSet<Material> MaterialSet { get; set; }
        public DbSet<MaterialSpec> MaterialSpecSet { get; set; }
        public DbSet<Customer> CustomerSet { get; set; }
        public DbSet<PlaceProperty> PlacePropertySet { get; set; }
        public DbSet<PlacePropertyLog> PlacePropertyLogSet { get; set; }
        public DbSet<WorkBigClass> WorkBigClassSet { get; set; }
        public DbSet<WorkSmallClass> WorkSmallClassSet { get; set; }
        public DbSet<CustomerUser> CustomerUserSet { get; set; }
        public DbSet<PlaceOwner> PlaceOwnerSet { get; set; }
        public DbSet<DutyUser> DutyUser { get; set; }
        #endregion

        #region BMMgmt
        public DbSet<PlanningApply> PlanningApplySet { get; set; }
        public DbSet<PlanningApplyHeader> PlanningApplyHeaderSet { get; set; }
        public DbSet<OperatorsPlanning> OperatorsPlanningSet { get; set; }
        public DbSet<Planning> PlanningSet { get; set; }
        public DbSet<Addressing> AddressingSet { get; set; }
        public DbSet<Purchase> PurchaseSet { get; set; }
        public DbSet<OperatorsConfirm> OperatorsConfirmSet { get; set; }
        public DbSet<OperatorsConfirmDetail> OperatorsConfirmDetailSet { get; set; }
        public DbSet<OperatorsSharing> OperatorsSharingSet { get; set; }
        public DbSet<Remodeling> RemodelingSet { get; set; }
        public DbSet<ConstructionTask> ConstructionTaskSet { get; set; }
        public DbSet<OperatorsPlanningDemand> OperatorsPlanningDemandSet { get; set; }
        public DbSet<Tower> TowerSet { get; set; }
        public DbSet<TowerBase> TowerBaseSet { get; set; }
        public DbSet<MachineRoom> MachineRoomSet { get; set; }
        public DbSet<ExternalElectricPower> ExternalElectricPowerSet { get; set; }
        public DbSet<EquipmentInstall> EquipmentInstallSet { get; set; }
        public DbSet<AddressExplor> AddressExplorSet { get; set; }
        public DbSet<FoundationTest> FoundationTestSet { get; set; }
        public DbSet<PlaceDesign> PlaceDesignSet { get; set; }
        public DbSet<MaterialList> MaterialListSet { get; set; }
        public DbSet<TaskProperty> TaskPropertySet { get; set; }
        public DbSet<TowerLog> TowerLogSet { get; set; }
        public DbSet<TowerBaseLog> TowerBaseLogSet { get; set; }
        public DbSet<MachineRoomLog> MachineRoomLogSet { get; set; }
        public DbSet<ExternalElectricPowerLog> ExternalElectricPowerLogSet { get; set; }
        public DbSet<EquipmentInstallLog> EquipmentInstallLogSet { get; set; }
        public DbSet<AddressExplorLog> AddressExplorLogSet { get; set; }
        public DbSet<FoundationTestLog> FoundationTestLogSet { get; set; }
        public DbSet<TaskPropertyLog> TaskPropertyLogSet { get; set; }
        public DbSet<WorkApply> WorkApplySet { get; set; }
        public DbSet<WorkOrder> WorkOrderSet { get; set; }
        public DbSet<WorkOrderDetail> WorkOrderDetailSet { get; set; }
        public DbSet<DelayApply> DelayApplySet { get; set; }
        public DbSet<ProjectCodeList> ProjectCodeListSet { get; set; }
        public DbSet<MaterialSpecList> MaterialSpecListSet { get; set; }
        public DbSet<ProjectTask> ProjectTaskSet { get; set; }
        public DbSet<EngineeringTask> EngineeringTaskSet { get; set; }
        public DbSet<BusinessVolume> BusinessVolumeSet { get; set; }
        public DbSet<Notice> NoticeSet { get; set; }
        public DbSet<PlaceBusinessVolume> PlaceBusinessVolumeSet { get; set; }
        public DbSet<BlindSpotFeedBack> BlindSpotFeedBackSet { get; set; }
        #endregion

        #region FileMgmt
        public DbSet<File> FileSet { get; set; }
        public DbSet<FileAssociation> FileAssociationSet { get; set; }
        #endregion

        #region WorkFlow
        public DbSet<WFActivity> WFActivitySet { get; set; }
        public DbSet<WFActivityEditor> WFActivityEditorSet { get; set; }
        public DbSet<WFCategory> WFCategorySet { get; set; }
        public DbSet<WFProcess> WFProcessSet { get; set; }
        public DbSet<WFProcessInstance> WFProcessInstanceSet { get; set; }
        public DbSet<WFActivityInstance> WFActivityInstanceSet { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
