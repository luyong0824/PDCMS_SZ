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
    public class FoundationTestService : DataService, IFoundationTestService
    {
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public FoundationTestService(IRepositoryContext context,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.foundationTestRepository = foundationTestRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据桩基动测Id获取桩基动测
        /// </summary>
        /// <param name="id">桩基动测Id</param>
        /// <returns>桩基动测维护对象</returns>
        public FoundationTestMaintObject GetFoundationTestById(Guid id)
        {
            FoundationTest foundationTest = foundationTestRepository.FindByKey(id);
            if (foundationTest != null)
            {
                FoundationTestMaintObject foundationTestMaintObject = MapperHelper.Map<FoundationTest, FoundationTestMaintObject>(foundationTest);
                return foundationTestMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的桩基动测在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改桩基动测
        /// </summary>
        /// <param name="foundationTestMaintObject">要新增或者修改的桩基动测对象</param>
        public void AddOrUpdateFoundationTest(FoundationTestMaintObject foundationTestMaintObject)
        {
            if (foundationTestMaintObject.Id == Guid.Empty)
            {
                FoundationTest foundationTest = AggregateFactory.CreateFoundationTest(foundationTestMaintObject.ParentId, (PropertyType)foundationTestMaintObject.PropertyType, foundationTestMaintObject.BudgetPrice, 0, foundationTestMaintObject.Memos, foundationTestMaintObject.CreateUserId);
                foundationTestRepository.Add(foundationTest);
            }
            else
            {
                FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestMaintObject.Id);
                if (foundationTest != null)
                {
                    foundationTest.Modify(foundationTestMaintObject.BudgetPrice, 0, foundationTestMaintObject.Memos, foundationTestMaintObject.ModifyUserId);
                    foundationTestRepository.Update(foundationTest);
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

        public void RemoveFoundationTest(IList<FoundationTestMaintObject> foundationTestMaintObjects)
        {
            foreach (FoundationTestMaintObject foundationTestMaintObject in foundationTestMaintObjects)
            {
                FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestMaintObject.Id);
                if (foundationTest != null)
                {
                    foundationTestRepository.Remove(foundationTest);
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
