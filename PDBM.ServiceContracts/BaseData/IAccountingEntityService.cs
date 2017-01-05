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
    /// 会计主体服务接口
    /// </summary>
    [ServiceContract]
    public interface IAccountingEntityService : IDistributedService
    {
        /// <summary>
        /// 根据会计主体Id获取会计主体
        /// </summary>
        /// <param name="id">会计主体Id</param>
        /// <returns>会计主体维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        AccountingEntityMaintObject GetAccountingEntityById(Guid id);

        /// <summary>
        /// 获取会计主体列表
        /// </summary>
        /// <returns>会计主体维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<AccountingEntityMaintObject> GetAccountingEntitys();

        /// <summary>
        /// 获取状态为使用的会计主体列表
        /// </summary>
        /// <returns>会计主体选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<AccountingEntitySelectObject> GetUsedAccountingEntitys();

        /// <summary>
        /// 新增或者修改会计主体
        /// </summary>
        /// <param name="accountingEntityMaintObject">要新增或者修改的会计主体维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateAccountingEntity(AccountingEntityMaintObject accountingEntityMaintObject);

        /// <summary>
        /// 删除会计主体
        /// </summary>
        /// <param name="accountingEntityMaintObjects">要删除的会计主体维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveAccountingEntitys(IList<AccountingEntityMaintObject> accountingEntityMaintObjects);
    }
}
