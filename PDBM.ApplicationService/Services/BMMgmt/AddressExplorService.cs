using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class AddressExplorService : DataService, IAddressExplorService
    {
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public AddressExplorService(IRepositoryContext context,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.addressExplorRepository = addressExplorRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据地质勘探Id获取地质勘探
        /// </summary>
        /// <param name="id">地质勘探Id</param>
        /// <returns>地质勘探维护对象</returns>
        public AddressExplorMaintObject GetAddressExplorById(Guid id)
        {
            AddressExplor addressExplor = addressExplorRepository.FindByKey(id);
            if (addressExplor != null)
            {
                AddressExplorMaintObject addressExplorMaintObject = MapperHelper.Map<AddressExplor, AddressExplorMaintObject>(addressExplor);
                return addressExplorMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的地质勘探在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改地质勘探
        /// </summary>
        /// <param name="addressExplorMaintObject">要新增或者修改的地质勘探对象</param>
        public void AddOrUpdateAddressExplor(AddressExplorMaintObject addressExplorMaintObject)
        {
            if (addressExplorMaintObject.Id == Guid.Empty)
            {
                AddressExplor addressExplor = AggregateFactory.CreateAddressExplor(addressExplorMaintObject.ParentId, (PropertyType)addressExplorMaintObject.PropertyType, addressExplorMaintObject.BudgetPrice, 0, addressExplorMaintObject.Memos, addressExplorMaintObject.CreateUserId);
                addressExplorRepository.Add(addressExplor);
            }
            else
            {
                AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorMaintObject.Id);
                if (addressExplor != null)
                {
                    addressExplor.Modify(addressExplorMaintObject.BudgetPrice, 0, addressExplorMaintObject.Memos, addressExplorMaintObject.ModifyUserId);
                    addressExplorRepository.Update(addressExplor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveAddressExplor(IList<AddressExplorMaintObject> addressExplorMaintObjects)
        {
            foreach (AddressExplorMaintObject addressExplorMaintObject in addressExplorMaintObjects)
            {
                AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorMaintObject.Id);
                if (addressExplor != null)
                {
                    addressExplorRepository.Remove(addressExplor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
