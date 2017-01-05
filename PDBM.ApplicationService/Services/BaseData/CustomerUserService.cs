using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BaseData
{
    public class CustomerUserService : DataService, ICustomerUserService
    {
        private readonly IRepository<CustomerUser> customerUserRepository;

        public CustomerUserService(IRepositoryContext context,
            IRepository<CustomerUser> customerUserRepository)
            : base(context)
        {
            this.customerUserRepository = customerUserRepository;
        }

        /// <summary>
        /// 根据公司Id，部门Id，往来单位Id获取往来单位用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="customerId">往来单位Id</param>
        /// <returns>往来单位用户列表的Json字符串</returns>
        public string GetCustomerUsers(Guid companyId, Guid departmentId, Guid customerId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryCustomerUsers", parameters))
            {
                dt.Columns.Add("isLeaf", typeof(bool), "Convert(IsLeafStr, 'System.Boolean')");
                dt.Columns.Add("asyncLoad", typeof(bool), "Convert(AsyncLoadStr, 'System.Boolean')");
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者删除往来单位用户
        /// </summary>
        /// <param name="customerUserMaintObjects">要新增或者删除的往来单位用户维护对象列表</param>
        public void AddOrRemoveCustomerUsers(IList<CustomerUserMaintObject> customerUserMaintObjects)
        {
            foreach (var customerUserMaintObject in customerUserMaintObjects)
            {
                if (customerUserMaintObject.Id == Guid.Empty)
                {
                    CustomerUser customerUser = AggregateFactory.CreateCustomerUser(customerUserMaintObject.CustomerId,
                        customerUserMaintObject.UserId, customerUserMaintObject.CreateUserId);
                    customerUserRepository.Add(customerUser);
                }
                else
                {
                    CustomerUser customerUser = customerUserRepository.FindByKey(customerUserMaintObject.Id);
                    if (customerUser != null)
                    {
                        customerUserRepository.Remove(customerUser);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_CustomerIdUserId"))
                {
                    throw new ApplicationFault("同一用户只能关联一个往来单位");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_CustomerUser_dbo.tbl_Customer_CustomerId"))
                {
                    throw new ApplicationFault("选择的往来单位在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_CustomerUser_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("选择的用户在系统中不存在");
                }
                throw ex;
            }
        }
    }
}
