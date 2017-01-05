using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BaseData
{
    [ServiceContract]
    public interface IPlacePropertyLogService : IDistributedService
    {
        /// <summary>
        /// 根据站点属性Id获取站点属性
        /// </summary>
        /// <param name="id">站点属性Id</param>
        /// <returns>站点属性维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlacePropertyLogMaintObject GetPlacePropertyLogById(Guid id);

        /// <summary>
        /// 获取运营商共享历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="companyNameId">运营商名称Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlacePropertyLog(int propertyType, Guid parentId, int companyNameId);
    }
}
