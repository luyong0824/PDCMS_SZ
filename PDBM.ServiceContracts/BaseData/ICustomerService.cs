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
    /// <summary>
    /// 往来单位服务接口
    /// </summary>
    [ServiceContract]
    public interface ICustomerService : IDistributedService
    {
        /// <summary>
        /// 根据往来单位Id获取往来单位
        /// </summary>
        /// <param name="id">往来单位Id</param>
        /// <returns>往来单位维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        CustomerMaintObject GetCustomerById(Guid id);

        /// <summary>
        /// 根据条件获取分页往来单位列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="customerType">往来单位分类</param>
        /// <param name="customerCode">往来单位编码</param>
        /// <param name="customerName">往来单位简称</param>
        /// <param name="customerFullName">往来单位全称</param>
        /// <param name="state">状态</param>
        /// <returns>分页往来单位列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetCustomersPage(int pageIndex, int pageSize, int customerType, string customerCode, string customerName, string customerFullName, int state);

        /// <summary>
        /// 新增或者修改往来单位
        /// </summary>
        /// <param name="customerMaintObject">要新增或者修改的往来单位维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateCustomer(CustomerMaintObject customerMaintObject);

        /// <summary>
        /// 删除往来单位
        /// </summary>
        /// <param name="customerMaintObjects">要删除的往来单位维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveCustomers(IList<CustomerMaintObject> customerMaintObjects);

        /// <summary>
        /// 根据条件获取往来单位分页列表，用于选择往来单位
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="customerCode">往来单位编码</param>
        /// <param name="customerName">往来单位简称</param>
        /// <param name="customerFullName">往来单位全称</param>
        /// <param name="customerType">单位分类</param>
        /// <returns>分页往来单位列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetCustomersPageBySelect(int pageIndex, int pageSize, string customerCode, string customerName, string customerFullName, int customerType);

        /// <summary>
        /// 获取往来单位
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<CustomerMaintObject> GetCustomers();

        /// <summary>
        /// 获取所有状态为使用的往来单位名称列表
        /// </summary>
        /// <returns>往来单位名称列表Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAllUsedCustomers();

        /// <summary>
        /// 根据往来单位分类获取往来单位列表
        /// </summary>
        /// <param name="customerType">往来单位分类</param>
        /// <returns>往来单位维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<CustomerMaintObject> GetCustomersByType(int customerType);

        /// <summary>
        /// 获取所有往来单位列表
        /// </summary>
        /// <returns>往来单位维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<CustomerMaintObject> GetCustomersByAll();

        /// <summary>
        /// 根据用户Id获取往来单位
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>往来单位维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        CustomerMaintObject GetCustomerByUserId(Guid userId);
    }
}
