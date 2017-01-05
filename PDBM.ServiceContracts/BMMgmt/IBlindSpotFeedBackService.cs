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
    /// 盲点反馈服务接口
    /// </summary>
    [ServiceContract]
    public interface IBlindSpotFeedBackService : IDistributedService
    {
        /// <summary>
        /// 根据盲点反馈Id获取任务
        /// </summary>
        /// <param name="id">盲点反馈Id</param>
        /// <returns>盲点反馈维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        BlindSpotFeedBackMaintObject GetBlindSpotFeedBackById(Guid id);

        /// <summary>
        /// 新增或者修改盲点反馈
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObject">要新增或者修改的盲点反馈维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateBlindSpotFeedBack(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject);

        /// <summary>
        /// 删除盲点反馈
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObjects">要删除的盲点反馈维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveBlindSpotFeedBacks(IList<BlindSpotFeedBackMaintObject> blindSpotFeedBackMaintObjects);

        /// <summary>
        /// 根据条件获取分页盲点反馈导入列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">盲点地名</param>
        /// <param name="doState">处理状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>分页盲点反馈导入列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetBlindSpotFeedBacksPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, Guid areaId, string placeName, int doState, Guid createUserId, Guid companyId);

        /// <summary>
        /// 保存反馈处理
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObject">要修改的盲点反馈维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveBlindSpotHanding(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject);

        /// <summary>
        /// 保存盲点反馈(移动端)
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObject">要保存的盲点反馈维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveBlindSpotFeedBackMobile(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject);
    }
}
