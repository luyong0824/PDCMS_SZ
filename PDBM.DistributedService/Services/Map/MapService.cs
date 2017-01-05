using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.Map;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.Map;

namespace PDBM.DistributedService.Services.Map
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MapService : IMapService
    {
        private readonly IMapService mapServiceImpl = ServiceLocator.Instance.GetService<IMapService>();

        public PointObject GetPointByBlindSpotFeedBack(Guid blindSpotFeedBackId, Guid areaId, string placeName, decimal lng, decimal lat, int profession, Guid companyId)
        {
            try
            {
                return mapServiceImpl.GetPointByBlindSpotFeedBack(blindSpotFeedBackId, areaId, placeName, lng, lat, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByPlanningApply(Guid planningApplyId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, Guid companyId)
        {
            try
            {
                return mapServiceImpl.GetPointByPlanningApply(planningApplyId, areaId, planningName, lng, lat, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByOperatorsPlanning(Guid operatorsPlanningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, Guid companyId)
        {
            try
            {
                return mapServiceImpl.GetPointByOperatorsPlanning(operatorsPlanningId, areaId, planningName, lng, lat, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByPlanning(Guid planningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, int isFromPlanning, Guid companyId)
        {
            try
            {
                return mapServiceImpl.GetPointByPlanning(planningId, areaId, planningName, lng, lat, profession, isFromPlanning, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByPlanningAndOperatorsPlannings(Guid planningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, string operatorsPlanningIdsSql)
        {
            try
            {
                return mapServiceImpl.GetPointByPlanningAndOperatorsPlannings(planningId, areaId, planningName, lng, lat, profession, operatorsPlanningIdsSql);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByPlanningAndAssociatedOperatorsPlannings(Guid planningId)
        {
            try
            {
                return mapServiceImpl.GetPointByPlanningAndAssociatedOperatorsPlannings(planningId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByPurchase(Guid purchaseId, Guid areaId, string placeName, decimal lng, decimal lat, int profession)
        {
            try
            {
                return mapServiceImpl.GetPointByPurchase(purchaseId, areaId, placeName, lng, lat, profession);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByPlace(Guid placeId, Guid areaId, string placeName, decimal lng, decimal lat, int profession)
        {
            try
            {
                return mapServiceImpl.GetPointByPlace(placeId, areaId, placeName, lng, lat, profession);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByPlaceAndOperatorsPlannings(Guid placeId, Guid areaId, string placeName, decimal lng, decimal lat, int profession, string operatorsPlanningIdsSql)
        {
            try
            {
                return mapServiceImpl.GetPointByPlaceAndOperatorsPlannings(placeId, areaId, placeName, lng, lat, profession, operatorsPlanningIdsSql);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointsBySearch(string placeIdsSql)
        {
            try
            {
                return mapServiceImpl.GetPointsBySearch(placeIdsSql);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPlanningAndPlacePoints(string planningIdsSql, string placeIdsSql)
        {
            try
            {
                return mapServiceImpl.GetPlanningAndPlacePoints(planningIdsSql, placeIdsSql);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetOperatorsPlanningPointsBySearch(string operatorsIdsSql)
        {
            try
            {
                return mapServiceImpl.GetOperatorsPlanningPointsBySearch(operatorsIdsSql);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPlanningPointsBySearch(string planningIdsSql)
        {
            try
            {
                return mapServiceImpl.GetPlanningPointsBySearch(planningIdsSql);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPlanningApplyPointsBySearch(string planningApplyIdsSql)
        {
            try
            {
                return mapServiceImpl.GetPlanningApplyPointsBySearch(planningApplyIdsSql);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetNearbyPlanningsAndPlaces(decimal lng, decimal lat, Guid planningId, Guid placeId, string bsPlanningPlaceCategorySql, string bsPlaceCategorySql, decimal distance)
        {
            try
            {
                return mapServiceImpl.GetNearbyPlanningsAndPlaces(lng, lat, planningId, placeId, bsPlanningPlaceCategorySql, bsPlaceCategorySql, distance);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetNearbyPlanningsAndPlacesID(decimal lng, decimal lat, Guid planningId, Guid placeId, string idPlanningPlaceCategorySql, string idPlaceCategorySql, decimal distance)
        {
            try
            {
                return mapServiceImpl.GetNearbyPlanningsAndPlacesID(lng, lat, planningId, placeId, idPlanningPlaceCategorySql, idPlaceCategorySql, distance);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetNearbyPlanningsAndPlacesMobile(decimal lng, decimal lat, decimal distance)
        {
            try
            {
                return mapServiceImpl.GetNearbyPlanningsAndPlacesMobile(lng, lat, distance);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetNearbyPlanningsAndPlacesAll(decimal lng, decimal lat, Guid planningId, Guid placeId, string planningProfessionsSql, string placeProfessionsSql, decimal distance)
        {
            try
            {
                return mapServiceImpl.GetNearbyPlanningsAndPlacesAll(lng, lat, planningId, placeId, planningProfessionsSql, placeProfessionsSql, distance);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PointObject GetPointByOperatorsPlanningDemandAndAssociatedOperatorsPlannings(Guid operatorsPlanningDemandId)
        {
            try
            {
                return mapServiceImpl.GetPointByOperatorsPlanningDemandAndAssociatedOperatorsPlannings(operatorsPlanningDemandId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetNearbyPlanningsAndPlacesListMobile(string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId)
        {
            try
            {
                return mapServiceImpl.GetNearbyPlanningsAndPlacesListMobile(professionListSql, lng, lat, distance, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            mapServiceImpl.Dispose();
        }
    }
}
