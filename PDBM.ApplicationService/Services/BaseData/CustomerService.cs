using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
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
    /// <summary>
    /// 往来单位应用层服务
    /// </summary>
    public class CustomerService : DataService, ICustomerService
    {
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<CustomerUser> customerUserRepository;
        private readonly IRepository<MaterialSpec> materialSpecRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<PlaceDesign> placeDesignRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly ICodeSeedRepository codeSeedRepository;

        public CustomerService(IRepositoryContext context,
            IRepository<Customer> customerRepository,
            IRepository<CustomerUser> customerUserRepository,
            IRepository<MaterialSpec> materialSpecRepository,
            IRepository<User> userRepository,
            IRepository<PlaceDesign> placeDesignRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<WorkApply> workApplyRepository,
            IRepository<WorkOrder> workOrderRepository,
            ICodeSeedRepository codeSeedRepository)
            : base(context)
        {
            this.customerRepository = customerRepository;
            this.customerUserRepository = customerUserRepository;
            this.materialSpecRepository = materialSpecRepository;
            this.userRepository = userRepository;
            this.placeDesignRepository = placeDesignRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.workApplyRepository = workApplyRepository;
            this.workOrderRepository = workOrderRepository;
            this.codeSeedRepository = codeSeedRepository;
        }

        /// <summary>
        /// 根据往来单位Id获取往来单位
        /// </summary>
        /// <param name="id">往来单位Id</param>
        /// <returns>往来单位维护对象</returns>
        public CustomerMaintObject GetCustomerById(Guid id)
        {
            Customer customer = customerRepository.FindByKey(id);
            if (customer != null)
            {
                CustomerMaintObject customerMaintObject = MapperHelper.Map<Customer, CustomerMaintObject>(customer);
                if (customer.CustomerUserId != Guid.Empty)
                {
                    User user = userRepository.FindByKey(customer.CustomerUserId);
                    customerMaintObject.CustomerUserId = customer.CustomerUserId;
                    customerMaintObject.CustomerUserFullName = user.FullName;
                }
                else
                {
                    customerMaintObject.CustomerUserId = Guid.Empty;
                    customerMaintObject.CustomerUserFullName = "请选择";
                }
                return customerMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的往来单位在系统中不存在");
            }
        }

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
        public string GetCustomersPage(int pageIndex, int pageSize, int customerType, string customerCode, string customerName, string customerFullName, int state)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "CustomerType", Type = SqlDbType.Int, Value = customerType });
            parameters.Add(new Parameter() { Name = "CustomerCode", Type = SqlDbType.NVarChar, Value = customerCode });
            parameters.Add(new Parameter() { Name = "CustomerName", Type = SqlDbType.NVarChar, Value = customerName });
            parameters.Add(new Parameter() { Name = "CustomerFullName", Type = SqlDbType.NVarChar, Value = customerFullName });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryCustomersPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改往来单位
        /// </summary>
        /// <param name="customerMaintObject">要新增或者修改的往来单位维护对象</param>
        public void AddOrUpdateCustomer(CustomerMaintObject customerMaintObject)
        {
            if (customerMaintObject.Id == Guid.Empty)
            {
                Customer customer = AggregateFactory.CreateCustomer((CustomerType)customerMaintObject.CustomerType, codeSeedRepository.GenerateCode("Customer"), customerMaintObject.CustomerName, customerMaintObject.CustomerFullName, customerMaintObject.CustomerUserId, customerMaintObject.ContactMan, customerMaintObject.ContactTel, customerMaintObject.ContactAddr, customerMaintObject.Remarks, (State)customerMaintObject.State, customerMaintObject.CreateUserId);
                customerRepository.Add(customer);
            }
            else
            {
                Customer customer = customerRepository.FindByKey(customerMaintObject.Id);
                if (customer != null)
                {
                    customer.Modify((CustomerType)customerMaintObject.CustomerType, customerMaintObject.CustomerName, customerMaintObject.CustomerFullName, customerMaintObject.CustomerUserId, customerMaintObject.ContactMan, customerMaintObject.ContactTel, customerMaintObject.ContactAddr, customerMaintObject.Remarks, (State)customerMaintObject.State, customerMaintObject.ModifyUserId);
                    customerRepository.Update(customer);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_CustomerCode"))
                {
                    throw new ApplicationFault("往来单位编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_CustomerName"))
                {
                    throw new ApplicationFault("往来单位简称重复");
                }
                else if (ex.Message.Contains("IX_UQ_CustomerFullName"))
                {
                    throw new ApplicationFault("往来单位全称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除往来单位
        /// </summary>
        /// <param name="customerMaintObjects">要删除的往来单位维护对象列表</param>
        public void RemoveCustomers(IList<CustomerMaintObject> customerMaintObjects)
        {
            foreach (CustomerMaintObject customerMaintObject in customerMaintObjects)
            {
                Customer customer = customerRepository.FindByKey(customerMaintObject.Id);
                if (customer != null)
                {
                    if (materialSpecRepository.Exists(Specification<MaterialSpec>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在设计规格中使用", customer.CustomerCode);
                    }
                    if (placeDesignRepository.Exists(Specification<PlaceDesign>.Eval(entity => entity.DesignCustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为设计单位使用", customer.CustomerCode);
                    }
                    if (placeDesignRepository.Exists(Specification<PlaceDesign>.Eval(entity => entity.SupervisorCustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为监理单位使用", customer.CustomerCode);
                    }
                    if (towerRepository.Exists(Specification<Tower>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为铁塔施工单位使用", customer.CustomerCode);
                    }
                    if (towerBaseRepository.Exists(Specification<TowerBase>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为铁塔基础施工单位使用", customer.CustomerCode);
                    }
                    if (machineRoomRepository.Exists(Specification<MachineRoom>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为机房施工单位使用", customer.CustomerCode);
                    }
                    if (externalElectricPowerRepository.Exists(Specification<ExternalElectricPower>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为外电引入施工单位使用", customer.CustomerCode);
                    }
                    if (equipmentInstallRepository.Exists(Specification<EquipmentInstall>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为设备安装施工单位使用", customer.CustomerCode);
                    }
                    if (addressExplorRepository.Exists(Specification<AddressExplor>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为地质勘探施工单位使用", customer.CustomerCode);
                    }
                    if (foundationTestRepository.Exists(Specification<FoundationTest>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被作为桩基动测施工单位使用", customer.CustomerCode);
                    }
                    if (workApplyRepository.Exists(Specification<WorkApply>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在隐患上报单中使用", customer.CustomerCode);
                    }
                    if (workOrderRepository.Exists(Specification<WorkOrder>.Eval(entity => entity.CustomerId == customer.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在零星派工单中使用", customer.CustomerCode);
                    }
                    customerRepository.Remove(customer);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_MaterialSpec_dbo.tbl_Customer_CustomerId"))
                {
                    throw new ApplicationFault("已在设计规格中使用");
                }
                if (ex.Message.Contains("FK_dbo.tbl_WorkApply_dbo.tbl_Customer_CustomerId"))
                {
                    throw new ApplicationFault("已在隐患上报单中使用");
                }
                if (ex.Message.Contains("FK_dbo.tbl_WorkOrder_dbo.tbl_Customer_CustomerId"))
                {
                    throw new ApplicationFault("已在零星派工单中使用");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 根据条件获取往来单位分页列表，用于选择往来单位
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="customerCode">往来单位编码</param>
        /// <param name="customerName">往来单位简称</param>
        /// <param name="customerFullName">往来单位全称</param>
        /// <param name="customerType">单位分类</param>
        /// <param name="state">状态</param>
        /// <returns>分页往来单位列表的Json字符串</returns>
        public string GetCustomersPageBySelect(int pageIndex, int pageSize, string customerCode, string customerName, string customerFullName, int customerType)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "CustomerCode", Type = SqlDbType.NVarChar, Value = customerCode });
            parameters.Add(new Parameter() { Name = "CustomerName", Type = SqlDbType.NVarChar, Value = customerName });
            parameters.Add(new Parameter() { Name = "CustomerFullName", Type = SqlDbType.NVarChar, Value = customerFullName });
            parameters.Add(new Parameter() { Name = "CustomerType", Type = SqlDbType.Int, Value = customerType });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryCustomersPageBySelect", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 获取往来单位列表
        /// </summary>
        /// <returns></returns>
        public IList<CustomerMaintObject> GetCustomers()
        {
            IList<CustomerMaintObject> customerMaintObject = new List<CustomerMaintObject>();
            IEnumerable<Customer> customers = customerRepository.FindAll(null, "CustomerCode");
            if (customers != null)
            {
                foreach (var customer in customers)
                {
                    customerMaintObject.Add(MapperHelper.Map<Customer, CustomerMaintObject>(customer));
                }
            }
            return customerMaintObject;
        }

        /// <summary>
        /// 获取所有状态为使用的往来单位名称列表
        /// </summary>
        /// <returns>往来单位名称列表Json字符串</returns>
        public string GetAllUsedCustomers()
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = 1 });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetAllUsedCustomers", parameters))
            {
                dt.Columns.Add("isLeaf", typeof(bool), "Convert(IsLeafStr, 'System.Boolean')");
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据往来单位分类获取往来单位列表
        /// </summary>
        /// <param name="customerType">往来单位分类</param>
        /// <returns>往来单位维护对象</returns>
        public IList<CustomerMaintObject> GetCustomersByType(int customerType)
        {
            if (customerType != 0)
            {
                IList<CustomerMaintObject> customerMaintObject = new List<CustomerMaintObject>();
                IEnumerable<Customer> customers = customerRepository.FindAll(Specification<Customer>.Eval(entity => entity.CustomerType == (CustomerType)customerType && entity.State == State.使用), "CustomerCode");
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        customerMaintObject.Add(MapperHelper.Map<Customer, CustomerMaintObject>(customer));
                    }
                }
                return customerMaintObject;
            }
            else
            {
                IList<CustomerMaintObject> customerMaintObject = new List<CustomerMaintObject>();
                IEnumerable<Customer> customers = customerRepository.FindAll(Specification<Customer>.Eval(entity => entity.State == State.使用), "CustomerCode");
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        customerMaintObject.Add(MapperHelper.Map<Customer, CustomerMaintObject>(customer));
                    }
                }
                return customerMaintObject;
            }
        }

        /// <summary>
        /// 获取所有往来单位列表
        /// </summary>
        /// <returns>往来单位维护对象</returns>
        public IList<CustomerMaintObject> GetCustomersByAll()
        {
            IList<CustomerMaintObject> customerMaintObject = new List<CustomerMaintObject>();
            IEnumerable<Customer> customers = customerRepository.FindAll(null, "CustomerCode");
            if (customers != null)
            {
                foreach (var customer in customers)
                {
                    customerMaintObject.Add(MapperHelper.Map<Customer, CustomerMaintObject>(customer));
                }
            }
            return customerMaintObject;
        }

        /// <summary>
        /// 根据用户Id获取往来单位
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>往来单位维护对象</returns>
        public CustomerMaintObject GetCustomerByUserId(Guid userId)
        {
            CustomerUser customerUser = customerUserRepository.Find(Specification<CustomerUser>.Eval(entity => entity.UserId == userId));
            if (customerUser != null)
            {
                Customer customer = customerRepository.FindByKey(customerUser.CustomerId);
                CustomerMaintObject customerMaintObject = MapperHelper.Map<Customer, CustomerMaintObject>(customer);
                customerMaintObject.Id = customer.Id;
                customerMaintObject.CustomerName = customer.CustomerName;
                return customerMaintObject;
            }
            else
            {
                CustomerMaintObject customerMaintObject = new CustomerMaintObject();
                customerMaintObject.Id = Guid.Empty;
                customerMaintObject.CustomerName = "请选择";
                return customerMaintObject;
            }
        }
    }
}
