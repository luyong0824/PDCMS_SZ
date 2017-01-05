using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models
{
    /// <summary>
    /// 聚合工厂，用于创建聚合
    /// </summary>
    public static class AggregateFactory
    {
        #region BaseData

        /// <summary>
        /// 创建部门实体
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentCode">部门编码</param>
        /// <param name="departmentName">部门名称</param>
        /// <param name="managerUserId">部门经理用户Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">部门状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Department CreateDepartment(Guid companyId, string departmentCode, string departmentName, Guid managerUserId, string remarks, State state, Guid createUserId)
        {
            return new Department(companyId, departmentCode, departmentName, managerUserId, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建用户实体
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="fullName">姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="state">用户状态</param>
        /// <param name="uniqueCode">唯一标识</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static User CreateUser(Guid departmentId, string userName, string fullName, string email, string phoneNumber, State state, Guid uniqueCode, Guid createUserId)
        {
            return new User(departmentId, userName, fullName, email, phoneNumber, state, uniqueCode, createUserId);
        }

        /// <summary>
        /// 创建角色实体
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">角色状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Role CreateRole(string roleCode, string roleName, string remarks, State state, Guid createUserId)
        {
            return new Role(roleCode, roleName, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建角色菜单实体
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuItemId">菜单项Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static RoleMenuItem CreateRoleMenuItem(Guid roleId, Guid menuItemId, Guid createUserId)
        {
            return new RoleMenuItem(roleId, menuItemId, createUserId);
        }

        /// <summary>
        /// 创建角色用户实体
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static RoleUser CreateRoleUser(Guid roleId, Guid userId, Guid createUserId)
        {
            return new RoleUser(roleId, userId, createUserId);
        }

        /// <summary>
        /// 创建岗位实体
        /// </summary>
        /// <param name="postCode">岗位编码</param>
        /// <param name="postName">岗位名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">岗位状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Post CreatePost(string postCode, string postName, string remarks, State state, Guid createUserId)
        {
            return new Post(postCode, postName, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建岗位用户实体
        /// </summary>
        /// <param name="postId">岗位Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static PostUser CreatePostUser(Guid postId, Guid userId, Guid createUserId)
        {
            return new PostUser(postId, userId, createUserId);
        }

        /// <summary>
        /// 创建会计主体实体
        /// </summary>
        /// <param name="accountingEntityCode">会计主体编码</param>
        /// <param name="accountingEntityName">会计主体名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">会计主体状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static AccountingEntity CreateAccountingEntity(string accountingEntityCode, string accountingEntityName, string remarks, State state, Guid createUserId)
        {
            return new AccountingEntity(accountingEntityCode, accountingEntityName, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建项目实体
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="projectName">项目简称</param>
        /// <param name="projectFullName">项目全称</param>
        /// <param name="projectCategory">项目类型</param>
        /// <param name="accountingEntityId">会计主体Id</param>
        /// <param name="managerUserId">分管总经理用户Id</param>
        /// <param name="responsibleUserId">项目负责人用户Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="state">状态</param>
        /// <param name="professionList">所涉专业列表</param>
        /// <param name="projectApplyDate">申请立项时间</param>
        /// <param name="projectDoApplyDate">完成立项时间</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Project CreateProject(string projectCode, string projectName, string projectFullName, ProjectCategory projectCategory,
            Guid? accountingEntityId, Guid? managerUserId, Guid? responsibleUserId, string remarks, ProjectProgress projectProgress,
            State state, string professionList, decimal budgetPrice, DateTime projectApplyDate, DateTime projectDoApplyDate, Guid createUserId)
        {
            return new Project(projectCode, projectName, projectFullName, projectCategory, accountingEntityId, managerUserId, responsibleUserId, remarks, projectProgress, state, professionList, budgetPrice, projectApplyDate, projectDoApplyDate, createUserId);
        }

        /// <summary>
        /// 创建项目专业实体
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="profession">专业</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static ProjectProfession CreateProjectProfession(Guid projectId, Profession profession, Guid createUserId)
        {
            return new ProjectProfession(projectId, profession, createUserId);
        }

        /// <summary>
        /// 创建区域实体
        /// </summary>
        /// <param name="areaCode">区域编码</param>
        /// <param name="areaName">区域名称</param>
        /// <param name="lng">参考经度</param>
        /// <param name="lat">参考纬度</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">区域状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Area CreateArea(string areaCode, string areaName, decimal lng, decimal lat, Guid? areaManagerId, string remarks, State state, Guid createUserId)
        {
            return new Area(areaCode, areaName, lng, lat, areaManagerId, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建网格实体
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauCode">网格编码</param>
        /// <param name="reseauName">网格名称</param>
        /// <param name="reseauManagerId">网格经理</param>
        /// <param name="planningManagerId">规划经理</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">网格状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Reseau CreateReseau(Guid areaId, string reseauCode, string reseauName, Guid? reseauManagerId, Guid? planningManagerId, string remarks, State state, Guid createUserId)
        {
            return new Reseau(areaId, reseauCode, reseauName, reseauManagerId, planningManagerId, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建站点类型实体
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryCode">站点类型编码</param>
        /// <param name="placeCategoryName">站点类型名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">站点类型状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static PlaceCategory CreatePlaceCategory(Profession profession, string placeCategoryCode, string placeCategoryName, string remarks, State state, Guid createUserId)
        {
            return new PlaceCategory(profession, placeCategoryCode, placeCategoryName, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建周边场景实体
        /// </summary>
        /// <param name="sceneCode">周边场景编码</param>
        /// <param name="sceneName">周边场景名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">周边场景状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Scene CreateScene(string sceneCode, string sceneName, string remarks, State state, Guid createUserId)
        {
            return new Scene(sceneCode, sceneName, remarks, state, createUserId);
        }

        /// <summary>
        /// 构造站点实体
        /// </summary>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="addressingDepartmentId">租赁部门</param>
        /// <param name="addressingRealName">租赁人</param>
        /// <param name="ownerName">业主名称</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="placeMapState">站点地图状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Place CreatePlace(string placeCode, string placeName, Profession profession, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat, Guid placeOwner,
            Importance importance, Guid addressingDepartmentId, string addressingRealName, string ownerName, string ownerContact, string ownerPhoneNumber, string detailedAddress,
            string remarks, PlaceMapState placeMapState, Guid createUserId)
        {
            return new Place(placeCode, placeName, profession, placeCategoryId, reseauId, lng, lat, placeOwner, importance, addressingDepartmentId,
                addressingRealName, ownerName, ownerContact, ownerPhoneNumber, detailedAddress, remarks, placeMapState, createUserId);
        }

        /// <summary>
        /// 创建站点运营商共享情况实体
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="mobileShare">移动共享</param>
        /// <param name="mobilePoleNumber">移动抱杆数量</param>
        /// <param name="mobileCabinetNumber">移动机柜数量</param>
        /// <param name="mobilePowerUsed">移动用电量</param>
        /// <param name="mobileCreateUserId">移动创建人用户Id</param>
        /// <param name="telecomShare">电信共享</param>
        /// <param name="telecomPoleNumber">电信抱杆数量</param>
        /// <param name="telecomCabinetNumber">电信机柜数量</param>
        /// <param name="telecomPowerUsed">电信用电量</param>
        /// <param name="telecomCreateUserId">电信创建人用户Id</param>
        /// <param name="unicomShare">联通共享</param>
        /// <param name="unicomPoleNumber">联通抱杆数数量</param>
        /// <param name="unicomCabinetNumber">联通机柜数量</param>
        /// <param name="unicomPowerUsed">联通用电量</param>
        /// <param name="unicomCreateUserId">联通创建人用户Id</param>
        /// <returns></returns>
        public static PlaceProperty CreatePlaceProperty(Guid parentId, PropertyType propertyType, Bool mobileShare, int mobilePoleNumber, int mobileCabinetNumber, decimal mobilePowerUsed, Guid? mobileCreateUserId,
            Bool telecomShare, int telecomPoleNumber, int telecomCabinetNumber, decimal telecomPowerUsed, Guid? telecomCreateUserId, Bool unicomShare, int unicomPoleNumber, int unicomCabinetNumber,
            decimal unicomPowerUsed, Guid? unicomCreateUserId)
        {
            return new PlaceProperty(parentId, propertyType, mobileShare, mobilePoleNumber, mobileCabinetNumber, mobilePowerUsed, mobileCreateUserId, telecomShare, telecomPoleNumber, telecomCabinetNumber, telecomPowerUsed,
                telecomCreateUserId, unicomShare, unicomPoleNumber, unicomCabinetNumber, unicomPowerUsed, unicomCreateUserId);
        }

        /// <summary>
        /// 创建计量单位实体
        /// </summary>
        /// <param name="unitName">单位名称</param>
        /// <param name="state">状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Unit CreateUnit(string unitName, State state, Guid createUserId)
        {
            return new Unit(unitName, state, createUserId);
        }

        /// <summary>
        /// 创建物资类别实体
        /// </summary>
        /// <param name="materialCategoryCode">物资类别编码</param>
        /// <param name="materialCategoryName">物资类别名称</param>
        /// <param name="state">物资类别状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static MaterialCategory CreateMaterialCategory(string materialCategoryCode, string materialCategoryName, State state, Guid createUserId)
        {
            return new MaterialCategory(materialCategoryCode, materialCategoryName, state, createUserId);
        }

        /// <summary>
        /// 创建物资名称实体
        /// </summary>
        /// <param name="materialCode">物资编码</param>
        /// <param name="materialName">物资名称</param>
        /// <param name="materialCategoryId">物资类别Id</param>
        /// <param name="state">物资名称状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Material CreateMaterial(string materialCode, string materialName, Guid materialCategoryId, State state, Guid createUserId)
        {
            return new Material(materialCode, materialName, materialCategoryId, state, createUserId);
        }

        /// <summary>
        /// 创建设计规格实体
        /// </summary>
        /// <param name="materialSpecCode">规格编码</param>
        /// <param name="materialSpecName">规格名称</param>
        /// <param name="materialId">物资名称Id</param>
        /// <param name="unitId">计量单位Id</param>
        /// <param name="price">参考单价</param>
        /// <param name="customerId">供应商Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">规格状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static MaterialSpec CreateMaterialSpec(string materialSpecCode, string materialSpecName, Guid materialId, Guid unitId, decimal price, Guid? customerId, string remarks, State state, Guid createUserId)
        {
            return new MaterialSpec(materialSpecCode, materialSpecName, materialId, unitId, price, customerId, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建往来单位实体
        /// </summary>
        /// <param name="customerType">往来单位分类</param>
        /// <param name="customerCode">往来单位编码</param>
        /// <param name="customerName">往来单位简称</param>
        /// <param name="customerFullName">往来单位全称</param>
        /// <param name="customerUserId">登陆人Id</param>
        /// <param name="contactMan">联系人</param>
        /// <param name="contactTel">联系电话</param>
        /// <param name="contactAddr">联系地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Customer CreateCustomer(CustomerType customerType, string customerCode, string customerName, string customerFullName, Guid customerUserId, string contactMan, string contactTel, string contactAddr, string remarks, State state, Guid createUserId)
        {
            return new Customer(customerType, customerCode, customerName, customerFullName, customerUserId, contactMan, contactTel, contactAddr, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建站点运营商共享情况历史记录实体
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="companyNameId">分公司名称Id</param>
        /// <param name="mobileShare">移动共享</param>
        /// <param name="mobilePoleNumber">移动抱杆数量</param>
        /// <param name="mobileCabinetNumber">移动机柜数量</param>
        /// <param name="mobilePowerUsed">移动用用电量</param>
        /// <param name="mobileCreateUserId">移动创建人用户Id</param>
        /// <param name="telecomShare">电信共享</param>
        /// <param name="telecomPoleNumber">电信抱杆数量</param>
        /// <param name="telecomCabinetNumber">电信机柜数量</param>
        /// <param name="telecomPowerUsed">电信用电量</param>
        /// <param name="telecomCreateUserId">电信创建人用户Id</param>
        /// <param name="unicomShare">联通共享</param>
        /// <param name="unicomPoleNumber">联通抱杆数量</param>
        /// <param name="unicomCabinetNumber">联通机柜数量</param>
        /// <param name="unicomPowerUsed">联通用电量</param>
        /// <param name="unicomCreateUserId">联通创建人用户Id</param>
        /// <returns></returns>
        public static PlacePropertyLog CreatePlacePropertyLog(OperationType operationType, Guid parentId, PropertyType propertyType, CompanyNameId companyNameId, Bool mobileShare, int mobilePoleNumber, int mobileCabinetNumber, decimal mobilePowerUsed, Guid? mobileCreateUserId,
            Bool telecomShare, int telecomPoleNumber, int telecomCabinetNumber, decimal telecomPowerUsed, Guid? telecomCreateUserId, Bool unicomShare, int unicomPoleNumber, int unicomCabinetNumber,
            decimal unicomPowerUsed, Guid? unicomCreateUserId)
        {
            return new PlacePropertyLog(operationType, parentId, propertyType, companyNameId, mobileShare, mobilePoleNumber, mobileCabinetNumber, mobilePowerUsed, mobileCreateUserId, telecomShare, telecomPoleNumber, telecomCabinetNumber, telecomPowerUsed,
                telecomCreateUserId, unicomShare, unicomPoleNumber, unicomCabinetNumber, unicomPowerUsed, unicomCreateUserId);
        }

        /// <summary>
        /// 创建派工大类实体
        /// </summary>
        /// <param name="bigClassCode">大类编码</param>
        /// <param name="bigClassName">大类名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WorkBigClass CreateWorkBigClass(string bigClassCode, string bigClassName, string remarks, State state, Guid createUserId)
        {
            return new WorkBigClass(bigClassCode, bigClassName, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建派工小类实体
        /// </summary>
        /// <param name="bigClassId">派工大类Id</param>
        /// <param name="smallClassCode">小类编码</param>
        /// <param name="smallClassName">小类名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WorkSmallClass CreateWorkSmallClass(Guid workBigClassId, string smallClassCode, string smallClassName, string remarks, State state, Guid createUserId)
        {
            return new WorkSmallClass(workBigClassId, smallClassCode, smallClassName, remarks, state, createUserId);
        }

        /// <summary>
        /// 构造往来单位用户实体
        /// </summary>
        /// <param name="customerId">往来单位Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static CustomerUser CreateCustomerUser(Guid customerId, Guid userId, Guid createUserId)
        {
            return new CustomerUser(customerId, userId, createUserId);
        }

        /// <summary>
        /// 构造产权实体
        /// </summary>
        /// <param name="placeOwnerCode">产权编码</param>
        /// <param name="placeOwnerName">产权名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">产权状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static PlaceOwner CreatePlaceOwner(string placeOwnerCode, string placeOwnerName, string remarks, State state, Guid createUserId)
        {
            return new PlaceOwner(placeOwnerCode, placeOwnerName, remarks, state, createUserId);
        }

        /// <summary>
        /// 构造职务用户
        /// </summary>
        /// <param name="duty">职务</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static DutyUser CreateDutyUser(Duty duty, Guid userId, Guid createUserId)
        {
            return new DutyUser(duty, userId, createUserId);
        }

        #endregion

        #region BMMgmt

        /// <summary>
        /// 构造项目任务实体
        /// </summary>
        /// <param name="projectType">项目类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static ProjectTask CreateProjectTask(ProjectType projectType, Guid parentId, Guid placeId, string projectCode, Guid createUserId)
        {
            return new ProjectTask(projectType, parentId, placeId, projectCode, createUserId);
        }

        /// <summary>
        /// 构造工程任务实体
        /// </summary>
        /// <param name="taskModel">工程名称</param>
        /// <param name="projectTaskId">项目任务Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static EngineeringTask CreateEngineeringTask(TaskModel taskModel, Guid projectTaskId, Guid createUserId)
        {
            return new EngineeringTask(taskModel, projectTaskId, createUserId);
        }

        /// <summary>
        /// 构造建设申请实体
        /// </summary>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">建设理由</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static PlanningApply CreatePlanningApply(string planningCode, string planningName, Profession profession, Guid reseauId, decimal lng, decimal lat,
            Importance importance, string detailedAddress, string remarks, Guid createUserId)
        {
            return new PlanningApply(planningCode, planningName, profession, reseauId, lng, lat, importance, detailedAddress, remarks, createUserId);
        }

        /// <summary>
        /// 构造建设申请单实体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static PlanningApplyHeader CreatePlanningApplyHeader(string title, Guid createUserId)
        {
            return new PlanningApplyHeader(title, createUserId);
        }

        /*--------------------------------------------------分割线--------------------------------------------------*/

        /// <summary>
        /// 创建运营商规划实体
        /// </summary>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="antennaHeight">天线挂高(米)</param>
        /// <param name="poleNumber">抱杆数量(根)</param>
        /// <param name="cabinetNumber">机柜数量(个)</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否采纳</param>
        /// <param name="remarks">备注</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="planningId">基站规划Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="currentCompanyNature">创建人所在公司性质</param>
        /// <returns></returns>
        public static OperatorsPlanning CreateOperatorsPlanning(string planningCode, string planningName, Profession profession, Guid placeCategoryId, Guid areaId, decimal lng, decimal lat,
            decimal antennaHeight, int poleNumber, int cabinetNumber, Urgency urgency, Bool solved, string remarks, Guid companyId, Guid? planningId, Guid createUserId, CompanyNature currentCompanyNature)
        {
            if (currentCompanyNature != CompanyNature.运营商)
            {
                throw new DomainFault("只能由运营商用户进行操作");
            }

            return new OperatorsPlanning(planningCode, planningName, profession, placeCategoryId, areaId, lng, lat, antennaHeight,
                poleNumber, cabinetNumber, urgency, solved, remarks, companyId, planningId, createUserId);
        }

        /// <summary>
        /// 构造规划实体
        /// </summary>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">建设理由</param>
        /// <param name="proposedNetwork">拟建网络</param>
        /// <param name="optionalAddress">可选位置</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Planning CreatePlanning(string planningCode, string planningName, Profession profession, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat,
              string detailedAddress, string remarks, string proposedNetwork, string optionalAddress, Importance importance, Guid placeOwner, Guid createUserId)
        {
            return new Planning(planningCode, planningName, profession, placeCategoryId, reseauId, lng, lat, detailedAddress, remarks, proposedNetwork, optionalAddress,
                importance, placeOwner, createUserId);
        }

        /// <summary>
        /// 构造寻址确认实体
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="addressingDepartmentId">租赁部门Id</param>
        /// <param name="addressingRealName">租赁人</param>
        /// <param name="ownerName">业主</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Addressing CreateAddressing(Guid planningId, string placeName, Guid addressingDepartmentId, string addressingRealName,
            string ownerName, string ownerContact, string ownerPhoneNumber, string remarks, Guid createUserId)
        {
            return new Addressing(planningId, placeName, addressingDepartmentId, addressingRealName, ownerName, ownerContact, ownerPhoneNumber,
                remarks, createUserId);
        }

        /// <summary>
        /// 创建购置站点实体
        /// </summary>
        /// <param name="purchaseDate">购置日期</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="propertyRight">产权</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="sceneId">周边场景Id</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="ownerName">业主名称</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="telecomShare">是否电信共享</param>
        /// <param name="mobileShare">是否移动共享</param>
        /// <param name="unicomShare">是否联通共享</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Purchase CreatePurchase(DateTime purchaseDate, string groupPlaceCode, string placeName, Profession profession, Guid placeCategoryId, Guid reseauId,
            decimal lng, decimal lat, PropertyRight propertyRight, Importance importance, Guid sceneId, string detailedAddress,
            string ownerName, string ownerContact, string ownerPhoneNumber, Bool telecomShare, Bool mobileShare, Bool unicomShare,
            string remarks, Guid createUserId)
        {
            return new Purchase(purchaseDate, groupPlaceCode, placeName, profession, placeCategoryId, reseauId, lng, lat, propertyRight, importance,
                sceneId, detailedAddress, ownerName, ownerContact, ownerPhoneNumber, telecomShare, mobileShare, unicomShare,
                remarks, createUserId);
        }

        /// <summary>
        /// 创建运营商确认实体
        /// </summary>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static OperatorsConfirm CreateOperatorsConfirm(Guid createUserId)
        {
            return new OperatorsConfirm(createUserId);
        }

        /// <summary>
        /// 创建运营商确认明细实体
        /// </summary>
        /// <param name="operatorsConfirmId">运营商确认Id</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="mobileDemand">移动需求确认</param>
        /// <param name="telecomDemand">电信需求确认</param>
        /// <param name="unicomDemand">联通需求确认</param>
        /// <param name="mobileConfirmUserId">移动确认人用户Id</param>
        /// <param name="telecomConfirmUserId">电信确认人用户Id</param>
        /// <param name="unicomConfirmUserId">联通确认人用户Id</param>
        /// <returns></returns>
        public static OperatorsConfirmDetail CreateOperatorsConfirmDetail(Guid operatorsConfirmId, Guid planningId, Demand mobileDemand, Demand telecomDemand, Demand unicomDemand,
            Guid mobileConfirmUserId, Guid telecomConfirmUserId, Guid unicomConfirmUserId, Guid createUserId)
        {
            return new OperatorsConfirmDetail(operatorsConfirmId, planningId, mobileDemand, telecomDemand, unicomDemand, mobileConfirmUserId, telecomConfirmUserId, unicomConfirmUserId, createUserId);
        }

        /// <summary>
        /// 创建运营商共享实体
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="antennaHeight">天线挂高(米)</param>
        /// <param name="poleNumber">抱杆数量(根)</param>
        /// <param name="cabinetNumber">机柜数量(个)</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否采纳</param>
        /// <param name="remarks">备注</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="remodelingId">基站改造Id</param>
        /// <param name="operatorsPlanningDemandId">建议共享需求Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="currentCompanyNature">创建人所在公司性质</param>
        /// <returns></returns>
        public static OperatorsSharing CreateOperatorsSharing(Profession profession, string placeCode, Guid placeId, decimal powerUsed, int poleNumber,
            int cabinetNumber, Urgency urgency, Bool solved, string remarks, Guid companyId, Guid? remodelingId, Guid? operatorsPlanningDemandId, Guid createUserId, CompanyNature currentCompanyNature)
        {
            if (currentCompanyNature != CompanyNature.运营商)
            {
                throw new DomainFault("只能由运营商用户进行操作");
            }

            return new OperatorsSharing(profession, placeCode, placeId, powerUsed, poleNumber, cabinetNumber, urgency, solved, remarks, companyId, remodelingId, operatorsPlanningDemandId, createUserId);
        }

        /// <summary>
        /// 创建改造实体
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="proposedNetwork">拟建网络</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static Remodeling CreateRemodeling(Profession profession, string placeCode, Guid placeId, string proposedNetwork, string remarks, Guid createUserId)
        {
            return new Remodeling(profession, placeCode, placeId, proposedNetwork, remarks, createUserId);
        }

        public static ConstructionTask CreateConstructionTask(ConstructionMethod constructionMethod, Guid placeId, Guid projectId, Guid projectManagerId, Guid supervisorCustomerId, Guid supervisorUserId, EngineeringProgress constructionProgress, string progressMemos)
        {
            return new ConstructionTask(constructionMethod, placeId, projectId, projectManagerId, supervisorCustomerId, supervisorUserId, constructionProgress, progressMemos);
        }

        public static OperatorsPlanningDemand CreateOperatorsPlanningDemand(Guid operatorsPlanningId, Guid placeId, Guid createUserId)
        {
            return new OperatorsPlanningDemand(operatorsPlanningId, placeId, createUserId);
        }

        public static Tower CreateTower(Guid parentId, PropertyType propertyType, TowerType towerType, decimal towerHeight, int platFormNumber, int poleNumber, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new Tower(parentId, propertyType, towerType, towerHeight, platFormNumber, poleNumber, budgetPrice, timeLimit, memos, createUserId);
        }

        public static TowerBase CreateTowerBase(Guid parentId, PropertyType propertyType, TowerBaseType towerBaseType, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new TowerBase(parentId, propertyType, towerBaseType, budgetPrice, timeLimit, memos, createUserId);
        }

        public static MachineRoom CreateMachineRoom(Guid parentId, PropertyType propertyType, MachineRoomType machineRoomType, decimal machineRoomArea, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new MachineRoom(parentId, propertyType, machineRoomType, machineRoomArea, budgetPrice, timeLimit, memos, createUserId);
        }

        public static ExternalElectricPower CreateExternalElectricPower(Guid parentId, PropertyType propertyType, ExternalElectric externalElectric, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new ExternalElectricPower(parentId, propertyType, externalElectric, budgetPrice, timeLimit, memos, createUserId);
        }

        public static EquipmentInstall CreateEquipmentInstall(Guid parentId, PropertyType propertyType, decimal switchPower, decimal battery, int cabinetNumber, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new EquipmentInstall(parentId, propertyType, switchPower, battery, cabinetNumber, budgetPrice, timeLimit, memos, createUserId);
        }

        public static AddressExplor CreateAddressExplor(Guid parentId, PropertyType propertyType, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new AddressExplor(parentId, propertyType, budgetPrice, timeLimit, memos, createUserId);
        }

        public static FoundationTest CreateFoundationTest(Guid parentId, PropertyType propertyType, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new FoundationTest(parentId, propertyType, budgetPrice, timeLimit, memos, createUserId);
        }

        public static PlaceDesign CreatePlaceDesign(Guid parentId, PropertyType propertyType, Guid createUserId)
        {
            return new PlaceDesign(parentId, propertyType, createUserId);
        }

        public static MaterialList CreateMaterialList(Guid parentId, PropertyType propertyType, Guid materialId, Guid materialSpecId, decimal budgetPrice, decimal specNumber, string memos, Guid createUserId)
        {
            return new MaterialList(parentId, propertyType, materialId, materialSpecId, budgetPrice, specNumber, memos, createUserId);
        }

        public static TaskProperty CreateTaskProperty(Guid constructionTaskId, TaskModel taskModel, Guid parentId, Guid constructionCustomerId, Guid supervisorCustomerId, int timeLimit)
        {
            return new TaskProperty(constructionTaskId, taskModel, parentId, constructionCustomerId, supervisorCustomerId, timeLimit);
        }

        public static TowerLog CreateTowerLog(OperationType operationType, Guid parentId, PropertyType propertyType, TowerType towerType, decimal towerHeight, int platFormNumber, int poleNumber, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new TowerLog(operationType, parentId, propertyType, towerType, towerHeight, platFormNumber, poleNumber, budgetPrice, timeLimit, memos, createUserId);
        }

        public static TowerBaseLog CreateTowerBaseLog(OperationType operationType, Guid parentId, PropertyType propertyType, TowerBaseType towerBaseType, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new TowerBaseLog(operationType, parentId, propertyType, towerBaseType, budgetPrice, timeLimit, memos, createUserId);
        }

        public static MachineRoomLog CreateMachineRoomLog(OperationType operationType, Guid parentId, PropertyType propertyType, MachineRoomType machineRoomType, decimal machineRoomArea, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new MachineRoomLog(operationType, parentId, propertyType, machineRoomType, machineRoomArea, budgetPrice, timeLimit, memos, createUserId);
        }

        public static ExternalElectricPowerLog CreateExternalElectricPowerLog(OperationType operationType, Guid parentId, PropertyType propertyType, ExternalElectric externalElectric, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new ExternalElectricPowerLog(operationType, parentId, propertyType, externalElectric, budgetPrice, timeLimit, memos, createUserId);
        }

        public static EquipmentInstallLog CreateEquipmentInstallLog(OperationType operationType, Guid parentId, PropertyType propertyType, decimal switchPower, decimal battery, int cabinetNumber, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new EquipmentInstallLog(operationType, parentId, propertyType, switchPower, battery, cabinetNumber, budgetPrice, timeLimit, memos, createUserId);
        }

        public static AddressExplorLog CreateAddressExplorLog(OperationType operationType, Guid parentId, PropertyType propertyType, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new AddressExplorLog(operationType, parentId, propertyType, budgetPrice, timeLimit, memos, createUserId);
        }

        public static FoundationTestLog CreateFoundationTestLog(OperationType operationType, Guid parentId, PropertyType propertyType, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            return new FoundationTestLog(operationType, parentId, propertyType, budgetPrice, timeLimit, memos, createUserId);
        }

        public static TaskPropertyLog CreateTaskPropertyLog(RegisterType registerType, Guid constructionTaskId, TaskModel taskModel, Guid parentId, Guid constructionCustomerId, Guid supervisorCustomerId, EngineeringProgress constructionProgress,
            string progressMemos, Guid? progressUserId, SubmitState submitState, Guid? submitUserId, int timeLimit)
        {
            return new TaskPropertyLog(registerType, constructionTaskId, taskModel, parentId, constructionCustomerId, supervisorCustomerId, constructionProgress, progressMemos, progressUserId, submitState, submitUserId, timeLimit);
        }

        /// <summary>
        /// 创建隐患上报实体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="customerId">申请单位Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="reseauManagerId">网格经理Id</param>
        /// <param name="sceneContactMan">现场联系人</param>
        /// <param name="sceneContactTel">现场联系电话</param>
        /// <param name="applyReason">申请事由</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WorkApply CreateWorkApply(string title, Guid customerId, Guid reseauId, Guid reseauManagerId, string applyReason, string sceneContactMan, string sceneContactTel, Guid createUserId)
        {
            return new WorkApply(title, customerId, reseauId, reseauManagerId, applyReason, sceneContactMan, sceneContactTel, createUserId);
        }

        /// <summary>
        /// 创建派工单实体
        /// </summary>
        /// <param name="placeName">站点名称</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="workSmallClassId">工单小类Id</param>
        /// <param name="sceneContactMan">现场联系人</param>
        /// <param name="sceneContactTel">现场联系电话</param>
        /// <param name="requireSendDate">要求派工日期</param>
        /// <param name="days">派工时长</param>
        /// <param name="customerId">代维单位</param>
        /// <param name="customerUserId">代维联系人Id</param>
        /// <param name="workContent">工作内容</param>
        /// <param name="humanRequire">用人要求</param>
        /// <param name="carRequire">车辆要求</param>
        /// <param name="materialRequire">材料要求</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WorkOrder CreateWorkOrder(string placeName, string title, Guid reseauId, Guid workSmallClassId, string sceneContactMan, string sceneContactTel, DateTime requireSendDate, int days, Guid customerId,
            Guid customerUserId, string workContent, string humanRequire, string carRequire, string materialRequire, Guid createUserId)
        {
            return new WorkOrder(placeName, title, reseauId, workSmallClassId, sceneContactMan, sceneContactTel, requireSendDate, days, customerId, customerUserId, workContent,
                humanRequire, carRequire, materialRequire, createUserId);
        }

        /// <summary>
        /// 创建派工单明细实体
        /// </summary>
        /// <param name="workOrderId">派工单Id</param>
        /// <param name="workBeginDate">工作起始时间</param>
        /// <param name="beginHour">工作起始时间(小时)</param>
        /// <param name="beginMinute">工作起始时间(分钟)</param>
        /// <param name="workEndDate">工作结束时间</param>
        /// <param name="endHour">工作结束时间(小时)</param>
        /// <param name="endMinute">工作结束时间(分钟)</param>
        /// <param name="isFinish">是否完成</param>
        /// <param name="executeSituation">执行情况</param>
        /// <param name="materialConsumption">材料消耗</param>
        /// <param name="personnelNumber">人员数量</param>
        /// <param name="carType">车辆类型</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WorkOrderDetail CreateWorkOrderDetail(Guid workOrderId, DateTime workBeginDate, int beginHour, int beginMinute, DateTime workEndDate, int endHour, int endMinute, int isFinish,
            string executeSituation, string materialConsumption, string personnelNumber, string carType, Guid createUserId)
        {
            return new WorkOrderDetail(workOrderId, workBeginDate, beginHour, beginMinute, workEndDate, endHour, endMinute, isFinish, executeSituation, materialConsumption, personnelNumber, carType, createUserId);
        }

        /// <summary>
        /// 构造工期延误申请实体
        /// </summary>
        /// <param name="constructionTaskId">建设任务Id</param>
        /// <param name="title">标题</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="delayDays">延期天数</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static DelayApply CreateDelayApply(Guid constructionTaskId, string title, EngineeringProgress constructionProgress, int delayDays, string remarks, Guid createUserId)
        {
            return new DelayApply(constructionTaskId, title, constructionProgress, delayDays, remarks, createUserId);
        }

        /// <summary>
        /// 构造立项编号信息表实体
        /// </summary>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectDate">立项时间</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static ProjectCodeList CreateProjectCodeList(string projectCode, ProjectType projectType, DateTime projectDate, string placeName, Guid reseauId, Guid projectManagerId, State state, Guid createUserId)
        {
            return new ProjectCodeList(projectCode, projectType, projectDate, placeName, reseauId, projectManagerId, state, createUserId);
        }

        /// <summary>
        /// 构造规格型号信息表实体
        /// </summary>
        /// <param name="projectCode">立项编号</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">规格型号分类</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="specNumber">数量</param>
        /// <param name="totalPrice">金额</param>
        /// <param name="orderCode">订单编号</param>
        /// <param name="state">使用状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static MaterialSpecList CreateMaterialSpecList(string projectCode, string customerName, MaterialSpecType materialSpecType, string materialSpecName, decimal unitPrice, decimal specNumber, decimal totalPrice, string orderCode, State state, Guid createUserId)
        {
            return new MaterialSpecList(projectCode, customerName, materialSpecType, materialSpecName, unitPrice, specNumber, totalPrice, orderCode, state, createUserId);
        }

        /// <summary>
        /// 构造业务量实体
        /// </summary>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="trafficVolumes">话务量</param>
        /// <param name="businessVolumes">业务量</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <param name="createDate">创建日期</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static BusinessVolume CreateBusinessVolume(LogicalType logicalType, string logicalNumber, decimal trafficVolumes, decimal businessVolumes, Profession profession, Guid companyId, DateTime createDate, Guid createUserId)
        {
            return new BusinessVolume(logicalType, logicalNumber, trafficVolumes, businessVolumes, profession, companyId, createDate, createUserId);
        }

        /// <summary>
        /// 构造通知实体
        /// </summary>
        /// <param name="noticeType">通知类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="receiveUserId">接收人用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static Notice CreateNotice(NoticeType noticeType, Guid parentId, decimal lng, decimal lat, string noticeContent, Guid receiveUserId, Guid createUserId)
        {
            return new Notice(noticeType, parentId, lng, lat, noticeContent, receiveUserId, createUserId);
        }

        /// <summary>
        /// 构造站点业务量实体
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="g2BusinessVolumeId">2G业务量Id</param>
        /// <param name="d2BusinessVolumeId">2D业务量Id</param>
        /// <param name="g3BusinessVolumeId">3G业务量Id</param>
        /// <param name="g4BusinessVolumeId">4G业务量Id</param>
        /// <param name="companyId">分公司Id</param>
        public static PlaceBusinessVolume CreatePlaceBusinessVolume(Guid placeId, Guid g2BusinessVolumeId, Guid d2BusinessVolumeId, Guid g3BusinessVolumeId, Guid g4BusinessVolumeId, Guid companyId)
        {
            return new PlaceBusinessVolume(placeId, g2BusinessVolumeId, d2BusinessVolumeId, g3BusinessVolumeId, g4BusinessVolumeId, companyId);
        }

        /// <summary>
        /// 构造盲点反馈实体
        /// </summary>
        /// <param name="placeName">盲点地名</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="feedBackContent">反馈内容</param>
        /// <param name="createUserId">创建人用户Id</param>
        public static BlindSpotFeedBack CreateBlindSpotFeedBack(string placeName, Guid areaId, decimal lng, decimal lat, string feedBackContent, Guid createUserId)
        {
            return new BlindSpotFeedBack(placeName, areaId, lng, lat, feedBackContent, createUserId);
        }

        #endregion

        #region FileMgmt

        /// <summary>
        /// 创建文件实体
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileType">文件MIME类型</param>
        /// <param name="fileExtension">文件扩展名</param>
        /// <param name="fileSize">文件字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="uploadUserId">上传用户Id</param>
        /// <returns></returns>
        public static File CreateFile(Guid id, string fileName, string fileType, string fileExtension, long fileSize, string filePath, Guid uploadUserId)
        {
            return new File(id, fileName, fileType, fileExtension, fileSize, filePath, uploadUserId);
        }

        /// <summary>
        /// 创建文件关联
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="entityId">实体Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static FileAssociation CreateFileAssociation(string entityName, Guid entityId, string fileIdList, Guid createUserId)
        {
            return new FileAssociation(entityName, entityId, fileIdList, createUserId);
        }

        #endregion

        #region WorkFlow

        /// <summary>
        /// 创建工作流过程实体
        /// </summary>
        /// <param name="wfCategoryId">工作流类型Id</param>
        /// <param name="wfProcessCode">工作流过程编码</param>
        /// <param name="wfProcessName">工作流过程名称</param>
        /// <param name="isApprovedByManager">是否部门经理审批</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">工作流过程状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WFProcess CreateWFProcess(Guid wfCategoryId, string wfProcessCode, string wfProcessName, Bool isApprovedByManager, string remarks, State state, Guid createUserId)
        {
            return new WFProcess(wfCategoryId, wfProcessCode, wfProcessName, isApprovedByManager, remarks, state, createUserId);
        }

        /// <summary>
        /// 创建工作流活动实体
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="wfActivityName">工作流活动名称</param>
        /// <param name="wfActivityOperate">工作流活动操作类型</param>
        /// <param name="wfActivityEditorId">工作流活动编辑器Id</param>
        /// <param name="wfActivityOrder">工作流活动顺序类型</param>
        /// <param name="serialId">序号</param>
        /// <param name="rowId">行号</param>
        /// <param name="timelimit">期限</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="postId">岗位Id</param>
        /// <param name="isMustEdit">是否必须编辑</param>
        /// <param name="isNecessaryStep">是否必要步骤</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WFActivity CreateWFActivity(Guid wfProcessId, string wfActivityName, WFActivityOperate wfActivityOperate, Guid wfActivityEditorId, WFActivityOrder wfActivityOrder,
            int serialId, int rowId, int timelimit, Guid companyId, Guid departmentId, Guid userId, Guid postId, Bool isMustEdit, Bool isNecessaryStep, Guid createUserId)
        {
            return new WFActivity(wfProcessId, wfActivityName, wfActivityOperate, wfActivityEditorId, wfActivityOrder,
             serialId, rowId, timelimit, companyId, departmentId, userId, postId, isMustEdit, isNecessaryStep, createUserId);
        }

        /// <summary>
        /// 创建工作流过程实例实体
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="entityId">实体Id</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="content">内容</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WFProcessInstance CreateWFProcessInstance(Guid wfProcessId, Guid entityId, string wfProcessInstanceCode, string wfProcessInstanceName, string content, Guid createUserId)
        {
            return new WFProcessInstance(wfProcessId, entityId, wfProcessInstanceCode, wfProcessInstanceName, content, createUserId);
        }

        /// <summary>
        /// 创建工作流活动实例实体
        /// </summary>
        /// <param name="wfProcessInstanceId">工作流过程实例Id</param>
        /// <param name="wfActivityInstanceName">工作流活动实例名称</param>
        /// <param name="wfActivityOperate">工作流活动操作类型</param>
        /// <param name="wfActivityEditorId">工作流活动编辑器Id</param>
        /// <param name="wfActivityOrder">工作流活动顺序类型</param>
        /// <param name="serialId">序号</param>
        /// <param name="rowId">行号</param>
        /// <param name="timelimit">期限(小时)</param>
        /// <param name="userId">批阅用户Id</param>
        /// <param name="isMustEdit">是否必须编辑</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public static WFActivityInstance CreateWFActivityInstance(Guid wfProcessInstanceId, string wfActivityInstanceName, WFActivityOperate wfActivityOperate, Guid wfActivityEditorId,
            WFActivityOrder wfActivityOrder, int serialId, int rowId, int timelimit, Guid userId, Bool isMustEdit, Guid createUserId)
        {
            return new WFActivityInstance(wfProcessInstanceId, wfActivityInstanceName, wfActivityOperate, wfActivityEditorId,
                wfActivityOrder, serialId, rowId, timelimit, userId, isMustEdit, createUserId);
        }

        /// <summary>
        /// 创建单据编辑实例实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static WFActivityInstanceEditor CreateWFActivityInstanceEditor(Guid wfActivityInstanceId)
        {
            return new WFActivityInstanceEditor(wfActivityInstanceId);
        }

        #endregion
    }
}
