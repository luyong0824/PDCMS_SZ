begin tran DataBaseInit

--添加唯一索引
----BaseData
alter table tbl_Menu
add constraint IX_UQ_MenuName unique(MenuName);

alter table tbl_Menu
add constraint IX_UQ_IndexId unique(IndexId);

alter table tbl_MenuSub
add constraint IX_UQ_MenuIdMenuSubName unique(MenuId,MenuSubName);

alter table tbl_MenuSub
add constraint IX_UQ_MenuIdIndexId unique(MenuId,IndexId);

alter table tbl_MenuItem
add constraint IX_UQ_MenuSubIdMenuItemName unique(MenuSubId,MenuItemName);

alter table tbl_MenuItem
add constraint IX_UQ_MenuSubIdIndexId unique(MenuSubId,IndexId);

alter table tbl_Role
add constraint IX_UQ_RoleCode unique(RoleCode);

alter table tbl_Role
add constraint IX_UQ_RoleName unique(RoleName);

alter table tbl_RoleMenuItem
add constraint IX_UQ_RoleIdMenuItemId unique(RoleId,MenuItemId);

alter table tbl_RoleUser
add constraint IX_UQ_RoleIdUserId unique(RoleId,UserId);

alter table tbl_Post
add constraint IX_UQ_PostCode unique(PostCode);

alter table tbl_Post
add constraint IX_UQ_PostName unique(PostName);

alter table tbl_PostUser
add constraint IX_UQ_PostIdUserId unique(PostId,UserId);

alter table tbl_Company
add constraint IX_UQ_CompanyCode unique(CompanyCode);

alter table tbl_Company
add constraint IX_UQ_CompanyName unique(CompanyName);

alter table tbl_Company
add constraint IX_UQ_CompanyFullName unique(CompanyFullName);

alter table tbl_Company
add constraint IX_UQ_ApplyCodePrefix unique(ApplyCodePrefix);

alter table tbl_Department
add constraint IX_UQ_CompanyIdDepartmentCode unique(CompanyId,DepartmentCode);

alter table tbl_Department
add constraint IX_UQ_CompanyIdDepartmentName unique(CompanyId,DepartmentName);

alter table tbl_User
add constraint IX_UQ_UserName unique(UserName,DepartmentId,IsCurrentUsed);

alter table tbl_AccountingEntity
add constraint IX_UQ_AccountingEntityCode unique(AccountingEntityCode);

alter table tbl_AccountingEntity
add constraint IX_UQ_AccountingEntityName unique(AccountingEntityName);

alter table tbl_Project
add constraint IX_UQ_ProjectCode unique(ProjectCode);

alter table tbl_Project
add constraint IX_UQ_ProjectName unique(ProjectName);

alter table tbl_Project
add constraint IX_UQ_ProjectFullName unique(ProjectFullName);

alter table tbl_ProjectProfession
add constraint IX_UQ_ProjectIdProfession unique(ProjectId,Profession);

alter table tbl_Area
add constraint IX_UQ_AreaCode unique(AreaCode);

alter table tbl_Area
add constraint IX_UQ_AreaName unique(AreaName);

alter table tbl_Reseau
add constraint IX_UQ_AreaIdReseauCode unique(AreaId,ReseauCode);

alter table tbl_Reseau
add constraint IX_UQ_AreaIdReseauName unique(AreaId,ReseauName);

alter table tbl_PlaceCategory
add constraint IX_UQ_ProfessionPlaceCategoryCode unique(Profession,PlaceCategoryCode);

alter table tbl_PlaceCategory
add constraint IX_UQ_ProfessionPlaceCategoryName unique(Profession,PlaceCategoryName);

alter table tbl_Scene
add constraint IX_UQ_SceneCode unique(SceneCode);

alter table tbl_Scene
add constraint IX_UQ_SceneName unique(SceneName);

alter table tbl_Place
add constraint IX_UQ_PlaceCode unique(PlaceCode);

alter table tbl_Place
add constraint IX_UQ_PlaceName unique(PlaceName);

alter table tbl_Place
add constraint IX_UQ_GroupPlaceCode unique(GroupPlaceCode);

alter table tbl_CodeSeed
add constraint IX_UQ_EntityName unique(EntityName);

alter table tbl_OrderCodeSeed
add constraint IX_UQ_Seed unique(Seed);

alter table tbl_Unit
add constraint IX_UQ_UnitName unique(UnitName);

alter table tbl_MaterialCategory
add constraint IX_UQ_MaterialCategoryCode unique(MaterialCategoryCode);

alter table tbl_MaterialCategory
add constraint IX_UQ_MaterialCategoryName unique(MaterialCategoryName);

alter table tbl_Material
add constraint IX_UQ_MaterialCode unique(MaterialCategoryId,MaterialCode);

alter table tbl_Material
add constraint IX_UQ_MaterialName unique(MaterialCategoryId,MaterialName);

alter table tbl_MaterialSpec
add constraint IX_UQ_MaterialSpecCode unique(MaterialId,MaterialSpecCode);

alter table tbl_MaterialSpec
add constraint IX_UQ_MaterialSpecName unique(MaterialId,MaterialSpecName);

alter table tbl_Customer
add constraint IX_UQ_CustomerCode unique(CustomerCode);

alter table tbl_Customer
add constraint IX_UQ_CustomerName unique(CustomerName);

alter table tbl_Customer
add constraint IX_UQ_CustomerFullName unique(CustomerFullName);

alter table tbl_WorkBigClass
add constraint IX_UQ_BigClassCode unique(BigClassCode);

alter table tbl_WorkBigClass
add constraint IX_UQ_BigClassName unique(BigClassName);

alter table tbl_WorkSmallClass
add constraint IX_UQ_WorkBigClassIdSmallClassCode unique(WorkBigClassId,SmallClassCode);

alter table tbl_WorkSmallClass
add constraint IX_UQ_WorkBigClassIdSmallClassName unique(WorkBigClassId,SmallClassName);

alter table tbl_CustomerUser
add constraint IX_UQ_CustomerIdUserId unique(UserId);

----BMMgmt
alter table tbl_OperatorsPlanning
add constraint IX_UQ_OperatorsPlanningCode unique(PlanningCode);

alter table tbl_OperatorsPlanning
add constraint IX_UQ_CompanyIdOperatorsPlanningName unique(CompanyId,PlanningName);

alter table tbl_Planning
add constraint IX_UQ_PlanningCode unique(PlanningCode);

alter table tbl_Addressing
add constraint IX_UQ_PlanningId unique(PlanningId);

alter table tbl_OperatorsConfirmDetail
add constraint IX_UQ_OperatorsConfirmIdPlanningId unique(OperatorsConfirmId,PlanningId);

alter table tbl_OperatorsPlanningDemand
add constraint IX_UQ_OperatorsPlanningIdPlaceId unique(OperatorsPlanningId,PlaceId);

alter table tbl_Tower
add constraint IX_UQ_TowerParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_TowerBase
add constraint IX_UQ_TowerBaseParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_MachineRoom
add constraint IX_UQ_MachineRoomParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_ExternalElectricPower
add constraint IX_UQ_ExternalElectricPowerParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_EquipmentInstall
add constraint IX_UQ_EquipmentInstallParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_AddressExplor
add constraint IX_UQ_AddressExplorParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_FoundationTest
add constraint IX_UQ_FoundationTestParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_PlaceDesign
add constraint IX_UQ_PlacedesignParentIdPropertyType unique(ParentId,PropertyType);

alter table tbl_MaterialList
add constraint IX_UQ_MaterialListParentIdPropertyTypeMaterialIdMaterialSpecId unique(ParentId,PropertyType,MaterialId,MaterialSpecId);

alter table tbl_TaskProperty
add constraint IX_UQ_TaskPropertyParentIdTaskModel unique(ParentId,TaskModel);

alter table tbl_WorkApply
add constraint IX_UQ_WorkApplyTitle unique(Title)

--alter table tbl_WorkOrder
--add constraint IX_UQ_WorkOrderTitle unique(Title)

alter table tbl_DelayApply
add constraint IX_UQ_DelayApplyTitle unique(Title)

alter table tbl_ProjectCodeList
add constraint IX_UQ_ProjectCodeListProjectCode unique(ProjectCode)

----FileMgmt
alter table tbl_FileAssociation
add constraint IX_UQ_EntityNameEntityId unique(EntityName,EntityId);

----WorkFlow
alter table tbl_WFCategory
add constraint IX_UQ_WFCategoryCode unique(WFCategoryCode);

alter table tbl_WFCategory
add constraint IX_UQ_WFCategoryName unique(WFCategoryName);

alter table tbl_WFCategory
add constraint IX_UQ_WFCategoryEntityName unique(EntityName);

alter table tbl_WFCategory
add constraint IX_UQ_CodePrefix unique(CodePrefix);

alter table tbl_WFProcess
add constraint IX_UQ_WFProcessCode unique(WFProcessCode);

alter table tbl_WFProcess
add constraint IX_UQ_WFProcessName unique(WFProcessName);

--alter table tbl_WFActivityEditor
--add constraint IX_UQ_WFActivityEditorCode unique(WFActivityEditorCode);

--alter table tbl_WFActivityEditor
--add constraint IX_UQ_WFActivityEditorName unique(WFActivityEditorName);

alter table tbl_WFProcessInstance
add constraint IX_UQ_WFProcessInstanceCode unique(WFProcessInstanceCode);

alter table tbl_WFProcessInstance
add constraint IX_UQ_WFProcessInstance_EntityId unique(EntityId);

--添加初始数据：公司，部门，用户，角色，角色用户
declare @CompanyId uniqueidentifier,
		@DepartmentId uniqueidentifier,
		@UserId uniqueidentifier,
		@RoleId uniqueidentifier

insert tbl_Company
	values('9D4A4487-2AD6-4C19-8633-00742E8F1D28','01','苏州铁塔','中国铁塔苏州市分公司','TT',1,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_Company
	values('6365F3DE-0FC5-4930-A321-2350EE6269BB','02','苏州移动','中国移动苏州市分公司','YD',2,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_Company
	values('2E0FFE5F-C03A-4767-9915-9683F0DB0B53','03','苏州电信','中国电信苏州市分公司','DX',2,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_Company
	values('0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600','04','苏州联通','中国联通苏州市分公司','LT',2,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @CompanyId = '9D4A4487-2AD6-4C19-8633-00742E8F1D28';
set @DepartmentId = NEWID();
insert tbl_Department
	values(@DepartmentId,@CompanyId,'01','领导办公室',null,'',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @UserId = NEWID();
insert tbl_User
	values(@UserId,@DepartmentId,'admin','jlxwzZRPPZzeLQo1OBa3Ng==','系统管理员','admin@163.com','120',1,1,NEWID(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @RoleId = NEWID();
insert tbl_Role
	values(@RoleId,'01','系统管理员','',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_RoleUser
	values(NEWID(),@RoleId,@UserId,'00000000-0000-0000-0000-000000000000',GETDATE());

/*
--初始化运营商部门
--移动公司
set @CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB';
set @DepartmentId = NEWID();
insert tbl_Department
	values(@DepartmentId,@CompanyId,'01','移动网建部',null,'',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @UserId = NEWID();
insert tbl_User
	values(@UserId,@DepartmentId,'wjs','jlxwzZRPPZzeLQo1OBa3Ng==','王皆顺','','',1,1,NEWID(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_RoleUser
	values(NEWID(),@RoleId,@UserId,'00000000-0000-0000-0000-000000000000',GETDATE());

--电信公司
set @CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53';
set @DepartmentId = NEWID();
insert tbl_Department
	values(@DepartmentId,@CompanyId,'01','电信网建部',null,'',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @UserId = NEWID();
insert tbl_User
	values(@UserId,@DepartmentId,'zzd','jlxwzZRPPZzeLQo1OBa3Ng==','张哲栋','','',1,1,NEWID(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_RoleUser
	values(NEWID(),@RoleId,@UserId,'00000000-0000-0000-0000-000000000000',GETDATE());

--联通公司
set @CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600';
set @DepartmentId = NEWID();
insert tbl_Department
	values(@DepartmentId,@CompanyId,'01','联通网建部',null,'',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @UserId = NEWID();
insert tbl_User
	values(@UserId,@DepartmentId,'zjh','jlxwzZRPPZzeLQo1OBa3Ng==','周菊华','','',1,1,NEWID(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_RoleUser
	values(NEWID(),@RoleId,@UserId,'00000000-0000-0000-0000-000000000000',GETDATE());
--初始化结束
*/

--添加菜单
declare @MenuId uniqueidentifier,
		@MenuSubId uniqueidentifier;

----基础数据
set @MenuId = NEWID();
insert tbl_Menu values(@MenuId,'基础数据',1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'组织机构',1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司','BaseData/Company',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'部门','BaseData/Department',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'岗位','BaseData/Post',3,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'用户管理',2,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'用户账号','BaseData/UserAccount',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'用户信息','BaseData/UserInfo',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'角色用户','BaseData/RoleUser',3,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'岗位用户','BaseData/PostUser',4,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'部门调动','BaseData/UserDepartmentChange',5,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'角色权限',3,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'角色','BaseData/Role',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'角色菜单','BaseData/RoleMenuItem',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'项目管理',4,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'会计主体','BaseData/AccountingEntity',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目信息','BaseData/Project',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'站点管理',5,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域','BaseData/Area',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格','BaseData/Reseau',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'周边场景','BaseData/Scene',3,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点类型','BaseData/PlaceCategory',4,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点信息','BaseData/Place',5,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId=NEWID()
insert tbl_MenuSub values(@MenuSubId,@MenuId,'物资管理',6,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'计量单位','BaseData/Unit',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'物资类别','BaseData/MaterialCategory',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'物资名称','BaseData/Material',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'设计规格','BaseData/MaterialSpec',4,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'物资申购','BaseData/MaterialPurchase',5,1,'00000000-0000-0000-0000-000000000000',GETDATE())
set @MenuSubId=NEWID()
insert tbl_MenuSub values(@MenuSubId,@MenuId,'往来单位',7,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'往来单位','BaseData/Customer',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'往来单位用户','BaseData/CustomerUser',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
set @MenuSubId=NEWID()
insert tbl_MenuSub values(@MenuSubId,@MenuId,'工单管理',8,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工单大类','BaseData/WorkBigClass',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工单小类','BaseData/WorkSmallClass',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())

----业务流程
set @MenuId = NEWID();
insert tbl_Menu values(@MenuId,'业务流程',2,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'流程管理',1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'流程定义','WorkFlow/WFProcess',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'流程步骤','WorkFlow/WFActivity',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'公文管理',2,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'我的公文','WorkFlow/MyWFInstance',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公文查询','WorkFlow/WFInstanceQuery',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());

----基站建维
set @MenuId = NEWID();
insert tbl_Menu values(@MenuId,'基站建维',3,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'规划寻址',1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'运营商基站规划','BaseStationBM/OperatorsPlanning',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站规划','BaseStationBM/Planning',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'运营商需求确认','BaseStationBM/OperatorsConfirm',3,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'改造站需求确认','BaseStationBM/OperatorsPlanningDemand',4,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'寻址确认','BaseStationBM/Addressing',5,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'运营商共享基站','BaseStationBM/OperatorsSharing',6,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'共享基站汇总','BaseStationBM/ShareSummary',7,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'新增基站','BaseStationBM/NewPlanning',8,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'改造基站','BaseStationBM/NewRemodeling',9,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'施工管理',2,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'新增安装登记','BaseStationBM/RegisterPlanning',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'改造安装登记','BaseStationBM/RegisterRemodeing',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目管理','BaseStationBM/ProjectManagement',3,1,'00000000-0000-0000-0000-000000000000',getdate())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程管理','BaseStationBM/EngineeringManagement',4,1,'00000000-0000-0000-0000-000000000000',getdate())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目设置','BaseStationBM/ProjectSetting',5,1,'00000000-0000-0000-0000-000000000000',getdate())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程设置','BaseStationBM/EngineeringSetting',6,1,'00000000-0000-0000-0000-000000000000',getdate())

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'维护管理',3,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点导入','BaseStationBM/Purchase',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'资源导入','BaseStationBM/ResourceImport',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点维护','BaseStationBM/PlaceMaintenance',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'基站报表',4,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站清单','BaseStationBM/PlaceReport',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'运营商规划清单','BaseStationBM/OperatorsPlanningReport',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'新增基站建设进度表','BaseStationBM/ConstructionPlanningReport',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'改造基站建设进度表','BaseStationBM/ConstructionRemodeingReport',4,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目信息表','BaseStationBM/ProjectInformation',5,1,'00000000-0000-0000-0000-000000000000',GETDATE())

----零星用工
set @MenuId = NEWID();
insert tbl_Menu values(@MenuId,'零星用工',4,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'零星用工',1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'隐患上报','BaseStationBM/WorkApply',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'零星派工','BaseStationBM/WorkOrder',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'派工报表',2,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'隐患上报清单','BaseStationBM/WorkApplyReport',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'零星派工清单','BaseStationBM/WorkOrderReport',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'隐患立项清单','BaseStationBM/WorkApplyProjectReport',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())

----物资管理
set @MenuId = NEWID();
insert tbl_Menu values(@MenuId,'物资管理',5,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'物资管理',1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'导入立项信息','BaseStationBM/ImportProjectCodeList',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'导入采购清单','BaseStationBM/ImportMaterialSpecList',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'导出清单','BaseStationBM/ExportProjectMaterial',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())

--为系统管理员角色添加所有菜单
insert tbl_RoleMenuItem
	select NEWID(),@RoleId,tbl_MenuItem.Id,'00000000-0000-0000-0000-000000000000',GETDATE() from tbl_MenuItem;

--初始化编码种子
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Place',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'OperatorsPlanning',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Planning',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Customer',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());

--初始化单据编码种子
declare @Index int,
		@Seed varchar(5)
set @Index = 1;
while @Index <= 9999
begin
	set @Seed = SUBSTRING('00000',1,5 - LEN(CAST(@Index as varchar(5)))) + CAST(@Index as varchar(5));
	insert tbl_OrderCodeSeed values(NEWID(),@Seed);
	set @Index = @Index + 1;
end

--初始化工作流类型
insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],CreateDate)
	values('959026E9-C525-4D94-B54C-EC508C933181','01','寻址确认','Addressing','XZQR','PrintPage/Addressing','1,2',1,GETDATE())
--insert tbl_WFActivityEditor values(NEWID(),'959026E9-C525-4D94-B54C-EC508C933181','01','寻址确认编辑','BaseStationBM/AddressingEdit',1,1,GETDATE())
insert tbl_WFActivityEditor values('3BF652D2-5EEE-4A3A-B0FE-E03EED01A5B3','959026E9-C525-4D94-B54C-EC508C933181','01','指定设计单位','BaseStationBM/AppointDesign',1,1,GETDATE())
insert tbl_WFActivityEditor values('90C81C32-B84E-46ED-9041-A00CB9B2C04E','959026E9-C525-4D94-B54C-EC508C933181','02','指定设计人员','BaseStationBM/AppointDesignUser',1,1,GETDATE())
insert tbl_WFActivityEditor values('97F9AAE8-BAE1-4FB4-A5D9-061BCA9831E4','959026E9-C525-4D94-B54C-EC508C933181','03','施工设计','BaseStationBM/ConstructionDesign',1,1,GETDATE())
insert tbl_WFActivityEditor values('407D12A4-EDF1-4F3F-8D33-112B34017B6E','959026E9-C525-4D94-B54C-EC508C933181','04','运营商确认','BaseStationBM/OperatorConfirm',1,1,GETDATE())
insert tbl_WFActivityEditor values('5BED6799-0BED-48EA-8B19-13778154CC8D','959026E9-C525-4D94-B54C-EC508C933181','05','指定项目及站点编码','BaseStationBM/AppointProjectAndPlaceCode',1,1,GETDATE())
insert tbl_WFActivityEditor values('1EDF9EDD-F8AE-44D8-B442-67B69B20B47D','959026E9-C525-4D94-B54C-EC508C933181','06','指定施工单位及设计规格','BaseStationBM/AppointCustomer',1,1,GETDATE())
insert tbl_WFActivityEditor values('078548EC-FFB8-4DF4-8974-45D20B97604B','959026E9-C525-4D94-B54C-EC508C933181','07','指定项目经理及监理单位','BaseStationBM/AppointManagerAndSupervisor',1,1,GETDATE())

insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],CreateDate)
	values('ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','02','改造确认','Remodeling','GZQR','PrintPage/Remodeling','1,2',1,GETDATE())
insert tbl_WFActivityEditor values('27C1E8FF-C396-411B-A827-BB51C8F88560','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','01','指定设计单位','BaseStationBM/AppointDesignR',1,1,GETDATE())
insert tbl_WFActivityEditor values('95154645-69F3-4C49-95B1-DC77B8C4C962','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','02','指定设计人员','BaseStationBM/AppointDesignUserR',1,1,GETDATE())
insert tbl_WFActivityEditor values('F11ADB5A-ED98-4320-80B1-D000F60C9BCF','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','03','施工设计','BaseStationBM/ConstructionDesignR',1,1,GETDATE())
insert tbl_WFActivityEditor values('2C3C8E36-D976-49AE-9A27-FB15869A1119','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','04','指定项目','BaseStationBM/AppointProjectR',1,1,GETDATE())
insert tbl_WFActivityEditor values('23EC8B54-3973-4753-B8A0-D977B89D3ABE','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','05','指定施工单位及设计规格','BaseStationBM/AppointCustomerR',1,1,GETDATE())
insert tbl_WFActivityEditor values('DD0BD5C9-83FE-4DD4-AEAD-6A7A4CDA5349','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','06','指定项目经理及监理单位','BaseStationBM/AppointManagerAndSupervisorR',1,1,GETDATE())

--insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],CreateDate)
--	values('0BFD8077-80C8-43F2-92A7-DAAE3BC673AE','03','隐患上报','WorkApply','YHSB','PrintPage/WorkApply','1,2',1,GETDATE())

	
insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],CreateDate)
	values('DCC1B95B-70FB-4CD7-8C48-FC70C9BCCDC5','04','零星派工','WorkOrder','LXPG','PrintPage/WorkOrder','1,2',1,GETDATE())
insert tbl_WFActivityEditor values('631CA9B8-FEA5-4381-80DF-B136C15361E4','DCC1B95B-70FB-4CD7-8C48-FC70C9BCCDC5','01','登记结算','BaseStationBM/WorkSettlement',1,1,GETDATE())

insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],CreateDate)
	values('B4D03EB0-E277-4733-82F1-0FCE97BF09F8','03','工期延误','DelayApply','GQYW','PrintPage/DelayApply','1,2',1,GETDATE())

commit tran DataBaseInit