using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class AddressExplorLogService : DataService, IAddressExplorLogService
    {
        private readonly IRepository<AddressExplorLog> addressExplorLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public AddressExplorLogService(IRepositoryContext context,
            IRepository<AddressExplorLog> addressExplorLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.addressExplorLogRepository = addressExplorLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改地质勘探
        /// </summary>
        /// <param name="addressExplorMaintObject">要新增或者修改的地质勘探对象</param>
        public void AddOrUpdateAddressExplorLog(AddressExplorMaintObject addressExplorMaintObject)
        {
            if (addressExplorMaintObject.Id == Guid.Empty)
            {
                AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.新增, addressExplorMaintObject.ParentId, (PropertyType)addressExplorMaintObject.PropertyType, addressExplorMaintObject.BudgetPrice, 0, addressExplorMaintObject.Memos, addressExplorMaintObject.CreateUserId);
                addressExplorLogRepository.Add(addressExplorLog);
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

        /// <summary>
        /// 根据资源类型获取地质勘探历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        public string GetAddressExplorLog(int propertyType, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetAddressExplorLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
