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
    /// 公司服务接口
    /// </summary>
    [ServiceContract]
    public interface ICompanyService : IDistributedService
    {

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns>公司维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<CompanyMaintObject> GetCompanys();

        /// <summary>
        /// 获取状态为使用的公司列表
        /// </summary>
        /// <returns>公司选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<CompanySelectObject> GetUsedCompanys();

        /// <summary>
        /// 根据公司性质获取状态为使用的公司列表
        /// </summary>
        /// <param name="companyNature">公司性质</param>
        /// <returns>公司选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<CompanySelectObject> GetUsedCompanysByNature(int companyNature);
    }
}
