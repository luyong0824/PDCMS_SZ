using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 会计主体应用层服务
    /// </summary>
    public class AccountingEntityService : DataService, IAccountingEntityService
    {
        private readonly IRepository<AccountingEntity> accountingEntityRepository;
        private readonly IRepository<Project> projectRepository;

        public AccountingEntityService(IRepositoryContext context,
            IRepository<AccountingEntity> accountingEntityRepository,
            IRepository<Project> projectRepository)
            : base(context)
        {
            this.accountingEntityRepository = accountingEntityRepository;
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// 根据会计主体Id获取会计主体
        /// </summary>
        /// <param name="id">会计主体Id</param>
        /// <returns>会计主体维护对象</returns>
        public AccountingEntityMaintObject GetAccountingEntityById(Guid id)
        {
            AccountingEntity accountingEntity = accountingEntityRepository.FindByKey(id);
            if (accountingEntity != null)
            {
                AccountingEntityMaintObject accountingEntityMaintObject = MapperHelper.Map<AccountingEntity, AccountingEntityMaintObject>(accountingEntity);
                return accountingEntityMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的会计主体在系统中不存在");
            }
        }

        /// <summary>
        /// 获取会计主体列表
        /// </summary>
        /// <returns>会计主体维护对象列表</returns>
        public IList<AccountingEntityMaintObject> GetAccountingEntitys()
        {
            IList<AccountingEntityMaintObject> accountingEntityMaintObjects = new List<AccountingEntityMaintObject>();
            IEnumerable<AccountingEntity> accountingEntitys = accountingEntityRepository.FindAll(null, "AccountingEntityCode");
            if (accountingEntitys != null)
            {
                foreach (var accountingEntity in accountingEntitys)
                {
                    accountingEntityMaintObjects.Add(MapperHelper.Map<AccountingEntity, AccountingEntityMaintObject>(accountingEntity));
                }
            }
            return accountingEntityMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的会计主体列表
        /// </summary>
        /// <returns>会计主体选择对象列表</returns>
        public IList<AccountingEntitySelectObject> GetUsedAccountingEntitys()
        {
            IList<AccountingEntitySelectObject> accountingEntitySelectObjects = new List<AccountingEntitySelectObject>();
            IEnumerable<AccountingEntity> accountingEntitys = accountingEntityRepository.FindAll(Specification<AccountingEntity>.Eval(entity => entity.State == State.使用), "AccountingEntityCode");
            if (accountingEntitys != null)
            {
                foreach (var accountingEntity in accountingEntitys)
                {
                    accountingEntitySelectObjects.Add(MapperHelper.Map<AccountingEntity, AccountingEntitySelectObject>(accountingEntity));
                }
            }
            return accountingEntitySelectObjects;
        }

        /// <summary>
        /// 新增或者修改会计主体
        /// </summary>
        /// <param name="accountingEntityMaintObject">要新增或者修改的会计主体维护对象</param>
        public void AddOrUpdateAccountingEntity(AccountingEntityMaintObject accountingEntityMaintObject)
        {
            if (accountingEntityMaintObject.Id == Guid.Empty)
            {
                AccountingEntity accountingEntity = AggregateFactory.CreateAccountingEntity(accountingEntityMaintObject.AccountingEntityCode, accountingEntityMaintObject.AccountingEntityName,
                    accountingEntityMaintObject.Remarks, (State)accountingEntityMaintObject.State, accountingEntityMaintObject.CreateUserId);
                accountingEntityRepository.Add(accountingEntity);
            }
            else
            {
                AccountingEntity accountingEntity = accountingEntityRepository.FindByKey(accountingEntityMaintObject.Id);
                if (accountingEntity != null)
                {
                    accountingEntity.Modify(accountingEntityMaintObject.AccountingEntityCode, accountingEntityMaintObject.AccountingEntityName, accountingEntityMaintObject.Remarks,
                        (State)accountingEntityMaintObject.State, accountingEntityMaintObject.ModifyUserId);
                    accountingEntityRepository.Update(accountingEntity);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_AccountingEntityCode"))
                {
                    throw new ApplicationFault("会计主体编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_AccountingEntityName"))
                {
                    throw new ApplicationFault("会计主体名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除会计主体
        /// </summary>
        /// <param name="accountingEntityMaintObjects">要删除的会计主体维护对象列表</param>
        public void RemoveAccountingEntitys(IList<AccountingEntityMaintObject> accountingEntityMaintObjects)
        {
            foreach (AccountingEntityMaintObject accountingEntityMaintObject in accountingEntityMaintObjects)
            {
                AccountingEntity accountingEntity = accountingEntityRepository.FindByKey(accountingEntityMaintObject.Id);
                if (accountingEntity != null)
                {
                    if (projectRepository.Exists(Specification<Project>.Eval(entity => entity.AccountingEntityId == accountingEntity.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在项目中使用", accountingEntity.AccountingEntityCode);
                    }
                    accountingEntityRepository.Remove(accountingEntity);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Project_dbo.tbl_AccountingEntity_AccountingEntityId"))
                {
                    throw new ApplicationFault("已在项目中使用");
                }
                throw ex;
            }
        }
    }
}
