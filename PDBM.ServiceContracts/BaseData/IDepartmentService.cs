using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BaseData
{
    /// <summary>
    /// 部门服务接口
    /// </summary>
    [ServiceContract]
    public interface IDepartmentService : IDistributedService
    {
        /// <summary>
        /// 根据部门Id获取部门
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>部门维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        DepartmentMaintObject GetDepartmentById(Guid id);

        /// <summary>
        /// 根据公司Id获取部门列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns>部门列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetDepartments(Guid companyId);

        /// <summary>
        /// 根据公司Id获取状态为使用的部门列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns>部门选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<DepartmentSelectObject> GetUsedDepartments(Guid companyId);

        /// <summary>
        /// 根据公司Id和岗位Id获取状态为使用的部门列表，用于发送工作流实例
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="postId">岗位Id</param>
        /// <returns>部门列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetUsedDepartmentsBySend(Guid companyId, Guid postId);

        /// <summary>
        /// 新增或者修改部门
        /// </summary>
        /// <param name="departmentMaintObject">要新增或者修改的部门维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateDepartment(DepartmentMaintObject departmentMaintObject);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentMaintObjects">要删除的部门维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveDepartments(IList<DepartmentMaintObject> departmentMaintObjects);
    }
}
