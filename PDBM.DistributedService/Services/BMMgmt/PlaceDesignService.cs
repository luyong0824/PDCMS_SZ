using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 站点设计信息分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlaceDesignService : IPlaceDesignService
    {
        private readonly IPlaceDesignService placeDesignServiceImpl = ServiceLocator.Instance.GetService<IPlaceDesignService>();

        public PlaceDesignMaintObject GetPlaceDesignById(Guid id)
        {
            try
            {
                return placeDesignServiceImpl.GetPlaceDesignById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePlaceDesign(PlaceDesignMaintObject placeDesignMaintObject)
        {
            try
            {
                placeDesignServiceImpl.AddOrUpdatePlaceDesign(placeDesignMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlaceDesign(IList<PlaceDesignMaintObject> placeDesignMaintObjects)
        {
            try
            {
                placeDesignServiceImpl.RemovePlaceDesign(placeDesignMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveAppointDesign(PlaceDesignMaintObject placeDesignMaintObject)
        {
            try
            {
                placeDesignServiceImpl.SaveAppointDesign(placeDesignMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveAppointDesignUser(PlaceDesignMaintObject placeDesignMaintObject)
        {
            try
            {
                placeDesignServiceImpl.SaveAppointDesignUser(placeDesignMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveConstructionDesign(AddressingEditorObject addressingEditorObject)
        {
            try
            {
                placeDesignServiceImpl.SaveConstructionDesign(addressingEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveCustomer(AddressingEditorObject addressingEditorObject)
        {
            try
            {
                placeDesignServiceImpl.SaveCustomer(addressingEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveManagerAndSupervisor(PlaceDesignMaintObject placeDesignMaintObject)
        {
            try
            {
                placeDesignServiceImpl.SaveManagerAndSupervisor(placeDesignMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveAppointDesignR(PlaceDesignMaintObject placeDesignMaintObject)
        {
            try
            {
                placeDesignServiceImpl.SaveAppointDesignR(placeDesignMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveAppointDesignUserR(PlaceDesignMaintObject placeDesignMaintObject)
        {
            try
            {
                placeDesignServiceImpl.SaveAppointDesignUserR(placeDesignMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveConstructionDesignR(RemodelingEditorObject remodelingEditorObject)
        {
            try
            {
                placeDesignServiceImpl.SaveConstructionDesignR(remodelingEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveCustomerR(RemodelingEditorObject remodelingEditorObject)
        {
            try
            {
                placeDesignServiceImpl.SaveCustomerR(remodelingEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveManagerAndSupervisorR(PlaceDesignMaintObject placeDesignMaintObject)
        {
            try
            {
                placeDesignServiceImpl.SaveManagerAndSupervisorR(placeDesignMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            placeDesignServiceImpl.Dispose();
        }
    }
}
