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
add constraint IX_UQ_UserName unique(UserName);

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

alter table tbl_WFActivityEditor
add constraint IX_UQ_WFActivityEditorCode unique(WFActivityEditorCode);

alter table tbl_WFActivityEditor
add constraint IX_UQ_WFActivityEditorName unique(WFActivityEditorName);

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
	values(@UserId,@DepartmentId,'admin','jlxwzZRPPZzeLQo1OBa3Ng==','系统管理员','admin@163.com','120',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

set @RoleId = NEWID();
insert tbl_Role
	values(@RoleId,'01','系统管理员','',1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',GETDATE(),GETDATE());

insert tbl_RoleUser
	values(NEWID(),@RoleId,@UserId,'00000000-0000-0000-0000-000000000000',GETDATE());

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
insert tbl_MenuItem values(NEWID(),@MenuSubId,'寻址确认','BaseStationBM/Addressing',4,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'购置基站登记','BaseStationBM/Purchase',5,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'运营商共享基站','BaseStationBM/OperatorsSharing',6,1,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_MenuItem values(NEWID(),@MenuSubId,'基站改造安排','BaseStationBM/Remodeling',7,1,'00000000-0000-0000-0000-000000000000',GETDATE());

--为系统管理员角色添加所有菜单
insert tbl_RoleMenuItem
	select NEWID(),@RoleId,tbl_MenuItem.Id,'00000000-0000-0000-0000-000000000000',GETDATE() from tbl_MenuItem;

--初始化编码种子
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Place',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'OperatorsPlanning',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());
insert tbl_CodeSeed(Id,EntityName,Digit,Prefix,Seed,CreateUserId,CreateDate) values(NEWID(),'Planning',5,'',0,'00000000-0000-0000-0000-000000000000',GETDATE());

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

commit tran DataBaseInit