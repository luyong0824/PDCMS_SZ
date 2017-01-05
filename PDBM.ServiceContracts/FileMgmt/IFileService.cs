using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.FileMgmt;
using PDBM.Infrastructure.Common;
using PDBM.DataTransferObjects.BaseData;

namespace PDBM.ServiceContracts.FileMgmt
{
    /// <summary>
    /// 文件服务接口
    /// </summary>
    [ServiceContract]
    public interface IFileService : IDistributedService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileUploadObject">文件上传对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void UploadFile(FileUploadObject fileUploadObject);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">要下载的文件存储路径</param>
        /// <returns>要下载的文件流</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        Stream DownloadFile(string filePath);

        /// <summary>
        /// 根据文件Id获取要下载的文件对象
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <returns>要下载的文件对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        FileDownloadObject GetFileDownloadByFileId(Guid fileId);

        /// <summary>
        /// 根据文件Id列表获取文件列表
        /// </summary>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetFiles(string fileIdList);

        /// <summary>
        /// 获取盲点反馈附件
        /// </summary>
        /// <param name="blindSpotFeedBackId">盲点反馈Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetBlindSpotFeedBackFiles(Guid blindSpotFeedBackId, string fileIdList);

        /// <summary>
        /// 获取基站改造附件
        /// </summary>
        /// <param name="remodelingId">基站改造Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetRemodelingFiles(Guid remodelingId, string fileIdList);

        /// <summary>
        /// 获取工程现场摄像附件
        /// </summary>
        /// <param name="engineeringTaskId">工程任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetEngineeringProgressFiles(Guid engineeringTaskId, string fileIdList);

        /// <summary>
        /// 获取项目现场摄像附件
        /// </summary>
        /// <param name="projectTaskId">项目任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetProjectProgressFiles(Guid projectTaskId, string fileIdList);

        /// <summary>
        /// 获取施工图附件
        /// </summary>
        /// <param name="engineeringTaskId">工程任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetEngineeringDesignFiles(Guid engineeringTaskId, string fileIdList);

        /// <summary>
        /// 获取总设图附件
        /// </summary>
        /// <param name="projectTaskId">项目任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetGeneralDesignFiles(Guid projectTaskId, string fileIdList);

        /// <summary>
        /// 获取建设申请附件
        /// </summary>
        /// <param name="planningApplyId">建设申请Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetPlanningApplyFiles(Guid planningApplyId, string fileIdList);

        /// <summary>
        /// 获取规划附件
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetPlanningFiles(Guid planningId, string fileIdList);

        /// <summary>
        /// 获取寻址确认示意图
        /// </summary>
        /// <param name="addressingId">寻址确认Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetAddressingFiles(Guid addressingId, string fileIdList);

        /// <summary>
        /// 获取购置站点示意图
        /// </summary>
        /// <param name="purchaseId">购置站点Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetPurchaseFiles(Guid purchaseId, string fileIdList);

        /// <summary>
        /// 获取站点示意图
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetPlaceFiles(Guid placeId, string fileIdList);

        /// <summary>
        /// 获取工作流过程实例附件
        /// </summary>
        /// <param name="wfProcessInstanceId">工作流过程实例Id</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetWFProcessInstanceFiles(Guid wfProcessInstanceId);

        /// <summary>
        /// 获取工作流活动实例附件
        /// </summary>
        /// <param name="wfActivityInstanceId">工作流活动实例Id</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetWFActivityInstanceFiles(Guid wfActivityInstanceId);

        /// <summary>
        /// 获取铁塔图纸
        /// </summary>
        /// <param name="towerId">铁塔Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetTowerFiles(Guid towerId, string fileIdList);

        /// <summary>
        /// 获取塔基图纸
        /// </summary>
        /// <param name="towerBaseId">铁塔基础Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetTowerBaseFiles(Guid towerBaseId, string fileIdList);

        /// <summary>
        /// 获取机房图纸
        /// </summary>
        /// <param name="machineRoomId">机房Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetMachineRoomFiles(Guid machineRoomId, string fileIdList);

        /// <summary>
        /// 获取路由图
        /// </summary>
        /// <param name="externalElectricPowerId">外电引入Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetExternalElectricPowerFiles(Guid externalElectricPowerId, string fileIdList);

        /// <summary>
        /// 获取安装图纸
        /// </summary>
        /// <param name="equipmentInstallId">设备安装Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetEquipmentInstallFiles(Guid equipmentInstallId, string fileIdList);

        /// <summary>
        /// 获取地勘报告
        /// </summary>
        /// <param name="addressExplorId">地质勘探Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetAddressExplorFiles(Guid addressExplorId, string fileIdList);

        /// <summary>
        /// 获取动测报告
        /// </summary>
        /// <param name="foundationTestId">桩基动测Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetFoundationTestFiles(Guid foundationTestId, string fileIdList);

        /// <summary>
        /// 获取铁塔图纸
        /// </summary>
        /// <param name="towerLogId">铁塔Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetTowerLogFiles(Guid towerLogId, string fileIdList);

        /// <summary>
        /// 获取塔基图纸
        /// </summary>
        /// <param name="towerBaseLogId">铁塔基础Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetTowerBaseLogFiles(Guid towerBaseLogId, string fileIdList);

        /// <summary>
        /// 获取机房图纸
        /// </summary>
        /// <param name="machineRoomLogId">机房Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetMachineRoomLogFiles(Guid machineRoomLogId, string fileIdList);

        /// <summary>
        /// 获取路由图
        /// </summary>
        /// <param name="externalElectricPowerLogId">外电引入Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetExternalElectricPowerLogFiles(Guid externalElectricPowerLogId, string fileIdList);

        /// <summary>
        /// 获取安装图纸
        /// </summary>
        /// <param name="equipmentInstallLogId">设备安装Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetEquipmentInstallLogFiles(Guid equipmentInstallLogId, string fileIdList);

        /// <summary>
        /// 获取地勘报告
        /// </summary>
        /// <param name="addressExplorLogId">地质勘探Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetAddressExplorLogFiles(Guid addressExplorLogId, string fileIdList);

        /// <summary>
        /// 获取动测报告
        /// </summary>
        /// <param name="foundationTestLogId">桩基动测Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetFoundationTestLogFiles(Guid foundationTestLogId, string fileIdList);

        /// <summary>
        /// 获取现场影像
        /// </summary>
        /// <param name="id">子任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetProgressFiles(Guid id, string fileIdList);

        /// <summary>
        /// 获取现场影像历史记录
        /// </summary>
        /// <param name="id">子任务历史记录Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetProgressLogFiles(Guid id, string fileIdList);

        /// <summary>
        /// 获取隐患上报单附件
        /// </summary>
        /// <param name="id">隐患上保单Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetWorkApplyFiles(Guid id, string fileIdList);

        /// <summary>
        /// 获取零星派工单附件
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetWorkOrderFiles(Guid id, string fileIdList);

        /// <summary>
        /// 获取零星派工单执行说明附件
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<FileObject> GetWorkOrderWFFiles(Guid id, string fileIdList);
    }
}
