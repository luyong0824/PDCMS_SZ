using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.Map;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.Map
{
    /// <summary>
    /// 地图服务接口
    /// </summary>
    [ServiceContract]
    public interface IMapService : IDistributedService
    {
        /// <summary>
        /// 根据盲点反馈获取点对象
        /// </summary>
        /// <param name="blindSpotFeedBackId">盲点反馈Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">盲点地名</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByBlindSpotFeedBack(Guid blindSpotFeedBackId, Guid areaId, string placeName, decimal lng, decimal lat, int profession, Guid companyId);

        /// <summary>
        /// 根据建设申请获取点对象
        /// </summary>
        /// <param name="planningApplyId">建设申请Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByPlanningApply(Guid planningApplyId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, Guid companyId);

        /// <summary>
        /// 根据运营商规划获取点对象
        /// </summary>
        /// <param name="operatorsPlanningId">运营商规划Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByOperatorsPlanning(Guid operatorsPlanningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, Guid companyId);

        /// <summary>
        /// 根据规划获取点对象
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="isFromPlanning">是否是从规划页面查看</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByPlanning(Guid planningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, int isFromPlanning, Guid companyId);

        /// <summary>
        /// 根据规划获取点对象，包括选中的运营商规划列表
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="operatorsPlanningIdsSql">运营商规划Id列表Sql语句</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByPlanningAndOperatorsPlannings(Guid planningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, string operatorsPlanningIdsSql);

        /// <summary>
        /// 根据规划和关联的运营商规划获取点对象
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByPlanningAndAssociatedOperatorsPlannings(Guid planningId);

        /// <summary>
        /// 根据购置获取点对象
        /// </summary>
        /// <param name="purchaseId">购置Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByPurchase(Guid purchaseId, Guid areaId, string placeName, decimal lng, decimal lat, int profession);

        /// <summary>
        /// 根据站点获取点对象
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByPlace(Guid placeId, Guid areaId, string placeName, decimal lng, decimal lat, int profession);

        /// <summary>
        /// 根据站点获取点对象，包括选中的运营商规划列表
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="operatorsPlanningIdsSql">运营商规划Id列表Sql语句</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByPlaceAndOperatorsPlannings(Guid placeId, Guid areaId, string placeName, decimal lng, decimal lat, int profession, string operatorsPlanningIdsSql);

        /// <summary>
        /// 根据站点列表获取对象
        /// </summary>
        /// <param name="placeIdsSql">站点Id列表Sql语句</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointsBySearch(string placeIdsSql);

        /// <summary>
        /// 获取规划站点及已有站点数据(移动端)
        /// </summary>
        /// <param name="planningIdsSql">规划站点Id列表Sql语句</param>
        /// <param name="placeIdsSql">已有站点Id列表Sql语句</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPlanningAndPlacePoints(string planningIdsSql, string placeIdsSql);

        /// <summary>
        /// 根据站点列表获取对象
        /// </summary>
        /// <param name="planningIdsSql">规划站点Id列表Sql语句</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPlanningPointsBySearch(string planningIdsSql);

        /// <summary>
        /// 根据站点列表获取对象
        /// </summary>
        /// <param name="planningApplyIdsSql">规划站点Id列表Sql语句</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPlanningApplyPointsBySearch(string planningApplyIdsSql);

        /// <summary>
        /// 根据运营商规划列表获取对象
        /// </summary>
        /// <param name="operatorsIdsSql">运营商规划Id列表sql数据</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetOperatorsPlanningPointsBySearch(string operatorsIdsSql);

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="bsPlanningPlaceCategorySql">规划基站类型列表Sql语句</param>
        /// <param name="bsPlaceCategorySql">基站类型列表Sql语句</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetNearbyPlanningsAndPlaces(decimal lng, decimal lat, Guid planningId, Guid placeId,
            string bsPlanningPlaceCategorySql, string bsPlaceCategorySql, decimal distance);

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表(室分)
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="idPlanningPlaceCategorySql">规划室分类型列表Sql语句</param>
        /// <param name="idPlaceCategorySql">室分类型列表Sql语句</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetNearbyPlanningsAndPlacesID(decimal lng, decimal lat, Guid planningId, Guid placeId,
            string idPlanningPlaceCategorySql, string idPlaceCategorySql, decimal distance);

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetNearbyPlanningsAndPlacesMobile(decimal lng, decimal lat, decimal distance);

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表(室分)
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="planningProfessionsSql">规划站专业</param>
        /// <param name="placeProfessionsSql">已有站专业</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetNearbyPlanningsAndPlacesAll(decimal lng, decimal lat, Guid planningId, Guid placeId,
            string planningProfessionsSql, string placeProfessionsSql, decimal distance);

        /// <summary>
        /// 根据运营商需求确认和关联的运营商规划获取点对象
        /// </summary>
        /// <param name="operatorsPlanningDemandId">运营商需求确认Id</param>
        /// <returns>点对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PointObject GetPointByOperatorsPlanningDemandAndAssociatedOperatorsPlannings(Guid operatorsPlanningDemandId);

        /// <summary>
        /// 获取周边资源列表(移动端)
        /// </summary>
        /// <param name="professionListSql">专业sql</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="distance">距离(公里)</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetNearbyPlanningsAndPlacesListMobile(string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId);
    }
}
