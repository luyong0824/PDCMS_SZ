using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Domain.Services
{
    /// <summary>
    /// 建维管理领域服务接口
    /// </summary>
    public interface IBMMgmtService
    {
        /// <summary>
        /// 根据运营商确认明细实体修改规划实体需求确认信息
        /// </summary>
        /// <param name="modifiedPlanning">要修改的规划实体</param>
        /// <param name="operatorsConfirmDetail">运营商确认明细实体</param>
        /// <returns>修改后的规划实体</returns>
        Planning ModifyPlanningDemand(Planning modifiedPlanning, OperatorsConfirmDetail operatorsConfirmDetail);

        /// <summary>
        /// 根据规划实体寻址确认实体创建站点实体
        /// </summary>
        /// <param name="addressing">寻址确认实体</param>
        /// <param name="planning">规划实体</param>
        /// <param name="placeCode">站点编码</param>
        /// <returns>站点实体</returns>
        Place CreatePlace(Addressing addressing, Planning planning, PlaceDesign placedesign, string placeCode);

        /// <summary>
        /// 根据购置站点实体创建站点实体
        /// </summary>
        /// <param name="purchase">购置站点实体</param>
        /// <param name="placeCode">站点编码</param>
        /// <returns>站点实体</returns>
        Place CreatePlace(Purchase purchase, string placeCode);

        /// <summary>
        /// 根据购置站点实体修改站点实体
        /// </summary>
        /// <param name="modifiedPlace">要修改的站点实体</param>
        /// <param name="purchase">购置站点实体</param>
        /// <returns>修改后的站点实体</returns>
        Place ModifyPlace(Place modifiedPlace, Purchase purchase);

        /// <summary>
        /// 关联规划检查
        /// </summary>
        /// <param name="planningCreateUserId">规划创建人用户Id</param>
        /// <param name="currentUserId">当前操作用户Id</param>
        void CheckByAssociate(Guid planningCreateUserId, Guid currentUserId);

        /// <summary>
        /// 关联改造检查
        /// </summary>
        /// <param name="remodelingCreateUserId">改造创建人用户Id</param>
        /// <param name="currentUserId">当前操作人用户Id</param>
        void CheckByRemodelingAssociate(Guid remodelingCreateUserId, Guid currentUserId);
    }
}
