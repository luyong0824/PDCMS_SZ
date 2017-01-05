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
    /// 寻址确认分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AddressingService : IAddressingService
    {
        private readonly IAddressingService addressingServiceImpl = ServiceLocator.Instance.GetService<IAddressingService>();

        public AddressingMaintObject GetAddressingById(Guid id, Guid planningId)
        {
            try
            {
                return addressingServiceImpl.GetAddressingById(id, planningId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public AddressingPrintObject GetAddressingPrintById(Guid id)
        {
            try
            {
                return addressingServiceImpl.GetAddressingPrintById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public AddressingEditorObject GetAddressingEditorById(Guid id)
        {
            try
            {
                return addressingServiceImpl.GetAddressingEditorById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAddressingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingUserId)
        {
            try
            {
                return addressingServiceImpl.GetAddressingsPage(pageIndex, pageSize, beginDate, endDate, planningCode, planningName, profession, placeCategoryId, areaId, reseauId, importance, addressingState, addressingUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateAddressing(AddressingMaintObject addressingMaintObject)
        {
            try
            {
                addressingServiceImpl.AddOrUpdateAddressing(addressingMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void UpdateAddressingEdit(AddressingMaintObject addressingMaintObject)
        {
            try
            {
                addressingServiceImpl.UpdateAddressingEdit(addressingMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void ReturnAddressings(IList<AddressingMaintObject> addressingMaintObjects)
        {
            try
            {
                addressingServiceImpl.ReturnAddressings(addressingMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void GetAddressings(IList<AddressingMaintObject> addressingMaintObjects)
        {
            try
            {
                addressingServiceImpl.GetAddressings(addressingMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveOperatorConfirm(AddressingMaintObject addressingMaintObject)
        {
            try
            {
                addressingServiceImpl.SaveOperatorConfirm(addressingMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveProjectAndPlaceCode(AddressingMaintObject addressingMaintObject)
        {
            try
            {
                addressingServiceImpl.SaveProjectAndPlaceCode(addressingMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveApplyProject(AddressingEditorObject addressingEditorObject)
        {
            try
            {
                addressingServiceImpl.SaveApplyProject(addressingEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveDoApplyProject(AddressingEditorObject addressingEditorObject)
        {
            try
            {
                addressingServiceImpl.SaveDoApplyProject(addressingEditorObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAddressingReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession,
                Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingDepartmentId, Guid addressingUserId, int isAppoint,
                Guid companyId)
        {
            try
            {
                return addressingServiceImpl.GetAddressingReportPage(pageIndex, pageSize, beginDate, endDate, planningCode, planningName, profession, placeCategoryId, areaId,
                    reseauId, importance, addressingState, addressingDepartmentId, addressingUserId, isAppoint, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAddressingMonthReseau(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            try
            {
                return addressingServiceImpl.GetAddressingMonthReseau(beginDate, endDate, areaId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAddressingMonthUser(DateTime beginDate, DateTime endDate, Guid departmentId, int profession, Guid companyId)
        {
            try
            {
                return addressingServiceImpl.GetAddressingMonthUser(beginDate, endDate, departmentId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            addressingServiceImpl.Dispose();
        }
    }
}
