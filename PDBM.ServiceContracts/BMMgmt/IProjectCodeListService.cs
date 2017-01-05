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
    /// 立项信息服务接口
    /// </summary>
    [ServiceContract]
    public interface IProjectCodeListService : IDistributedService
    {
        /// <summary>
        /// 根据立项信息表Id获取立项信息
        /// </summary>
        /// <param name="id">立项信息表Id</param>
        /// <returns>立项信息维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectCodeListMaintObject GetProjectCodeListById(Guid id);

        /// <summary>
        /// 新增或者修改立项信息
        /// </summary>
        /// <param name="projectCodeListMaintObject">要新增或者修改的立项信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateProjectCodeList(ProjectCodeListMaintObject projectCodeListMaintObject);

        /// <summary>
        /// 获取分页立项信息列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectCodeListPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, Guid createUserId);
    }
}
