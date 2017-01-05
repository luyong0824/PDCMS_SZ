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
    /// 站点业务量服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlaceBusinessVolumeService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改站点业务量
        /// </summary>
        /// <param name="placeBusinessVolumeMaintObject">要新增或者修改的站点业务量维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdatePlaceBusinessVolume(PlaceBusinessVolumeMaintObject placeBusinessVolumeMaintObject);
    }
}
