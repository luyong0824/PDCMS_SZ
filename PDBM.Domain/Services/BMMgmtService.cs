using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Services
{
    /// <summary>
    /// 建维管理领域服务
    /// </summary>
    public class BMMgmtService : IBMMgmtService
    {
        /// <summary>
        /// 根据运营商确认明细实体修改规划实体需求确认信息
        /// </summary>
        /// <param name="modifiedPlanning">要修改的规划实体</param>
        /// <param name="operatorsConfirmDetail">运营商确认明细实体</param>
        /// <returns>修改后的规划实体</returns>
        public Planning ModifyPlanningDemand(Planning modifiedPlanning, OperatorsConfirmDetail operatorsConfirmDetail)
        {
            //if (modifiedPlanning.Id != operatorsConfirmDetail.PlanningId)
            //{
            //    throw new DomainFault("运营商确认明细与规划不匹配");
            //}
            //if (modifiedPlanning.OperatorsConfirmDetailId != operatorsConfirmDetail.Id)
            //{
            //    throw new DomainFault("{0}<br>不是最新的确认请求", modifiedPlanning.PlanningCode);
            //}

            //modifiedPlanning.ModifyDemand(operatorsConfirmDetail.TelecomDemand, operatorsConfirmDetail.MobileDemand, operatorsConfirmDetail.UnicomDemand);
            return modifiedPlanning;
        }

        /// <summary>
        /// 根据规划实体寻址确认实体创建站点实体
        /// </summary>
        /// <param name="addressing">寻址确认实体</param>
        /// <param name="planning">规划实体</param>
        /// <param name="placeCode">站点编码</param>
        /// <returns>站点实体</returns>
        public Place CreatePlace(Addressing addressing, Planning planning, PlaceDesign placeDesign, string placeCode)
        {
            if (addressing.PlanningId != planning.Id)
            {
                throw new DomainFault("规划与寻址确认不匹配");
            }

            Place place = AggregateFactory.CreatePlace(placeCode, addressing.PlaceName, planning.Profession, planning.PlaceCategoryId, planning.ReseauId, planning.Lng,
                planning.Lat, planning.PlaceOwner, planning.Importance, addressing.AddressingDepartmentId, addressing.AddressingRealName, addressing.OwnerName, addressing.OwnerContact, addressing.OwnerPhoneNumber,
                planning.DetailedAddress, "", PlaceMapState.寻址确认, planning.AddressingUserId == null ? Guid.Empty : (Guid)planning.AddressingUserId);
            planning.PlaceId = place.Id;
            return place;
        }

        /// <summary>
        /// 根据购置站点实体创建站点实体
        /// </summary>
        /// <param name="purchase">购置站点实体</param>
        /// <param name="placeCode">站点编码</param>
        /// <returns>站点实体</returns>
        public Place CreatePlace(Purchase purchase, string placeCode)
        {
            Place place = AggregateFactory.CreatePlace(placeCode, purchase.PlaceName, purchase.Profession, purchase.PlaceCategoryId, purchase.ReseauId, purchase.Lng, purchase.Lat,
                Guid.Empty, purchase.Importance, Guid.Empty, "", purchase.OwnerName, purchase.OwnerContact, purchase.OwnerPhoneNumber, purchase.DetailedAddress, "", PlaceMapState.寻址确认, purchase.CreateUserId);
            purchase.PlaceId = place.Id;
            purchase.PlaceCode = place.PlaceCode;
            return place;
        }

        /// <summary>
        /// 根据购置站点实体修改站点实体
        /// </summary>
        /// <param name="modifiedPlace">要修改的站点实体</param>
        /// <param name="purchase">购置站点实体</param>
        /// <returns>修改后的站点实体</returns>
        public Place ModifyPlace(Place modifiedPlace, Purchase purchase)
        {
            if (modifiedPlace.Id != purchase.PlaceId)
            {
                throw new DomainFault("购置站点与站点不匹配");
            }

            modifiedPlace.Modify(purchase.PlaceName, purchase.PlaceCategoryId, purchase.ReseauId, purchase.Lng, purchase.Lat,
                Guid.Empty, purchase.Importance, "", purchase.OwnerName, purchase.OwnerContact,
                purchase.OwnerPhoneNumber, purchase.DetailedAddress, modifiedPlace.Remarks, modifiedPlace.State, "", "", "", "", "",
                purchase.ModifyUserId);
            return modifiedPlace;
        }

        /// <summary>
        /// 关联规划检查
        /// </summary>
        /// <param name="planningCreateUserId">规划创建人用户Id</param>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByAssociate(Guid planningCreateUserId, Guid currentUserId)
        {
            if (planningCreateUserId != currentUserId)
            {
                throw new DomainFault("不能操作别人创建的规划");
            }
        }

        /// <summary>
        /// 关联改造检查
        /// </summary>
        /// <param name="remodelingCreateUserId">改造创建人用户Id</param>
        /// <param name="currentUserId">当前操作人用户Id</param>
        public void CheckByRemodelingAssociate(Guid remodelingCreateUserId, Guid currentUserId)
        {
            if (remodelingCreateUserId != currentUserId)
            {
                throw new DomainFault("不能操作别人创建的改造安排");
            }
        }
    }
}
