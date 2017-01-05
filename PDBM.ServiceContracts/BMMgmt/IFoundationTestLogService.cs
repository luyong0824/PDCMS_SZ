﻿using PDBM.DataTransferObjects;
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
    /// 桩基动测服务接口
    /// </summary>
    [ServiceContract]
    public interface IFoundationTestLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改桩基动测
        /// </summary>
        /// <param name="foundationTestMaintObject">要新增或者修改的桩基动测维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateFoundationTestLog(FoundationTestMaintObject foundationTestMaintObject);

        /// <summary>
        /// 根据资源类型获取桩基动测历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetFoundationTestLog(int propertyType, Guid parentId);
    }
}
