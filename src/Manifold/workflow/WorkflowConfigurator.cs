using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifold.Configuration;
using Manifold.Configuration.Pipeline;

namespace Manifold.Workflow
{
    public class WorkflowConfigurator<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly PipeDefinition _pipeDefinition;

        public WorkflowConfigurator(PipeDefinition pipeDefinition)
        {
            _pipeDefinition = pipeDefinition;
        }

        public WorkflowConfigurator<TWorkflow, TState, TTrigger, TTriggerContext> For(TWorkflow workflow)
        {
            return null;
        }

        public WorkflowConfigurator<TWorkflow, TState, TTrigger, TTriggerContext> Configure(TState state)
        {
            return null;
        }

        public WorkflowConfigurator<TWorkflow, TState, TTrigger, TTriggerContext> BindEntry<TAction>()
        {
            return null;
        }

        public WorkflowConfigurator<TWorkflow, TState, TTrigger, TTriggerContext> BindExit<TAction>()
        {
            return null;
        }

        public WorkflowConfigurator<TWorkflow, TState, TTrigger, TTriggerContext> Permit(TTrigger trigger, TState destinationState)
        {
            return null;
        }

        //public void Sample()
        //{
        //    IPipeCreator creator;

        //    creator.RegisterWorkflow<WorkflowType, WorkflowState, WorkflowTrigger, IWorkflowContext>()
        //        .For(WorkflowType.Contract)
        //           .Configure(WorkflowState.New)
//                         .OnEntry()
//                              .Do<NotifyWhenCompositeTemplateSelectedAcion>()
//                              .Do(p => p.GoNext(),WorkflowTrigger.Submit)
//                         .OnExit()
//                         .Permit

        
        //              .Permit(WorkflowTrigger.Submit, WorkflowState.RegionalApproval)
        //              .BindExit<NotifyWhenCompositeTemplateSelectedAction>()
        //              .BindExit<NotifyProductManagementFirstDerivativesSelectedAction>()
        //              .BindExit<WorkflowTrigger.Reject, WorkflowState.Rejected>()
        //           .Configure(WorkflowState.RegionalApproval)
        //              .Permit(WorkflowTrigger.Approve, WorkflowState.LegalApproval)
        //              .Permit(WorkflowTrigger.Reject, WorkflowState.Rejected)
        //              .BindEntry<WorkflowStateAction>()
        //              .BindMutatableEntry<ValidateStepAction>().DependsOn<WorkflowStateAction>() // this becomes inferred
        //              .BindEntry<RegionalEntryNotificationAction>().DependsOn<ValidateStepAction>()
        //              .BindExit<TransitionLog>()
        //              .BindExit<RegionalExitNotificationAction>().DependsOn<TransitionLog>();

        //}
    }
}
