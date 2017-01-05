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
    /// 站点设计信息服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlaceDesignService : IDistributedService
    {
        /// <summary>
        /// 根据站点设计信息Id获取任务
        /// </summary>
        /// <param name="id">站点设计信息Id</param>
        /// <returns>站点设计信息维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlaceDesignMaintObject GetPlaceDesignById(Guid id);

        /// <summary>
        /// 新增或者修改站点设计信息
        /// </summary>
        /// <param name="placeDesignMaintObject">要新增或者修改的站点设计信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdatePlaceDesign(PlaceDesignMaintObject placeDesignMaintObject);

        /// <summary>
        /// 删除站点设计信息
        /// </summary>
        /// <param name="towerMaintObjects">要删除的站点设计信息维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlaceDesign(IList<PlaceDesignMaintObject> placeDesignMaintObjects);

        /// <summary>
        /// 指定设计单位
        /// </summary>
        /// <param name="placeDesignMaintObject">站点设计信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveAppointDesign(PlaceDesignMaintObject placeDesignMaintObject);

        /// <summary>
        /// 指定设计人员
        /// </summary>
        /// <param name="placeDesignMaintObject">站点设计信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveAppointDesignUser(PlaceDesignMaintObject placeDesignMaintObject);

        /// <summary>
        /// 施工设计
        /// </summary>
        /// <param name="addressingEditorObject">寻址确认编辑维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveConstructionDesign(AddressingEditorObject addressingEditorObject);

        /// <summary>
        /// 指定施工单位
        /// </summary>
        /// <param name="addressingEditorObject">寻址确认编辑维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveCustomer(AddressingEditorObject addressingEditorObject);

        /// <summary>
        /// 指定工程经理及监理单位
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveManagerAndSupervisor(PlaceDesignMaintObject placeDesignMaintObject);

        /// <summary>
        /// 改造站指定设计单位
        /// </summary>
        /// <param name="placeDesignMaintObject">站点设计信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveAppointDesignR(PlaceDesignMaintObject placeDesignMaintObject);

        /// <summary>
        /// 改造站指定设计人员
        /// </summary>
        /// <param name="placeDesignMaintObject">站点设计信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveAppointDesignUserR(PlaceDesignMaintObject placeDesignMaintObject);

        /// <summary>
        /// 改造站施工设计
        /// </summary>
        /// <param name="remodelingEditorObject">改造确认编辑维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveConstructionDesignR(RemodelingEditorObject remodelingEditorObject);

        /// <summary>
        /// 改造站指定施工单位
        /// </summary>
        /// <param name="remodelingEditorObject">改造确认编辑维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveCustomerR(RemodelingEditorObject remodelingEditorObject);

        /// <summary>
        /// 改造站指定工程经理及监理单位
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveManagerAndSupervisorR(PlaceDesignMaintObject placeDesignMaintObject);
    }
}
