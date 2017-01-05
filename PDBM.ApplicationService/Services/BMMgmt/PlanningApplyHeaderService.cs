using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 建设申请单应用层服务
    /// </summary>
    public class PlanningApplyHeaderService : DataService, IPlanningApplyHeaderService
    {
        private readonly IRepository<PlanningApplyHeader> planningApplyHeaderRepository;
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;
        private readonly ICodeSeedRepository codeSeedRepository;

        public PlanningApplyHeaderService(IRepositoryContext context,
            IRepository<PlanningApplyHeader> planningApplyHeaderRepository,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<Area> areaRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository,
            ICodeSeedRepository codeSeedRepository)
            : base(context)
        {
            this.planningApplyHeaderRepository = planningApplyHeaderRepository;
            this.planningApplyRepository = planningApplyRepository;
            this.areaRepository = areaRepository;
            this.reseauRepository = reseauRepository;
            this.placeOwnerRepository = placeOwnerRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
            this.codeSeedRepository = codeSeedRepository;
        }

        /// <summary>
        /// 根据建设申请单Id获取建设申请单
        /// </summary>
        /// <param name="id">建设申请单Id</param>
        /// <returns>建设申请单维护对象</returns>
        public PlanningApplyHeaderMaintObject GetPlanningApplyHeaderById(Guid id)
        {
            PlanningApplyHeader planningApplyHeader = planningApplyHeaderRepository.FindByKey(id);
            if (planningApplyHeader != null)
            {
                PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject = MapperHelper.Map<PlanningApplyHeader, PlanningApplyHeaderMaintObject>(planningApplyHeader);
                planningApplyHeaderMaintObject.HeaderEditId = planningApplyHeader.Id;
                planningApplyHeaderMaintObject.Title = planningApplyHeader.Title;
                planningApplyHeaderMaintObject.TitleEdit = planningApplyHeader.Title;
                planningApplyHeaderMaintObject.CreateDateText = planningApplyHeader.CreateDate.ToShortDateString();
                return planningApplyHeaderMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的建设申请单在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改建设申请单
        /// </summary>
        /// <param name="planningMaintObject">要新增或者修改的建设申请单维护对象</param>
        public void AddOrUpdatePlanningApplyHeader(PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject)
        {
            if (planningApplyHeaderMaintObject.Id == Guid.Empty)
            {
                PlanningApplyHeader planningApplyHeader = AggregateFactory.CreatePlanningApplyHeader(planningApplyHeaderMaintObject.Title, planningApplyHeaderMaintObject.CreateUserId);
                planningApplyHeaderRepository.Add(planningApplyHeader);
            }
            else
            {
                PlanningApplyHeader planningApplyHeader = planningApplyHeaderRepository.FindByKey(planningApplyHeaderMaintObject.Id);
                if (planningApplyHeader != null)
                {
                    planningApplyHeader.CheckByUpdate(planningApplyHeaderMaintObject.ModifyUserId);
                    planningApplyHeader.Modify(planningApplyHeaderMaintObject.Title, planningApplyHeaderMaintObject.ModifyUserId);
                    planningApplyHeaderRepository.Update(planningApplyHeader);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlanningApplyHeaderTitle"))
                {
                    throw new ApplicationFault("单标题重复");
                }
                throw ex;
            }
        }
    }
}
