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

alter table tbl_PlaceOwner
add constraint IX_UQ_PlaceOwnerCode unique(PlaceOwnerCode);

alter table tbl_PlaceOwner
add constraint IX_UQ_PlaceOwnerName unique(PlaceOwnerName);

alter table tbl_DutyUser
add constraint IX_UQ_DutyUserId unique(Duty,UserId);

----BMMgmt
alter table tbl_PlanningApply
add constraint IX_UQ_PlanningApplyCode unique(PlanningCode);

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

alter table tbl_PlanningApplyHeader
add constraint IX_UQ_PlanningApplyHeaderTitle unique(Title);

--alter table tbl_WorkOrder
--add constraint IX_UQ_WorkOrderTitle unique(Title)

alter table tbl_DelayApply
add constraint IX_UQ_DelayApplyTitle unique(Title)

alter table tbl_ProjectCodeList
add constraint IX_UQ_ProjectCodeListProjectCode unique(ProjectCode)

alter table tbl_ProjectTask
add constraint IX_UQ_ProjectTaskProjectTypeParentId unique(ProjectType,ParentId);

alter table tbl_EngineeringTask
add constraint IX_UQ_EngineeringTaskTaskModelProjectTaskId unique(TaskModel,ProjectTaskId);

alter table tbl_BusinessVolume
add constraint IX_UQ_BusinessVolumeLogicalTypeLogicalNumberCreateDate unique(LogicalType,LogicalNumber,CreateDate);

alter table tbl_PlaceBusinessVolume
add constraint IX_UQ_PlaceBusinessVolumePlaceIdCreateDate unique(PlaceId,CreateDate);

----FileMgmt
alter table tbl_FileAssociation
add constraint IX_UQ_EntityNameEntityId unique(EntityName,EntityId);

----WorkFlow
alter table tbl_WFCategory
add constraint IX_UQ_WFCategoryCode unique(WFCategoryCode);

alter table tbl_WFCategory
add constraint IX_UQ_WFCategoryName unique(WFCategoryName);

alter table tbl_WFCategory
add constraint IX_UQ_WFCategoryEntityName unique(EntityName,Profession);

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
		@UserId2 uniqueidentifier,
		@RoleId uniqueidentifier

--insert tbl_Company
--	values('9D4A4487-2AD6-4C19-8633-00742E8F1D28','01','苏州铁塔','中国铁塔苏州市分公司','TT',1,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

--insert tbl_Company
--	values('6365F3DE-0FC5-4930-A321-2350EE6269BB','02','苏州移动','中国移动苏州市分公司','YD',2,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

--insert tbl_Company
--	values('2E0FFE5F-C03A-4767-9915-9683F0DB0B53','03','苏州电信','中国电信苏州市分公司','DX',2,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_Company
	values('0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600','SZ','苏州联通','中国联通苏州市分公司','LT',2,1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600';
set @DepartmentId = NEWID();
insert tbl_Department
	values(@DepartmentId,@CompanyId,'01','领导办公室',null,'',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @UserId = NEWID();
insert tbl_User
	values(@UserId,@DepartmentId,'admin','jlxwzZRPPZzeLQo1OBa3Ng==','系统管理员','admin@163.com','120',1,1,NEWID(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());
set @UserId2 = NEWID();
insert tbl_User
	values(@UserId2,@DepartmentId,'pdsoft','jlxwzZRPPZzeLQo1OBa3Ng==','攀登软件','admin@163.com','120',1,1,NEWID(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @RoleId = NEWID();
insert tbl_Role
	values(@RoleId,'01','系统管理员','',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_RoleUser
	values(NEWID(),@RoleId,@UserId,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_RoleUser
	values(NEWID(),@RoleId,@UserId2,'00000000-0000-0000-0000-000000000000',GETDATE());

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
insert tbl_MenuItem values(NEWID(),@MenuSubId,'用户职务','BaseData/DutyUser',6,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'角色权限',3,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'角色','BaseData/Role',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'角色菜单','BaseData/RoleMenuItem',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'站点管理',5,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域','BaseData/Area',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格','BaseData/Reseau',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'产权','BaseData/PlaceOwner',4,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点类型','BaseData/PlaceCategory',5,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点信息','BaseData/Place',6,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId=NEWID()
insert tbl_MenuSub values(@MenuSubId,@MenuId,'往来单位',7,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'往来单位','BaseData/Customer',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())

set @MenuSubId=NEWID()
insert tbl_MenuSub values(@MenuSubId,@MenuId,'盲点管理',8,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'盲点反馈','BaseStationBM/BlindSpotFeedBack',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'盲点处理','BaseStationBM/BlindSpotHanding',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId=NEWID()
insert tbl_MenuSub values(@MenuSubId,@MenuId,'移动业务',9,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点业务清单','BaseDataReport/BusinessVolume',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格业务清单','BaseDataReport/BusinessVolumeReseau',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域业务清单','BaseDataReport/BusinessVolumeArea',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司业务清单','BaseDataReport/BusinessVolumeCompany',4,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'站点业务月清单','BaseDataReport/BusinessVolumeMonthPlace',5,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格业务月清单','BaseDataReport/BusinessVolumeMonthReseau',6,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域业务月清单','BaseDataReport/BusinessVolumeMonthArea',7,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司业务月清单','BaseDataReport/BusinessVolumeMonthCompany',8,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格年度成长报表','BaseDataReport/BusinessVolumeYearGrowthReseau',9,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域年度成长报表','BaseDataReport/BusinessVolumeYearGrowthArea',10,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司年度成长报表','BaseDataReport/BusinessVolumeYearGrowthCompany',11,1,'00000000-0000-0000-0000-000000000000',GETDATE())

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
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站建设申请','BaseStationBM/PlanningApply',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'待处理建设申请','BaseStationBM/PlanningApplyWait',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站规划','BaseStationBM/Planning',3,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁任务分配','BaseStationBM/AddressingUser',4,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'寻址确认','BaseStationBM/Addressing',5,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站改造','BaseStationBM/Remodeling',6,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'规划调整通知','BaseStationBM/Notice',7,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'施工管理',2,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'任务分配','BaseStationBM/ProjectDesign',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目设计','BaseStationBM/DesignDrawing',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'施工设计','BaseStationBM/EngineeringDesign',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目进度登记','BaseStationBM/ProjectProgress',4,1,'00000000-0000-0000-0000-000000000000',getdate())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程进度登记','BaseStationBM/EngineeringProgress',5,1,'00000000-0000-0000-0000-000000000000',getdate())

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'维护管理',3,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站导入','BaseStationBM/PlaceImport',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'登记逻辑号','BaseStationBM/LogicalNumber',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'业务量导入','BaseStationBM/BusinessVolume',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'基站报表',4,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站清单','BaseStationReport/PlaceReport',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁进度表','BaseStationReport/Addressing',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目进度表','BaseStationReport/ProjectProgress',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程进度表','BaseStationReport/EngineeringProgress',4,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目设计清单','BaseStationReport/ProjectDesign',5,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程设计清单','BaseStationReport/EngineeringDesign',6,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'逻辑号业务清单','BaseStationReport/LogicalBusinessVolume',7,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站业务清单','BaseStationReport/BusinessVolume',8,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格业务清单','BaseStationReport/BusinessVolumeReseau',9,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域业务清单','BaseStationReport/BusinessVolumeArea',10,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司业务清单','BaseStationReport/BusinessVolumeCompany',11,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站业务月清单','BaseStationReport/BusinessVolumeMonthPlace',12,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格业务月清单','BaseStationReport/BusinessVolumeMonthReseau',13,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域业务月清单','BaseStationReport/BusinessVolumeMonthArea',14,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司业务月清单','BaseStationReport/BusinessVolumeMonthCompany',15,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格年度成长报表','BaseStationReport/BusinessVolumeYearGrowthReseau',16,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域年度成长报表','BaseStationReport/BusinessVolumeYearGrowthArea',17,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司年度成长报表','BaseStationReport/BusinessVolumeYearGrowthCompany',18,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目经理月报','BaseStationReport/ProjectTaskProjectManager',19,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'部门建设月报','BaseStationReport/ProjectTaskDepartment',20,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁月报','BaseStationReport/AddressingMonthReseau',21,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁人月报','BaseStationReport/AddressingMonthUser',22,1,'00000000-0000-0000-0000-000000000000',GETDATE())

----室分建维
set @MenuId = NEWID();
insert tbl_Menu values(@MenuId,'室分建维',5,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'规划寻址',1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'室分建设申请','IndoorDistributionBM/PlanningApply',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'待处理建设申请','IndoorDistributionBM/PlanningApplyWait',2,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'室分规划','IndoorDistributionBM/Planning',3,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁任务分配','IndoorDistributionBM/AddressingUser',4,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'寻址确认','IndoorDistributionBM/Addressing',5,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'室分改造','IndoorDistributionBM/Remodeling',6,1,'00000000-0000-0000-0000-000000000000',GETDATE());

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'施工管理',2,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'任务分配','IndoorDistributionBM/ProjectDesign',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目设计','IndoorDistributionBM/DesignDrawing',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'施工设计','IndoorDistributionBM/EngineeringDesign',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目进度登记','IndoorDistributionBM/ProjectProgress',4,1,'00000000-0000-0000-0000-000000000000',getdate())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程进度登记','IndoorDistributionBM/EngineeringProgress',5,1,'00000000-0000-0000-0000-000000000000',getdate())

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'维护管理',3,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'室分导入','IndoorDistributionBM/PlaceImport',1,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'登记逻辑号','IndoorDistributionBM/LogicalNumber',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'业务量导入','IndoorDistributionBM/BusinessVolume',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())

set @MenuSubId = NEWID();
insert tbl_MenuSub values(@MenuSubId,@MenuId,'室分报表',4,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'室分清单','IndoorDistributionReport/PlaceReport',1,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁进度表','IndoorDistributionReport/Addressing',2,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目进度表','IndoorDistributionReport/ProjectProgress',3,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程进度表','IndoorDistributionReport/EngineeringProgress',4,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目设计清单','IndoorDistributionReport/ProjectDesign',5,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'工程设计清单','IndoorDistributionReport/EngineeringDesign',6,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'逻辑号业务清单','IndoorDistributionReport/LogicalBusinessVolume',7,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'室分业务清单','IndoorDistributionReport/BusinessVolume',8,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格业务清单','IndoorDistributionReport/BusinessVolumeReseau',9,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域业务清单','IndoorDistributionReport/BusinessVolumeArea',10,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司业务清单','IndoorDistributionReport/BusinessVolumeCompany',11,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'室分业务月清单','IndoorDistributionReport/BusinessVolumeMonthPlace',12,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格业务月清单','IndoorDistributionReport/BusinessVolumeMonthReseau',13,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域业务月清单','IndoorDistributionReport/BusinessVolumeMonthArea',14,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司业务月清单','IndoorDistributionReport/BusinessVolumeMonthCompany',15,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'网格年度成长报表','IndoorDistributionReport/BusinessVolumeYearGrowthReseau',16,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'区域年度成长报表','IndoorDistributionReport/BusinessVolumeYearGrowthArea',17,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'公司年度成长报表','IndoorDistributionReport/BusinessVolumeYearGrowthCompany',18,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'项目经理月报','IndoorDistributionReport/ProjectTaskProjectManager',19,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'部门建设月报','IndoorDistributionReport/ProjectTaskDepartment',20,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁月报','IndoorDistributionReport/AddressingMonthReseau',21,1,'00000000-0000-0000-0000-000000000000',GETDATE())
insert tbl_MenuItem values(NEWID(),@MenuSubId,'租赁人月报','IndoorDistributionReport/AddressingMonthUser',22,1,'00000000-0000-0000-0000-000000000000',GETDATE())

--为系统管理员角色添加所有菜单
insert tbl_RoleMenuItem
	select NEWID(),@RoleId,tbl_MenuItem.Id,'00000000-0000-0000-0000-000000000000',GETDATE() from tbl_MenuItem;

--初始化编码种子
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Place',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'OperatorsPlanning',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Planning',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Customer',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'PlanningApply',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());

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
--insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],CreateDate)
--	values('02A4562A-FAB2-4EDC-8EBD-0284909E0D78','01','基站建设申请','PlanningApplyHeader','JSSQ','PrintPage/PlanningApply','1,2',1,GETDATE())
----insert tbl_WFActivityEditor values('9BEBABE2-C511-4C1D-BB52-9AA6C8A0BFF5','02A4562A-FAB2-4EDC-8EBD-0284909E0D78','01','业务审核','EditPage/BusinessAudit',1,1,GETDATE())
--insert tbl_WFActivityEditor values('64FE860D-FB1D-464C-9C63-913BFAA853AD','02A4562A-FAB2-4EDC-8EBD-0284909E0D78','01','技术审核','EditPage/TechnicalAudit',1,1,GETDATE())


insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],Profession,CreateDate)
	values('959026E9-C525-4D94-B54C-EC508C933181','01','基站寻址确认','Addressing','JZXZQR','PrintPage/Addressing','1,2',1,1,GETDATE())
insert tbl_WFActivityEditor values('3BF652D2-5EEE-4A3A-B0FE-E03EED01A5B3','959026E9-C525-4D94-B54C-EC508C933181','01','指定项目经理及总设单位','EditPage/AppointAreaAndDesignUser',1,1,GETDATE())
insert tbl_WFActivityEditor values('90C81C32-B84E-46ED-9041-A00CB9B2C04E','959026E9-C525-4D94-B54C-EC508C933181','02','任务分配','EditPage/ProjectDesign',1,1,GETDATE())
insert tbl_WFActivityEditor values('97F9AAE8-BAE1-4FB4-A5D9-061BCA9831E4','959026E9-C525-4D94-B54C-EC508C933181','03','项目设计','EditPage/DesignDrawing',1,1,GETDATE())
insert tbl_WFActivityEditor values('407D12A4-EDF1-4F3F-8D33-112B34017B6E','959026E9-C525-4D94-B54C-EC508C933181','04','登记逻辑号','EditPage/LogicalNumber',1,1,GETDATE())
insert tbl_WFActivityEditor values('5BED6799-0BED-48EA-8B19-13778154CC8D','959026E9-C525-4D94-B54C-EC508C933181','05','项目开通','EditPage/ProjectOpening',1,1,GETDATE())

insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],Profession,CreateDate)
	values('ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','02','基站改造确认','Remodeling','JZGZQR','PrintPage/Remodeling','1,2',1,1,GETDATE())
insert tbl_WFActivityEditor values('27C1E8FF-C396-411B-A827-BB51C8F88560','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','01','指定项目经理及总设单位','EditPage/AppointAreaAndDesignUserR',1,1,GETDATE())
insert tbl_WFActivityEditor values('95154645-69F3-4C49-95B1-DC77B8C4C962','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','02','任务分配','EditPage/ProjectDesignR',1,1,GETDATE())
insert tbl_WFActivityEditor values('F11ADB5A-ED98-4320-80B1-D000F60C9BCF','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','03','项目设计','EditPage/DesignDrawingR',1,1,GETDATE())
insert tbl_WFActivityEditor values('2C3C8E36-D976-49AE-9A27-FB15869A1119','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','04','登记逻辑号','EditPage/LogicalNumberR',1,1,GETDATE())
insert tbl_WFActivityEditor values('23EC8B54-3973-4753-B8A0-D977B89D3ABE','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','05','项目开通','EditPage/ProjectOpeningR',1,1,GETDATE())
insert tbl_WFActivityEditor values('6A2AF586-2C2A-4A36-9D5A-1770982A8E20','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','06','站点状态变更','EditPage/PlaceState',1,1,GETDATE())
insert tbl_WFActivityEditor values('E7280C0B-EC64-4FF7-AFF6-1DDEAE404480','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','07','指定项目经理及总设单位和任务分配','EditPage/AppointAreaAndDesignUserAndProjectDesignR',1,1,GETDATE())
insert tbl_WFActivityEditor values('A76718F8-E3EA-4D34-82D5-DA5AAC74683E','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','08','登记逻辑号及项目开通','EditPage/LogicalNumberAndProjectOpeningR',1,1,GETDATE())
insert tbl_WFActivityEditor values('5373124A-E84B-4667-B9A3-FFDDBF51F2D4','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','09','登记逻辑号及项目开通和站点状态变更','EditPage/LogicalNumberAndProjectOpeningAndPlaceStateR',1,1,GETDATE())

insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],Profession,CreateDate)
	values('7BB8D5D1-1888-44C9-A4D0-1AD6D4016765','04','室分寻址确认','Addressing','SFXZQR','PrintPage/AddressingID','1,2',1,2,GETDATE())
insert tbl_WFActivityEditor values('25807819-4364-4DF4-8B2A-10AD37E96F48','7BB8D5D1-1888-44C9-A4D0-1AD6D4016765','01','指定项目经理及总设单位','EditPage/AppointAreaAndDesignUserID',1,1,GETDATE())
insert tbl_WFActivityEditor values('86216182-9BC6-4968-B88B-94DE709ED2EA','7BB8D5D1-1888-44C9-A4D0-1AD6D4016765','02','任务分配','EditPage/ProjectDesignID',1,1,GETDATE())
insert tbl_WFActivityEditor values('C6F3E188-896D-4551-968A-A2C8B6FBA930','7BB8D5D1-1888-44C9-A4D0-1AD6D4016765','03','项目设计','EditPage/DesignDrawingID',1,1,GETDATE())
insert tbl_WFActivityEditor values('E6E6331D-E12E-4738-8FBB-6F477EBCDDD0','7BB8D5D1-1888-44C9-A4D0-1AD6D4016765','04','登记逻辑号','EditPage/LogicalNumberID',1,1,GETDATE())
insert tbl_WFActivityEditor values('33D9658B-3A77-4996-B448-230A0AAA5554','7BB8D5D1-1888-44C9-A4D0-1AD6D4016765','05','项目开通','EditPage/ProjectOpeningID',1,1,GETDATE())

insert tbl_WFCategory(Id,WFCategoryCode,WFCategoryName,EntityName,CodePrefix,PrintUrl,WFActivityOperateList,[State],Profession,CreateDate)
	values('43BCA49A-82F7-4838-BA79-70B9C6A13682','05','室分改造确认','Remodeling','SFGZQR','PrintPage/RemodelingID','1,2',1,2,GETDATE())
insert tbl_WFActivityEditor values('6C963526-9FFB-4952-B114-55F46589250F','43BCA49A-82F7-4838-BA79-70B9C6A13682','01','指定项目经理及总设单位','EditPage/AppointAreaAndDesignUserIDR',1,1,GETDATE())
insert tbl_WFActivityEditor values('47AA34FB-D043-48F9-AB1F-F68EE5B6A01F','43BCA49A-82F7-4838-BA79-70B9C6A13682','02','任务分配','EditPage/ProjectDesignIDR',1,1,GETDATE())
insert tbl_WFActivityEditor values('9A3469DD-E429-44DD-9CAF-F51CD76FFF15','43BCA49A-82F7-4838-BA79-70B9C6A13682','03','项目设计','EditPage/DesignDrawingIDR',1,1,GETDATE())
insert tbl_WFActivityEditor values('02B8542C-289D-4140-8150-CA112AC448DB','43BCA49A-82F7-4838-BA79-70B9C6A13682','04','登记逻辑号','EditPage/LogicalNumberIDR',1,1,GETDATE())
insert tbl_WFActivityEditor values('9B060738-ADC2-46A3-B6D9-F17D329A8763','43BCA49A-82F7-4838-BA79-70B9C6A13682','05','项目开通','EditPage/ProjectOpeningIDR',1,1,GETDATE())
insert tbl_WFActivityEditor values('F50634C1-F56B-46A8-BDF2-38F3ADBE391E','43BCA49A-82F7-4838-BA79-70B9C6A13682','06','站点状态变更','EditPage/PlaceStateID',1,1,GETDATE())
insert tbl_WFActivityEditor values('B63A5428-093B-49D5-BF6B-661BA540210D','43BCA49A-82F7-4838-BA79-70B9C6A13682','07','指定项目经理及总设单位和任务分配','EditPage/AppointAreaAndDesignUserAndProjectDesignIDR',1,1,GETDATE())
insert tbl_WFActivityEditor values('253AD8B1-14BF-4E72-9D84-0CE9CDF44B69','43BCA49A-82F7-4838-BA79-70B9C6A13682','08','登记逻辑号及项目开通','EditPage/LogicalNumberAndProjectOpeningIDR',1,1,GETDATE())
insert tbl_WFActivityEditor values('11C6D8BE-F115-4B74-BD43-A75A0C1C9BF4','43BCA49A-82F7-4838-BA79-70B9C6A13682','09','登记逻辑号及项目开通和站点状态变更','EditPage/LogicalNumberAndProjectOpeningAndPlaceStateIDR',1,1,GETDATE())

--初始化基础数据

select * from tbl_Customer
insert tbl_Customer values('B0924E23-6EF9-46DD-9AF7-8561E7F81FCE',1,'00001','设计单位1','设计单位1',@UserId,'','','','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Customer values('365A2087-2D5F-4C28-BAA0-11F903BE6437',2,'00002','施工单位1','施工单位1',@UserId,'','','','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Customer values('936FFC31-AE8B-4201-BBB8-5F2C73210FC9',3,'00003','监理单位1','监理单位1',@UserId,'','','','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_PlaceCategory values('F13AF73E-4171-40D5-B885-337240D548BB',1,'01','宏站','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_PlaceCategory values('71B8ADD4-83A5-44D4-A9A6-B3E543D94ADE',1,'02','小站','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_PlaceCategory values('CA00B7E9-5344-4F1F-871D-12C921A32448',2,'01','公众','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_PlaceCategory values('376C25A2-787F-4909-BE72-9107BBADB959',2,'02','校园','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Area values('19B32AA3-E42C-4AC7-945C-14BD2DF7FBC5','01','市区',120.12345,31.12345,'00000000-0000-0000-0000-000000000000','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Area values('41AFCFF3-F234-41B2-9FC0-2864C32981F8','02','园区',120.23456,31.23456,'00000000-0000-0000-0000-000000000000','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Area values('BC371D02-3BD5-4191-9010-1404066FFE18','03','新区',120.34567,31.34567,'00000000-0000-0000-0000-000000000000','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Reseau values('F36AF5CC-1550-4597-9B7A-DA13D92779A3','19B32AA3-E42C-4AC7-945C-14BD2DF7FBC5','01','姑苏','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Reseau values('F435861D-BCE7-4CEC-A803-3546DBC250E8','41AFCFF3-F234-41B2-9FC0-2864C32981F8','01','娄葑','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_Reseau values('F6A06A83-3FB7-4A6A-9CFD-677E280662CA','BC371D02-3BD5-4191-9010-1404066FFE18','01','横塘','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_PlaceOwner values('35B323DD-6902-47BB-8077-4D5841A8A376','01','联通','',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE())
insert tbl_PlaceOwner values('1081D354-2E39-4EE4-8E43-2FF05154300A','02','铁塔','',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE())
--insert tbl_WFProcess values('EF6F5885-15E4-440F-AB02-17F57291632B','02A4562A-FAB2-4EDC-8EBD-0284909E0D78','01','建设申请',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
--insert tbl_WFActivity values('223E31D6-340E-4AE7-A55A-57744089E792','EF6F5885-15E4-440F-AB02-17F57291632B','业务审核',3,'9BEBABE2-C511-4C1D-BB52-9AA6C8A0BFF5',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
--insert tbl_WFActivity values('B7DCA09D-3F1B-436D-8912-52EEB53E6BC7','EF6F5885-15E4-440F-AB02-17F57291632B','技术审核',3,'64FE860D-FB1D-464C-9C63-913BFAA853AD',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('BEF03003-5D61-4ED1-B73D-866FB651A5AB','959026E9-C525-4D94-B54C-EC508C933181','01','基站寻址确认',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('22A36E90-805D-4555-9400-5C20EA375109','BEF03003-5D61-4ED1-B73D-866FB651A5AB','指定区域经理及总设单位',3,'3BF652D2-5EEE-4A3A-B0FE-E03EED01A5B3',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('0366B942-41F5-480C-BFCE-80E0F922ADA1','BEF03003-5D61-4ED1-B73D-866FB651A5AB','任务分配',3,'90C81C32-B84E-46ED-9041-A00CB9B2C04E',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('9334786E-BC48-48AB-90B5-2D0449102542','BEF03003-5D61-4ED1-B73D-866FB651A5AB','项目设计',3,'97F9AAE8-BAE1-4FB4-A5D9-061BCA9831E4',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('31AEC06E-2BB6-4099-89B0-06C45834A6BA','BEF03003-5D61-4ED1-B73D-866FB651A5AB','登记逻辑号',3,'407D12A4-EDF1-4F3F-8D33-112B34017B6E',1,4,4,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('3C9F4146-7996-412C-B4CE-4B197BBCA3A5','BEF03003-5D61-4ED1-B73D-866FB651A5AB','项目开通',3,'5BED6799-0BED-48EA-8B19-13778154CC8D',1,4,5,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('6382BBAE-70C3-44CE-A4E3-3EB3D00EACC3','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','02','基站改造确认',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('27B5DA85-87A3-4D61-A60F-AB89498E071B','6382BBAE-70C3-44CE-A4E3-3EB3D00EACC3','指定区域经理及总设单位',3,'27C1E8FF-C396-411B-A827-BB51C8F88560',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('8B7A29B5-B559-4E16-9AAA-AC9F66F18C10','6382BBAE-70C3-44CE-A4E3-3EB3D00EACC3','任务分配',3,'95154645-69F3-4C49-95B1-DC77B8C4C962',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('E4DF39C1-CADB-47A6-B369-D36722871A45','6382BBAE-70C3-44CE-A4E3-3EB3D00EACC3','项目设计',3,'F11ADB5A-ED98-4320-80B1-D000F60C9BCF',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('03ACF4CF-4390-4B3D-A9D3-4F971BCA9C6A','6382BBAE-70C3-44CE-A4E3-3EB3D00EACC3','登记逻辑号',3,'2C3C8E36-D976-49AE-9A27-FB15869A1119',1,4,4,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('212EA27A-CAF5-43A0-83FA-2DE44A0F0892','6382BBAE-70C3-44CE-A4E3-3EB3D00EACC3','项目开通',3,'23EC8B54-3973-4753-B8A0-D977B89D3ABE',1,5,5,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('AAAD6096-FE1F-4D09-8F68-6618868BDBF0','6382BBAE-70C3-44CE-A4E3-3EB3D00EACC3','站点状态变更',3,'6A2AF586-2C2A-4A36-9D5A-1770982A8E20',1,6,6,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('36DE12FC-ABA5-4F1B-BAAA-1CEEA742D765','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','03','基站改造_扩容',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('81EA10B8-49DE-47E8-A5F0-7623E195F730','36DE12FC-ABA5-4F1B-BAAA-1CEEA742D765','指定责任人',3,'E7280C0B-EC64-4FF7-AFF6-1DDEAE404480',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('AE6072B3-58D8-4573-A808-225220B30D92','36DE12FC-ABA5-4F1B-BAAA-1CEEA742D765','项目设计',3,'F11ADB5A-ED98-4320-80B1-D000F60C9BCF',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('0AFA9F5C-FB6E-4B04-BF95-3D8357B8C2DB','36DE12FC-ABA5-4F1B-BAAA-1CEEA742D765','登记逻辑号',3,'2C3C8E36-D976-49AE-9A27-FB15869A1119',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('E23B6580-7484-49D3-9712-131848B26F7B','36DE12FC-ABA5-4F1B-BAAA-1CEEA742D765','项目开通',3,'23EC8B54-3973-4753-B8A0-D977B89D3ABE',1,4,4,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('BDA8FADB-65E2-4302-AA0B-8383BBB359D1','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','04','基站改造_部分拆除',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('F418C3E7-97E6-4D90-ACCD-F8F12A64DF6F','BDA8FADB-65E2-4302-AA0B-8383BBB359D1','指定责任人',3,'E7280C0B-EC64-4FF7-AFF6-1DDEAE404480',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('5889FDBF-5021-4BEA-ADAE-233824D32CC8','BDA8FADB-65E2-4302-AA0B-8383BBB359D1','项目设计',3,'F11ADB5A-ED98-4320-80B1-D000F60C9BCF',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('E77ACFAB-8BA6-4493-9C95-85EE3DB83CFC','BDA8FADB-65E2-4302-AA0B-8383BBB359D1','资源调整',3,'A76718F8-E3EA-4D34-82D5-DA5AAC74683E',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('A359584B-48BF-478A-BEA6-ECF66FE15700','ABDAED6E-6D03-4553-9A66-76E92E8FDCEC','05','基站改造_全部拆除',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('4AF760E1-C843-4823-AAE6-7FCCE2268118','A359584B-48BF-478A-BEA6-ECF66FE15700','指定责任人',3,'E7280C0B-EC64-4FF7-AFF6-1DDEAE404480',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('D8762E07-206B-4D13-9910-67AB81FF0EC5','A359584B-48BF-478A-BEA6-ECF66FE15700','资源调整',3,'5373124A-E84B-4667-B9A3-FFDDBF51F2D4',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())

insert tbl_WFProcess values('889DB02F-58C1-4B8C-BF59-AB50E55EE2FB','7BB8D5D1-1888-44C9-A4D0-1AD6D4016765','06','室分寻址确认',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('98D55252-722C-4BBF-A00D-E570AD9981C2','889DB02F-58C1-4B8C-BF59-AB50E55EE2FB','指定区域经理及总设单位',3,'25807819-4364-4DF4-8B2A-10AD37E96F48',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('AFF31B2C-2879-4007-B41A-1C0F02C45E05','889DB02F-58C1-4B8C-BF59-AB50E55EE2FB','任务分配',3,'86216182-9BC6-4968-B88B-94DE709ED2EA',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('55F54E59-7284-4B00-9D6C-7BBB40235FC5','889DB02F-58C1-4B8C-BF59-AB50E55EE2FB','项目设计',3,'C6F3E188-896D-4551-968A-A2C8B6FBA930',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('B21BD525-2F7B-41CD-A83B-FD105C55BD8F','889DB02F-58C1-4B8C-BF59-AB50E55EE2FB','登记逻辑号',3,'E6E6331D-E12E-4738-8FBB-6F477EBCDDD0',1,4,4,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('FC269CAA-08EC-4B9D-A573-1630D71CE943','889DB02F-58C1-4B8C-BF59-AB50E55EE2FB','项目开通',3,'33D9658B-3A77-4996-B448-230A0AAA5554',1,4,5,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('2A117C94-BBF9-45D4-8EBE-9D4767438523','43BCA49A-82F7-4838-BA79-70B9C6A13682','07','室分改造确认',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('FA16C198-3221-4436-AE27-9719BAC4E933','2A117C94-BBF9-45D4-8EBE-9D4767438523','指定区域经理及总设单位',3,'6C963526-9FFB-4952-B114-55F46589250F',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('D711F8F8-ECCC-45AA-B75C-C6B1DF2128C5','2A117C94-BBF9-45D4-8EBE-9D4767438523','任务分配',3,'47AA34FB-D043-48F9-AB1F-F68EE5B6A01F',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('21F82542-06EA-47E5-9C1B-551B8FC4EE3E','2A117C94-BBF9-45D4-8EBE-9D4767438523','项目设计',3,'9A3469DD-E429-44DD-9CAF-F51CD76FFF15',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('21E56396-150F-4137-A529-2294FD94C0D8','2A117C94-BBF9-45D4-8EBE-9D4767438523','登记逻辑号',3,'02B8542C-289D-4140-8150-CA112AC448DB',1,4,4,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('8310998C-1765-4599-B6A2-B9654149207E','2A117C94-BBF9-45D4-8EBE-9D4767438523','项目开通',3,'9B060738-ADC2-46A3-B6D9-F17D329A8763',1,5,5,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('B73AD893-C6F9-4DF3-858E-620F2B4D53CA','2A117C94-BBF9-45D4-8EBE-9D4767438523','站点状态变更',3,'F50634C1-F56B-46A8-BDF2-38F3ADBE391E',1,6,6,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('7351F01D-A271-4797-9785-265050F94108','43BCA49A-82F7-4838-BA79-70B9C6A13682','08','室分改造_扩容',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('BBF40389-00F2-42FE-9004-F243FDCA868D','7351F01D-A271-4797-9785-265050F94108','指定责任人',3,'B63A5428-093B-49D5-BF6B-661BA540210D',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('134CFBE8-7FD4-4E74-AEF4-0E083D7EEB71','7351F01D-A271-4797-9785-265050F94108','项目设计',3,'9A3469DD-E429-44DD-9CAF-F51CD76FFF15',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('2B8A1D9C-689E-4592-8A62-A6476AC8A860','7351F01D-A271-4797-9785-265050F94108','登记逻辑号',3,'02B8542C-289D-4140-8150-CA112AC448DB',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('EDC33989-BFE5-43A3-B3BE-D485FF421761','7351F01D-A271-4797-9785-265050F94108','项目开通',3,'9B060738-ADC2-46A3-B6D9-F17D329A8763',1,4,4,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('2FDE5D3E-2C17-4419-920E-FA9DF21F5D88','43BCA49A-82F7-4838-BA79-70B9C6A13682','09','室分改造_部分拆除',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('57D41D6F-C032-4049-8AD3-531013C7B9C4','2FDE5D3E-2C17-4419-920E-FA9DF21F5D88','指定责任人',3,'6C963526-9FFB-4952-B114-55F46589250F',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('72E44D52-0E54-4E9E-8CC6-C0CD08F7D27C','2FDE5D3E-2C17-4419-920E-FA9DF21F5D88','项目设计',3,'9A3469DD-E429-44DD-9CAF-F51CD76FFF15',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('A06E3548-A7EB-453D-9C7C-443BE42FA55F','2FDE5D3E-2C17-4419-920E-FA9DF21F5D88','资源调整',3,'253AD8B1-14BF-4E72-9D84-0CE9CDF44B69',1,3,3,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFProcess values('F9E9CA60-9EEF-4076-ACB2-F04432671210','43BCA49A-82F7-4838-BA79-70B9C6A13682','10','室分改造_全部拆除',2,'',1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('3657BB07-42B1-4269-B44A-F46BF0A11AEB','F9E9CA60-9EEF-4076-ACB2-F04432671210','指定责任人',3,'B63A5428-093B-49D5-BF6B-661BA540210D',1,1,1,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())
insert tbl_WFActivity values('7C879006-0FDD-4334-908D-DA90E168835D','F9E9CA60-9EEF-4076-ACB2-F04432671210','资源调整',3,'11C6D8BE-F115-4B74-BD43-A75A0C1C9BF4',1,2,2,24,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600',@DepartmentId,@UserId,null,1,1,@UserId,@UserId,GETDATE(),GETDATE())

commit tran DataBaseInit
