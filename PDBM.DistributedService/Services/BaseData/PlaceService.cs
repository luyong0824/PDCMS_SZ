using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 站点分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceService placeServiceImpl = ServiceLocator.Instance.GetService<IPlaceService>();

        public PlaceMaintObject GetPlaceById(Guid id)
        {
            try
            {
                return placeServiceImpl.GetPlaceById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlacesPage(int pageIndex, int pageSize, int profession, string placeName, Guid areaId, Guid reseauId, Guid placeOwner, int state)
        {
            try
            {
                return placeServiceImpl.GetPlacesPage(pageIndex, pageSize, profession, placeName, areaId, reseauId, placeOwner, state);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlacesPageBySelect(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId)
        {
            try
            {
                return placeServiceImpl.GetPlacesPageBySelect(pageIndex, pageSize, placeCode, placeName, profession, placeCategoryId, areaId, reseauId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void UpdatePlace(PlaceMaintObject placeMaintObject)
        {
            try
            {
                placeServiceImpl.UpdatePlace(placeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PlaceInfoObject GetPlaceInfoById(Guid id)
        {
            try
            {
                return placeServiceImpl.GetPlaceInfoById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetResourcePlacesPage(int pageIndex, int pageSize, string groupPlaceCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int propertyRight, int importance, int telecomShare, int mobileShare, int unicomShare, int state)
        {
            try
            {
                return placeServiceImpl.GetResourcePlacesPage(pageIndex, pageSize, groupPlaceCode, placeName, profession, placeCategoryId, areaId, reseauId, propertyRight, importance, telecomShare, mobileShare, unicomShare, state);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PlaceMaintObject GetPlaceImportById(Guid id)
        {
            try
            {
                return placeServiceImpl.GetPlaceImportById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlaceImportsPage(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid placeCategoryId, Guid placeOwner,
            Guid areaId, Guid reseauId, int importance, int state, Guid companyId)
        {
            try
            {
                return placeServiceImpl.GetPlaceImportsPage(pageIndex, pageSize, placeCode, placeName, profession, placeCategoryId, placeOwner, areaId, reseauId, importance, state, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlaceImport(PlaceMaintObject placeMaintObject)
        {
            try
            {
                placeServiceImpl.SavePlaceImport(placeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public PlaceMaintObject GetLogicalNumberById(Guid id)
        {
            try
            {
                return placeServiceImpl.GetLogicalNumberById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetLogicalNumbersPage(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid areaId, Guid reseauId,
            int g2Mark, int d2Mark, int g3Mark, int g4Mark, string g2Number, string d2Number, string g3Number, string g4Number, int allMark)
        {
            try
            {
                return placeServiceImpl.GetLogicalNumbersPage(pageIndex, pageSize, placeCode, placeName, profession, areaId, reseauId, g2Mark, d2Mark, g3Mark, g4Mark, g2Number, d2Number, g3Number, g4Number, allMark);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveLogicalNumber(PlaceMaintObject placeMaintObject)
        {
            try
            {
                placeServiceImpl.SaveLogicalNumber(placeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlacesMobile(int pageIndex, int pageSize, string professionListSql, string placeName, Guid companyId)
        {
            try
            {
                return placeServiceImpl.GetPlacesMobile(pageIndex, pageSize, professionListSql, placeName, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPlacesPageMobile(int pageIndex, int pageSize, string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId)
        {
            try
            {
                return placeServiceImpl.GetPlacesPageMobile(pageIndex, pageSize, professionListSql, lng, lat, distance, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlacePositionMobile(PlaceMaintObject placeMaintObject)
        {
            try
            {
                placeServiceImpl.SavePlacePositionMobile(placeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SavePlaceMobile(PlaceMaintObject placeMaintObject)
        {
            try
            {
                placeServiceImpl.SavePlaceMobile(placeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            placeServiceImpl.Dispose();
        }
    }
}
