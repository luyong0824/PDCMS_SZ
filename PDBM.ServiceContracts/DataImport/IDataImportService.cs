using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.DataImport;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.DataImport
{
    /// <summary>
    /// 数据导入服务接口
    /// </summary>
    [ServiceContract]
    public interface IDataImportService : IDistributedService
    {
        /// <summary>
        /// 导入运营商基站规划
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="currentCompanyNature">创建人所在公司性质</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportOperatorsPlanningBS(Guid excelFileId, Guid createUserId, Guid companyId, int currentCompanyNature);

        /// <summary>
        /// 导入运营商共享基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="currentCompanyNature">创建人所在公司性质</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportOperatorsSharingBS(Guid excelFileId, Guid createUserId, Guid companyId, int currentCompanyNature);

        /// <summary>
        /// 导入购置基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportPurchaseBS(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入基站规划
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportPlanning(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入基站改造
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportRemodeling(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportPlace(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入逻辑号
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportLogicalNumber(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入业务量
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="profession">专业</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportBusinessVolume(Guid excelFileId, int logicalType, int profession, Guid createUserId);

        /// <summary>
        /// 导入资源信息
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportResources(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入新增基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportNewPlanningBS(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入改造基站
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportNewRemodelingBS(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入立项信息
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportProjectCodeList(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入采购清单
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportMaterialSpecList(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入基站建设
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportPlanningApply(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入室分建设
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportPlanningApplyID(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入室分规划
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportPlanningID(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入室分改造
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportRemodelingID(Guid excelFileId, Guid createUserId);

        /// <summary>
        /// 导入室分
        /// </summary>
        /// <param name="excelFileId">Excel文件Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>导入错误对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ImportErrorObject> ImportPlaceID(Guid excelFileId, Guid createUserId);
    }
}
