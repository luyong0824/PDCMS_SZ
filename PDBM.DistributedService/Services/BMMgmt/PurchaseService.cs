using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 购置站点分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseService purchaseServiceImpl = ServiceLocator.Instance.GetService<IPurchaseService>();

        public PurchaseMaintObject GetPurchaseById(Guid id)
        {
            try
            {
                return purchaseServiceImpl.GetPurchaseById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetPurchasesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string groupPlaceCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, string propertyRightSql, int importance, int telecomShare, int mobileShare, int unicomShare)
        {
            try
            {
                return purchaseServiceImpl.GetPurchasesPage(pageIndex, pageSize, beginDate, endDate, groupPlaceCode, placeName, profession, placeCategoryId, areaId, reseauId, propertyRightSql, importance, telecomShare, mobileShare, unicomShare);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePurchase(PurchaseMaintObject purchaseMaintObject)
        {
            try
            {
                purchaseServiceImpl.AddOrUpdatePurchase(purchaseMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePurchases(IList<PurchaseMaintObject> purchaseMaintObjects)
        {
            try
            {
                purchaseServiceImpl.RemovePurchases(purchaseMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            purchaseServiceImpl.Dispose();
        }
    }
}
