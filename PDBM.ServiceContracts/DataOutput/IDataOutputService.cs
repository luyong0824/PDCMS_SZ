using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.DataImport;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.DataOutput
{
    [ServiceContract]
    public interface IDataOutputService : IDistributedService
    {
        /// <summary>
        /// 导出租赁人月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>租赁人月报Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportAddressingMonthUserExcel(DateTime beginDate, DateTime endDate, Guid departmentId, int profession, Guid companyId);

        /// <summary>
        /// 导出租赁月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>租赁月报Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportAddressingMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId);

        /// <summary>
        /// 导出部门建设月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="beginDateYear">起始年份1月1号</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>部门建设月报Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportProjectTaskDepartmentExcel(DateTime beginDate, DateTime beginDateYear, int profession, Guid companyId);

        /// <summary>
        /// 导出项目经理月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="beginDateYear">起始年份1月1号</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>项目经理月报Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportProjectTaskProjectManagerExcel(DateTime beginDate, DateTime beginDateYear, Guid departmentId, int profession, Guid companyId);

        /// <summary>
        /// 导出公司年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>公司年度成长(话务量)Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeYearGrowthCompanyTVExcel(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 导出公司年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>公司年度成长(业务量)Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeYearGrowthCompanyBVExcel(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 导出区域年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>区域年度成长(话务量)Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeYearGrowthAreaTVExcel(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 导出区域年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>区域年度成长(业务量)Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeYearGrowthAreaBVExcel(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 导出网格年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>网格年度成长(话务量)Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeYearGrowthReseauTVExcel(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 导出网格年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>网格年度成长(业务量)Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeYearGrowthReseauBVExcel(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 导出公司业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>公司业务月清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeMonthCompanyExcel(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 导出区域业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>区域业务月清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeMonthAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId);

        /// <summary>
        /// 导出网格业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>网格业务月清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId);

        /// <summary>
        /// 导出基站业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>基站业务月清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeMonthPlaceExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, string placeName, int profession, Guid companyId);

        /// <summary>
        /// 导出公司业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeCompanyExcel(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 导出区域业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId);

        /// <summary>
        /// 导出网格业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId);

        /// <summary>
        /// 导出基站业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>基站业务清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportBusinessVolumeExcel(DateTime beginDate, DateTime endDate, string placeName, Guid areaId, Guid reseauId, int profession, Guid companyId);

        /// <summary>
        /// 导出逻辑号业务清单
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>逻辑号业务清单Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportLogicalBusinessVolumeExcel(DateTime beginDate, DateTime endDate, int logicalType, string logicalNumber, int profession, Guid companyId);

        /// <summary>
        /// 导出站点
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="state">状态</param>
        /// <returns>站点的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportPlaceAllExcel(int profession, string placeName, Guid areaId, Guid reseauId, Guid placeOwner, int state);

        /// <summary>
        /// 导出工程设计清单
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="designCustomerId">设计单位</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>工程设计清单的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportEngineeringDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            string designRealName, Guid designCustomerId, int profession,Guid companyId);

        /// <summary>
        /// 导出工程进度表
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="engineeringProgress">工程进度</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="constructionCustomerId">施工单位Id</param>
        /// <param name="supervisionCustomerId">监理单位Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>工程进度表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportEngineeringProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel, int engineeringProgress, int projectType,
            Guid projectManagerId, Guid constructionCustomerId, Guid supervisionCustomerId, int profession, Guid companyId);

        /// <summary>
        /// 导出项目设计清单
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="generalDesignId">总设单位Id</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>项目设计清单的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportProjectDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, Guid generalDesignId, string designRealName, int profession, Guid companyId);

        /// <summary>
        /// 导出项目进度表
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="isOverTime">是否超时</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>项目进度表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportProjectProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int projectType,
            int projectProgress, Guid projectManagerId, int isOverTime, int profession, Guid companyId);

        /// <summary>
        /// 根据条件导出租赁进度表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="addressingDepartmentId">租赁部门</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <param name="isAppoint">指定租赁</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>租赁进度表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportAddressingReportExcel(DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, Guid placeCategoryId,
            Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingDepartmentId, Guid addressingUserId, int isAppoint, Guid companyId);

        /// <summary>
        /// 根据条件获取分页基站站点列表
        /// </summary>
        /// <param name="placeCode">基站编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="g2Mark">2G</param>
        /// <param name="d2Mark">2D</param>
        /// <param name="g3Mark">3G</param>
        /// <param name="g4Mark">4G</param>
        /// <param name="g2Number">2G逻辑号</param>
        /// <param name="d2Number">2D逻辑号</param>
        /// <param name="g3Number">3G逻辑号</param>
        /// <param name="g4Number">4G逻辑号</param>
        /// <param name="allMark">全部</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportLogicalNumbersExcel(string placeCode, string placeName, int profession, Guid areaId, Guid reseauId,
            int g2Mark, int d2Mark, int g3Mark, int g4Mark, string g2Number, string d2Number, string g3Number, string g4Number, int allMark);

        /// <summary>
        /// 导出基站清单
        /// </summary>
        /// <param name="placeCode">基站编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportPlacesBaseStationExcel(string placeCode, string placeName, Guid placeCategoryId, Guid placeOwner, Guid areaId, Guid reseauId, int importance, int state, int profession, Guid companyId);

        /// <summary>
        /// 获取运营商规划列表数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportOperatorsPlanningsExcel();

        /// <summary>
        /// 获取新增基站建设进度表数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportConstructionTaskPlanningsExcel();

        /// <summary>
        /// 获取新改造基站建设进度表数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportConstructionTaskRemodeingsExcel();

        /// <summary>
        /// 获取物资清单
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportMaterialPurchaseExcel(string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string materialName, int doState);

        /// <summary>
        /// 导出项目信息表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="propertyRightSql">产权</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="constructionMethod">建设方式</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportProjectInformationExcel(DateTime beginDate, DateTime endDate, string propertyRightSql, string groupPlaceCode, string placeName, Guid areaId, Guid reseauId, int constructionMethod, int constructionProgress);

        /// <summary>
        /// 导出隐患上报清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="customerId">申请单位Id</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="isSoved">是否解决</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportWorkApplysExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid customerId, int orderState, int isSoved, Guid createUserId);

        /// <summary>
        /// 导出零星派工清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="workBigClassId">派工大类Id</param>
        /// <param name="workSmallClassId">派工小类Id</param>
        /// <param name="customerId">派工单位id</param>
        /// <param name="maintainContactMan">派工联系人</param>
        /// <param name="sendUserId">网格经理Id</param>
        /// <param name="isFinish">是否完成</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="createUserId">申请人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportWorkOrdersExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid workBigClassId, Guid workSmallClassId, Guid customerId, string maintainContactMan, Guid sendUserId, int isFinish, int orderState, Guid createUserId);

        /// <summary>
        /// 导出隐患立项清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="isProject">是否立项</param>
        /// <param name="sendUserId">网格经理</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportWorkApplyProjectsExcel(DateTime beginDate, DateTime endDate, string title, string projectCode, int isProject, Guid sendUserId);

        /// <summary>
        /// 导出清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">型号类别</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="orderCode">订单编号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string ExportProjectMaterial(DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, string customerName, int materialSpecType, string materialSpecName, string orderCode);
    }
}
