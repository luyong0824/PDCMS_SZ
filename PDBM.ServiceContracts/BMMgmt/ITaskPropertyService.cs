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
    /// 子任务服务接口
    /// </summary>
    [ServiceContract]
    public interface ITaskPropertyService : IDistributedService
    {
        /// <summary>
        /// 根据任务属性Id获取任务属性
        /// </summary>
        /// <param name="id">任务属性Id</param>
        /// <returns>任务属性维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        TaskPropertyMaintObject GetTaskPropertyById(Guid id);

        /// <summary>
        /// 新增或者修改任务属性
        /// </summary>
        /// <param name="taskPropertyMaintObject">要新增或者修改的任务属性维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateTaskProperty(TaskPropertyMaintObject taskPropertyMaintObject);

        /// <summary>
        /// 保存工程信息登记
        /// </summary>
        /// <param name="taskPropertyMaintObject">要修改的任务属性维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveTaskProperty(TaskPropertyMaintObject taskPropertyMaintObject);

        /// <summary>
        /// 设置施工单位
        /// </summary>
        /// <param name="taskPropertyMaintObjects">要设置施工单位的任务维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SettingConstructionCustomer(IList<TaskPropertyMaintObject> taskPropertyMaintObjects);
    }
}
