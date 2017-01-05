using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 业务量服务接口
    /// </summary>
    [ServiceContract]
    public interface IBusinessVolumeService : IDistributedService
    {
        /// <summary>
        /// 根据业务量Id获取任务
        /// </summary>
        /// <param name="id">业务量Id</param>
        /// <returns>业务量维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        BusinessVolumeMaintObject GetBusinessVolumeById(Guid id);

        /// <summary>
        /// 新增或者修改业务量
        /// </summary>
        /// <param name="businessVolumeMaintObject">要新增或者修改的业务量维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateBusinessVolume(BusinessVolumeMaintObject businessVolumeMaintObject);

        /// <summary>
        /// 删除业务量
        /// </summary>
        /// <param name="towerMaintObjects">要删除的业务量维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveBusinessVolumes(IList<BusinessVolumeMaintObject> businessVolumeMaintObjects);

        /// <summary>
        /// 根据条件获取分页业务量导入列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>分页业务量导入列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int logicalType, string logicalNumber, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取分页基站业务报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>分页基站业务报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeName, Guid areaId, Guid reseauId, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取网格业务报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeReseau(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取区域业务报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeArea(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取公司业务报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取基站业务月报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortOrder">排序方向</param>
        /// <returns>基站业务月报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthPlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, string placeName, int profession, Guid companyId, string sortField, string sortOrder);

        /// <summary>
        /// 根据条件获取网格业务月报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务月报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthReseau(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取区域业务月报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务月报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthArea(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取公司业务月报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务月报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取基站业务月增量报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>基站业务月增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthRisePlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取基站业务年增量报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>基站业务年增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeYearRisePlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取网格业务月增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务月增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthRiseReseau(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取网格业务年增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务年增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeYearRiseReseau(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取区域业务月增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务月增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthRiseArea(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取区域业务年增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务年增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeYearRiseArea(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取公司业务月增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务月增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeMonthRiseCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取公司业务年增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务年增量报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeYearRiseCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取网格年度成长报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格年度成长报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeYearGrowthReseau(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取区域年度成长报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域年度成长报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeYearGrowthArea(DateTime beginDate, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取公司年度成长报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司年度成长报表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBusinessVolumeYearGrowthCompany(DateTime beginDate, int profession, Guid companyId);
    }
}
