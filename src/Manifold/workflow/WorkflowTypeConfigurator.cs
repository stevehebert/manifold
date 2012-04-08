using System;
using Manifold.Workflow.Compiler.Configuration;

namespace Manifold.Workflow
{

    


    

    public enum WorkflowType
    {
        Contract,
        Cancellation
    }

    public enum StateType
    {
        Rejected,
        RegionalReview,
        LegalReview,
        SignatureAcquisition
    }


    public enum TriggerType
    {
        Approve,
        Reject
    }

    public interface ITriggerContext
    {
        
    }

    public class Sample
    {
        public void Go(WorkflowStateConfigurator<WorkflowType, StateType, TriggerType, ITriggerContext> configurator )
        {
            configurator.For(WorkflowType.Contract)
                .Configure(StateType.RegionalReview)
                    .Permit(TriggerType.Approve, StateType.LegalReview)
                    .Permit(TriggerType.Reject, StateType.Rejected)
                    .OnEntry()
                        .Do<string>()
                        .Do<string>()
                        .When((a) => false, TriggerType.Approve)
                        .Do<string>()
                    .OnExit()
                        .Do<string>()
                        .Do<string>()
                        .Do<string>()
                .Configure(StateType.LegalReview)
                    .Permit(TriggerType.Approve, StateType.SignatureAcquisition)
                    .Permit(TriggerType.Reject, StateType.Rejected)
                    .OnEntry()
                        .Do<string>()
                        .Do<string>()
                        .When(a => false, TriggerType.Approve)
                        .Do<string>()
                    .OnExit()
                        .Do<string>()
                        .Do<string>();

                ;
            
        }
}

    public class WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext>
    {

        public WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext> For(TWorkflow workflow)
        {
            return this;

        }

        public WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext> Configure(TState state)
        {
            return this;
        }

        public WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext> Permit(TTrigger trigger, TState destinationState)
        {
            return this;
        }


        public WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext> OnEntry()
        {
            return this;
        }


        public WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext> OnExit()
        {
            return this;
        }

        public WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext> Do()
        {
            return this;
        } 

        public WorkflowStateConfigurator1<TWorkflow, TState, TTrigger, TTriggerContext> When(Func<bool> evaluator, TTrigger trigger )
        {
            return this;
        }
    }
}
