using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 寻址确认服务接口
    /// </summary>
    [ServiceContract]
    public interface IAddressingService : IDistributedService
    {
        /// <summary>
        /// 根据寻址确认Id和规划Id获取寻址确认
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="planningId">规划Id</param>
        /// <returns>寻址确认维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        AddressingMaintObject GetAddressingById(Guid id, Guid planningId);

        /// <summary>
        /// 根据寻址确认Id获取寻址确认打印信息
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <returns>寻址确认打印对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        AddressingPrintObject GetAddressingPrintById(Guid id);

        /// <summary>
        /// 根据寻址确认Id获取寻址确认编辑信息
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <returns>寻址确认编辑对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        AddressingEditorObject GetAddressingEditorById(Guid id);

        /// <summary>
        /// 根据条件获取分页寻址确认列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <returns>分页寻址确认列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAddressingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate,
            string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId,
            int importance, int addressingState, Guid addressingUserId);

        /// <summary>
        /// 新增或者修改寻址确认
        /// </summary>
        /// <param name="addressingMaintObject">要新增或者修改的寻址确认维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateAddressing(AddressingMaintObject addressingMaintObject);

        /// <summary>
        /// 修改寻址确认编辑
        /// </summary>
        /// <param name="addressingMaintObject">修改的寻址确认维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void UpdateAddressingEdit(AddressingMaintObject addressingMaintObject);

        /// <summary>
        /// 退回寻址确认任务
        /// </summary>
        /// <param name="addressingMaintObjects">要退回任务的寻址确认维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void ReturnAddressings(IList<AddressingMaintObject> addressingMaintObjects);

        /// <summary>
        /// 获取寻址确认任务
        /// </summary>
        /// <param name="addressingMaintObjects">要获取任务的寻址确认维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void GetAddressings(IList<AddressingMaintObject> addressingMaintObjects);

        /// <summary>
        /// 运营商再次确认
        /// </summary>
        /// <param name="addressingMaintObject">修改的寻址确认维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveOperatorConfirm(AddressingMaintObject addressingMaintObject);

        /// <summary>
        /// 指定项目及站点编码
        /// </summary>
        /// <param name="addressingMaintObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveProjectAndPlaceCode(AddressingMaintObject addressingMaintObject);

        /// <summary>
        /// 项目申请立项
        /// </summary>
        /// <param name="addressingEditorObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveApplyProject(AddressingEditorObject addressingEditorObject);

        /// <summary>
        /// 项目完成立项
        /// </summary>
        /// <param name="addressingEditorObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveDoApplyProject(AddressingEditorObject addressingEditorObject);

        /// <summary>
        /// 根据条件获取分页租赁进度表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="addressingDepartmentId">租赁部门</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <param name="isAppoint">指定租赁</param>
        /// <returns>分页租赁进度表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAddressingReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate,
            string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId,
            int importance, int addressingState, Guid addressingDepartmentId, Guid addressingUserId, int isAppoint, Guid companyId);

        /// <summary>
        /// 根据条件获取租赁月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAddressingMonthReseau(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取租赁人月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAddressingMonthUser(DateTime beginDate, DateTime endDate, Guid departmentId, int profession, Guid companyId);
    }
}
