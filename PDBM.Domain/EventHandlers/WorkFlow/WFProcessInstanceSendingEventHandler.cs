using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Events.WorkFlow;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Domain.Models;
using PDBM.Domain.Specifications;

namespace PDBM.Domain.EventHandlers.WorkFlow
{
    /// <summary>
    /// 工作流过程实例发送事件处理器
    /// </summary>
    public class WFProcessInstanceSendingEventHandler : IDomainEventHandler<WFProcessInstanceSendingEvent>
    {
        private readonly IRepository<WFProcessInstance> wfProcessInstanceRepository;
        private readonly IRepository<WFProcess> wfProcessRepository;
        private readonly IRepository<WFCategory> wfCategoryRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<PlanningApplyHeader> planningApplyHeaderRepository;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;

        public WFProcessInstanceSendingEventHandler(IRepository<WFProcessInstance> wfProcessInstanceRepository,
            IRepository<WFProcess> wfProcessRepository,
            IRepository<WFCategory> wfCategoryRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Planning> planningRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<WorkApply> workApplyRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<PlanningApplyHeader> planningApplyHeaderRepository,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository)
        {
            this.wfProcessInstanceRepository = wfProcessInstanceRepository;
            this.wfProcessRepository = wfProcessRepository;
            this.wfCategoryRepository = wfCategoryRepository;
            this.addressingRepository = addressingRepository;
            this.planningRepository = planningRepository;
            this.remodelingRepository = remodelingRepository;
            this.workApplyRepository = workApplyRepository;
            this.workOrderRepository = workOrderRepository;
            this.planningApplyRepository = planningApplyRepository;
            this.planningApplyHeaderRepository = planningApplyHeaderRepository;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
        }

        public void Handle(WFProcessInstanceSendingEvent evt)
        {
            WFProcessInstance wfProcessInstance = evt.Source as WFProcessInstance;

            WFProcess wfProcess = wfProcessRepository.GetByKey(wfProcessInstance.WFProcessId);
            WFCategory wfCategory = wfCategoryRepository.GetByKey(wfProcess.WFCategoryId);
            if (wfCategory.CodePrefix == "JZXZQR") //基站寻址确认
            {
                Addressing addressing = addressingRepository.GetByKey(wfProcessInstance.EntityId);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                if (planning.AddressingState != AddressingState.未寻址确认)
                {
                    throw new DomainFault("{0}<br>选择的单据已经发送过流程", planning.PlanningCode);
                }
                planning.AddressingState = AddressingState.流转中;
                addressing.OrderCode = wfProcessInstance.WFProcessInstanceCode;
                addressing.OrderState = WFProcessInstanceState.流转中;
                planningRepository.Update(planning);
                addressingRepository.Update(addressing);
            }
            else if (wfCategory.CodePrefix == "JZGZQR") //基站改造确认
            {
                Remodeling remodeling = remodelingRepository.GetByKey(wfProcessInstance.EntityId);
                if (remodeling.OrderState != WFProcessInstanceState.未发送)
                {
                    throw new DomainFault("{0}<br>选择的单据已经发送过流程", remodeling.PlaceCode);
                }
                remodeling.OrderCode = wfProcessInstance.WFProcessInstanceCode;
                remodeling.OrderState = WFProcessInstanceState.流转中;
                remodelingRepository.Update(remodeling);
            }
            else if (wfCategory.CodePrefix == "SFXZQR") //室分寻址确认
            {
                Addressing addressing = addressingRepository.GetByKey(wfProcessInstance.EntityId);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                if (planning.AddressingState != AddressingState.未寻址确认)
                {
                    throw new DomainFault("{0}<br>选择的单据已经发送过流程", planning.PlanningCode);
                }
                planning.AddressingState = AddressingState.流转中;
                addressing.OrderCode = wfProcessInstance.WFProcessInstanceCode;
                addressing.OrderState = WFProcessInstanceState.流转中;
                planningRepository.Update(planning);
                addressingRepository.Update(addressing);
            }
            else if (wfCategory.CodePrefix == "SFGZQR") //室分改造确认
            {
                Remodeling remodeling = remodelingRepository.GetByKey(wfProcessInstance.EntityId);
                if (remodeling.OrderState != WFProcessInstanceState.未发送)
                {
                    throw new DomainFault("{0}<br>选择的单据已经发送过流程", remodeling.PlaceCode);
                }
                remodeling.OrderCode = wfProcessInstance.WFProcessInstanceCode;
                remodeling.OrderState = WFProcessInstanceState.流转中;
                remodelingRepository.Update(remodeling);
            }
        }
    }
}
