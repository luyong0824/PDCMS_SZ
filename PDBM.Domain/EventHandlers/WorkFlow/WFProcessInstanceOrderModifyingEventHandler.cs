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
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.EventHandlers.WorkFlow
{
    /// <summary>
    /// 工作流实例中的单据修改事件处理器
    /// </summary>
    public class WFProcessInstanceOrderModifyingEventHandler : IDomainEventHandler<WFProcessInstanceOrderModifyingEvent>
    {
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<WFProcessInstance> wfProcessInstanceRepository;
        private readonly IRepository<WFActivityInstance> wfActivityInstanceRepository;

        public WFProcessInstanceOrderModifyingEventHandler(IRepository<Addressing> addressingRepository,
            IRepository<WFProcessInstance> wfProcessInstanceRepository,
            IRepository<WFActivityInstance> wfActivityInstanceRepository)
        {
            this.addressingRepository = addressingRepository;
            this.wfProcessInstanceRepository = wfProcessInstanceRepository;
            this.wfActivityInstanceRepository = wfActivityInstanceRepository;
        }

        public void Handle(WFProcessInstanceOrderModifyingEvent evt)
        {
            if (evt.Source is Planning) //修改寻址确认
            {
                Planning planning = evt.Source as Planning;
                if (planning.AddressingState == AddressingState.流程终止 || planning.AddressingState == AddressingState.已寻址确认)
                {
                    throw new DomainFault("{0}<br>只能操作状态为未寻址确认的规划", planning.PlanningCode);
                }
                else if (planning.AddressingState == AddressingState.流转中)
                {
                    Addressing addressing = addressingRepository.Get(Specification<Addressing>.Eval(entity => entity.PlanningId == planning.Id));
                    WFProcessInstance wfProcessInstance = wfProcessInstanceRepository.Get(Specification<WFProcessInstance>.Eval(entity => entity.EntityId == addressing.Id));
                    if (!wfActivityInstanceRepository.Exists(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfProcessInstance.Id &&
                        entity.UserId == planning.AddressingUserId && entity.WFActivityOperate != WFActivityOperate.阅 && entity.WFActivityInstanceState == WFActivityInstanceState.待处理)))
                    {
                        throw new DomainFault("{0}<br>只能操作状态为未寻址确认的规划", planning.PlanningCode);
                    }
                }
            }
            if (evt.Source is Remodeling) //修改改造确认
            {
                Remodeling remodeling = evt.Source as Remodeling;
                if (remodeling.OrderState == WFProcessInstanceState.流程终止 || remodeling.OrderState == WFProcessInstanceState.流程通过)
                {
                    throw new DomainFault("{0}<br>只能操作状态为未审批完成的改造确认", remodeling.PlaceCode);
                }
                else if (remodeling.OrderState == WFProcessInstanceState.流转中)
                {
                    WFProcessInstance wfProcessInstance = wfProcessInstanceRepository.Get(Specification<WFProcessInstance>.Eval(entity => entity.EntityId == remodeling.Id));
                    if (!wfActivityInstanceRepository.Exists(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfProcessInstance.Id &&
                        entity.UserId == remodeling.CreateUserId && entity.WFActivityOperate != WFActivityOperate.阅 && entity.WFActivityInstanceState == WFActivityInstanceState.待处理)))
                    {
                        throw new DomainFault("{0}<br>只能操作状态为未审批完成的改造确认", remodeling.PlaceCode);
                    }
                }
            }
            if (evt.Source is WorkApply) //修改隐患上报
            {
                WorkApply workApply = evt.Source as WorkApply;
                if (workApply.OrderState == WFProcessInstanceState.流程终止 || workApply.OrderState == WFProcessInstanceState.流程通过)
                {
                    //throw new DomainFault("{0}<br>只能操作状态为未审批完成的隐患上报单", workApply.Title);
                    throw new DomainFault("{0}<br>只能操作状态为未发送的隐患上报单", workApply.Title);
                }
                else if (workApply.OrderState == WFProcessInstanceState.流转中)
                {
                    WFProcessInstance wfProcessInstance = wfProcessInstanceRepository.Get(Specification<WFProcessInstance>.Eval(entity => entity.EntityId == workApply.Id));
                    if (!wfActivityInstanceRepository.Exists(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfProcessInstance.Id &&
                        entity.UserId == workApply.CreateUserId && entity.WFActivityOperate != WFActivityOperate.阅 && entity.WFActivityInstanceState == WFActivityInstanceState.待处理)))
                    {
                        throw new DomainFault("{0}<br>只能操作状态为未审批完成的隐患上报单", workApply.Title);
                    }
                }
            }
            if (evt.Source is WorkOrder) //修改零星派工单
            {
                WorkOrder workOrder = evt.Source as WorkOrder;
                if (workOrder.OrderState == WFProcessInstanceState.流程终止 || workOrder.OrderState == WFProcessInstanceState.流程通过)
                {
                    throw new DomainFault("{0}<br>只能操作状态为未审批完成的零星派工单", workOrder.Title);
                }
                else if (workOrder.OrderState == WFProcessInstanceState.流转中)
                {
                    WFProcessInstance wfProcessInstance = wfProcessInstanceRepository.Get(Specification<WFProcessInstance>.Eval(entity => entity.EntityId == workOrder.Id));
                    if (!wfActivityInstanceRepository.Exists(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfProcessInstance.Id &&
                        entity.UserId == workOrder.CreateUserId && entity.WFActivityOperate != WFActivityOperate.阅 && entity.WFActivityInstanceState == WFActivityInstanceState.待处理)))
                    {
                        throw new DomainFault("{0}<br>只能操作状态为未审批完成的零星派工单", workOrder.Title);
                    }
                }
            }
        }
    }
}
