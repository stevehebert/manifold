using System;

namespace Manifold.Workflow.Compiler.Configuration
{
    public class WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext>
    {
        private readonly TWorkflow _workflow;
        private readonly TState _state;
        private readonly Action<TWorkflow, TState, IConfigurationAction> _configurationRecorder;

        internal WorkflowStateConfigurator(TWorkflow workflow, TState state, Action<TWorkflow, TState, IConfigurationAction> configurationRecorder)
        {
            _workflow = workflow;
            _state = state;
            _configurationRecorder = configurationRecorder;
        }

        public WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext> For(TWorkflow workflow)
        {
            return new WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext>(workflow, _configurationRecorder);
        }

        public WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext> Configure(TState state)
        {
            return new WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext>(_workflow, state,
                                                                                        _configurationRecorder);
        }

        public WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext> Permit(TTrigger trigger, TState state)
        {
            _configurationRecorder(_workflow, _state, null);
            return this;
        }
        public WorkflowEntryConfigurator<TWorkflow, TState, TTrigger, TContext> OnEntry()
        {
            return new WorkflowEntryConfigurator<TWorkflow, TState, TTrigger, TContext>(_workflow, _state,
                                                                                        _configurationRecorder);
        }
        public WorkflowExitConfigurator<TWorkflow, TState, TTrigger, TContext> OnExit()
        {
            return new WorkflowExitConfigurator<TWorkflow, TState, TTrigger, TContext>(_workflow, _state,
                                                                                       _configurationRecorder);
        }
    }
}