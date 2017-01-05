using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.Enum
{
    /// <summary>
    /// 枚举值服务接口
    /// </summary>
    public interface IEnumService
    {
        /// <summary>
        /// 获取是或者否枚举值列表
        /// </summary>
        /// <returns>是或者否枚举值列表</returns>
        IList<Dictionary<string, string>> GetBoolEnum();

        /// <summary>
        /// 获取公司性质枚举值列表
        /// </summary>
        /// <returns>公司性质枚举值列表</returns>
        IList<Dictionary<string, string>> GetCompanyNatureEnum();

        /// <summary>
        /// 获取专业枚举值列表
        /// </summary>
        /// <returns>专业枚举值列表</returns>
        IList<Dictionary<string, string>> GetProfessionEnum();

        /// <summary>
        /// 获取专业枚举值列表，用于树形显示
        /// </summary>
        /// <returns>专业枚举值列表</returns>
        IList<Dictionary<string, object>> GetProfessionEnumByTree();

        /// <summary>
        /// 获取项目类型枚举值列表
        /// </summary>
        /// <returns>项目类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetProjectCategoryEnum();

        /// <summary>
        /// 获取项目进度枚举值列表
        /// </summary>
        /// <returns>项目进度枚举值列表</returns>
        IList<Dictionary<string, string>> GetProjectProgressEnum();

        /// <summary>
        /// 获取使用或者停用状态枚举值列表
        /// </summary>
        /// <returns>使用或者停用枚举值列表</returns>
        IList<Dictionary<string, string>> GetStateEnum();

        /// <summary>
        /// 获取产权枚举值列表
        /// </summary>
        /// <returns>产权枚举值列表</returns>
        IList<Dictionary<string, string>> GetPropertyRightEnum();

        /// <summary>
        /// 获取产权枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的产权枚举值列表</returns>
        IList<Dictionary<string, string>> GetPropertyRightEnum(string specifiedValuesStr);

        /// <summary>
        /// 获取重要性程度枚举值列表
        /// </summary>
        /// <returns>重要性程度枚举值列表</returns>
        IList<Dictionary<string, string>> GetImportanceEnum();

        /// <summary>
        /// 获取紧要程度枚举值列表
        /// </summary>
        /// <returns>紧要程度枚举值列表</returns>
        IList<Dictionary<string, string>> GetUrgencyEnum();

        /// <summary>
        /// 获取需求确认枚举值列表
        /// </summary>
        /// <returns>需求确认枚举值列表</returns>
        IList<Dictionary<string, string>> GetDemandEnum();

        /// <summary>
        /// 获取需求确认枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的需求确认枚举值列表</returns>
        IList<Dictionary<string, string>> GetDemandEnum(string specifiedValuesStr);

        /// <summary>
        /// 获取需求确认状态枚举值列表
        /// </summary>
        /// <returns>需求确认状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetDemandStateEnum();

        /// <summary>
        /// 获取寻址状态枚举值列表
        /// </summary>
        /// <returns>寻址状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetAddressingStateEnum();

        /// <summary>
        /// 获取工作流活动实例流转类型枚举值列表
        /// </summary>
        /// <returns>工作流活动实例流转类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFActivityInstanceFlowEnum();

        /// <summary>
        /// 获取工作流活动实例流转类型枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的工作流活动实例流转类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFActivityInstanceFlowEnum(string specifiedValuesStr);

        /// <summary>
        /// 获取工作流活动实例结果枚举值列表
        /// </summary>
        /// <returns>工作流活动实例结果枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFActivityInstanceResultEnum();

        /// <summary>
        /// 获取工作流活动实例状态枚举值列表
        /// </summary>
        /// <returns>工作流活动实例状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFActivityInstanceStateEnum();

        /// <summary>
        /// 获取工作流活动操作类型枚举值列表
        /// </summary>
        /// <returns>工作流活动操作类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFActivityOperateEnum();

        /// <summary>
        /// 获取工作流活动顺序类型枚举值列表
        /// </summary>
        /// <returns>工作流活动顺序类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFActivityOrderEnum();

        /// <summary>
        /// 获取工作流过程实例状态枚举值列表
        /// </summary>
        /// <returns>工作流过程实例状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFProcessInstanceStateEnum();

        /// <summary>
        /// 获取工作流过程实例状态枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的工作流过程实例状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetWFProcessInstanceStateEnum(string specifiedValuesStr);

        /// <summary>
        /// 获取建设方式枚举值列表
        /// </summary>
        /// <returns>建设方式枚举值列表</returns>
        IList<Dictionary<string, string>> GetConstructionMethodEnum();

        /// <summary>
        /// 获取建设进度枚举值列表
        /// </summary>
        /// <returns>建设进度枚举值列表</returns>
        IList<Dictionary<string, string>> GetEngineeringProgressEnum();

        /// <summary>
        /// 获取铁塔类型枚举值列表
        /// </summary>
        /// <returns>铁塔类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetTowerTypeEnum();

        /// <summary>
        /// 获取机房类型枚举值列表
        /// </summary>
        /// <returns>机房类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetMachineRoomTypeEnum();

        /// <summary>
        /// 获取外电引入枚举值列表
        /// </summary>
        /// <returns>外电引入枚举值列表</returns>
        IList<Dictionary<string, string>> GetExternalElectricEnum();

        /// <summary>
        /// 获取消防枚举值列表
        /// </summary>
        /// <returns>消防枚举值列表</returns>
        IList<Dictionary<string, string>> GetFireControlEnum();

        /// <summary>
        /// 获取铁塔基础类型枚举值列表
        /// </summary>
        /// <returns>铁塔基础类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetTowerBaseTypeEnum();

        /// <summary>
        /// 获取处理状态枚举值列表
        /// </summary>
        /// <returns>处理状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetDoStateEnum();

        /// <summary>
        /// 获取资源名称枚举值列表
        /// </summary>
        /// <returns>资源名称举值列表</returns>
        IList<Dictionary<string, string>> GetTaskModelEnum();

        /// <summary>
        /// 获取资料状态枚举值列表
        /// </summary>
        /// <returns>资料状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetSubmitStateEnum();

        /// <summary>
        /// 获取操作类型名称枚举值列表
        /// </summary>
        /// <returns>操作类型名称枚举值列表</returns>
        IList<Dictionary<string, string>> GetOperationTypeEnum();

        /// <summary>
        /// 获取登记类型名称枚举值列表
        /// </summary>
        /// <returns>登记类型名称枚举值列表</returns>
        IList<Dictionary<string, string>> GetRegisterTypeEnum();

        /// <summary>
        /// 获取运营商Id枚举值列表
        /// </summary>
        /// <returns>运营商Id枚举值列表</returns>
        IList<Dictionary<string, string>> GetCompanyNameIdEnum();

        /// <summary>
        /// 获取往来单位分类名称枚举值列表
        /// </summary>
        /// <returns>往来单位分类名称枚举值列表</returns>
        IList<Dictionary<string, string>> GetCustomerTypeEnum();

        /// <summary>
        /// 获取完工请求状态名称枚举值列表
        /// </summary>
        /// <returns>完工请求状态名称枚举值列表</returns>
        IList<Dictionary<string, string>> GetRequestStateEnum();

        /// <summary>
        /// 获取立项建设方式名称枚举值列表
        /// </summary>
        /// <returns>立项建设方式名称枚举值列表</returns>
        IList<Dictionary<string, string>> GetProjectTypeEnum();

        /// <summary>
        /// 获取规格型号分类名称枚举值列表
        /// </summary>
        /// <returns>规格型号分类名称枚举值列表</returns>
        IList<Dictionary<string, string>> GetMaterialSpecTypeEnum();


        /// <summary>
        /// 获取项目类型枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的项目类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetProjectTypeEnum(string specifiedValuesStr);

        /// <summary>
        /// 获取逻辑号类型枚举值列表
        /// </summary>
        /// <param name="specifiedValuesStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>指定值的逻辑号类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetLogicalTypeEnumPart(string specifiedValuesStr);

        /// <summary>
        /// 获取通知类型枚举值列表
        /// </summary>
        /// <returns>通知类型枚举值列表</returns>
        IList<Dictionary<string, string>> GetNoticeTypeEnum();

        /// <summary>
        /// 获取通知状态枚举值列表
        /// </summary>
        /// <returns>通知状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetNoticeStateEnum();

        /// <summary>
        /// 获取站点地图状态枚举值列表
        /// </summary>
        /// <returns>站点地图状态枚举值列表</returns>
        IList<Dictionary<string, string>> GetPlaceMapStateEnum();

        /// <summary>
        /// 获取规划意见枚举值列表
        /// </summary>
        /// <returns>规划意见枚举值列表</returns>
        IList<Dictionary<string, string>> GetPlanningAdviceEnum();

        /// <summary>
        /// 获取职务枚举值列表
        /// </summary>
        /// <returns>职务枚举值列表</returns>
        IList<Dictionary<string, string>> GetDutyEnum();

        /// <summary>
        /// 获取职务枚举值列表，用于树形显示
        /// </summary>
        /// <returns>职务枚举值列表</returns>
        IList<Dictionary<string, object>> GetDutyEnumByTree();
    }
}
