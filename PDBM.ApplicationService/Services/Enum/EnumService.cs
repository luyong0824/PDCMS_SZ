using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.Enum;

namespace PDBM.ApplicationService.Services.Enum
{
    /// <summary>
    /// 枚举值应用层服务
    /// </summary>
    public class EnumService : IEnumService
    {
        /// <summary>
        /// 获取是或者否枚举值列表
        /// </summary>
        /// <returns>是或者否枚举值列表</returns>
        public IList<Dictionary<string, string>> GetBoolEnum()
        {
            return EnumHelper.EnumToList(typeof(Bool));
        }

        /// <summary>
        /// 获取公司性质枚举值列表
        /// </summary>
        /// <returns>公司性质枚举值列表</returns>
        public IList<Dictionary<string, string>> GetCompanyNatureEnum()
        {
            return EnumHelper.EnumToList(typeof(CompanyNature));
        }

        /// <summary>
        /// 获取专业枚举值列表
        /// </summary>
        /// <returns>专业枚举值列表</returns>
        public IList<Dictionary<string, string>> GetProfessionEnum()
        {
            return EnumHelper.EnumToList(typeof(Profession));
        }

        /// <summary>
        /// 获取专业枚举值列表，用于树形显示
        /// </summary>
        /// <returns>专业枚举值列表</returns>
        public IList<Dictionary<string, object>> GetProfessionEnumByTree()
        {
            return EnumHelper.EnumToListByTree(typeof(Profession));
        }

        /// <summary>
        /// 获取项目类型枚举值列表
        /// </summary>
        /// <returns>项目类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetProjectCategoryEnum()
        {
            return EnumHelper.EnumToList(typeof(ProjectCategory));
        }

        /// <summary>
        /// 获取项目进度枚举值列表
        /// </summary>
        /// <returns>项目进度枚举值列表</returns>
        public IList<Dictionary<string, string>> GetProjectProgressEnum()
        {
            return EnumHelper.EnumToList(typeof(ProjectProgress));
        }

        /// <summary>
        /// 获取使用或者停用状态枚举值列表
        /// </summary>
        /// <returns>使用或者停用枚举值列表</returns>
        public IList<Dictionary<string, string>> GetStateEnum()
        {
            return EnumHelper.EnumToList(typeof(State));
        }

        /// <summary>
        /// 获取产权枚举值列表
        /// </summary>
        /// <returns>产权枚举值列表</returns>
        public IList<Dictionary<string, string>> GetPropertyRightEnum()
        {
            return EnumHelper.EnumToList(typeof(PropertyRight));
        }

        /// <summary>
        /// 获取产权枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的产权枚举值列表</returns>
        public IList<Dictionary<string, string>> GetPropertyRightEnum(string specifiedValuesStr)
        {
            return EnumHelper.EnumToList(typeof(PropertyRight), specifiedValuesStr);
        }

        /// <summary>
        /// 获取重要性程度枚举值列表
        /// </summary>
        /// <returns>重要性程度枚举值列表</returns>
        public IList<Dictionary<string, string>> GetImportanceEnum()
        {
            return EnumHelper.EnumToList(typeof(Importance));
        }

        /// <summary>
        /// 获取紧要程度枚举值列表
        /// </summary>
        /// <returns>紧要程度枚举值列表</returns>
        public IList<Dictionary<string, string>> GetUrgencyEnum()
        {
            return EnumHelper.EnumToList(typeof(Urgency));
        }

        /// <summary>
        /// 获取需求确认枚举值列表
        /// </summary>
        /// <returns>需求确认枚举值列表</returns>
        public IList<Dictionary<string, string>> GetDemandEnum()
        {
            return EnumHelper.EnumToList(typeof(Demand));
        }

        /// <summary>
        /// 获取需求确认枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的需求确认枚举值列表</returns>
        public IList<Dictionary<string, string>> GetDemandEnum(string specifiedValuesStr)
        {
            return EnumHelper.EnumToList(typeof(Demand), specifiedValuesStr);
        }

        /// <summary>
        /// 获取需求确认状态枚举值列表
        /// </summary>
        /// <returns>需求确认状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetDemandStateEnum()
        {
            return EnumHelper.EnumToList(typeof(DemandState));
        }

        /// <summary>
        /// 获取寻址状态枚举值列表
        /// </summary>
        /// <returns>寻址状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetAddressingStateEnum()
        {
            return EnumHelper.EnumToList(typeof(AddressingState));
        }

        /// <summary>
        /// 获取工作流活动实例流转类型枚举值列表
        /// </summary>
        /// <returns>工作流活动实例流转类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFActivityInstanceFlowEnum()
        {
            return EnumHelper.EnumToList(typeof(WFActivityInstanceFlow));
        }

        /// <summary>
        /// 获取工作流活动实例流转类型枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的工作流活动实例流转类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFActivityInstanceFlowEnum(string specifiedValuesStr)
        {
            return EnumHelper.EnumToList(typeof(WFActivityInstanceFlow), specifiedValuesStr);
        }

        /// <summary>
        /// 获取工作流活动实例结果枚举值列表
        /// </summary>
        /// <returns>工作流活动实例结果枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFActivityInstanceResultEnum()
        {
            return EnumHelper.EnumToList(typeof(WFActivityInstanceResult));
        }

        /// <summary>
        /// 获取工作流活动实例状态枚举值列表
        /// </summary>
        /// <returns>工作流活动实例状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFActivityInstanceStateEnum()
        {
            return EnumHelper.EnumToList(typeof(WFActivityInstanceState));
        }

        /// <summary>
        /// 获取工作流活动操作类型枚举值列表
        /// </summary>
        /// <returns>工作流活动操作类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFActivityOperateEnum()
        {
            return EnumHelper.EnumToList(typeof(WFActivityOperate));
        }

        /// <summary>
        /// 获取工作流活动顺序类型枚举值列表
        /// </summary>
        /// <returns>工作流活动顺序类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFActivityOrderEnum()
        {
            return EnumHelper.EnumToList(typeof(WFActivityOrder));
        }

        /// <summary>
        /// 获取工作流过程实例状态枚举值列表
        /// </summary>
        /// <returns>工作流过程实例状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFProcessInstanceStateEnum()
        {
            return EnumHelper.EnumToList(typeof(WFProcessInstanceState));
        }

        /// <summary>
        /// 获取工作流过程实例状态枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的工作流过程实例状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetWFProcessInstanceStateEnum(string specifiedValuesStr)
        {
            return EnumHelper.EnumToList(typeof(WFProcessInstanceState), specifiedValuesStr);
        }

        /// <summary>
        /// 获取建设方式枚举值列表
        /// </summary>
        /// <returns>建设方式枚举值列表</returns>
        public IList<Dictionary<string, string>> GetConstructionMethodEnum()
        {
            return EnumHelper.EnumToList(typeof(ConstructionMethod));
        }

        /// <summary>
        /// 获取建设进度枚举值列表
        /// </summary>
        /// <returns>建设进度枚举值列表</returns>
        public IList<Dictionary<string, string>> GetEngineeringProgressEnum()
        {
            return EnumHelper.EnumToList(typeof(EngineeringProgress));
        }

        /// <summary>
        /// 获取铁塔类型枚举值列表
        /// </summary>
        /// <returns>铁塔类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetTowerTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(TowerType));
        }

        /// <summary>
        /// 获取机房类型枚举值列表
        /// </summary>
        /// <returns>机房类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetMachineRoomTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(MachineRoomType));
        }

        /// <summary>
        /// 获取外电引入枚举值列表
        /// </summary>
        /// <returns>外电引入枚举值列表</returns>
        public IList<Dictionary<string, string>> GetExternalElectricEnum()
        {
            return EnumHelper.EnumToList(typeof(ExternalElectric));
        }

        /// <summary>
        /// 获取消防枚举值列表
        /// </summary>
        /// <returns>消防枚举值列表</returns>
        public IList<Dictionary<string, string>> GetFireControlEnum()
        {
            return EnumHelper.EnumToList(typeof(FireControl));
        }

        /// <summary>
        /// 获取铁塔基础类型枚举值列表
        /// </summary>
        /// <returns>铁塔基础类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetTowerBaseTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(TowerBaseType));
        }

        /// <summary>
        /// 获取申购状态枚举值列表
        /// </summary>
        /// <returns>申购类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetDoStateEnum()
        {
            return EnumHelper.EnumToList(typeof(DoState));
        }

        /// <summary>
        /// 获取提交状态枚举值列表
        /// </summary>
        /// <returns>提交状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetSubmitStateEnum()
        {
            return EnumHelper.EnumToList(typeof(SubmitState));
        }

        /// <summary>
        /// 获取资源名称枚举值列表
        /// </summary>
        /// <returns>资源名称枚举值列表</returns>
        public IList<Dictionary<string, string>> GetTaskModelEnum()
        {
            return EnumHelper.EnumToList(typeof(TaskModel));
        }

        /// <summary>
        /// 获取操作类型名称枚举值列表
        /// </summary>
        /// <returns>操作类型名称枚举值列表</returns>
        public IList<Dictionary<string, string>> GetOperationTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(OperationType));
        }

        /// <summary>
        /// 获取登记类型名称枚举值列表
        /// </summary>
        /// <returns>登记类型名称枚举值列表</returns>
        public IList<Dictionary<string, string>> GetRegisterTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(RegisterType));
        }

        /// <summary>
        /// 获取运营商Id枚举值列表
        /// </summary>
        /// <returns>运营商Id枚举值列表</returns>
        public IList<Dictionary<string, string>> GetCompanyNameIdEnum()
        {
            return EnumHelper.EnumToList(typeof(CompanyNameId));
        }

        /// <summary>
        /// 获取往来单位分类名称枚举值列表
        /// </summary>
        /// <returns>往来单位分类名称枚举值列表</returns>
        public IList<Dictionary<string, string>> GetCustomerTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(CustomerType));
        }

        /// <summary>
        /// 获取完工请求状态名称枚举值列表
        /// </summary>
        /// <returns>完工请求状态名称枚举值列表</returns>
        public IList<Dictionary<string, string>> GetRequestStateEnum()
        {
            return EnumHelper.EnumToList(typeof(RequestState));
        }

        /// <summary>
        /// 获取立项建设方式名称枚举值列表
        /// </summary>
        /// <returns>立项建设方式名称枚举值列表</returns>
        public IList<Dictionary<string, string>> GetProjectTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(ProjectType));
        }

        /// <summary>
        /// 获取规格型号分类名称枚举值列表
        /// </summary>
        /// <returns>规格型号分类名称枚举值列表</returns>
        public IList<Dictionary<string, string>> GetMaterialSpecTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(MaterialSpecType));
        }

        /// <summary>
        /// 获取项目类型枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的项目类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetProjectTypeEnum(string specifiedValuesStr)
        {
            return EnumHelper.EnumToList(typeof(ProjectType), specifiedValuesStr);
        }

        /// <summary>
        /// 获取逻辑号类型枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的逻辑号类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetLogicalTypeEnumPart(string specifiedValuesStr)
        {
            return EnumHelper.EnumToList(typeof(LogicalType), specifiedValuesStr);
        }

        /// <summary>
        /// 获取通知类型枚举值列表
        /// </summary>
        /// <returns>通知类型枚举值列表</returns>
        public IList<Dictionary<string, string>> GetNoticeTypeEnum()
        {
            return EnumHelper.EnumToList(typeof(NoticeType));
        }

        /// <summary>
        /// 获取通知状态枚举值列表
        /// </summary>
        /// <returns>通知状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetNoticeStateEnum()
        {
            return EnumHelper.EnumToList(typeof(NoticeState));
        }

        /// <summary>
        /// 获取站点地图状态枚举值列表
        /// </summary>
        /// <returns>站点地图状态枚举值列表</returns>
        public IList<Dictionary<string, string>> GetPlaceMapStateEnum()
        {
            return EnumHelper.EnumToList(typeof(PlaceMapState));
        }

        /// <summary>
        /// 获取规划意见枚举值列表
        /// </summary>
        /// <returns>规划意见枚举值列表</returns>
        public IList<Dictionary<string, string>> GetPlanningAdviceEnum()
        {
            return EnumHelper.EnumToList(typeof(PlanningAdvice));
        }

        /// <summary>
        /// 获取职务枚举值列表
        /// </summary>
        /// <returns>职务枚举值列表</returns>
        public IList<Dictionary<string, string>> GetDutyEnum()
        {
            return EnumHelper.EnumToList(typeof(Duty));
        }

        /// <summary>
        /// 获取职务枚举值列表，用于树形显示
        /// </summary>
        /// <returns>职务枚举值列表</returns>
        public IList<Dictionary<string, object>> GetDutyEnumByTree()
        {
            return EnumHelper.EnumToListByTree(typeof(Duty));
        }
    }
}
