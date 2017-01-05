using PDBM.DataTransferObjects;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.WorkFlow;

namespace PDBM.ServiceContracts.WorkFlow
{
    [ServiceContract]
    public interface IWFActivityInstanceEditorService : IDistributedService
    {/// <summary>
        /// 根据批复步骤IdId获取单据编辑维护对象Id
        /// </summary>
        /// <param name="id">批复步骤IdId</param>
        /// <returns>单据编辑维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WFActivityInstanceEditorMaintObject GetWFActivityInstanceEditorById(Guid id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="wfActivityInstanceEditorMaintObject">要新增的单据编辑维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddWFActivityInstanceEditor(WFActivityInstanceEditorMaintObject wfActivityInstanceEditorMaintObject);
    }
}
