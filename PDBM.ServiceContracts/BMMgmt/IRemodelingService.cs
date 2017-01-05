using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 改造服务接口
    /// </summary>
    [ServiceContract]
    public interface IRemodelingService : IDistributedService
    {
        /// <summary>
        /// 根据改造Id获取改造
        /// </summary>
        /// <param name="id">改造Id</param>
        /// <returns>改造维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        RemodelingMaintObject GetRemodelingById(Guid id);

        /// <summary>
        /// 根据条件获取分页改造列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectType">项目类型</param>
        /// <param name="orderState">改造状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns>分页改造列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetRemodelingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int projectType, int orderState, Guid createUserId);

        /// <summary>
        /// 新增或者修改改造
        /// </summary>
        /// <param name="remodelingMaintObject">要新增或者修改的改造维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateRemodeling(RemodelingMaintObject remodelingMaintObject);

        /// <summary>
        /// 删除改造
        /// </summary>
        /// <param name="remodelingMaintObjects">要删除的改造维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveRemodelings(IList<RemodelingMaintObject> remodelingMaintObjects);

        /// <summary>
        /// 根据改造确认Id获取改造确认打印信息
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <returns>改造确认打印对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        RemodelingPrintObject GetRemodelingPrintById(Guid id);
    }
}
