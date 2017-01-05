using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models
{
    /// <summary>
    /// 实体属性验证器
    /// </summary>
    public static class PropertyValidator
    {
        /// <summary>
        /// 验证int类型是否为正数
        /// </summary>
        /// <param name="i">int类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsPositive(this int i, string propertyName)
        {
            if (i <= 0)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证int类型是否为非负数
        /// </summary>
        /// <param name="i">int类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsNonnegative(this int i, string propertyName)
        {
            if (i < 0)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证decimal类型是否为正数
        /// </summary>
        /// <param name="d">decimal类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsPositive(this decimal d, string propertyName)
        {
            if (d <= 0)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证decimal类型是否为非负数
        /// </summary>
        /// <param name="d">decimal类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsNonnegative(this decimal d, string propertyName)
        {
            if (d < 0)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证Guid类型是否为空
        /// </summary>
        /// <param name="guid">Guid类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsEmpty(this Guid guid, string propertyName)
        {
            if (guid == Guid.Empty)
            {
                throw new DomainFault("{0}为空", propertyName);
            }
        }

        /// <summary>
        /// 验证字符串是否为Null
        /// </summary>
        /// <param name="str">string类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsNull(this string str, string propertyName)
        {
            if (str == null)
            {
                throw new DomainFault("{0}为Null", propertyName);
            }
        }

        /// <summary>
        /// 验证字符串是否为Null或者空或者长度过长
        /// </summary>
        /// <param name="str">string类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="isUnicode">是否为Unicode字符</param>
        /// <param name="maxLength">最大长度</param>
        public static void IsNullOrEmptyOrTooLong(this string str, string propertyName, bool isUnicode, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new DomainFault("{0}为Null或者空", propertyName);
            }
            if ((isUnicode ? str.Length : Encoding.Default.GetBytes(str).Length) > maxLength)
            {
                throw new DomainFault("{0}长度过长", propertyName);
            }
        }

        /// <summary>
        /// 验证字符串是否为Null或者长度过长
        /// </summary>
        /// <param name="str">string类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="isUnicode">是否为Unicode字符</param>
        /// <param name="maxLength">最大长度</param>
        public static void IsNullOrTooLong(this string str, string propertyName, bool isUnicode, int maxLength)
        {
            if (str == null)
            {
                throw new DomainFault("{0}为Null", propertyName);
            }
            if ((isUnicode ? str.Length : Encoding.Default.GetBytes(str).Length) > maxLength)
            {
                throw new DomainFault("{0}长度过长", propertyName);
            }
        }

        /// <summary>
        /// 验证是或者否枚举值是否无效
        /// </summary>
        /// <param name="b">Bool类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this Bool b, string propertyName)
        {
            if (b != Bool.是 && b != Bool.否)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证状态枚举值是否无效
        /// </summary>
        /// <param name="state">State类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this State state, string propertyName)
        {
            if (state != State.使用 && state != State.停用)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证专业枚举值是否无效
        /// </summary>
        /// <param name="profession">Profession类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this Profession profession, string propertyName)
        {
            if (profession != Profession.基站 && profession != Profession.室分)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证项目类型枚举值是否无效
        /// </summary>
        /// <param name="projectCategory">ProjectCategory类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this ProjectCategory projectCategory, string propertyName)
        {
            if (projectCategory != ProjectCategory.集团投资 && projectCategory != ProjectCategory.省分投资 && projectCategory != ProjectCategory.市分投资)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证项目进度枚举值是否无效
        /// </summary>
        /// <param name="projectProgress">ProjectProgress类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this ProjectProgress projectProgress, string propertyName)
        {
            if (projectProgress != ProjectProgress.未开工 && projectProgress != ProjectProgress.进行中 && projectProgress != ProjectProgress.完工 && projectProgress != ProjectProgress.开通 && projectProgress != ProjectProgress.暂缓 && projectProgress != ProjectProgress.撤销)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证产权枚举值是否无效
        /// </summary>
        /// <param name="propertyRight">PropertyRight类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this PropertyRight propertyRight, string propertyName)
        {
            if (propertyRight != PropertyRight.铁塔 && propertyRight != PropertyRight.购电信 &&
                propertyRight != PropertyRight.购移动 && propertyRight != PropertyRight.购联通)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证重要性程度枚举值是否无效
        /// </summary>
        /// <param name="importance">Importance类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this Importance importance, string propertyName)
        {
            if (importance != Importance.A && importance != Importance.B && importance != Importance.C)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证站点来源类型枚举值是否无效
        /// </summary>
        /// <param name="sourceType">SourceType类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this SourceType sourceType, string propertyName)
        {
            if (sourceType != SourceType.寻址 && sourceType != SourceType.购置)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证紧要程度枚举值是否无效
        /// </summary>
        /// <param name="urgency">Urgency类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this Urgency urgency, string propertyName)
        {
            if (urgency != Urgency.一级 && urgency != Urgency.二级)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证工作流活动操作类型枚举值是否无效
        /// </summary>
        /// <param name="wfActivityOperate">WFActivityOperate类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this WFActivityOperate wfActivityOperate, string propertyName)
        {
            if (wfActivityOperate != WFActivityOperate.审批 && wfActivityOperate != WFActivityOperate.阅 && wfActivityOperate != WFActivityOperate.单据编辑)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证工作流活动顺序枚举值是否无效
        /// </summary>
        /// <param name="wfActivityOrder">WFActivityOrder类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this WFActivityOrder wfActivityOrder, string propertyName)
        {
            if (wfActivityOrder != WFActivityOrder.顺序 && wfActivityOrder != WFActivityOrder.并发)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证工作流活动实例流转类型枚举值是否无效
        /// </summary>
        /// <param name="wfActivityInstanceFlow">WFActivityInstanceFlow类型</param>
        /// <param name="propertyName">属性名称</param>
        public static void IsInvalid(this WFActivityInstanceFlow wfActivityInstanceFlow, string propertyName)
        {
            if (wfActivityInstanceFlow != WFActivityInstanceFlow.正常流转 && wfActivityInstanceFlow != WFActivityInstanceFlow.转发他人 &&
                wfActivityInstanceFlow != WFActivityInstanceFlow.退回并转至自己 && wfActivityInstanceFlow != WFActivityInstanceFlow.终止流转)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证建设方式枚举值是否无效
        /// </summary>
        /// <param name="constructionMethod"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this ConstructionMethod constructionMethod, string propertyName)
        {
            if (constructionMethod != ConstructionMethod.新建 && constructionMethod != ConstructionMethod.改造)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证建设监督枚举值是否无效
        /// </summary>
        /// <param name="constructionProgress"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this EngineeringProgress constructionProgress, string propertyName)
        {
            if (constructionProgress != EngineeringProgress.未开工 && constructionProgress != EngineeringProgress.进行中 && constructionProgress != EngineeringProgress.已完工 && constructionProgress != EngineeringProgress.暂缓 && constructionProgress != EngineeringProgress.取消)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证铁塔类型枚举值是否无效
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this TowerType towerType, string propertyName)
        {
            if (towerType != TowerType.抱杆 && towerType != TowerType.插接式单管塔 && towerType != TowerType.灯杆景观塔 && towerType != TowerType.仿生树 && towerType != TowerType.角钢塔 && towerType != TowerType.路灯杆塔 && towerType != TowerType.落地拉线塔 && towerType != TowerType.三管塔 && towerType != TowerType.双轮景观塔 && towerType != TowerType.屋顶拉线塔 && towerType != TowerType.增高架 && towerType != TowerType.支撑杆)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证外电引入枚举值是否无效
        /// </summary>
        /// <param name="externalElectric"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this ExternalElectric externalElectric, string propertyName)
        {
            if (externalElectric != ExternalElectric.专变 && externalElectric != ExternalElectric.专线 && externalElectric != ExternalElectric.转供)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证机房类型枚举值是否无效
        /// </summary>
        /// <param name="machineRoomType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this MachineRoomType machineRoomType, string propertyName)
        {
            if (machineRoomType != MachineRoomType.其他 && machineRoomType != MachineRoomType.一体化机柜 && machineRoomType != MachineRoomType.自建彩钢板机房 && machineRoomType != MachineRoomType.自建砖混机房 && machineRoomType != MachineRoomType.租用砖混机房)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        public static void IsInvalid(this FireControl fireControl, string propertyName)
        {
            if (fireControl != FireControl.手提式 && fireControl != FireControl.悬挂式)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证工程名称枚举值是否无效
        /// </summary>
        /// <param name="taskModel"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this TaskModel taskModel, string propertyName)
        {
            if (taskModel != TaskModel.天桅 && taskModel != TaskModel.天桅基础 && taskModel != TaskModel.机房 && taskModel != TaskModel.外电引入 && taskModel != TaskModel.设备安装 && taskModel != TaskModel.线路)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证资源类型枚举值是否无效
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this PropertyType propertyType, string propertyName)
        {
            if (propertyType != PropertyType.寻址设计 && propertyType != PropertyType.改造设计 && propertyType != PropertyType.任务设计 && propertyType != PropertyType.站点参数)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证铁塔基础类型枚举值是否无效
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this TowerBaseType towerBaseType, string propertyName)
        {
            if (towerBaseType != TowerBaseType.独立桩基 && towerBaseType != TowerBaseType.开挖式基础 && towerBaseType != TowerBaseType.楼顶塔基础 && towerBaseType != TowerBaseType.拉线塔基础)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证处理状态枚举值是否有效
        /// </summary>
        /// <param name="doState"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this DoState doState, string propertyName)
        {
            if (doState != DoState.未处理 && doState != DoState.已处理)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证提交状态枚举值是否有效
        /// </summary>
        /// <param name="submitState"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this SubmitState submitState, string propertyName)
        {
            if (submitState != SubmitState.未提交 && submitState != SubmitState.已提交)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 验证操作类型枚举值是否有效
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this OperationType operationType, string propertyName)
        {
            if (operationType != OperationType.新增 && operationType != OperationType.修改 && operationType != OperationType.删除 && operationType != OperationType.导入)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 登记类型枚举值是否有效
        /// </summary>
        /// <param name="registerType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this RegisterType registerType, string propertyName)
        {
            if (registerType != RegisterType.进度登记 && registerType != RegisterType.资料登记)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 运营商Id枚举值是否有效
        /// </summary>
        /// <param name="compangNameId"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this CompanyNameId compangNameId, string propertyName)
        {
            if (compangNameId != CompanyNameId.移动 && compangNameId != CompanyNameId.电信 && compangNameId != CompanyNameId.联通)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 往来单位分类枚举值是否有效
        /// </summary>
        /// <param name="customerType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this CustomerType customerType, string propertyName)
        {
            if (customerType != CustomerType.设计单位 && customerType != CustomerType.施工单位 && customerType != CustomerType.监理单位)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 完工请求状态枚举值是否有效
        /// </summary>
        /// <param name="requestState"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this RequestState requestState, string propertyName)
        {
            if (requestState != RequestState.未请求 && requestState != RequestState.请求中 && requestState != RequestState.请求完成 && requestState != RequestState.请求退回)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 立项建设方式枚举值是否有效
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this ProjectType projectType, string propertyName)
        {
            if (projectType != ProjectType.新建 && projectType != ProjectType.改造 && projectType != ProjectType.部分拆除 && projectType != ProjectType.全部拆除)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 规格型号分类枚举值是否有效
        /// </summary>
        /// <param name="materialSpecType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this MaterialSpecType materialSpecType, string propertyName)
        {
            if (materialSpecType != MaterialSpecType.地勘 && materialSpecType != MaterialSpecType.电力线缆 && materialSpecType != MaterialSpecType.监理 && materialSpecType != MaterialSpecType.开关电源 && materialSpecType != MaterialSpecType.美化外罩 && materialSpecType != MaterialSpecType.设计 && materialSpecType != MaterialSpecType.施工 && materialSpecType != MaterialSpecType.室外机柜 && materialSpecType != MaterialSpecType.铁塔 && materialSpecType != MaterialSpecType.土建 && materialSpecType != MaterialSpecType.外电引入 && materialSpecType != MaterialSpecType.蓄电池 && materialSpecType != MaterialSpecType.桩基检测)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 逻辑号类型枚举值是否有效
        /// </summary>
        /// <param name="logicalType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this LogicalType logicalType, string propertyName)
        {
            if (logicalType != LogicalType.G2 && logicalType != LogicalType.D2 && logicalType != LogicalType.G3 && logicalType != LogicalType.G4 && logicalType != LogicalType.G5)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 通知类型枚举值是否有效
        /// </summary>
        /// <param name="noticeType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this NoticeType noticeType, string propertyName)
        {
            if (noticeType != NoticeType.经纬度变更)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 通知状态枚举值是否有效
        /// </summary>
        /// <param name="noticeType"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this NoticeState noticeState, string propertyName)
        {
            if (noticeState != NoticeState.未阅 && noticeState != NoticeState.已阅)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 站点地图状态枚举值是否有效
        /// </summary>
        /// <param name="placeMapState"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this PlaceMapState placeMapState, string propertyName)
        {
            if (placeMapState != PlaceMapState.寻址确认 && placeMapState != PlaceMapState.项目开通)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 规划意见枚举值是否有效
        /// </summary>
        /// <param name="planningAdvice"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this PlanningAdvice planningAdvice, string propertyName)
        {
            if (planningAdvice != PlanningAdvice.列入规划 && planningAdvice != PlanningAdvice.暂不考虑 && planningAdvice != PlanningAdvice.正在解决)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }

        /// <summary>
        /// 职务枚举值是否有效
        /// </summary>
        /// <param name="duty"></param>
        /// <param name="propertyName"></param>
        public static void IsInvalid(this Duty duty, string propertyName)
        {
            if (duty != Duty.网优人员)
            {
                throw new DomainFault("{0}无效", propertyName);
            }
        }
    }
}
